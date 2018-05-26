using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.Api.Model.Config
{
   public class AppSettings
    {
        public string CacheStorage { get; set; }
        public bool IsAuthentication { get; set; }
        public string CorsDomains { get; set; }
        public string RedisReaderPath { get; set; }
        public string RedisWriterPath { get; set; }
    }
}
