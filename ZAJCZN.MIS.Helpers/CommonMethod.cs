using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ZAJCZN.MIS.Helpers
{
    public static class CommonMethod
    {
        /// <summary>
        /// 获取以时间开头的ID
        /// </summary>
        /// <returns></returns>
        public static string CreateGuidWithDateFirst()
        {
            return DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 根据guid生成字符串（去除-，变成32位）
        /// </summary>
        /// <returns></returns>
        public static string CreateGuidFor32Bit()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIPAdd()
        {
            HttpRequest request = HttpContext.Current.Request;
            string result = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result)) { result = request.ServerVariables["REMOTE_ADDR"]; }
            if (string.IsNullOrEmpty(result)) { result = request.UserHostAddress; }
            if (string.IsNullOrEmpty(result)) { result = "0.0.0.0"; }
            return result;
        }

        /// <summary>
        /// 获取销售订单前名
        /// </summary>
        /// <returns></returns>
        public static string GetSaleBillFrontName()
        {
            return ConfigHelper.GetConfigString("saleBillNameFront");
        }

        #region - 国通请求需要 -

        public static string GetSignatureForGuoTong(
            string url, string secret, string method
            , Dictionary<string, string> sParaTemp,DateTime curDate)
        {
            string curtts = DateHelper.GetUnixTimestampFromDotnetTime(curDate); //当前时间戳
            string token = CreateGuidFor32Bit(); // 32位随机字符串
            
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(url);
            string base64Url =Convert.ToBase64String(byteArray);

            string calcs = method.ToUpper() + "|" + base64Url + "|" + curtts + "|" + token + "|";
            foreach(var keyValue in sParaTemp)
            {
                calcs += "&" + keyValue.Key + '=' + keyValue.Value;
            }
            //return hmac(sha1, calcs, secret);
            return "";
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        #endregion


        /// <summary>
        /// 生成6位随机数
        /// </summary>
        /// <returns></returns>
        public static int CreateRandomNum_SixLen()
        {
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int iUp = 999999;
            int iDown = 0;

            return ran.Next(iDown, iUp);
        }

        /// <summary>
        /// 生成4位验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateRandomNum_FourLen()
        {
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int iUp = 9999;
            int iDown = 0;
            
            int returnInt= ran.Next(iDown, iUp);
            string returnStr = "0000" + returnInt;
            return returnStr.Substring(returnStr.Length - 4, 4);
        }
    }
}
