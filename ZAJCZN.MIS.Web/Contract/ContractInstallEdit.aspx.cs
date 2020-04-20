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
    public partial class ContractInstallEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreContractInstall";
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
                CheckPowerWithButton("CoreContractInstall", btnSaveClose);
                //绑定销售人员
                BindSalerInfo();
                //获取合同信息
                GetOrderInfo();
                BindGrid();
                dpContractDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                //dbInstalldate.Text = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd");               
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
            qryList.Add(Expression.Eq("UserType", "安装人员"));

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
                lbldate.Text = objInfo.ContractDate;
                lblMdate.Text = objInfo.MeasureDate;
                txtProjectName.Text = objInfo.ProjectName;
                tbCustomer.Text = objInfo.CustomerName;
                tbPhone.Text = objInfo.ContactPhone;
                tbxRemark.Text = objInfo.Remark;
                if (objInfo.ContractState > 8)
                {
                    Alert.Show("订单已经安装完成，不能重新安装派工！", String.Empty, ActiveWindow.GetHideReference()); 
                }
            }
        }

        private void BindGrid()
        {
            string sqlHW = string.Format(@"select * from ContractCostInfo where ProduceState=2 and SendingState=2 and ContractID ={0} order by CostType", OrderID);
            DataSet dsHW = DbHelperSQL.Query(sqlHW);
            if (dsHW.Tables[0] != null)
            {
                gdHandWare.DataSource = dsHW.Tables[0];
                gdHandWare.DataBind();
            }
        }

        #endregion

        #region Events

        private void SaveItem()
        {
            ContractInfo objInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(OrderID);
            objInfo.InstalDate = DateTime.Parse(dpContractDate.Text).ToString("yyyy-MM-dd");
            objInfo.InstallPerson = int.Parse(ddlSaler.SelectedValue);
            objInfo.ContractState = 8;
            if (OrderID > 0)
            {
                Core.Container.Instance.Resolve<IServiceContractInfo>().Update(objInfo);
            }
            //更新明细
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractID", OrderID));
            qryList.Add(Expression.Eq("ProduceState", 2));
            qryList.Add(Expression.Eq("SendingState", 2));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", false);
            orderList[0] = orderli;

            IList<ContractCostInfo> list = Core.Container.Instance.Resolve<IServiceContractCostInfo>().GetAllByKeys(qryList, orderList);
            foreach (ContractCostInfo obj in list)
            {
                obj.InstalDate = DateTime.Parse(dpContractDate.Text).ToString("yyyy-MM-dd");
                obj.InstallPerson = int.Parse(ddlSaler.SelectedValue);
                obj.SendingState = 3;
                Core.Container.Instance.Resolve<IServiceContractCostInfo>().Update(obj);
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