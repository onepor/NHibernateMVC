using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractDoorPrint : PageBase
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
                    strInfo = string.Format("{0}|{1}", contractInfo.CustomerName, contractInfo.ProjectName);
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
                TotalAmount = 0;
                //获取合同信息
                ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(OrderID);
                //获取订单明细
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("ContractInfo.ID", OrderID));
                Order[] orderList = new Order[1];
                Order orderli = new Order("ID", true);
                orderList[0] = orderli;
                IList<ContractDoorInfo> list = Core.Container.Instance.Resolve<IServiceContractDoorInfo>().GetAllByKeys(qryList, orderList);

                List<ContractDoorInfo> listNew = new List<ContractDoorInfo>();
                listNew.AddRange(list);
                int recordIndex1 = 1;
                foreach (ContractDoorInfo row in listNew)
                {
                    row.ID = recordIndex1;
                    row.GoodsAmount = row.GoodsAmount + row.PassAmount + row.OtherAmount;
                    row.InstallCost = row.InstallCost + row.HardWareAmount;
                    recordIndex1++;
                    TotalAmount += row.OrderAmount;
                }
                rpInfoList.DataSource = listNew;
                rpInfoList.DataBind();
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
                //获取合同信息
                ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(OrderID);
                //获取订单明细
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("ContractInfo.ID", OrderID));
                Order[] orderList = new Order[1];
                Order orderli = new Order("ID", true);
                orderList[0] = orderli;
                IList<ContractDoorInfo> list = Core.Container.Instance.Resolve<IServiceContractDoorInfo>().GetAllByKeys(qryList, orderList);

                List<ContractDoorInfo> listNew = new List<ContractDoorInfo>();
                listNew.AddRange(list);
                int recordIndex1 = 1;
                foreach (ContractDoorInfo row in listNew)
                {
                    row.ID = recordIndex1;
                    row.GoodsAmount = row.GoodsAmount + row.PassAmount + row.OtherAmount;
                    row.InstallCost = row.InstallCost + row.HardWareAmount;
                    recordIndex1++;
                }
                rpInfoList.DataSource = listNew;
                rpInfoList.DataBind();

                ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script> javascript:window.print();</script>");
            }
            catch (Exception ex)
            {
                // LogManager.Error(ex);
                // Msg.Show("打印失败！");
            }
        }
    }
}
