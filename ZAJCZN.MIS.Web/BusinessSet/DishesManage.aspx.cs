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

namespace ZAJCZN.MIS.Web
{
    public partial class DishesManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreDishesView";
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
            CheckPowerWithButton("CoreDishesEdit", btnNew);
            CheckPowerWithButton("CoreDishesEdit", btnDeleteSelected);

            ResolveDeleteButtonForGrid(btnDeleteSelected, Grid2, "确定要从当前类别中移除选中的{0}项记录吗？");

            Inits();
            //绑定物品大类
            BindGrid1();
            // 每页记录数
            Grid2.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
            //绑定物品类别
            //BindGrid2();
        }

        #region 初始化提示信息,注册JS
        private void Inits()
        {
            btnDeleteSelected.OnClientClick = Grid1.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
            btnDeleteSelected.ConfirmText = String.Format("确定要删除选中的<span class=\"highlight\"><script>{0}</script></span>项记录吗？", Grid1.GetSelectedCountReference());
            btnDeleteSelected.ConfirmTarget = FineUIPro.Target.Top;

            btnEnableUsers.OnClientClick = Grid1.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
            btnEnableUsers.ConfirmText = String.Format("确定要启用选中的<span class=\"highlight\"><script>{0}</script></span>项记录吗？", Grid1.GetSelectedCountReference());
            btnEnableUsers.ConfirmTarget = FineUIPro.Target.Top;

            btnDisableUsers.OnClientClick = Grid1.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
            btnDisableUsers.ConfirmText = String.Format("确定要停用选中的<span class=\"highlight\"><script>{0}</script></span>项记录吗？", Grid1.GetSelectedCountReference());
            btnDisableUsers.ConfirmTarget = FineUIPro.Target.Top;
        }
        #endregion

        private void BindGrid1()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("IsUsed", 2));

            Order[] orderList = new Order[1];
            Order orderli = new Order("SortIndex", true);
            orderList[0] = orderli;
            IList<tm_FoodClass> list = Core.Container.Instance.Resolve<IServiceFoodClass>().GetAllByKeys(qryList, orderList);

            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        private void BindGrid2()
        {
            int classID = GetSelectedDataKeyID(Grid1);

            if (classID > 0)
            {
                string searchText = ttbSearchUser.Text.Trim();
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("ClassID", classID));
                if (!string.IsNullOrEmpty(searchText))
                {
                    qryList.Add(Expression.Like("DishesName", searchText, MatchMode.Anywhere) || Expression.Like("DishesPY", searchText, MatchMode.Anywhere) || Expression.Like("DishesCode", searchText, MatchMode.Anywhere));
                }

                Order[] orderList = new Order[1];
                Order orderli = new Order("ID", true);
                orderList[0] = orderli;
                IList<tm_Dishes> list = Core.Container.Instance.Resolve<IServiceDishes>().GetAllByKeys(qryList, orderList);

                Grid2.DataSource = list;
                Grid2.DataBind();
            }
        }
        #endregion

        #region 条件查询
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

        public string GetClassName(string id)
        {
            if (int.Parse(id) > 0)
            {
                tm_FoodClass objType = Core.Container.Instance.Resolve<IServiceFoodClass>().GetEntity(int.Parse(id));
                return objType.ClassName;
            }
            return "";
        }

        public string GetPrintName(string id)
        {
            if (int.Parse(id) > 0)
            {
                tm_Printer objType = Core.Container.Instance.Resolve<IServicePrinter>().GetEntity(int.Parse(id));
                return objType.PrinterName;
            }
            return "";
        }
        #endregion

        #region Events

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid2.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid2();
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid2();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            int typeID = GetSelectedDataKeyID(Grid1);
            string addUrl = String.Format("~/BusinessSet/DishesEdit.aspx?typeid={0}", typeID);
            PageContext.RegisterStartupScript(Window1.GetShowReference(addUrl, "添加菜品"));
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
            CheckPowerWithLinkButtonField("CoreDishesEdit", Grid2, "deleteField");
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
            if (!CheckPower("CoreDishesEdit"))
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
             * */



            // 清空当前选中的项
            Grid2.SelectedRowIndexArray = null;

            // 重新绑定表格
            BindGrid2();
        }


        protected void Grid2_RowCommand(object sender, GridCommandEventArgs e)
        {
            object[] values = Grid2.DataKeys[e.RowIndex];
            int dishesID = Convert.ToInt32(values[0]);

            if (e.CommandName == "Delete")
            {
                // 在操作之前进行权限检查
                if (!CheckPower("CoreDishesEdit"))
                {
                    CheckPowerFailWithAlert();
                    return;
                }


                //int deptID = GetSelectedDataKeyID(Grid1);


                //User user = DB.Users.Include(u => u.Dept)
                //    .Where(u => u.ID == userID)
                //    .FirstOrDefault();

                //if (user != null)
                //{
                //    user.Dept = null;

                //    DB.SaveChanges();
                //}

                BindGrid2();

            }
            if (e.CommandName == "Basket")
            {
                PageContext.Redirect(string.Format("~/BusinessSet/DishesBatchingManage.aspx?id={0}", dishesID));
            }
        }

        #endregion

        #region 设置启用状态

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
            string isUsed = enabled ? "1" : "0";
            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid2);
            // 执行数据库操作
            foreach (int ID in ids)
            {
                tm_Dishes entity = Core.Container.Instance.Resolve<IServiceDishes>().GetEntity(ID);
                entity.IsUsed = int.Parse(isUsed);
                Core.Container.Instance.Resolve<IServiceDishes>().Update(entity);
            }
            // 重新绑定表格
            BindGrid2();
        }

        #endregion 设置启用状态

    }
}
