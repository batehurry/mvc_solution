using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using System.Web;

namespace CommonUtil
{
    /// <summary>
    /// 日志记录类
    /// </summary>
    public class LogHelper
    {
        private static bool isinit = false;

        static LogHelper()
        {
            if (isinit == false)
            {
                isinit = true;
                SetConfig();
            }
        }

        //private static readonly log4net.ILog LogInfo = log4net.LogManager.GetLogger("LogInfo");

        //private static readonly log4net.ILog LogError = log4net.LogManager.GetLogger("LogError");

        //private static readonly log4net.ILog LogException = log4net.LogManager.GetLogger("LogException");

        //private static readonly log4net.ILog LogComplement = log4net.LogManager.GetLogger("LogComplement");

        //private static readonly log4net.ILog LogDubug = log4net.LogManager.GetLogger("LogDubug");


        private static bool LogInfoEnable = false;
        private static bool LogErrorEnable = false;
        private static bool LogExceptionEnable = false;
        private static bool LogComplementEnable = false;
        private static bool LogDubugEnable = false;
        private static bool LogFatalEnabled = false;

        private static Logger logger = LogManager.GetCurrentClassLogger();



        /// <summary>
        /// 设置初始值。
        /// </summary>
        public static void SetConfig()
        {
            //log4net.Config.DOMConfigurator.Configure();
            //LogInfoEnable=LogInfo.IsInfoEnabled;
            //LogErrorEnable=LogError.IsErrorEnabled;
            //LogExceptionEnable=LogException.IsErrorEnabled;
            //LogComplementEnable=LogComplement.IsErrorEnabled;
            //LogDubugEnable = LogDubug.IsDebugEnabled;

            LogInfoEnable = logger.IsInfoEnabled;
            LogErrorEnable = logger.IsErrorEnabled;
            LogExceptionEnable = logger.IsErrorEnabled;
            LogComplementEnable = logger.IsTraceEnabled;
            LogFatalEnabled = logger.IsFatalEnabled;
            LogDubugEnable = logger.IsDebugEnabled;

        }
        /// <summary>
        /// 写入普通日志消息
        /// </summary>
        /// <param name="info"></param>
        public static void WriteInfo(string info)
        {
            if (LogInfoEnable)
            {
                logger.Info(BuildMessage(info));
                //LogInfo.Info(info);
            }
        }
        /// <summary>
        /// 写入Debug日志消息
        /// </summary>
        /// <param name="info"></param>
        public static void WriteDebug(string info)
        {
            if (LogDubugEnable)
            {
                logger.Debug(BuildMessage(info));
                //LogDubug.Debug(info);
            }
        }
        /// <summary>
        /// 写入错误日志消息
        /// </summary>
        /// <param name="info"></param>
        public static void WriteError(string info)
        {
            if (LogErrorEnable)
            {
                logger.Error(BuildMessage(info));
                //LogError.Error(info);
            }
        }

        /// <summary>
        /// 写入异常日志信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ex"></param>
        public static void WriteException(string info, Exception ex)
        {
            if (LogExceptionEnable)
            {
                logger.Error(BuildMessage(info, ex));
                //LogException.Error(info,ex);
            }
        }

        /// <summary>
        /// 写入严重错误日志消息
        /// </summary>
        /// <param name="info"></param>
        public static void WriteFatal(string info)
        {
            if (LogErrorEnable)
            {
                logger.Fatal(BuildMessage(info));
            }
        }
        /// <summary>
        /// 写入补充日志
        /// </summary>
        /// <param name="info"></param>
        public static void WriteComplement(string info)
        {
            if (LogComplementEnable)
            {
                logger.Trace(BuildMessage(info));
                //LogComplement.Error(info);
            }
        }
        /// <summary>
        /// 写入补充日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ex"></param>
        public static void WriteComplement(string info, Exception ex)
        {
            if (LogComplementEnable)
            {
                logger.Trace(BuildMessage(info, ex));
            }
        }


        static string BuildMessage(string info)
        {
            return BuildMessage(info, null);
        }

        static string BuildMessage(string info, Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            HttpContext ctx = HttpContext.Current;
            sb.AppendFormat("Time:{0}-{1}\r\n", DateTime.Now, info);

            if (ctx != null)
            {
                sb.AppendFormat("Url:{0}\r\n", ctx.Request.Url);
                if (null != ctx.Request.UrlReferrer)
                {
                    sb.AppendFormat("UrlReferrer:{0}\r\n", ctx.Request.UrlReferrer);
                }
                sb.AppendFormat("UserHostAddress:{0}\r\n", ctx.Request.UserHostAddress);
            }

            if (ex != null)
            {
                sb.AppendFormat("Exception:{0}\r\n", ex.ToString());
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
