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
    public partial class ContractCabinetPrint : PageBase
    {
        #region request param

        //订单ID（传入参数）
        private int OrderID
        {
            get { return GetQueryIntValue("printNO"); }
        }

        #endregion request param

        public decimal TotalAmount
        {
            get
            {
                return ViewState["amount"] != null ? decimal.Parse(ViewState["amount"].ToString()) : 0;
            }
            set
            {
                ViewState["amount"] = value;
            }
        }
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
                BindList();
            }
        }

        public string GetOrderInfo(string type)
        {
            string strInfo = "";
            //获取合同信息
            ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(OrderID);
            switch (type)
            {
                case "1":
                    strInfo = contractInfo.ContractNO;
                    break;
                case "2":
                    strInfo = ConfigHelper.StoreName;
                    break;
                case "3":
                    strInfo = ConfigHelper.ContractPhone;
                    break;
                case "4":
                    strInfo = contractInfo.ProjectName;
                    break;
                case "5":
                    strInfo = contractInfo.ContactPhone;
                    break;
                case "6":
                    strInfo = string.Format("安装售后：{0} 量尺设计：{1} 投诉电话：{2} 地址：{3}"
                            , ConfigHelper.InstallPhone
                            , ConfigHelper.DesignPhone
                            , ConfigHelper.ComplaintPhone
                            , ConfigHelper.Address);
                    break;
                case "7":
                    strInfo = TotalAmount.ToString();
                    break;
            }

            return strInfo;
        }

        protected void BindList()
        {
            try
            {
                //获取合同信息
                ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(OrderID);
                //获取订单明细
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("ContractInfo.ID", OrderID));
                Order[] orderList = new Order[1];
                Order orderli = new Order("ID", true);
                orderList[0] = orderli;
                IList<ContractCabinetInfo> list = Core.Container.Instance.Resolve<IServiceContractCabinetInfo>().GetAllByKeys(qryList, orderList);

                //获取位置信息 
                string sql = string.Format(@"select GoodsType from ContractCabinetInfo where ContractID ={0} group by GoodsType ", OrderID);
                DataSet ds = DbHelperSQL.Query(sql);

                StringBuilder sb = new StringBuilder();

                #region - 拼凑导出的数据行 -

                int recordIndex1 = 1;
                int index = 1;
                decimal singleAmount = 0;
                TotalAmount = 0;
                //导出商品明细
                if (ds.Tables[0] != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        //获取位置
                        string goodLocation = row["GoodsType"].ToString();
                        //获取位置下面的产品信息
                        List<ContractCabinetInfo> listInfo = list.Where(obj => obj.GoodsType == goodLocation).OrderBy(obj => obj.ID).ToList();
                        int rowspan = listInfo.Count;
                        index = 1;
                        singleAmount = 0;
                        foreach (ContractCabinetInfo obj in listInfo)
                        {
                            sb.Append("<tr style=\"height: 30px;\">");
                            sb.AppendFormat("<td style=\"width: 80px;\">{0}</td>", recordIndex1);
                            if (index == 1)
                            {
                                sb.AppendFormat("<td  style=\"width: 150px;\" rowspan=\"{1}\">{0}</td>", goodLocation, rowspan);
                            }
                            sb.AppendFormat("<td style=\"width: 150px;\">{0}</td>", obj.GoodsName);
                            sb.AppendFormat("<td style=\"width: 100px;\">{0}</td>", obj.GWide / 1000);
                            sb.AppendFormat("<td style=\"width: 100px;\">{0}</td>", obj.GHeight / 1000);
                            sb.AppendFormat("<td style=\"width: 100px;\">{0}</td>", obj.OrderNumber);
                            sb.AppendFormat("<td style=\"width: 100px;\">{0}</td>", obj.GArea);
                            sb.AppendFormat("<td style=\"width: 100px;\">{0}</td>", obj.GPrice);
                            sb.AppendFormat("<td style=\"width: 160px;\">{0}</td>", obj.OrderAmount);
                            sb.AppendFormat("<td style=\"width: 400px;text-align: left;\">{0}</td>", obj.Remark);
                            sb.Append("</tr>");
                            singleAmount += obj.OrderAmount;
                            TotalAmount += obj.OrderAmount;
                            recordIndex1++;
                            index++;
                        }
                        //小计
                        //sb.Append("<tr style=\"height: 30px;\">");
                        //sb.Append("<th colspan=\"8\" style=\"text-align: right;\">小计：</th>");
                        //sb.AppendFormat("<td>{0}</td>", singleAmount);
                        //sb.Append("<td></td>");
                        //sb.Append("</tr>");
                    }
                }

                //导出五金明细 
                qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("ContractID", OrderID));
                qryList.Add(Expression.Eq("IsFree", 1));
                qryList.Add(Expression.Eq("OrderType", 2));
                orderList = new Order[1];
                orderli = new Order("ID", true);
                orderList[0] = orderli;
                IList<ContractHandWareDetail> listHWDetail = Core.Container.Instance.Resolve<IServiceContractHandWareDetail>().GetAllByKeys(qryList, orderList);
                index = 1;
                singleAmount = 0;
                foreach (ContractHandWareDetail obj in listHWDetail)
                {
                    sb.Append("<tr style=\"height: 30px;\">");
                    sb.AppendFormat("<td style=\"width: 80px;\">{0}</td>", recordIndex1);
                    if (index == 1)
                    {
                        sb.AppendFormat("<td rowspan=\"{0}\">五金及配件</td>", listHWDetail.Count);
                    }
                    sb.AppendFormat("<td style=\"width: 300px;\">{0}</td>", obj.GoodsName);
                    sb.Append("<td style=\"width: 100px;\">0</td>");
                    sb.Append("<td style=\"width: 100px;\">0</td>");
                    sb.AppendFormat("<td style=\"width: 100px;\">{0}</td>", obj.GoodsNumber);
                    sb.AppendFormat("<td style=\"width: 100px;\">0</td>");
                    sb.AppendFormat("<td style=\"width: 100px;\">{0}</td>", obj.GoodsUnitPrice);
                    sb.AppendFormat("<td style=\"width: 160px;\">{0}</td>", obj.GoodAmount);
                    sb.AppendFormat("<td style=\"width: 400px;text-align: left;\">{0}</td>", "");
                    sb.Append("</tr>");
                    recordIndex1++;
                    index++;
                    singleAmount += obj.GoodAmount;
                    TotalAmount += obj.GoodAmount;
                }
                //五金明细小计
                //sb.Append("<tr style=\"height: 30px;\">");
                //sb.Append("<th colspan=\"8\" style=\"text-align: right;\">小计：</th>");
                //sb.AppendFormat("<td>{0}</td>", singleAmount);
                //sb.Append("<td></td>");
                //sb.Append("</tr>"); 
                #endregion 
                StrHtml = sb.ToString();

            }
            catch (Exception ex)
            {
                Alert.Show("打印数据获取错误！");
            }
        }

        public void btnPrinting(object sender, EventArgs e)
        {
            try
            {
                BindList();
                ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script> javascript:window.print();</script>");
            }
            catch (Exception ex)
            {
                Alert.Show("打印错误！");
            }
        }
    }
}
