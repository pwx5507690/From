using GS.SQL.DataSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.SQLModel.Cms
{
    public class Site : Base<Site>
    {
        public Site() : base("Cms") {

        }
        [Column("Name", DbType.String)]
        public string Name { get; set; }
        [Column("PageName", DbType.String)]
        public string PageName { get; set; }
        [Column("Url", DbType.String)]
        public string Url { get; set; }
        [Column("Access", DbType.Int32)]
        public Int32? Access { get; set; }
        [Column("Icon", DbType.String)]
        public string Icon { get; set; }
        [Column("IpFilter", DbType.String)]
        public string IpFilter { get; set; }
    }
}
