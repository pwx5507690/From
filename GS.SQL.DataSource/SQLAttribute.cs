using System;
using System.Data;

namespace GS.SQL.DataSource
{
    [AttributeUsage(AttributeTargets.All)]
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute(bool isDbOption)
        {
            IsDbOption = isDbOption;
        }

        public ColumnAttribute(string name, DbType type, bool ident = false, bool key = false, bool isDbOption = true)
        {
            Name = name.ToLower();
            Type = type;
            Ident = ident;
            Key = key;
            IsDbOption = isDbOption;
        }
        public bool IsDbOption { get; set; }
        public string Name { get; set; }
        public DbType Type { get; set; }
        public bool Ident { get; set; }
        public bool Key { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public TableAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}
