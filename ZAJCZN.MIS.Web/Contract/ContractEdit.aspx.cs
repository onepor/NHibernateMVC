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

namespace ZAJCZN.MIS.Web
{
    public partial class ContractEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreContractEdit";
            }
        }

        #endregion

        private int OrderID
        {
            get { return GetQueryIntValue("id"); }
        }
        private int ActionType
        {
            get { return GetQueryIntValue("type"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
                CheckPowerWithButton("CoreContractEdit", btnSaveClose);
                //绑定销售人员
                BindSalerInfo();
                //获取合同信息
                GetOrderInfo();
                if (ActionType == 1)
                {
                    FormRowState.Hidden = true;
                }
            }
        }

        #region 绑定数据

        /// <summary>
        /// 绑定销售人员
        /// </summary>
        public void BindSalerInfo()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("IsUsed", "1"));
            qryList.Add(Expression.Eq("UserType", "销售人员"));

            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", false);
            orderList[0] = orderli;

            IList<EmployeeInfo> list = Core.Container.Instance.Resolve<IServiceEmployeeInfo>().GetAllByKeys(qryList, orderList);

            ddlSaler.DataSource = list;
            ddlSaler.DataBind();
            ddlSaler.SelectedIndex = 0;
        }

        /// <summary>
        /// 获取合同信息
        /// </summary>
        public void GetOrderInfo()
        {
            if (OrderID > 0)
            {
                ContractInfo objInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(OrderID);

                if (objInfo == null)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                    return;
                }
                //合同基本信息
                lblNo.Text = objInfo.ContractNO;
                dpContractDate.Text = objInfo.ContractDate;
                txtProjectName.Text = objInfo.ProjectName;
                tbCustomer.Text = objInfo.CustomerName;
                tbPhone.Text = objInfo.ContactPhone;
                tbxRemark.Text = objInfo.Remark;
                dpSendDate.Text = objInfo.PerSendDate;
                dpInstalDate.Text = objInfo.PerInstalDate;
                cbIsUrgent.Checked = objInfo.IsUrgent == 1;
                ddlSaler.SelectedValue = objInfo.SalePerson.ToString();
                if (ActionType == 2)
                {
                    ddlState.SelectedValue = objInfo.ContractState.ToString();
                }
            }
            else
            {
                //初始化页面数据
                lblNo.Text = string.Format("JXL{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                dpContractDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        #endregion

        #region Events

        private void SaveItem()
        {
            ContractInfo objInfo = new ContractInfo();
            if (OrderID > 0)
            {
                objInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(OrderID);
            }
            objInfo.ContactPhone = tbPhone.Text;
            objInfo.ContractDate = DateTime.Parse(dpContractDate.Text).ToString("yyyy-MM-dd");
            objInfo.CustomerName = tbCustomer.Text;
            objInfo.ProjectName = txtProjectName.Text;
            objInfo.Remark = tbxRemark.Text;
            objInfo.PerSendDate = dpSendDate.Text;
            objInfo.PerInstalDate = dpInstalDate.Text;
            objInfo.IsUrgent = cbIsUrgent.Checked ? 1 : 0;
            objInfo.SalePerson = int.Parse(ddlSaler.SelectedValue);
            if (ActionType == 2)
            {
                if (!string.IsNullOrEmpty(ddlState.SelectedValue))
                {
                    objInfo.ContractState = int.Parse(ddlState.SelectedValue);
                }
            }

            if (OrderID > 0)
            {
                Core.Container.Instance.Resolve<IServiceContractInfo>().Update(objInfo);
            }
            else
            {
                objInfo.ContractNO = lblNo.Text;
                objInfo.ContractState = 0;
                objInfo.CabinetCost = 0;
                objInfo.DoorCost = 0;
                objInfo.OtherCost = 0;
                objInfo.HandWareCost = 0;
                objInfo.AfterSaleCost = 0;
                objInfo.ProfitMoney = 0;
                objInfo.PayCostMoney = 0;
                objInfo.ReturnMoney = 0;
                objInfo.WaitingPaymentMoney = 0;
                objInfo.SendCost = 0;
                objInfo.DoorAmount = 0;
                objInfo.CabinetAmount = 0;
                objInfo.TotalAmount = 0;
                objInfo.Operator = User.Identity.Name;
                objInfo.DiscountAmount = 0;
                Core.Container.Instance.Resolve<IServiceContractInfo>().Create(objInfo);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        #endregion
    }
}