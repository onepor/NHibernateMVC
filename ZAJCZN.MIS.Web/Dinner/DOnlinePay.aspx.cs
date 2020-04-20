using FineUIPro;
using ZAJCZN.MIS.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class DOnlinePay : PageBase
    {
        //定时器次数控制
        public static int TimerCount = 1;

        //支付金额
        protected string moneys
        {
            get { return (decimal.Parse(GetQueryValue("money")) * 100).ToString("#"); }
        }

        //请求类型 010微信 020 支付宝
        protected string pay_type
        {
            get { return GetQueryValue("pay_type"); }
        }

        //订单号
        protected string _terminal_trace
        {
            get { return GetQueryValue("terminal_trace"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (pay_type.Equals("010"))
                    tbxBarcode.Label = "微信条码";
                else
                    tbxBarcode.Label = "支付宝条码";
                TimerCount = 1;
            }
        }

        /// <summary>
        /// 条码确认---支付
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            //获取用户授权码
            string number = tbxBarcode.Text.Trim();

            //请求支付
            string returnBackText = GetHTTPInfoFind(number, pay_type, moneys, _terminal_trace);

            //接口返回Json对象
            JObject JObjectBack = (JObject)JsonConvert.DeserializeObject(returnBackText);

            //利楚唯一订单号
            hfdout_trade_no.Text = JObjectBack["out_trade_no"].ToString();

            //成功
            if (JObjectBack["return_code"].ToString().Equals("01") && JObjectBack["result_code"].ToString().Equals("01"))
            {
                tm_OnlinePayInfo entity = new tm_OnlinePayInfo();
                entity.pay_type = JObjectBack["pay_type"].ToString();
                entity.merchant_name = JObjectBack["merchant_name"].ToString();
                entity.merchant_no = JObjectBack["merchant_no"].ToString();
                entity.terminal_id = JObjectBack["terminal_id"].ToString();
                entity.terminal_trace= JObjectBack["terminal_trace"].ToString();
                entity.terminal_time = JObjectBack["terminal_time"].ToString();
                entity.total_fee = JObjectBack["total_fee"].ToString();
                entity.end_time =JObjectBack["end_time"].ToString();
                entity.out_trade_no = JObjectBack["out_trade_no"].ToString();
                entity.channel_trade_no = JObjectBack["channel_trade_no"].ToString();
                entity.channel_order_no = JObjectBack["channel_order_no"].ToString();
                entity.user_id = JObjectBack["user_id"].ToString();

                //保存新增付款记录
                Core.Container.Instance.Resolve<IServiceOnlinePayInfo>().Create(entity);

                //返回支付成功的时间到父页面,0.成功1.失败
                PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(JObjectBack["end_time"].ToString(), "0") + ActiveWindow.GetHidePostBackReference());
            }

            //失败
            if (JObjectBack["return_code"].ToString().Equals("02") || (JObjectBack["return_code"].ToString().Equals("01") && JObjectBack["result_code"].ToString().Equals("02")))
            {
                //Alert.ShowInTop(JObjectBack["return_msg"].ToString(), "支付失败", MessageBoxIcon.Error);
                //返回支付成功的时间到父页面,0.成功1.失败
                PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(JObjectBack["return_msg"].ToString(), "1") + ActiveWindow.GetHidePostBackReference());
            }

            //支付中
            if (JObjectBack["return_code"].ToString().Equals("01") && JObjectBack["result_code"].ToString().Equals("03"))
            {
                //订单支付中将定时器启动隔时间回发是否支付成功
                Timer1.Enabled = true;
                btnAccept.Enabled = false;
                btnAccept.Text = "等待用户完成支付...";
            }
        }

        /// <summary>
        /// 定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            if (TimerCount > 15)
            {
                Alert.ShowInTop("本次支付失败", "交易状态", MessageBoxIcon.Warning);
                //返回支付成功的时间到父页面,0.成功1.失败
                PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference("支付逾期", "1") + ActiveWindow.GetHidePostBackReference());
            }

            //利楚唯一订单号
            string OrderNo = hfdout_trade_no.Text;

            //调用查询接口查询状态
            string SearchBack = GetHTTPInfoSearch(OrderNo, pay_type, _terminal_trace);

            //接口返回Json对象
            JObject JObjectBack = (JObject)JsonConvert.DeserializeObject(SearchBack);

            //成功
            if (JObjectBack["return_code"].ToString().Equals("01") && JObjectBack["result_code"].ToString().Equals("01"))
            {
                //实例化对象
                tm_OnlinePayInfo entity = new tm_OnlinePayInfo();
                entity.pay_type = JObjectBack["pay_type"].ToString();
                entity.merchant_name = JObjectBack["merchant_name"].ToString();
                entity.merchant_no = JObjectBack["merchant_no"].ToString();
                entity.terminal_id = JObjectBack["terminal_id"].ToString();
                entity.terminal_trace = JObjectBack["terminal_trace"].ToString();
                entity.terminal_time = JObjectBack["terminal_time"].ToString();
                entity.total_fee = JObjectBack["total_fee"].ToString();
                entity.end_time = JObjectBack["end_time"].ToString();
                entity.out_trade_no = JObjectBack["out_trade_no"].ToString();
                entity.channel_trade_no = JObjectBack["channel_trade_no"].ToString();
                entity.channel_order_no = JObjectBack["channel_order_no"].ToString();
                entity.user_id = JObjectBack["user_id"].ToString();

                //保存新增付款记录
                Core.Container.Instance.Resolve<IServiceOnlinePayInfo>().Create(entity);

                PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(JObjectBack["end_time"].ToString(), "0") + ActiveWindow.GetHidePostBackReference());
            }

            //失败
            if (JObjectBack["return_code"].ToString().Equals("02") || (JObjectBack["return_code"].ToString().Equals("01") && JObjectBack["result_code"].ToString().Equals("02")))
            {
                //Alert.ShowInTop(JObjectBack["return_msg"].ToString(), "交易状态", MessageBoxIcon.Information);
                PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(JObjectBack["return_msg"].ToString(), "1") + ActiveWindow.GetHidePostBackReference());
            }

            //支付中
            if (JObjectBack["return_code"].ToString().Equals("01") && JObjectBack["result_code"].ToString().Equals("03"))
            {
                TimerCount++;
            }
        }

        /// <summary>
        /// 支付接口
        /// </summary>
        /// <param name="urlPath">接口地址</param>
        /// <param name="millisecond"></param>
        /// <returns></returns>
        public static string GetHTTPInfoFind(string number, string rblPayway, string moneys,string _terminal_trace)
        {
            WebRequest myHttpWebRequest = WebRequest.Create("https://pay.lcsw.cn/lcsw/pay/100/barcodepay");
            myHttpWebRequest.Method = "POST";
            PostDataClass entity = new PostDataClass();
            entity.pay_ver = ConfigurationManager.AppSettings["pay_ver"];
            entity.pay_type = rblPayway;
            entity.service_id = "010";
            entity.merchant_no = ConfigurationManager.AppSettings["merchant_no"];
            entity.terminal_id = ConfigurationManager.AppSettings["terminal_id"];
            entity.terminal_trace = _terminal_trace;
            entity.terminal_time = DateTime.Now.ToString("yyyyMMddHHmmss");
            entity.auth_no = number;
            entity.total_fee = moneys;

            string sign = "pay_ver=" + ConfigurationManager.AppSettings["pay_ver"] + "&pay_type=" + rblPayway + "&service_id=010&merchant_no=" + ConfigurationManager.AppSettings["merchant_no"] + "&terminal_id=" + ConfigurationManager.AppSettings["terminal_id"] + "&terminal_trace=" + _terminal_trace + "&terminal_time=" + DateTime.Now.ToString("yyyyMMddHHmmss") + "&auth_no=" + number + "&total_fee=" + moneys + "&access_token=" + ConfigurationManager.AppSettings["access_token"];
            string key_sign = EncryptUtils.MD5Encrypt(sign);
            entity.key_sign = key_sign;

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byte1 = encoding.GetBytes(JsonConvert.SerializeObject(entity));
            myHttpWebRequest.ContentType = "application/json";
            myHttpWebRequest.ContentLength = byte1.Length;
            Stream newStream = myHttpWebRequest.GetRequestStream();
            newStream.Write(byte1, 0, byte1.Length);
            newStream.Close();

            //发送成功后接收返回的XML信息
            HttpWebResponse response = (HttpWebResponse)myHttpWebRequest.GetResponse();
            string lcHtml = string.Empty;
            Encoding enc = Encoding.GetEncoding("UTF-8");
            Stream stream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream, enc);
            lcHtml = streamReader.ReadToEnd();
            return lcHtml;
        }

        /// <summary>
        /// 查询接口
        /// </summary>
        /// <param name="urlPath">接口地址</param>
        /// <param name="millisecond"></param>
        /// <returns></returns>
        public static string GetHTTPInfoSearch(string OrderNo, string rblPayway,string _terminal_trace)
        {
            WebRequest myHttpWebRequest = WebRequest.Create("https://pay.lcsw.cn/lcsw/pay/100/query");
            myHttpWebRequest.Method = "POST";
            PostSearchDataClass entity = new PostSearchDataClass();
            entity.pay_ver = ConfigurationManager.AppSettings["pay_ver"];
            entity.pay_type = rblPayway;
            entity.service_id = "020";
            entity.merchant_no = ConfigurationManager.AppSettings["merchant_no"];
            entity.terminal_id = ConfigurationManager.AppSettings["terminal_id"];
            entity.terminal_trace = _terminal_trace;
            entity.terminal_time = DateTime.Now.ToString("yyyyMMddHHmmss");
            entity.out_trade_no = OrderNo;

            string sign = "pay_ver=" + ConfigurationManager.AppSettings["pay_ver"] + "&pay_type=" + rblPayway + "&service_id=020&merchant_no=" + ConfigurationManager.AppSettings["merchant_no"] + "&terminal_id=" + ConfigurationManager.AppSettings["terminal_id"] + "&terminal_trace=" + _terminal_trace + "&terminal_time=" + DateTime.Now.ToString("yyyyMMddHHmmss") + "&out_trade_no=" + OrderNo + "&access_token=" + ConfigurationManager.AppSettings["access_token"];
            string key_sign = EncryptUtils.MD5Encrypt(sign);
            entity.key_sign = key_sign;

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byte1 = encoding.GetBytes(JsonConvert.SerializeObject(entity));
            myHttpWebRequest.ContentType = "application/json";
            myHttpWebRequest.ContentLength = byte1.Length;
            Stream newStream = myHttpWebRequest.GetRequestStream();
            newStream.Write(byte1, 0, byte1.Length);
            newStream.Close();

            //发送成功后接收返回的XML信息
            HttpWebResponse response = (HttpWebResponse)myHttpWebRequest.GetResponse();
            string lcHtml = string.Empty;
            Encoding enc = Encoding.GetEncoding("UTF-8");
            Stream stream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream, enc);
            lcHtml = streamReader.ReadToEnd();
            return lcHtml;
        }
    }

    /// <summary>
    /// 支付接口实体
    /// </summary>
    public class PostDataClass
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string pay_ver { get; set; }

        /// <summary>
        /// 请求类型
        /// </summary>
        public string pay_type { get; set; }

        /// <summary>
        /// 接口类型
        /// </summary>
        public string service_id { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string merchant_no { get; set; }

        /// <summary>
        /// 终端号
        /// </summary>
        public string terminal_id { get; set; }

        /// <summary>
        /// 终端流水号
        /// </summary>
        public string terminal_trace { get; set; }

        /// <summary>
        /// 终端交易时间
        /// </summary>
        public string terminal_time { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        public string auth_no { get; set; }

        /// <summary>
        /// 终端交易时间
        /// </summary>
        public string total_fee { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string key_sign { get; set; }
    }

    /// <summary>
    /// 查询接口对象
    /// </summary>
    public class PostSearchDataClass
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string pay_ver { get; set; }

        /// <summary>
        /// 请求类型
        /// </summary>
        public string pay_type { get; set; }

        /// <summary>
        /// 接口类型
        /// </summary>
        public string service_id { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string merchant_no { get; set; }

        /// <summary>
        /// 终端号
        /// </summary>
        public string terminal_id { get; set; }

        /// <summary>
        /// 终端查询流水号
        /// </summary>
        public string terminal_trace { get; set; }

        /// <summary>
        /// 终端查询时间
        /// </summary>
        public string terminal_time { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string key_sign { get; set; }
    }
}