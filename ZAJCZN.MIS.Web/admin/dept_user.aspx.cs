﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUIPro;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using NHibernate.Criterion;

namespace ZAJCZN.MIS.Web.admin
{
    public partial class dept_user : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreDeptUserView";
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
            // 权限检查
            CheckPowerWithButton("CoreDeptUserNew", btnNew);
            CheckPowerWithButton("CoreDeptUserDelete", btnDeleteSelected);


            ResolveDeleteButtonForGrid(btnDeleteSelected, Grid2, "确定要从当前部门移除选中的{0}项记录吗？");


            BindGrid1();

            // 默认选中第一个部门
            Grid1.SelectedRowIndex = 0;

            // 每页记录数
            Grid2.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();

            BindGrid2();
        }

        private void BindGrid1()
        {
            List<depts> mys = DeptHelper.Depts;

            Grid1.DataSource = mys;
            Grid1.DataBind();
        }

        private void BindGrid2()
        {
            int deptID = GetSelectedDataKeyID(Grid1);

            if (deptID == -1)
            {
                Grid2.RecordCount = 0;

                Grid2.DataSource = null;
                Grid2.DataBind();
            }
            else
            {
                // 在用户名称中搜索
                string searchText = ttbSearchUser.Text.Trim();

                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("DeptID", deptID));
                qryList.Add(!Expression.Eq("Name", "administrator"));
                if (!String.IsNullOrEmpty(searchText))
                {
                    qryList.Add(Expression.Like("Name", searchText, MatchMode.Anywhere));
                }
                Order[] orderList = new Order[1];
                Order orderli = new Order(Grid2.SortField, Grid2.SortDirection == "ASC" ? true : false);
                orderList[0] = orderli;
                int count = 0;
                IList<users> userList = Core.Container.Instance.Resolve<IServiceUsers>().GetPaged(qryList, orderList, Grid2.PageIndex, Grid2.PageSize, out count);

                // 在查询添加之后，排序和分页之前获取总记录数
                Grid2.RecordCount = count;
                Grid2.DataSource = userList;
                Grid2.DataBind();
            }
        }


        #endregion

        #region Events

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid2.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid2();
        }


        #endregion

        #region Grid1 Events

        protected void Grid1_RowClick(object sender, FineUIPro.GridRowClickEventArgs e)
        {
            BindGrid2();
        }

        #endregion

        #region Grid2 Events

        protected void ttbSearchUser_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearchUser.ShowTrigger1 = true;
            BindGrid2();
        }

        protected void ttbSearchUser_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearchUser.Text = String.Empty;
            ttbSearchUser.ShowTrigger1 = false;
            BindGrid2();
        }

        protected void Grid2_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithLinkButtonField("CoreDeptUserDelete", Grid2, "deleteField");
        }

        protected void Grid2_Sort(object sender, GridSortEventArgs e)
        {
            Grid2.SortDirection = e.SortDirection;
            Grid2.SortField = e.SortField;
            BindGrid2();
        }

        protected void Grid2_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid2.PageIndex = e.NewPageIndex;
            BindGrid2();
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            // 在操作之前进行权限检查
            if (!CheckPower("CoreDeptUserDelete"))
            {
                CheckPowerFailWithAlert();
                return;
            }

            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            int deptID = GetSelectedDataKeyID(Grid1);
            List<int> userIDs = GetSelectedDataKeyIDs(Grid2);

            /*
            Dept dept = DB.Depts.Include(d => d.Users)
                .Where(d => d.ID == deptID)
                .FirstOrDefault();

            foreach (int userID in userIDs)
            {
                User user = dept.Users.Where(u => u.ID == userID).FirstOrDefault();
                if (user != null)
                {
                    dept.Users.Remove(user);
                }
            }

            DB.SaveChanges();
             * 

            DB.Users.Include(u => u.Dept)
                .Where(u => userIDs.Contains(u.ID))
                .ToList()
                .ForEach(u => u.Dept = null);

            DB.SaveChanges();*/

            foreach (int userID in userIDs)
            {
                users user = Core.Container.Instance.Resolve<IServiceUsers>().GetEntity(userID);
                if (user != null)
                {
                    user.DeptID = 0;
                    Core.Container.Instance.Resolve<IServiceUsers>().Update(user);
                }
            }

            // 清空当前选中的项
            Grid2.SelectedRowIndexArray = null;
            // 重新绑定表格
            BindGrid2();
        }


        protected void Grid2_RowCommand(object sender, GridCommandEventArgs e)
        {
            object[] values = Grid2.DataKeys[e.RowIndex];
            int userID = Convert.ToInt32(values[0]);

            if (e.CommandName == "Delete")
            {
                // 在操作之前进行权限检查
                if (!CheckPower("CoreDeptUserDelete"))
                {
                    CheckPowerFailWithAlert();
                    return;
                }

                int deptID = GetSelectedDataKeyID(Grid1);

                users user = Core.Container.Instance.Resolve<IServiceUsers>().GetEntity(userID);

                //users user = DB.Users.Include(u => u.Dept)
                //.Where(u => u.ID == userID)
                //.FirstOrDefault();

                if (user != null)
                {
                    user.DeptID = 0;
                    Core.Container.Instance.Resolve<IServiceUsers>().Update(user);
                    //DB.SaveChanges();
                }

                BindGrid2();

            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid2();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            int deptID = GetSelectedDataKeyID(Grid1);
            string addUrl = String.Format("~/admin/dept_user_addnew.aspx?id={0}", deptID);

            PageContext.RegisterStartupScript(Window1.GetShowReference(addUrl, "添加用户到当前部门"));
        }

        #endregion

    }
}
