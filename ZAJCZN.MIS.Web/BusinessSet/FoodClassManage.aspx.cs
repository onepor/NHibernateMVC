using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class FoodClassManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreFoodClassView";
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
            CheckPowerWithButton("CoreFoodClassEdit", btnNew);
            CheckPowerWithButton("CoreAbnormalEdit", btnChangeEnableUsers);
            btnNew.OnClientClick = Window1.GetShowReference("~/BusinessSet/FoodClassEdit.aspx", "新增菜品类别");
            Inits();
            //绑定数据
            BindGrid();
        }

        #region 初始化提示信息,注册JS
        private void Inits()
        {
            btnEnableUsers.OnClientClick = Grid1.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
            btnEnableUsers.ConfirmText = String.Format("确定要启用选中的<span class=\"highlight\"><script>{0}</script></span>项记录吗？", Grid1.GetSelectedCountReference());
            btnEnableUsers.ConfirmTarget = FineUIPro.Target.Top;
            btnDisableUsers.OnClientClick = Grid1.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
            btnDisableUsers.ConfirmText = String.Format("确定要停用选中的<span class=\"highlight\"><script>{0}</script></span>项记录吗？", Grid1.GetSelectedCountReference());
            btnDisableUsers.ConfirmTarget = FineUIPro.Target.Top;
        }
        #endregion

        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            Order[] orderList = new Order[1];
            Order orderli = new Order("SortIndex", true);
            orderList[0] = orderli;

            IList<tm_FoodClass> list = Core.Container.Instance.Resolve<IServiceFoodClass>().GetAllByKeys(qryList, orderList);

            Grid1.DataSource = list;
            Grid1.DataBind();
        }

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

        public string GetType(string state)
        {
            return GetSystemEnumValue("CPDW", state);
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

        public string GetWHName(string id)
        {
            if (!string.IsNullOrEmpty(id) && int.Parse(id) > 0)
            {
                WareHouse objType = Core.Container.Instance.Resolve<IServiceWareHouse>().GetEntity(int.Parse(id));
                return objType.WHName;
            }
            return "";
        }
        #endregion

        #region Events

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreFoodClassEdit", Grid1, "editField");
            CheckPowerWithLinkButtonField("CoreFoodClassEdit", Grid1, "deleteField");
        }

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
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            // 执行数据库操作
            foreach (int ID in ids)
            {
                tm_FoodClass entity = Core.Container.Instance.Resolve<IServiceFoodClass>().GetEntity(ID);
                entity.IsUsed = int.Parse(isUsed);
                Core.Container.Instance.Resolve<IServiceFoodClass>().Update(entity);
            }
            // 重新绑定表格
            BindGrid();
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);

            if (e.CommandName == "Delete")
            {
                // 在操作之前进行权限检查
                if (!CheckPower("CoreFoodClassEdit"))
                {
                    CheckPowerFailWithAlert();
                    return;
                }

                ////检查是否有子类型
                //IList<ICriterion> qryList = new List<ICriterion>();

                //if (ddlType.SelectedValue != "0")
                //{
                //    qryList.Add(Expression.Eq("ParentID={0}", ID));
                //}

                //Order[] orderList = new Order[1];
                //Order orderli = new Order(Grid1.SortField, Grid1.SortDirection == "ASC" ? true : false);
                //orderList[0] = orderli;
                //int childCount = 0;
                //IList<tm_FoodClass> list = Core.Container.Instance.Resolve<IServiceFoodClass>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out childCount);

                //if (childCount > 0)
                //{
                //    Alert.ShowInTop("删除失败！请先删除菜品子类别！");
                //    return;
                //}

                //删除类型
                Core.Container.Instance.Resolve<IServiceFoodClass>().Delete(ID);
                //更新页面数据
                BindGrid();
            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            MenuHelper.Reload();
            BindGrid();
        }

        //项目进度筛选触发事件
        protected void rblEnableStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }
        #endregion

    }
}
