using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Data;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Helpers;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractWorkerManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreContractView";
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
            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
            //绑定数据
            BindGrid();
        }

        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            string qryName = txtSearch.Text.Trim();
            qryList.Add(!Expression.Eq("ContractState", 1));
            if (!string.IsNullOrEmpty(qryName))
            {
                qryList.Add(Expression.Disjunction()
                    .Add(Expression.Like("ContractNO", qryName, MatchMode.Anywhere))
                    .Add(Expression.Like("CustomerName", qryName, MatchMode.Anywhere))
                    .Add(Expression.Like("ContactPhone", qryName, MatchMode.Anywhere))
                    .Add(Expression.Like("ProjectName", qryName, MatchMode.Anywhere))
                    );
            }
            if (!string.IsNullOrEmpty(dpStartDate.Text))
            {
                qryList.Add(Expression.Ge("ContractDate", DateTime.Parse(dpStartDate.Text)));
            }
            if (!string.IsNullOrEmpty(dpEndDate.Text))
            {
                qryList.Add(Expression.Le("ContractDate", DateTime.Parse(dpEndDate.Text)));
            }


            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            int count = 0;
            IList<ContractInfo> list = Core.Container.Instance.Resolve<IServiceContractInfo>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        public string GetOrderState(string state)
        {
            string i = "";
            switch (state)
            {
                case "1":
                    i = "登记中";
                    break;
                case "2":
                    i = "测量中";
                    break;
                case "3":
                    i = "生产中";
                    break;
                case "4":
                    i = "生产完成";
                    break;
                case "5":
                    i = "送货中";
                    break;
                case "6":
                    i = "安装中";
                    break;
                case "7":
                    i = "售后中";
                    break;
                case "8":
                    i = "质保中";
                    break;
                case "9":
                    i = "质保到期";
                    break;
            }
            return i;
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

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithLinkButtonField("CoreContractDoor", Grid1, "lbtnEditDoorField");
            CheckPowerWithBoundField("CoreContractDoor", Grid1, "DoorAmount");
            // 数据绑定之前，进行权限检查
            CheckPowerWithLinkButtonField("CoreContractCabinet", Grid1, "lbtnEditCabinetField");
            CheckPowerWithBoundField("CoreContractCabinet", Grid1, "CabinetAmount");
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreContractEdit", Grid1, "editField");
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreContraceTrack", Grid1, "wfSupplierField");
            // 数据绑定之前，进行权限检查
            CheckPowerWithLinkButtonField("CorePayment", Grid1, "lbtnEditMoneyField");
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            //门设计
            if (e.CommandName == "editFieldDoor")
            {
                PageContext.Redirect(string.Format("~/Contract/ContractDoorDetail.aspx?id={0}", ID));
            }
            //柜子设计
            if (e.CommandName == "editFieldCabinet")
            {
                PageContext.Redirect(string.Format("~/Contract/ContractCabinetEdit.aspx?id={0}", ID));
            }
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindGrid();
        }

        #endregion
    }
}
