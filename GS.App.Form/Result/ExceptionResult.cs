using GS.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GS.App.Form.Result
{
    public class FormException : Exception
    {
        private readonly bool _isWriteLog;
        public FormException(string message, bool isWriteLog = false) : base(message)
        {
            _isWriteLog = isWriteLog;
        }
        public bool WriteLog
        {
            get
            {
                return _isWriteLog;
            }
        }
    }

    public class ExceptionResult : JsonNetResult
    {
        public ExceptionResult(string message, bool isWriteLog = true, string controller = null, string stackTrace = null)
        {
            Data = new { exception = message };
            if (isWriteLog) LogUtil.ErrorFormat(message + controller ?? string.Empty + stackTrace ?? string.Empty);
        }

    }
}