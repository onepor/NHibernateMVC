using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUIPro;
using EntityFramework.Extensions;
using NHibernate.Criterion;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web.admin
{
    public partial class role_user : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreRoleUserView";
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
            CheckPowerWithButton("CoreRoleUserNew", btnNew);
            CheckPowerWithButton("CoreRoleUserDelete", btnDeleteSelected);


            ResolveDeleteButtonForGrid(btnDeleteSelected, Grid2, "确定要从当前角色移除选中的{0}项记录吗？");


            BindGrid1();

            // 默认选中第一个角色
            Grid1.SelectedRowIndex = 0;

            // 每页记录数
            Grid2.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();

            BindGrid2();
        }

        private void BindGrid1()
        {
            IList<roles> q = Core.Container.Instance.Resolve<IServiceRoles>().GetAll();

            Grid1.DataSource = q;
            Grid1.DataBind();
        }

        private void BindGrid2()
        {
            int roleID = GetSelectedDataKeyID(Grid1);

            if (roleID == -1)
            {
                Grid2.RecordCount = 0;

                Grid2.DataSource = null;
                Grid2.DataBind();
            }
            else
            {
                IList<ICriterion> qryList = new List<ICriterion>();
                //IQueryable<User> q = DB.Users;

                // 在用户名称中搜索
                string searchText = ttbSearchUser.Text.Trim();
                if (!String.IsNullOrEmpty(searchText))
                {
                    qryList.Add(Expression.Like("Name", searchText, MatchMode.Anywhere));
                    //q = q.Where(u => u.Name.Contains(searchText));
                }
                qryList.Add(!Expression.Eq("Name", "administrator"));
                //q = q.Where(u => u.Name != "administrator");

                IList<ICriterion> qryListUser = new List<ICriterion>();
                qryListUser.Add(Expression.Eq("RoleID", roleID));
                IList<roleusers> rulist = Core.Container.Instance.Resolve<IServiceRoleUsers>().GetAllByKeys(qryListUser);
                int[] userIDs = rulist.Select(u => u.UserID).ToArray();
                // 过滤选中角色下的所有用户
                // string [] userIDs= String.Join(",", rulist.Select(u => u.UserID).ToArray());
                //q = q.Where(u => u.Roles.Any(r => r.ID == roleID));
                qryList.Add(Expression.In("ID", userIDs));

                Order[] orderList = new Order[1];
                Order orderli = new Order(Grid2.SortField, Grid2.SortDirection == "ASC" ? true : false);
                orderList[0] = orderli;
                int count = 0;
                IList<users> userList = Core.Container.Instance.Resolve<IServiceUsers>().GetPaged(qryList, orderList, Grid2.PageIndex, Grid2.PageSize, out count);


                // 在查询添加之后，排序和分页之前获取总记录数
                Grid2.RecordCount = count;
                //// 排列和分页
                //q = SortAndPage<User>(q, Grid2);
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

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid1();

            // 默认选中第一个角色
            Grid1.SelectedRowIndex = 0;

            BindGrid2();
        }

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
            CheckPowerWithLinkButtonField("CoreRoleUserDelete", Grid2, "deleteField");
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
            if (!CheckPower("CoreRoleUserDelete"))
            {
                CheckPowerFailWithAlert();
                return;
            }

            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            int roleID = GetSelectedDataKeyID(Grid1);
            List<int> userIDs = GetSelectedDataKeyIDs(Grid2);

            //role.Users.Where(u => userIDs.Contains(u.ID)).ToList().ForEach(u => role.Users.Remove(u));
            foreach (int userID in userIDs)
            {
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("RoleID", roleID));
                qryList.Add(Expression.Eq("UserID", userID));
                roleusers entity = Core.Container.Instance.Resolve<IServiceRoleUsers>().GetEntityByFields(qryList);

                if (entity != null)
                {
                    Core.Container.Instance.Resolve<IServiceRoleUsers>().Delete(entity);
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
                if (!CheckPower("CoreRoleUserDelete"))
                {
                    CheckPowerFailWithAlert();
                    return;
                }

                int roleID = GetSelectedDataKeyID(Grid1);

                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("RoleID", roleID));
                qryList.Add(Expression.Eq("UserID", userID));
                roleusers entity = Core.Container.Instance.Resolve<IServiceRoleUsers>().GetEntityByFields(qryList);

                if (entity != null)
                {
                    Core.Container.Instance.Resolve<IServiceRoleUsers>().Delete(entity);
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
            int roleID = GetSelectedDataKeyID(Grid1);
            string addUrl = String.Format("~/admin/role_user_addnew.aspx?id={0}", roleID);

            PageContext.RegisterStartupScript(Window1.GetShowReference(addUrl, "添加用户到当前角色"));
        }

        #endregion

    }
}
