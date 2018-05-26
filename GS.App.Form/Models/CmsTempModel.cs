using GS.SQLModel.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GS.App.Form.Models
{
    public class CmsTempModel
    {
        public int SiteId { get; set; }
        public Site Current { get; set; }
        public IEnumerable<Site> Site { get; set; }
        public IEnumerable<SiteTemp> SiteTemp { get; set; }
        public IEnumerable<SiteTemp> JsTemp { get; set; }
        public IEnumerable<SiteTemp> CssTemp { get; set; }
        public IEnumerable<SiteTemp> PageTemp { get; set; }
        public IEnumerable<SiteTemp> ControlTemp { get; set; }
        public IEnumerable<SiteResource> SiteResource { get; set; }
    }
}