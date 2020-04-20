using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web;
using System.IO;
namespace ZAJCZN.MIS.Comm
{
  public static  class StringHelper
    {
        /// <summary>
        /// MD532位加密
        /// </summary>
        /// <param name="source">要加密的明文</param>
        /// <returns>加密后的字符串密文</returns>
        public static string GetMd5HashStr(string source)
        {
            string pwd = string.Empty;
            //1.实例化MD5对象
            MD5 md5 = MD5.Create();
            //2.加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(source));
            //3.通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }
        public static DateTime GetDate(DateTime dt)
        {
            return Convert.ToDateTime(dt.ToString("yyyy-MM-dd"));
        }


        /// <summary>
        /// 可逆加密
        /// </summary>
        /// <param name="sourse"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string Encrypt(string sourse, string strKey = "#%@(&!&%")
        {
            try
            {
                byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                byte[] byKey = Encoding.UTF8.GetBytes(strKey.Substring(0, 8));
                byte[] inputByteArray = Encoding.UTF8.GetBytes(sourse);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }
        /// <summary>
        /// 可逆解密
        /// </summary>
        /// <param name="sourse"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string Decrypt(string sourse, string strKey = "#%@(&!&%")
        {
            try
            {
                byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                byte[] byKey = Encoding.UTF8.GetBytes(strKey.Substring(0, 8));
                byte[] inputByteArray = Convert.FromBase64String(sourse);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 把字符串转 按照, 分割 换为数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] StrToIntArr(string str)
        {
            return str.Split(new Char[] { ',' });
        }
        public static int[] StrToIntArr(string source, char splitChar)
        {
            List<int> ret = new List<int>();
            string[] arr = source.Split(splitChar);
            foreach (var item in arr)
            {
                try
                {
                    ret.Add(int.Parse(item));
                }
                catch
                {
                }

            }
            return ret.ToArray();
        }
    }
}
