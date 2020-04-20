using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractMeasureManage : PageBase
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

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //绑定测量人员
                BindSalerInfo();
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

        /// <summary>
        /// 绑定测量人员
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

            List<EmployeeInfo> listDate = new List<EmployeeInfo>();
            listDate.AddRange(list);
            listDate.Insert(0, new EmployeeInfo { ID = 0, EmployeeName = "--全部--" });

            ddlSaler.DataSource = listDate;
            ddlSaler.DataBind();
            ddlSaler.SelectedIndex = 0;
        }

        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            string qryName = txtSearch.Text.Trim();
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
                qryList.Add(Expression.Ge("MeasureDate", DateTime.Parse(dpStartDate.Text)));
            }
            if (!string.IsNullOrEmpty(dpEndDate.Text))
            {
                qryList.Add(Expression.Le("MeasureDate", DateTime.Parse(dpEndDate.Text)));
            }
            if (!ddlSaler.SelectedValue.Equals("0"))
            {
                qryList.Add(Expression.Ge("MeasurePerson", int.Parse(ddlSaler.SelectedValue)));
            }
            qryList.Add(Expression.Eq("ContractState", 0) || Expression.Eq("ContractState", 1) || Expression.Eq("ContractState", 2));

            Order[] orderList = new Order[1];
            Order orderli = new Order("ContractState", true);
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
                case "0":
                    i = "未派工";
                    break;
                case "1":
                    i = "未派工";
                    break;
                case "2":
                    i = "已派工";
                    break;
            }
            return i;
        }

        public System.Drawing.Color GetColor(string state)
        {
            System.Drawing.Color i = new System.Drawing.Color();
            switch (state)
            {
                case "0":
                    i = System.Drawing.Color.Red;
                    break;
                case "1":
                    i = System.Drawing.Color.Red;
                    break;
                case "2":
                    i = System.Drawing.Color.Black;
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
            CheckPowerWithWindowField("CoreContractMeasure", Grid1, "editField");
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);

            if (e.CommandName == "Delete")
            {
                // 在操作之前进行权限检查
                if (!CheckPower("CoreContractMeasure"))
                {
                    CheckPowerFailWithAlert();
                    return;
                }
                //删除类型
                Core.Container.Instance.Resolve<IServiceContractInfo>().Delete(ID);
                //更新页面数据
                BindGrid();
            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void Grid1_RowDoubleClick(object sender, GridRowClickEventArgs e)
        {
            // 在操作之前进行权限检查
            if (!CheckPower("CoreContractMeasure"))
            {
                int ID = GetSelectedDataKeyID(Grid1);
                PageContext.RegisterStartupScript(Window1.GetShowReference(string.Format("~/Contract/ContractMeasureEdit.aspx?id={0}", ID)
                    , "安排测量时间"));
            }
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

        protected void btnNew_Click(object sender, EventArgs e)
        {

            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            foreach (int id in ids)
            {
                ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(id);
                if (contractInfo.ContractState == 2)
                {
                    contractInfo.ContractState = 3;
                    Core.Container.Instance.Resolve<IServiceContractInfo>().Update(contractInfo);
                }
            }
            BindGrid();
        }

        #endregion

    }
}
