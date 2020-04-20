using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZAJCZN.MIS.Helpers
{
    public static class MySqlAutoBack
    {

        /*
备份：mysqldump -hlocalhost -P3306 -uroot -p123 --default-character-set=utf8 mydata > d:\test\mydata.sql
还原：mysql -hlocalhost -P3306 -uroot -p123 mydata  < d:\test\mydata.sql
         * */

        public static void DoBackup()
        {
            string[] ary = ReadFromText();

            string host = ary[0];
            string port = ary[1];
            string user = ary[2];
            string password = ary[3];
            string database = ary[4];
            string fileName = database + "_bak_" + DateTime.Now.ToString("yyyyMMddhhmmss");
            string bakPath = ary[5] + "\\" + fileName + ".sql";
            string logPath = ary[6];

            // todo 需要配置系统配置参数（mysql路径；）;
            string cmdStr = "/c mysqldump -h" + host + " -P" + port + " -u" + user + " -p" + password + " " + database + " > " + bakPath;


            try
            {
                System.Diagnostics.Process.Start("cmd", cmdStr);
            }
            catch (Exception ex)
            {
                WriteLog(logPath, ex.Message);
            }

        }

        /**/
        /// <summary>
        /// 读配置
        /// </summary>
        /// <returns></returns>
        public static string[] ReadFromText()
        {
            string FileName = @"C:\BackupIni.txt";
            ArrayList list = new ArrayList();
            if (File.Exists(FileName))
            {
                StreamReader sr = new StreamReader(FileName, Encoding.Default);
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    list.Add(s);
                }
            }
            return (string[])list.ToArray(typeof(string));
        }

        /**/
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="theStr"></param>
        public static void WriteLog(string filePath, string theStr)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            StreamWriter sw = new StreamWriter(filePath, true, Encoding.Default);
            sw.WriteLine(theStr);
            sw.Flush();
            sw.Close();
        }
    }
}
