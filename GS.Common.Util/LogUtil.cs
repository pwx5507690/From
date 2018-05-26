using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.Common.Util
{
    public class LogUtil
    {
        private static readonly string _filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Logs\\";
        private static readonly string _logKey = "LogFile";
        private static readonly string _patternLayout = "%date [%thread] %-5level - %message%newline";
        private static readonly string _datePattern = "yyyy-MM-dd\".txt\"";
        static LogUtil()
        {
            var appender = new log4net.Appender.RollingFileAppender();
            appender.File = _filePath;
            appender.LockingModel = new log4net.Appender.FileAppender.MinimalLock();
            appender.Layout = new log4net.Layout.PatternLayout(_patternLayout);
            appender.MaxSizeRollBackups = -1;
            appender.RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Date;
            appender.DatePattern = _datePattern;
            appender.StaticLogFileName = false;
            appender.LockingModel = new log4net.Appender.FileAppender.MinimalLock();
            appender.ActivateOptions();
            appender.Encoding = Encoding.Default;
            appender.StaticLogFileName = false;
            appender.AppendToFile = true;
            appender.Name = _logKey;
            log4net.Config.BasicConfigurator.Configure(appender);
        }
        public static string WebLog(string fileName)
        {
            return FileUtil.ReadFile($"{_filePath}{fileName}.txt");
        }
        [Conditional("DEBUG")]
        public static void InfoFormat(string format, params object[] args)
        {
            GetLog.InfoFormat(format, args);
        }
        public static void ErrorFormat(string format, params object[] args)
        {
            GetLog.ErrorFormat(format, args);
        }
        private static ILog GetLog
        {
            get
            {
                return LogManager.GetLogger(_logKey);
            }
        }
    }
}
