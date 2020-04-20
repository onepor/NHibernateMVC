using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class GoodsManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreGoodsView";
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
            // 权限检查
            CheckPowerWithButton("CoreGoodsEdit", btnChangeEnableUsers);
            CheckPowerWithButton("CoreGoodsEdit", btnNew);

            Inits();
            //绑定物品类型信息
            BindCostType();

            btnNew.OnClientClick = Window1.GetShowReference("~/StockSet/GoodsEdit.aspx", "新增库存物品");
            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();

            //加载物品信息
            BindGrid();
        }
        #endregion

        #region 绑定物品类型
        private void BindCostType()
        {
            List<JQueryFeature> myList = new List<JQueryFeature>();

            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ParentID", 0));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<tm_GoodsType> list = Core.Container.Instance.Resolve<IServiceGoodsType>().GetAllByKeys(qryList, orderList);

            foreach (tm_GoodsType good in list)
            {
                myList.Add(new JQueryFeature(good.ID.ToString(), good.TypeName, 1, true));

                IList<ICriterion> qryListChild = new List<ICriterion>();
                qryListChild.Add(Expression.Eq("ParentID", good.ID));
                Order[] orderListChild = new Order[1];
                Order orderliChild = new Order("ID", true);
                orderListChild[0] = orderliChild;
                IList<tm_GoodsType> listChild = Core.Container.Instance.Resolve<IServiceGoodsType>().GetAllByKeys(qryListChild, orderListChild);
                foreach (tm_GoodsType goodChild in listChild)
                {
                    myList.Add(new JQueryFeature(goodChild.ID.ToString(), goodChild.TypeName, 2, true));
                }
            }
            myList.Insert(0, new JQueryFeature("0", "全部分类", 1, true));
            ddlCostType.DataSource = myList;
            ddlCostType.DataBind();
            ddlCostType.SelectedIndex = 0;
        }
        #endregion

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

        #region 绑定数据
        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            string qryName = ttbSearchMessage.Text.Trim();
            if (!string.IsNullOrEmpty(qryName))
            {
                qryList.Add(Expression.Like("GoodsName", qryName, MatchMode.Anywhere)
                            || Expression.Like("GoodsPY", qryName.ToUpper(), MatchMode.Anywhere));
            }
            if (ddlIsUsed.SelectedValue != "0")
            {
                qryList.Add(Expression.Eq("IsUsed", int.Parse(ddlIsUsed.SelectedValue)));
            }
            if (ddlCostType.SelectedValue != "0")
            {
                qryList.Add(Expression.Eq("GoodsTypeID", int.Parse(ddlCostType.SelectedValue)));
            }
            Order[] orderList = new Order[1];
            Order orderli = new Order(Grid1.SortField, Grid1.SortDirection == "ASC" ? true : false);
            orderList[0] = orderli;
            int count = 0;
            IList<tm_Goods> list = Core.Container.Instance.Resolve<IServiceGoods>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
        }
        #endregion

        #region 页面数据转换
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
        //获取分类名称
        public string GetType(string typeID)
        {
            tm_GoodsType objType = Core.Container.Instance.Resolve<IServiceGoodsType>().GetEntity(int.Parse(typeID));
            return objType != null ? objType.TypeName : "";
        }
        //获取单位
        public string GetUnitName(string state)
        {
            return GetSystemEnumValue("WPDW", state);
        }
        //获取税率名称
        public string GetTaxName(string paramID)
        {
            Sys_Paras objType = Core.Container.Instance.Resolve<IServiceSysParams>().GetEntity(int.Parse(paramID));
            return objType != null ? objType.ParaName : "";
        }
        #endregion

        #region Events

        protected void btnEnableUsers_Click(object sender, EventArgs e)
        {
            SetSelectedUsersEnableStatus(true);
        }

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreGoodsEdit", Grid1, "editField");
            CheckPowerWithLinkButtonField("CoreGoodsEdit", Grid1, "deleteField");
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
                tm_Goods entity = Core.Container.Instance.Resolve<IServiceGoods>().GetEntity(ID);
                entity.IsUsed = int.Parse(isUsed);
                //entity.GoodsPY = GetChinesePY(entity.GoodsName);
                Core.Container.Instance.Resolve<IServiceGoods>().Update(entity);
            }
            // 重新绑定表格
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

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            if (e.CommandName == "Delete")
            {
                Core.Container.Instance.Resolve<IServiceGoods>().Delete(ID);
                BindGrid();
            }
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            foreach (int id in ids)
            {
                Core.Container.Instance.Resolve<IServiceGoods>().Delete(id);
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
            Session["GoodSType"] = ddlCostType.SelectedValue;
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

        #endregion
    }
}