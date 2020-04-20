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
using ZAJCZN.MIS.Service;
using ZAJCZN.MIS.Domain;

namespace ZAJCZN.MIS.Web.admin
{
    public partial class role : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreRoleView";
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
            CheckPowerWithButton("CoreRoleNew", btnNew);
            //CheckPowerDeleteWithButton(btnDeleteSelected);

            //ResolveDeleteButtonForGrid(btnDeleteSelected, Grid1);

            btnNew.OnClientClick = Window1.GetShowReference("~/admin/role_new.aspx", "新增角色");


            // 每页记录数
            Grid1.PageSize = ConfigHelper.PageSize;


            BindGrid();
        }

        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            // 在名称中搜索
            string searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                qryList.Add(Expression.Like("Name", searchText, MatchMode.Anywhere));
            }
            Order[] orderList = new Order[1];
            Order orderli = new Order(Grid1.SortField, Grid1.SortDirection == "ASC" ? true : false);
            orderList[0] = orderli;
            int count = 0;
            IList<roles> roleList = Core.Container.Instance.Resolve<IServiceRoles>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);

            // 在查询添加之后，排序和分页之前获取总记录数
            Grid1.RecordCount = count;

            // 排列和数据库分页
            //q = SortAndPage<Role>(q, Grid1);

            Grid1.DataSource = roleList;
            Grid1.DataBind();
        }

        #endregion

        #region Events

        protected void ttbSearchMessage_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearchMessage.ShowTrigger1 = true;
            BindGrid();
        }

        protected void ttbSearchMessage_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearchMessage.Text = String.Empty;
            ttbSearchMessage.ShowTrigger1 = false;
            BindGrid();
        }

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreRoleEdit", Grid1, "editField");
            CheckPowerWithLinkButtonField("CoreRoleDelete", Grid1, "deleteField");
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
            int roleID = GetSelectedDataKeyID(Grid1);

            if (e.CommandName == "Delete")
            {
                // 在操作之前进行权限检查
                if (!CheckPower("CoreRoleDelete"))
                {
                    CheckPowerFailWithAlert();
                    return;
                }
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("RoleID", roleID));
                int userCountUnderThisRole = Core.Container.Instance.Resolve<IServiceRoleUsers>().GetRecordCountByFields(qryList);
                //DB.Users.Where(u => u.Roles.Any(r => r.ID == roleID)).Count();

                if (userCountUnderThisRole > 0)
                {
                    Alert.ShowInTop("删除失败！需要先清空属于此角色的用户！");
                    return;
                }
                //删除角色权限信息
                IList<ICriterion> qryList1 = new List<ICriterion>();
                qryList1.Add(Expression.Eq("RoleID", roleID));
                IList<rolepowers> rpowersList = Core.Container.Instance.Resolve<IServiceRolePowers>().GetAllByKeys(qryList1);
                foreach (rolepowers rpower in rpowersList)
                {
                    Core.Container.Instance.Resolve<IServiceRolePowers>().Delete(rpower);
                }
                //删除角色信息
                Core.Container.Instance.Resolve<IServiceRoles>().Delete(roleID);
                // 执行数据库操作
                //DB.Roles.Where(r => r.ID == roleID).Delete();

                BindGrid();
            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        #endregion

    }
}
