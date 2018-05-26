using System.Collections.Generic;
using System.Linq;
using GS.Common.Util;

namespace GS.SQL.DataSource
{
    public enum FiledType
    {
        STRING,
        INT
    }
    public enum OrderType
    {
        ASC,
        DESC
    }
    public enum WhereType
    {
        OR,
        AND
    }
    public class SQLExpression
    {
        public string Expression { private get; set; }
        public virtual string VirtualExpression
        {
            get
            {
                return !string.IsNullOrEmpty(Expression) ? Expression : string.Empty;
            }
        }
    }
    public class Order : SQLExpression
    {
        public OrderType Type { get; set; }
        public string FiledName { get; set; }
        public override string VirtualExpression
        {
            get
            {
                var exp = base.VirtualExpression;
                if (string.IsNullOrEmpty(exp))
                {
                    if (string.IsNullOrEmpty(FiledName))
                        return exp;
                    exp = FiledName;
                }
                var type = Type.ToString();
                return $"{exp} {type}";
            }
        }
    }

    public class Where : SQLExpression
    {
        public WhereType Type { get; set; }
        public FiledType FiledType { get; set; }
        public string FiledName { get; set; }
        public string Value { get; set; }
        public override string VirtualExpression
        {
            get
            {
                var exp = base.VirtualExpression;
                if (string.IsNullOrEmpty(exp))
                {
                    if (string.IsNullOrEmpty(FiledName) || string.IsNullOrEmpty(Value))
                        return exp;
                    if (FiledType == FiledType.STRING)
                        exp = $"{FiledName}='{Value}'";
                    else
                        exp = $"{FiledName}={Value}";
                }
                var type = Type.ToString();
                return $"{type} {exp}";
            }
        }
    }

    public class SQLCondition : SQLExpression
    {
        public IEnumerable<Order> Order { get; set; }
        public IEnumerable<Where> Where { get; set; }
        public string WhereExpression
        {
            get
            {
                var where = string.Empty;
                if (Where != null)
                {
                    var w = Where.ToList();
                    for (int i = 0; i < w.Count(); i++)
                    {
                        var item = w[i];
                        if (i == 0)
                            where = item.VirtualExpression.Replace(WhereType.AND.ToString(), string.Empty).Replace(WhereType.OR.ToString(), string.Empty);
                        else
                            where = $"{where} {item.VirtualExpression}";

                    }
                }
                if (!string.IsNullOrEmpty(where))
                    where = $"where {where}";
                return where;
            }
        }
        public string OrderExpression
        {
            get
            {
                var order = Order != null ? Order.Select(t => t.VirtualExpression).JoinString(",") : string.Empty;
                if (!string.IsNullOrEmpty(order))
                    order = $"order by {order}";
                return order;
            }
        }
        public override string VirtualExpression
        {
            get
            {
                var exp = base.VirtualExpression;
                if (!string.IsNullOrEmpty(exp))
                    return exp;
                return $"{WhereExpression} {OrderExpression}";
            }
        }
    }
}
