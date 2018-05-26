using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GS.Common.Util;
using System.Text;
using System.Threading.Tasks;

namespace GS.SQL.DataSource
{
    public class ExpEnity<T>
    {
        internal ExpEnity()
        {
            pageSize = -1;
            currentPage = -1;
        }
        internal SQLDataEnity<T> Enity { get; set; }
        internal string GetBody(Expression<Func<string>> exp)
        {
            if (exp == null)
                return string.Empty;
            return exp.Body.ToString().Replace("\"", "");
        }
        internal int pageSize { get; set; }
        internal int currentPage { get; set; }
        internal string WhereExprBody
        {
            get
            {
                return GetBody(WhereExpr);
            }
        }
        internal string OrderExprBody
        {
            get
            {
                return GetBody(OrderExpr);
            }
        }
        internal Expression<Func<string>> WhereExpr { get; set; }
        internal Expression<Func<string>> OrderExpr { get; set; }
    }
    public static class SQLDataEnityExtensions
    {
        private static readonly IDictionary<string, string> _whereFilter;
        static SQLDataEnityExtensions()
        {
            _whereFilter = new Dictionary<string, string>() {
                { "\"", "'" },
                { "AndAlso", WhereType.AND.ToString()},
                {"OrElse", WhereType.OR.ToString() },
                { ".Like", " Like"},
                {".In", " in" },
                {".NotIn", " not in" },
                { "new []", ""},
                { "{", ""},
                {"}", "" },
                {"==", "=" },
                {"False", "0" },
                {"True", "1" },
            };
        }
        public static ExpEnity<T> Exp<T>(this SQLDataEnity<T> enity)
        {
            return new ExpEnity<T>()
            {
                Enity = enity
            };
        }
        public static bool NotIn<T>(this T value1, params T[] value2)
        {
            return true;
        }
        public static bool In<T>(this T value1, params T[] value2)
        {
            return true;
        }
        public static bool Like(this string value1, string value2)
        {
            return true;
        }
        private static string GetExpressionType(ExpressionType expressionType)
        {
            if (expressionType == ExpressionType.OrElse || expressionType == ExpressionType.Or)
                return " or ";
            else if (expressionType == ExpressionType.AndAlso || expressionType == ExpressionType.And)
                return " and ";
            return string.Empty;
        }
        private static string ResolveBinaryExpression(BinaryExpression bExp)
        {
            var bodyStr = string.Empty;
            if (bExp.Left.NodeType == ExpressionType.Call)
            {
                bodyStr += ResolveCallExpression((MethodCallExpression)bExp.Left);
            }
            else
            {
                var mlExp = bExp.Left as BinaryExpression;
                if (mlExp != null)
                    bodyStr += ResolveBinaryExpression(mlExp);
            }
            if (bExp.Right.NodeType == ExpressionType.Call)
            {
                bodyStr += GetExpressionType(bExp.NodeType) + ResolveCallExpression((MethodCallExpression)bExp.Right);
            }
            else
            {
                var mExp = bExp.Right as MemberExpression;
                if (mExp != null)
                {
                    var cast = Expression.Convert(mExp, typeof(object));
                    var result = Expression.Lambda<Func<object>>(cast).Compile().Invoke();

                    var value = string.Empty;
                    var type = result.GetType();

                    if (type == typeof(string) || type == typeof(DateTime))
                        value = $"'{result}'";
                    else
                        value = result.ToString();
                    bodyStr += GetExpressionType(bExp.NodeType) + bExp.ToString().Replace(mExp.ToString(), value);
                }
                else if (bExp.Right.NodeType == ExpressionType.Equal)
                {
                    var mrExp = bExp.Right as BinaryExpression;
                    bodyStr += GetExpressionType(bExp.NodeType) + ResolveBinaryExpression(mrExp);
                }
                else
                {
                    if (bExp.NodeType == ExpressionType.Equal)
                        bodyStr += GetExpressionType(bExp.NodeType) + bExp.ToString();
                    else
                        bodyStr += GetExpressionType(bExp.NodeType) + bExp.Right.ToString();
                }
            }
            return bodyStr;
        }
        private static string ResolveCallExpression(MethodCallExpression bExp)
        {
            var content = bExp.ToString();
            if (content.Contains(".Like"))
            {
                var result = Expression.Lambda<Func<object>>(Expression.Convert(bExp.Arguments[1], typeof(object))).Compile().Invoke();
                return content.Replace(bExp.Arguments[1].ToString(), result.ToString()).Replace("(", " '%").Replace(")", "%' ");
            }
            else if (content.Contains(".NotIn") || content.Contains(".In"))
            {
                var result = Expression.Lambda<Func<object>>(Expression.Convert(bExp.Arguments[1], typeof(object))).Compile().Invoke();
                var type = result.GetType();
                var value = string.Empty;
                if (type == typeof(string[]))
                    value = string.Join(",", ((string[])result).Select(t => $"'{t}'"));
                else if (type == typeof(int[]))
                    value = string.Join(",", (int[])result);
                return content.Replace(bExp.Arguments[1].ToString(), value);
            }
            return content;
        }
        private static string Resolve<T>(Expression<T> body)
        {
            if (body.Body.NodeType == ExpressionType.Call)
                return ResolveCallExpression((MethodCallExpression)body.Body);

            var bExp = body.Body as BinaryExpression;
            if (bExp == null)
                return body.Body.ToString();

            return ResolveBinaryExpression(bExp);
        }
        private static ExpEnity<T> Order<T, V>(this ExpEnity<T> expr1, Expression<Func<T, V>> expr2, OrderType type)
        {
            var expr1Body = expr1.OrderExprBody;
            var name = $"{expr2.Parameters[0].Name}.";
            var bodyContent = Resolve(expr2).Replace(name, "");
            var right = Expression.Constant(
                 $"( {bodyContent} )"
                );

            if (string.IsNullOrEmpty(expr1Body))
                expr1.OrderExpr = Expression.Lambda<Func<string>>(Expression.Constant($"{right} {type.ToString()}"));
            else
                expr1.OrderExpr = Expression.Lambda<Func<string>>(Expression.Constant($"{expr1Body} , {right} {type.ToString()}"));
            return expr1;

        }
        private static ExpEnity<T> Where<T>(this ExpEnity<T> expr1, Expression<Func<T, bool>> expr2, WhereType type)
        {
            var expr1Body = expr1.WhereExprBody;
            var name = $"{expr2.Parameters[0].Name}.";
            var bodyContent = Resolve(expr2).Replace(name, "");

            foreach (var item in _whereFilter)
                bodyContent = bodyContent.Replace(item.Key, item.Value);

            var right = Expression.Constant(
                 $"( {bodyContent} )"
                );

            if (string.IsNullOrEmpty(expr1Body))
                expr1.WhereExpr = Expression.Lambda<Func<string>>(Expression.Constant($"{right}"));
            else
                expr1.WhereExpr = Expression.Lambda<Func<string>>(Expression.Constant($"{expr1Body} {type.ToString()} {right}"));
            return expr1;
        }
        private static SQLCondition GetSQLCondition<T>(ExpEnity<T> expr1)
        {
            var expression = string.IsNullOrEmpty(expr1.WhereExprBody) ? string.Empty : $" where  { expr1.WhereExprBody }";
            expression = expression + (string.IsNullOrEmpty(expr1.OrderExprBody) ? string.Empty : $"order by {expr1.OrderExprBody}");
            return new SQLCondition() { Expression = expression };
        }
        public static ExpEnity<T> OrderDesc<T, V>(this ExpEnity<T> expr1, Expression<Func<T, V>> expr2)
        {
            return Order(expr1, expr2, OrderType.DESC);
        }
        public static ExpEnity<T> OrderAsc<T, V>(this ExpEnity<T> expr1, Expression<Func<T, V>> expr2)
        {
            return Order(expr1, expr2, OrderType.ASC);
        }
        public static ExpEnity<T> Or<T>(this ExpEnity<T> expr1, Expression<Func<T, bool>> expr2)
        {
            return Where(expr1, expr2, WhereType.OR);
        }
        public static ExpEnity<T> And<T>(this ExpEnity<T> expr1, Expression<Func<T, bool>> expr2)
        {
            return Where(expr1, expr2, WhereType.AND);
        }
        public static ExpEnity<T> Page<T>(this ExpEnity<T> expr1, int pageSize, int currentPage)
        {
            expr1.pageSize = pageSize;
            expr1.currentPage = currentPage;
            return expr1;
        }
        public static SQLPage<T> Query<T>(this ExpEnity<T> expr1, int pageSize, int currentPage)
        {
            var condition = GetSQLCondition(expr1);
            LogUtil.InfoFormat(condition.VirtualExpression);
            return expr1.Enity.Query(pageSize, currentPage, condition);
        }
        public static SQLPage<T> QueryPage<T>(this ExpEnity<T> expr1)
        {
            if (expr1.pageSize == -1 || expr1.currentPage == -1)
                throw new Exception("未设置页码和当前页");
            var condition = GetSQLCondition(expr1);
            LogUtil.InfoFormat(condition.VirtualExpression);
            return expr1.Enity.Query(expr1.pageSize, expr1.currentPage, condition);
        }
        public static IEnumerable<T> Query<T>(this ExpEnity<T> expr1)
        {
            var condition = GetSQLCondition(expr1);
            LogUtil.InfoFormat(condition.VirtualExpression);
            return expr1.Enity.Query(condition);
        }
    }
}
