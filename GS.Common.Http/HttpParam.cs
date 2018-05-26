using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GS.Common.Http
{
	public class ContentType
	{
        public const string CONTENTTYPE_TEXT = "application/json";
		public const string FORM_DATA = "application/x-www-form-urlencoded";  
    }
	public class HttpParam
	{
		public HttpParam()
		{
			Type = ContentType.CONTENTTYPE_TEXT;
			IsUseBase = true;
		}
		public string Url { get; set; }
		public HttpMethod Method { get; set; }
		public bool IsUseBase { get; set; }
		public string Type { get; set; }
		public HttpContent Content { get; set; }
		public virtual Uri Uri
		{
			get
			{
				return new Uri(Url);
			}
		}
	}
}
