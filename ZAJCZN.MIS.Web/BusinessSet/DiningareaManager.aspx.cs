﻿using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class DiningareaManager : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreDiningareaView";
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
            if (!IsPostBack)
            {
                //权限检查
                CheckPowerWithButton("CoreDiningareaEdit", btnNew);
                Inits();
                btnNew.OnClientClick = Window1.GetShowReference("~/BusinessSet/DiningareaEdit.aspx?action=add", "新增餐区");
                Grid1.PageSize = ConfigHelper.PageSize;
                ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
                BindGrid();
            }
        }
        #endregion

        #region 初始化提示信息,注册JS
        private void Inits()
        {
            btnDeleteSelected.OnClientClick = Grid1.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
            btnDeleteSelected.ConfirmText = String.Format("确定要删除选中的<span class=\"highlight\"><script>{0}</script></span>项记录吗？", Grid1.GetSelectedCountReference());
            btnDeleteSelected.ConfirmTarget = FineUIPro.Target.Top;
        }
        #endregion

        #region 绑定数据
        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            string qryName = ttbSearchMessage.Text.Trim();
            if (!string.IsNullOrEmpty(qryName))
            {
                qryList.Add(Expression.Like("AreaName", qryName, MatchMode.Anywhere));
            }
            Order[] orderList = new Order[1];
            Order orderli = new Order(Grid1.SortField, Grid1.SortDirection == "ASC" ? true : false);
            orderList[0] = orderli;
            int count = 0;
            IList<tm_Diningarea> list = Core.Container.Instance.Resolve<IServiceDiningarea>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        public int GetTables(string ID)
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("Diningarea_Tabie.ID", Int32.Parse(ID)));
            return Core.Container.Instance.Resolve<IServiceTabie>().Query(qryList).Count;
        }

        #endregion

        #region Events

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreDiningareaEdit", Grid1, "editField");
            CheckPowerWithLinkButtonField("CoreDiningareaEdit", Grid1, "deleteField");
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
                Core.Container.Instance.Resolve<IServiceDiningarea>().Delete(ID);
                BindGrid();
            }
            if (e.CommandName == "Tabies")
            {
                PageContext.Redirect("~/BusinessSet/TabieManager.aspx?id=" + ID);
            }
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {

            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            foreach (int id in ids)
            {
                Core.Container.Instance.Resolve<IServiceDiningarea>().Delete(id);
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