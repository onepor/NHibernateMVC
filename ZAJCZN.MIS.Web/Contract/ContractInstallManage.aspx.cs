using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Helpers;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractInstallManage : PageBase
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

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            //Grid1.PageSize = ConfigHelper.PageSize;
            //ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
            //绑定数据
            BindGrid();
        }

        private void BindGrid()
        {
            StringBuilder sqlHW = new StringBuilder();
            sqlHW.Append(@" select * from ContractInfo where (ContractState>6 OR ID in(
                           select  ContractID from ContractCostInfo where ProduceState=2 and SendingState=2)) ");
            string qryName = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(qryName))
            {
                sqlHW.AppendFormat(" AND (ContractNO like '%{0}%' OR ProjectName like '%{0}%' OR CustomerName like '%{0}%' OR ContactPhone like '%{0}%') ", qryName);
            }
            if (!string.IsNullOrEmpty(dpStartDate.Text))
            {
                sqlHW.AppendFormat(" AND InstalDate>= '{0}' ", dpStartDate.Text);
            }
            if (!string.IsNullOrEmpty(dpEndDate.Text))
            {
                sqlHW.AppendFormat(" AND InstalDate<= '{0}' ", dpEndDate.Text);
            }
            if (!string.IsNullOrEmpty(ddlState.SelectedValue))
            {
                sqlHW.AppendFormat(" AND ContractState = {0} ", ddlState.SelectedValue);
            }
            sqlHW.Append(" order by ContractState");
            DataSet dsHW = DbHelperSQL.Query(sqlHW.ToString());
            if (dsHW.Tables[0] != null)
            {
                Grid1.DataSource = dsHW.Tables[0];
                Grid1.DataBind();
            }
        }

        public string GetOrderState(string state)
        {
            /// 项目进度情况 0:登记中 1:已付定金 2:待测量 3:测量完成 4:生产中 5:生产完成  
            /// 6：送货中 7：送货完成 8：待安装 9：安装完成 10:质保中 11:售后中 12:质保结束 
            string i = "";
            switch (state)
            {
                case "7":
                    i = "未派工";
                    break;
                case "8":
                    i = "已派工";
                    break;
                case "9":
                    i = "安装完成";
                    break;
                case "10":
                    i = "质保中";
                    break;
                case "11":
                    i = "售后中";
                    break;
            }
            return i;
        }

        public System.Drawing.Color GetColor(string state)
        {
            System.Drawing.Color i = new System.Drawing.Color();
            switch (state)
            {
                case "7":
                    i = System.Drawing.Color.Red;
                    break;
                case "8":
                    i = System.Drawing.Color.Black;
                    break;
                case "9":
                    i = System.Drawing.Color.Blue;
                    break;
                case "10":
                    i = System.Drawing.Color.Green;
                    break;
                case "11":
                    i = System.Drawing.Color.Orange;
                    break;
                default:
                    i = System.Drawing.Color.Red;
                    break;
            }
            return i;
        }

        public string GetAfterCost(string id)
        {
            decimal amount = 0;
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractID", int.Parse(id)));
            qryList.Add(Expression.Eq("PayType", 3));
            IList<ContractPayInfo> list = Core.Container.Instance.Resolve<IServiceContractPayInfo>().GetAllByKeys(qryList);
            if (list.Count > 0)
            {
                amount = list.Sum(o => o.PayMoney);
                return amount.ToString();
            }
            return "";
        }

        public string GetPerson(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                EmployeeInfo info = Core.Container.Instance.Resolve<IServiceEmployeeInfo>().GetEntity(int.Parse(id));
                return info != null ? info.EmployeeName : "";
            }
            return "";
        }

        #endregion

        #region Events

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreContractInstall", Grid1, "editField");
            CheckPowerWithWindowField("CoreContractAfterSale", Grid1, "sheditField");
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            foreach (int id in ids)
            {
                ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(id);
                if (contractInfo.ContractState == 8)
                {
                    contractInfo.ContractState = 9;
                    Core.Container.Instance.Resolve<IServiceContractInfo>().Update(contractInfo);
                }
            }
            BindGrid();
        }

        protected void Grid1_PreRowDataBound(object sender, GridPreRowEventArgs e)
        {
            //DataRow contractInfo = e.DataItem as DataRow;
            //if (contractInfo != null && contractInfo["ContractState"].ToString() == "9")
            //{
            //    WindowField WindowField = Grid1.Rows[e.RowIndex].FindControl("editField") as WindowField;
            //    WindowField.Enabled = false;
            //}
        }

        protected void btnEnableUsers_Click(object sender, EventArgs e)
        {
            SetSelectedUsersEnableStatus(true);
        }

        protected void btnDisableUsers_Click(object sender, EventArgs e)
        {
            SetSelectedUsersEnableStatus(false);
        }

        private void SetSelectedUsersEnableStatus(bool enabled)
        {
            // 在操作之前进行权限检查
            if (!CheckPower("CoreContractAfterSale"))
            {
                CheckPowerFailWithAlert();
                return;
            }
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            foreach (int id in ids)
            {
                ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(id);
                contractInfo.ContractState = !enabled ? 10 : 11;
                Core.Container.Instance.Resolve<IServiceContractInfo>().Update(contractInfo);

            }
            BindGrid();
        }

        #endregion
    }
}
