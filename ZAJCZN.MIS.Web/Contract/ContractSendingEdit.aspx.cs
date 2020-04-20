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
    public partial class ContractSendingEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreContractSending";
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
                CheckPowerWithButton("CoreContractSending", btnSaveClose);
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
            qryList.Add(Expression.Eq("UserType", "送货人员"));

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
            }
        }

        private void BindGrid()
        {
            string sqlHW = string.Format(@"select * from ContractCostInfo where ProduceState=2 and (SendingState=0 or SendingState=1) and ContractID ={0} order by CostType", OrderID);
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
            objInfo.SendDate = DateTime.Parse(dpContractDate.Text).ToString("yyyy-MM-dd");
            objInfo.SendPerson = int.Parse(ddlSaler.SelectedValue);
            //如果订单状态是生产完成，更新订单状态为送货中
            //if (objInfo.ContractState == 4)
            //{
            objInfo.ContractState = 6;
            //}

            //更新明细
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractID", OrderID));
            qryList.Add(Expression.Eq("ProduceState", 2));
            qryList.Add(Expression.Eq("SendingState", 0) || Expression.Eq("SendingState", 1));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", false);
            orderList[0] = orderli;

            IList<ContractCostInfo> list = Core.Container.Instance.Resolve<IServiceContractCostInfo>().GetAllByKeys(qryList, orderList);
            foreach (ContractCostInfo obj in list)
            {
                obj.SendDate = DateTime.Parse(dpContractDate.Text).ToString("yyyy-MM-dd");
                obj.SendPerson = int.Parse(ddlSaler.SelectedValue);
                obj.SendingState = ckHandWare.Checked ? 2 : 1;
                Core.Container.Instance.Resolve<IServiceContractCostInfo>().Update(obj);
            }
            if (ckHandWare.Checked)
            {
                objInfo.ContractState = 7;
            }
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