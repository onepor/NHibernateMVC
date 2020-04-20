using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUIPro;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using NHibernate.Criterion;

namespace ZAJCZN.MIS.Web.admin
{
    public partial class dept_user_addnew : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreDeptUserNew";
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
            else
            {
                // 页面回发时要同步选中的行数据到隐藏字段（在触发控件事件之前）
                SyncSelectedRowIndexArrayToHiddenField(hfSelectedIDS, Grid1);
            }
        }

        private void LoadData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHideReference();

            int id = GetQueryIntValue("id");
            depts current = Core.Container.Instance.Resolve<IServiceDepts>().GetEntity(id);
            //Dept current = DB.Depts.Find(id);
            if (current == null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }

            // 每页记录数
            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();


            BindGrid();
        }


        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            int id = GetQueryIntValue("id");
            //IQueryable<User> q = DB.Users.Include(u => u.Dept);

            // 在职务名称中搜索
            string searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                qryList.Add(Expression.Like("Name", searchText, MatchMode.Anywhere) || Expression.Like("ChineseName", searchText, MatchMode.Anywhere) || Expression.Like("EnglishName", searchText, MatchMode.Anywhere));
                //q = q.Where(u => u.Name.Contains(searchText) || u.ChineseName.Contains(searchText) || u.EnglishName.Contains(searchText));
            }
            qryList.Add(!Expression.Eq("Name", "administrator"));
            //q = q.Where(u => u.Name != "administrator");
            // 排除所有已经属于某个部门的用户
            //q = q.Where(u => u.Dept == null);
            qryList.Add(!Expression.Eq("DeptID", id));
            // 在查询添加之后，排序和分页之前获取总记录数
            Order[] orderList = new Order[1];
            Order orderli = new Order(Grid1.SortField, Grid1.SortDirection == "ASC" ? true : false);
            orderList[0] = orderli;
            int count = 0;
            IList<users> userList = Core.Container.Instance.Resolve<IServiceUsers>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);

            Grid1.RecordCount = count;

            // 排列和分页
            //q = SortAndPage<User>(q, Grid1);
            Grid1.DataSource = userList;
            Grid1.DataBind();


            // 重新绑定表格数据之后，更新选中行
            UpdateSelectedRowIndexArray(hfSelectedIDS, Grid1);
        }

        #endregion

        #region Events

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            int deptID = GetQueryIntValue("id");

            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedIDsFromHiddenField(hfSelectedIDS);

            foreach (int id in ids)
            {
                users user = Core.Container.Instance.Resolve<IServiceUsers>().GetEntity(id);
                if (user != null)
                {
                    user.DeptID = deptID;
                    Core.Container.Instance.Resolve<IServiceUsers>().Update(user);
                }
            }
            //Dept dept = Attach<Dept>(deptID);

            //DB.Users.Where(u => ids.Contains(u.ID))
            //    .ToList()
            //    .ForEach(u => u.Dept = dept);

            //DB.SaveChanges();

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }



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


        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid();
        }

        #endregion


    }
}
