using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Helpers;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractPayPrint : PageBase
    {
        #region request param

        //订单ID（传入参数）
        private int OrderID
        {
            get { return GetQueryIntValue("printNO"); }
        }

        #endregion request param

        public string StrHtml
        {
            get
            {
                return ViewState["strHtml"] != null ? ViewState["strHtml"].ToString() : "";
            }
            set
            {
                ViewState["strHtml"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (OrderID < 0)
                {
                    Alert.Show("请选择要打印的单据!", String.Empty, ActiveWindow.GetHideReference());
                }
            }
        }

        public string GetOrderInfo(string type)
        {
            string strInfo = "";
            switch (type)
            {
                case "1":
                    strInfo = ConfigHelper.Address;
                    break;
                case "2":
                    strInfo = ConfigHelper.StoreName;
                    break;
                case "3":
                    strInfo = ConfigHelper.ContractPhone;
                    break;
            }

            return strInfo;
        }

        public string GetPayInfo(string type)
        {
            string strInfo = "";
            ContractPayInfo payInfo = Core.Container.Instance.Resolve<IServiceContractPayInfo>().GetEntity(OrderID);
            ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(payInfo.ContractID);
            switch (type)
            {
                case "1":
                    strInfo = string.Format("{0}|{1}", contractInfo.CustomerName, contractInfo.ProjectName);
                    break;
                case "2":
                    strInfo = string.Format("日期:{0}", payInfo.ApplyDate);
                    break;
                case "3":
                    strInfo = string.Format("经手人:{0}", payInfo.Operator);
                    break;
                case "4":
                    strInfo = payInfo.PayWay;
                    break;
                case "5":
                    strInfo = payInfo.PayMoney.ToString();
                    break;
                case "6":
                    strInfo = payInfo.Remark;
                    break;
            }

            return strInfo;
        }

        public void btnPrinting(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script> javascript:window.print();</script>");
            }
            catch (Exception ex)
            {
                Alert.Show("打印错误！");
            }
        }
    }
}
