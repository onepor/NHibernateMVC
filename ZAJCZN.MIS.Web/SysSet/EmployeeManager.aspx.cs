using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class EmployeeManager : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreEmployeeView";
            }
        }

        #endregion

        #region 加载 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            //权限检查
            CheckPowerWithButton("CoreEmployeeNew", btnNew);

            btnNew.OnClientClick = Window1.GetShowReference("~/SysSet/EmployeeEdit.aspx?action=add", "新增客户信息");
            
            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
            BindGrid();
        }
        #endregion
 
        #region 绑定数据

        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            string qryName = ttbSearchMessage.Text.Trim();
            if (!string.IsNullOrEmpty(qryName))
            {
                qryList.Add(Expression.Disjunction()
                    .Add(Expression.Like("EmployeeName", qryName, MatchMode.Anywhere))
                    .Add(Expression.Like("ContractPhone", qryName, MatchMode.Anywhere)) 
                    );
            }
            if (ddlIsUsed.SelectedValue != "0")
            {
                qryList.Add(Expression.Eq("UserType", ddlIsUsed.SelectedValue));
            }
            Order[] orderList = new Order[1];
            Order orderli = new Order(Grid1.SortField, Grid1.SortDirection == "ASC" ? true : false);
            orderList[0] = orderli;
            int count = 0;
            IList<EmployeeInfo> list = Core.Container.Instance.Resolve<IServiceEmployeeInfo>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        public string GetIsUsed(string state)
        {
            string i = "";
            switch (state)
            {
                case "0":
                    i = "停用";
                    break;
                case "1":
                    i = "启用";
                    break;
            }
            return i;
        }

        public string GetSex(string state)
        {
            string i = "";
            switch (state)
            {
                case "1":
                    i = "男";
                    break;
                case "2":
                    i = "女";
                    break;
            }
            return i;
        }
        #endregion

        #region Events
        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreEmployeeEdit", Grid1, "editField");
            CheckPowerWithLinkButtonField("CoreEmployeeDelete", Grid1, "deleteField");
        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();
        }


        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        { 
            int ID = GetSelectedDataKeyID(Grid1);

            if (e.CommandName == "editField")
            {
                if (!CheckPower("CoreEmployeeEdit"))
                    Alert.ShowInTop("您没有该操作权限，请从管理员处获取！", MessageBoxIcon.Information);
            }
            if (e.CommandName == "Delete")
            {
                if (!CheckPower("CoreEmployeeDelete"))
                    Alert.ShowInTop("您没有该操作权限，请从管理员处获取！", MessageBoxIcon.Information);
                else
                {
                    Core.Container.Instance.Resolve<IServiceEmployeeInfo>().Delete(ID);
                    BindGrid();
                }
            }
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {

            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            foreach (int id in ids)
            {
                Core.Container.Instance.Resolve<IServiceEmployeeInfo>().Delete(id);
            }
            BindGrid();
        }

        protected void ttbSearchMessage_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearchMessage.Text = String.Empty;
            ttbSearchMessage.ShowTrigger1 = false;
            BindGrid();
        }

        protected void ttbSearchMessage_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearchMessage.ShowTrigger1 = true;
            BindGrid();
        }

        //项目进度筛选触发事件
        protected void rblEnableStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindGrid();
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void Grid1_RowDoubleClick(object sender, GridRowClickEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            PageContext.RegisterStartupScript(Window1.GetShowReference(string.Format("~/SysSet/EmployeeEdit.aspx?id={0}&action=edit", ID)
                , "编辑员工信息"));

        }
        #endregion

    }
}