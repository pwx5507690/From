using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GS.App.Form.Models
{
    public class WebCmsSendModel
    {
        public string Cmd { get; set; }
        public string OldTempName { get; set; }
        public string TempType { get; set; }
        public string Content { get; set; }
        public string SiteName { get; set; }
        public string TempName { get; set; }
        public string Icon { get; set; }
        public string Option { get; set; }
        public string Resource { get; set; }
    }
}