using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ZAJCZN.MIS.Helpers
{
    public static class HttpPostGetHelper
    {
        /// <summary>
        /// POST方法(httpWebRequest)
        /// post的cotentType填写:"application/x-www-form-urlencoded"
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body">body是要传递的参数,格式"roleId=1&uid=2"</param>
        /// <param name="contentType"></param>
        /// <returns></returns>
     public static string PostHttp(string url, string body, string contentType)
     {
         HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
 
         httpWebRequest.ContentType = contentType;
         httpWebRequest.Method = "POST";
         httpWebRequest.Timeout = 20000;
         
         // 国通数据特定要求
         httpWebRequest.Headers.Add("t", DateHelper.GetUnixTimestampFromDotnetTime(DateTime.Now));
 
         byte[] btBodys = Encoding.UTF8.GetBytes(body);
         httpWebRequest.ContentLength = btBodys.Length;
         httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);
 
         HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
         StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
         string responseContent = streamReader.ReadToEnd();
 
         httpWebResponse.Close();
         streamReader.Close();
         httpWebRequest.Abort();
         httpWebResponse.Close();
 
         return responseContent;
     }

    }
}
