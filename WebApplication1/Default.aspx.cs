using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Expressions;

namespace WebApplication1
{
    class ReplaceExpressionVisitor
     : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            if (node == _oldValue)
                return _newValue;
            return base.Visit(node);
        }
    }
    public static class Exp
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> AndAlso<T>(
       this Expression<Func<T, bool>> expr1,
       Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
        }
    }
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public partial class _Default : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Exp.True<User>().AndAlso(a => a.Name == "aaa");

            //    var user = new User();
            //    //Expression.Lambda
            //    Expression.Constant
            //    Expression<Action> a = Expression.Lambda<Action>(
            //Expression.Call(typeof(Console).GetMethod("WriteLine", new[] { typeof(string) }), Expression.Constant("hello world！"))
            //       );
            //    a.Compile()();
            //    Expression<Func<int, int, int>> expr = (x, y) => x + y;

            //假如我们要拼接x=>x.Id==1，假如x的类型为User
            //Expression.Property
            var parameterExp = Expression.Parameter(typeof(int), "y");

            // Response.Write();
            //结果是这样：x=>，x是变量名
            //  var propertyExp = Expression.Property(parameterExp, "Id");
            //Expression.IfThen
            //结果是这样：x=>x.Id，这句是为了构建访问属性的表达式
            //上面这句第一个参数是你要取属性的对象表达式。我们要拼的表达式是x=>x.Id==1，==1这块先不管，其实就是x=>x.Id，那么其实我们就是对x进行取属性值，而x是parameterExp，所以第一个参数是parameterExp，第二个参数好说，就是属性名
            var constExp = Expression.Parameter(typeof(int), "x");
            var response = Expression.Parameter(typeof(HttpResponse), "response");
            // var ins = Expression.Parameter(typeof(HttpResponse), "d");
            var test = Expression.Equal(parameterExp, constExp);
            //var rowexp = Expression.Parameter(typeof(string), "Response");
            //  Expression.Invoke()
            // var s = Expression.Constant(Response);
            //var ifTrue = Expression.Call(typeof(Console).GetMethod("WriteLine", new[] { typeof(string) }),
            //    Expression.Constant("hello world！"));
            //var ifElse = Expression.Call(typeof(Console).GetMethod("WriteLine", new[] { typeof(string) }),

            //  Expression.Constant("hello world！"));
            // var ifTrue = Expression.Call(response,Response.GetType(), "Write", null, Expression.Constant("hello 11223！"));
            var ifTrue = Expression.Call(response, Response.GetType().GetMethod("Write", new[] { typeof(string) }), Expression.Constant("hello 11223！"));
            var ifElse = Expression.Call(response, Response.GetType().GetMethod("Write", new[] { typeof(string) }), Expression.Constant("hello 8988989！"));
            var body = Expression.IfThenElse(test, ifTrue, ifElse);

            Response.Write(body.ToString());
            //  Expression.Lambda<Func<int, int, int>>(exp, new ParameterExpression[] { left, right });
            var exp = Expression.Lambda<Action<int, int, HttpResponse>>(body,
                new ParameterExpression[] { parameterExp, constExp, response })
                .Compile();

            Response.Write(exp.ToString());
            //exp(1, 2, Response);
            //Expression.Constant(1);
            //结果是··没有结果，构建一个常量表达式，值为1（LINQ的世界，一切皆表达式树）
            //马上就是关键的一步了
            // var body = Expression.Equal(propertyExp, constExp);
            //结果是：x=>x.Id==1，这个··还需要解释么，很简单，不是么。创建一个相等的表达式，然后传入左边和右边的表达式
            //当然到这儿还不能用，还需要继续
            // var lambda = Expression.Lambda<Func<User, bool>>(body, parameterExp);
            //这句和第二句是我学的时候最难理解的两个地方。这句是将我们的成果封装成能用的，第一个参数就是我们的成果，第二个参数是实现这个成果所需要的参数，那当然是parameterExp，然后泛型参数Func<User,bool>就是我们想把这个表达式封装成什么样的东西，此时,lambda的类型就是Expression<Fun<User,bool>>，这个时候就能用了

            ////假如我们要拼接x=>x.Username.Contains("aaa")，假如x的类型为User
            //parameterExp = Expression.Parameter(typeof(User), "x");
            //var propertyExp = Expression.Property(parameterExp, "Username");
            //////上面两句不再介绍
            //var containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            //////因为我们要拼接的表达式中调用了string类型的Username的Contains方法，所以反射获取string类型的Contains方法
            //var constExps = Expression.Constant("aaa");
            //////不再解释
            //var containsExp = Expression.Call(propertyExp, containsMethod, constExps);
            //////结果是：x=>x.Username.Contains("aaa")，第一个参数，是要调用哪个实例的方法，这里是propertyExp，第二个是调用哪个方法，第三个是参数，理解了上一个示例，这个应该不难理解
            //var lambda = Expression.Lambda<Func<User, bool>>(containsExp, parameterExp);
            ////不再解释
            // lambda.Compile()(user);
        }
    }
}