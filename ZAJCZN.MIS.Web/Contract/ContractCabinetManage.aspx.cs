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
    public partial class ContractCabinetManage : PageBase
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
            qryList.Add(Expression.Eq("ContractState", 2));
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
                    i = "待测量";
                    break;
                case "3":
                    i = "生产中";
                    break;
                case "4":
                    i = "送货安装中";
                    break;
                case "5":
                    i = "质保中";
                    break;
                case "6":
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
            CheckPowerWithWindowField("CoreContractCabinet", Grid1, "editField");
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            //编辑发货单
            if (e.CommandName == "editField")
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
