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
    public partial class ContractMeasureEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreContractMeasure";
            }
        }

        #endregion

        private int OrderID
        {
            get { return GetQueryIntValue("id"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
                CheckPowerWithButton("CoreContractMeasure", btnSaveClose);
                //绑定销售人员
                BindSalerInfo();
                //获取合同信息
                GetOrderInfo();
                dpContractDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
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
            qryList.Add(Expression.Eq("UserType", "测量人员"));

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

                if (objInfo.IsUrgent == 1)
                {
                    lblNo.Text = string.Format("{0}[加急]", objInfo.ContractNO);
                    lblNo.CssStyle = "color:red;";
                }
                lbldate.Text = objInfo.ContractDate;
                txtProjectName.Text = objInfo.ProjectName;
                tbCustomer.Text = objInfo.CustomerName;
                tbPhone.Text = objInfo.ContactPhone;
                tbxRemark.Text = objInfo.Remark;
                lblSend.Text = !string.IsNullOrEmpty(objInfo.PerSendDate) ? objInfo.PerSendDate : "未预约";
                lblInstall.Text = !string.IsNullOrEmpty(objInfo.PerInstalDate) ? objInfo.PerInstalDate : "未预约";
                ddlSaler.SelectedValue = objInfo.MeasurePerson.ToString();
            }
        }

        #endregion

        #region Events

        private void SaveItem()
        {
            ContractInfo objInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(OrderID);
            objInfo.MeasureDate = DateTime.Parse(dpContractDate.Text).ToString("yyyy-MM-dd");
            objInfo.MeasurePerson = int.Parse(ddlSaler.SelectedValue);
            objInfo.ContractState = 2;
            if (OrderID > 0)
            {
                Core.Container.Instance.Resolve<IServiceContractInfo>().Update(objInfo);
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