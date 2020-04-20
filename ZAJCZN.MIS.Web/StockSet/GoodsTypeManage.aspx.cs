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
    public partial class GoodsTypeManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreGoodsTypeView";
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
            CheckPowerWithButton("CoreGoodsTypeEdit", btnNew);
            CheckPowerWithButton("CoreGoodsTypeEdit", btnDeleteSelected);

            ResolveDeleteButtonForGrid(btnDeleteSelected, Grid2, "确定要从当前类别中移除选中的{0}项记录吗？");

            Inits();
            //绑定物品大类
            BindGrid1();
            // 每页记录数
            Grid2.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
            //绑定物品类别
            BindGrid2();
        }

        #region 初始化提示信息,注册JS
        private void Inits()
        {
            btnDeleteSelected.OnClientClick = Grid2.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
            btnDeleteSelected.ConfirmText = String.Format("确定要删除选中的<span class=\"highlight\"><script>{0}</script></span>项记录吗？", Grid2.GetSelectedCountReference());
            btnDeleteSelected.ConfirmTarget = FineUIPro.Target.Top;

            btnEnableUsers.OnClientClick = Grid2.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
            btnEnableUsers.ConfirmText = String.Format("确定要启用选中的<span class=\"highlight\"><script>{0}</script></span>项记录吗？", Grid2.GetSelectedCountReference());
            btnEnableUsers.ConfirmTarget = FineUIPro.Target.Top;

            btnDisableUsers.OnClientClick = Grid2.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
            btnDisableUsers.ConfirmText = String.Format("确定要停用选中的<span class=\"highlight\"><script>{0}</script></span>项记录吗？", Grid2.GetSelectedCountReference());
            btnDisableUsers.ConfirmTarget = FineUIPro.Target.Top;

            mbtnEnableCalc.OnClientClick = Grid2.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
            mbtnEnableCalc.ConfirmText = String.Format("确定要启用选中的<span class=\"highlight\"><script>{0}</script></span>项记录吗？", Grid2.GetSelectedCountReference());
            mbtnEnableCalc.ConfirmTarget = FineUIPro.Target.Top;

            mbtnDisableCalc.OnClientClick = Grid2.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
            mbtnDisableCalc.ConfirmText = String.Format("确定要停用选中的<span class=\"highlight\"><script>{0}</script></span>项记录吗？", Grid2.GetSelectedCountReference());
            mbtnDisableCalc.ConfirmTarget = FineUIPro.Target.Top;

        }
        #endregion

        private void BindGrid1()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ParentID", 0));

            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            int count = 0;
            IList<tm_GoodsType> list = Core.Container.Instance.Resolve<IServiceGoodsType>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);

            List<tm_GoodsType> goodsList = list.ToList();
            goodsList.Insert(0, new tm_GoodsType { ID = 0, TypeName = "全部类别" });

            Grid1.DataSource = goodsList;
            Grid1.DataBind();
        }

        private void BindGrid2()
        {
            int deptID = GetSelectedDataKeyID(Grid1);

            if (deptID <= 0)
            {
                IList<tm_GoodsType> list = Core.Container.Instance.Resolve<IServiceGoodsType>().GetAll();
                Grid2.DataSource = list;
                Grid2.DataBind();
            }
            else
            {

                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Or(Expression.Eq("ParentID", deptID), Expression.Eq("ID", deptID)));

                Order[] orderList = new Order[1];
                Order orderli = new Order("ParentID", true);
                orderList[0] = orderli;
                int count = 0;
                IList<tm_GoodsType> list = Core.Container.Instance.Resolve<IServiceGoodsType>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);

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
                case "1":
                    i = "停用";
                    break;
                case "2":
                    i = "启用";
                    break;
            }
            return i;
        }

        public string GetIsCalc(string state)
        {
            switch (state)
            {
                case "1":
                    return "是";
                case "0":
                    return "否";
                default:
                    return "是";
            }
        }

        public string GetTypeName(string id)
        {
            if (int.Parse(id) > 0)
            {
                tm_GoodsType objType = Core.Container.Instance.Resolve<IServiceGoodsType>().GetEntity(int.Parse(id));
                return objType.TypeName;
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
            int deptID = GetSelectedDataKeyID(Grid1);
            string addUrl = String.Format("~/StockSet/GoodsTypeEdit.aspx?typeid={0}", deptID);
            PageContext.RegisterStartupScript(Window1.GetShowReference(addUrl, "添加物品类别"));
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
            CheckPowerWithLinkButtonField("CoreGoodsTypeEdit", Grid2, "deleteField");
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
            if (!CheckPower("CoreGoodsTypeEdit"))
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
            int userID = Convert.ToInt32(values[0]);

            if (e.CommandName == "Delete")
            {
                // 在操作之前进行权限检查
                if (!CheckPower("CoreGoodsTypeEdit"))
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
            string isUsed = enabled ? "2" : "1";
            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid2);
            // 执行数据库操作
            foreach (int ID in ids)
            {
                tm_GoodsType entity = Core.Container.Instance.Resolve<IServiceGoodsType>().GetEntity(ID);
                entity.IsUsed = int.Parse(isUsed);
                Core.Container.Instance.Resolve<IServiceGoodsType>().Update(entity);
            }
            // 重新绑定表格
            BindGrid2();
        }

        #endregion 设置启用状态

        #region 设置参与财务核算

        protected void btnEnableCalc_Click(object sender, EventArgs e)
        {
            SetSelectedCalcEnableStatus(true);
        }

        protected void btnDisableCalc_Click(object sender, EventArgs e)
        {
            SetSelectedCalcEnableStatus(false);
        }

        private void SetSelectedCalcEnableStatus(bool enabled)
        {
            string isUsed = enabled ? "1" : "0";
            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid2);
            // 执行数据库操作
            foreach (int ID in ids)
            {
                tm_GoodsType entity = Core.Container.Instance.Resolve<IServiceGoodsType>().GetEntity(ID);
                entity.IsCalc = int.Parse(isUsed);
                Core.Container.Instance.Resolve<IServiceGoodsType>().Update(entity);
            }
            // 重新绑定表格
            BindGrid2();
        }

        #endregion 设置参与财务核算
    }
}
