using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.Api.Model
{
    public class DyncExp
    {
        public string Table { get; set; }
        public string FiledName { get; set; }
        public IDictionary<string, string> Param { get; set; }
    }
}
