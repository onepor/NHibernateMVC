using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZAJCZN.MIS.Helpers
{
    public static class LogHelper
    {
        private static object lockerForError = new object();    //错误日志锁  

        /// <summary>  
        /// 保存错误日志  
        /// </summary>  
        /// <param name="source"></param>  
        /// <param name="type"></param>  
        /// <param name="error"></param>  
        public static void SaveLog(string errMessage,string method)
        {
            try
            {
                //if (CommonParameter.IsSaveErrorLog == 1)
                //{
                    lock (lockerForError)
                    {
                        string strCurPath = AppDomain.CurrentDomain.BaseDirectory.ToString();
                        string strErrorLogPath = Path.Combine(strCurPath, "ErrorLog");
                        string strErrorLogFile = Path.Combine(strErrorLogPath, DateTime.Now.ToString("yyyyMMdd") + ".log");
                        if (!Directory.Exists(strErrorLogPath))
                        {
                            Directory.CreateDirectory(strErrorLogPath);
                        }
                        FileStream fs = new FileStream(strErrorLogFile, FileMode.OpenOrCreate);
                        StreamWriter streamWriter = new StreamWriter(fs);
                        streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                        streamWriter.WriteLine("[" + DateTime.Now.ToString() + "]/r/n Error : " + errMessage + "/r/nError : [method]" + method + "/r/n");
                        streamWriter.Flush();
                        streamWriter.Close();
                        fs.Close();
                    }
                //}
            }
            catch (Exception exception)
            {
            }
        }  
    }
}
