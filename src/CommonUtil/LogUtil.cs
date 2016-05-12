using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using System.Web;
using NLog.Fluent;
using Newtonsoft.Json;

namespace CommonUtil
{
    /// <summary>
    /// 日志记录类
    /// </summary>
    public class LogUtil
    {
        private static bool isinit = false;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        static LogUtil()
        {
            if (isinit == false)
            {
                isinit = true;
                SetConfig();
            }
        }

        private static bool LogInfoEnable = false;
        private static bool LogErrorEnable = false;
        private static bool LogExceptionEnable = false;
        private static bool LogComplementEnable = false;
        private static bool LogDubugEnable = false;
        private static bool LogFatalEnabled = false;

        /// <summary>
        /// 设置初始值。
        /// </summary>
        public static void SetConfig()
        {
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
        /// <param name="msg"></param>
        /// <param name="param"></param>
        public static void WriteInfo(string msg, IDictionary<string, object> param = null)
        {
            if (LogInfoEnable)
            {
                var log = logger.Info().Message(msg);
                foreach (var kv in param)
                {
                    log.Property(kv.Key, kv.Value.GetType() == typeof(string) ? kv.Value : JsonConvert.SerializeObject(kv.Value));
                }

                log.Write();
            }
        }

        /// <summary>
        /// 写入补充日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteComplement(string msg, IDictionary<string, object> param = null)
        {
            if (LogComplementEnable)
            {
                var log = logger.Trace().Message(msg);
                foreach (var kv in param)
                {
                    log.Property(kv.Key, kv.Value.GetType() == typeof(string) ? kv.Value : JsonConvert.SerializeObject(kv.Value));
                }

                log.Write();
            }
        }

        /// <summary>
        /// 写入Debug日志消息
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteDebug(string msg, IDictionary<string, object> param = null)
        {
            if (LogDubugEnable)
            {
                var log = logger.Debug().Message(msg);
                foreach (var kv in param)
                {
                    log.Property(kv.Key, kv.Value.GetType() == typeof(string) ? kv.Value : JsonConvert.SerializeObject(kv.Value));
                }

                log.Write();
            }
        }

        /// <summary>
        /// 写入异常日志信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void WriteException(string msg, Exception ex)
        {
            if (LogExceptionEnable)
            {
                var log = logger.Error().Message(msg).Property("Detail", BuildDetail()).Exception(ex);
                log.Write();
            }
        }

        /// <summary>
        /// 写入严重错误日志消息
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteFatal(string msg, Exception ex)
        {
            if (LogFatalEnabled)
            {
                var log = logger.Error().Message(msg).Property("Detail", BuildDetail()).Exception(ex);
                log.Write();
            }
        }

        private static string BuildDetail(IDictionary<string, object> requestparam = null)
        {
            var sb = new StringBuilder();
            HttpContext ctx = HttpContext.Current;
            if (requestparam != null)
            {
                sb.AppendFormat("Params:{0},", JsonConvert.SerializeObject(requestparam));
            }
            if (ctx != null)
            {
                sb.AppendFormat("Url:{0}, UserHostAddress:{1},", ctx.Request.Url, ctx.Request.UserHostAddress);
                if (null != ctx.Request.UrlReferrer)
                {
                    sb.AppendFormat(" UrlReferrer:{0}", ctx.Request.UrlReferrer);
                }
            }

            return sb.ToString().TrimEnd(',');
        }
    }
}
