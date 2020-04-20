using System;
using System.Collections.Generic;
using System.Text;

namespace ZAJCZN.MIS.Helpers
{
    public static class Log4Helper
    {
        #region - 错误日志 -

        /// <summary>
        /// 输出错误日志到log类
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        public static void WriteErrLog(Type t, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Error("Error", ex);
        }

        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public static void WriteErrLog(Type t, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Error(msg);
        }

        public static void WriteErrLog(string type, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(type);
            log.Error(msg);
        }

        #endregion

        #region - 调试日志 -

        /// <summary>
        /// 输出调试日志到Log4Net
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public static void WriteDebug(Type t, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Debug(msg);
        }

        public static void WriteDebug(string type, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(type);
            log.Debug(msg);
        }

        #endregion

    }
}
