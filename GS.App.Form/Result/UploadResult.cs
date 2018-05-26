using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GS.App.Form.Result
{
    public class UploadResult : JsonNetResult
    {
        public enum UploadType
        {
            SUCCESS = 0,
            ERROR = 1
        }
        public UploadResult(string message, UploadType type = UploadType.SUCCESS)
        {
            Data = new Dictionary<string, string>() {
                {"type",type.GetHashCode().ToString() },
                { type == UploadType.ERROR ? "message" : "url",message}
           };
        }
    }
}