using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Text;
using System.Data;
using ZAJCZN.MIS.Helpers;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractCabinetAdd : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreContractCabinet";
            }
        }

        #endregion

        #region Page_Load

        private int InfoID
        {
            get { return GetQueryIntValue("id"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
                CheckPowerWithButton("CoreContractCabinet", btnSaveClose);

                //绑定厂家
                BindSupplier();
            }
        }

        /// <summary>
        /// 绑定厂家
        /// </summary>
        private void BindSupplier()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("IsUsed", "1"));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<SupplierInfo> list = Core.Container.Instance.Resolve<IServiceSupplierInfo>().GetAllByKeys(qryList, orderList);

            ddlSupply.DataSource = list;
            ddlSupply.DataBind();
            ddlSupply.SelectedIndex = 0;
        }

        #endregion

        #region Events

        private void SaveItem()
        {
            ContractCabinetInfo contractCabinetInfo = new ContractCabinetInfo();
            if (InfoID > 0)
            {
                contractCabinetInfo.ContractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(InfoID);
            }
            //获取新增柜体信息
            contractCabinetInfo.GoodsType = txtAddress.Text;
            contractCabinetInfo.GoodsName = txtCostName.Text;
            contractCabinetInfo.GColorOne = tbColor1.Text;
            contractCabinetInfo.GColorTwo = tbColor2.Text;
            contractCabinetInfo.GHeight = decimal.Parse(nbHeight.Text);
            contractCabinetInfo.GWide = decimal.Parse(nbWide.Text);
            contractCabinetInfo.Remark = txtRemark.Text;
            contractCabinetInfo.OrderNumber = decimal.Parse(nbCount.Text);
            contractCabinetInfo.GPrice = decimal.Parse(nbPrice.Text);
            contractCabinetInfo.SupplyID = int.Parse(ddlSupply.SelectedValue);
            contractCabinetInfo.GArea = (contractCabinetInfo.GHeight / 1000) * (contractCabinetInfo.GWide / 1000) * contractCabinetInfo.OrderNumber;
            contractCabinetInfo.OrderAmount = contractCabinetInfo.GArea * contractCabinetInfo.GPrice;
            contractCabinetInfo.OperatorName = User.Identity.Name;
            //创建柜子设计信息
            Core.Container.Instance.Resolve<IServiceContractCabinetInfo>().Create(contractCabinetInfo);
            //更新合同柜子总金额及合同总金额信息
            UpdateTotalAmount();
            //保存厂家成本信息
            CreateCostInfo();
            //清除多余厂家信息
            CheckCostInfo();
        }

        /// <summary>
        /// 更新合同柜子总金额及合同总金额信息
        /// </summary>
        public void UpdateTotalAmount()
        {
            ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(InfoID);
            //更新订单总金额
            decimal payNumber = 0;
            string sql = string.Format(@"select isnull(sum(OrderAmount),0) as OrderAmount from ContractCabinetInfo where ContractID ={0}  ", InfoID);
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0] != null)
            {
                payNumber = decimal.Parse(ds.Tables[0].Rows[0]["OrderAmount"].ToString());
            }
            //获取柜子收费五金总金额
            decimal hwNumber = 0;
            string sqlHW = string.Format(@"select isnull(sum(GoodAmount),0) as GoodAmount from ContractHandWareDetail where ContractID ={0} and IsFree=1 and OrderType=2 ", InfoID);
            DataSet dsHW = DbHelperSQL.Query(sqlHW);
            if (dsHW.Tables[0] != null)
            {
                hwNumber = decimal.Parse(dsHW.Tables[0].Rows[0]["GoodAmount"].ToString());
            }
            contractInfo.CabinetAmount = payNumber + hwNumber;
            contractInfo.TotalAmount = contractInfo.CabinetAmount + contractInfo.DoorAmount;
            Core.Container.Instance.Resolve<IServiceContractInfo>().Update(contractInfo);
        }

        /// <summary>
        /// 保存厂家成本信息
        /// </summary>
        public void CreateCostInfo()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractID", InfoID));
            qryList.Add(Expression.Eq("SuppilerID", int.Parse(ddlSupply.SelectedValue)));
            qryList.Add(Expression.Eq("CostType", 2));
            ContractCostInfo objInfo = Core.Container.Instance.Resolve<IServiceContractCostInfo>().GetEntityByFields(qryList);
            if (objInfo == null)
            {
                objInfo = new ContractCostInfo();
                objInfo.ContractID = InfoID;
                objInfo.CostAmount = 0;
                objInfo.CostType = 2;
                objInfo.SuppilerID = int.Parse(ddlSupply.SelectedValue);
                objInfo.SuppilerName = ddlSupply.SelectedText;
                objInfo.PayAmount = 0;
                objInfo.SuppilerState = 1;
                objInfo.ProduceState = 1;
                objInfo.SendingState = 0;
                objInfo.ProduceRemark = "下单";
                Core.Container.Instance.Resolve<IServiceContractCostInfo>().Create(objInfo);
            }
        }

        /// <summary>
        /// 清除多余厂家信息
        /// </summary>
        public void CheckCostInfo()
        {
            string sql = string.Format(@"delete from ContractCostInfo where ContractID ={0} and CostType=2 and SuppilerID not in
(select SupplyID from ContractCabinetInfo where ContractID ={0})", InfoID);
            DbHelperSQL.ExecuteSql(sql);
        }


        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();
            txtAddress.Text = "";
            txtCostName.Text = "";
            tbColor1.Text = "";
            tbColor2.Text = "";
            txtRemark.Text = "";
            nbHeight.Text = "0";
            nbWide.Text = "0";
            nbCount.Text = "0";
            nbPrice.Text = "0";
            ddlSupply.SelectedIndex = 0;
            //PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());           
        }

        #endregion
    }
}