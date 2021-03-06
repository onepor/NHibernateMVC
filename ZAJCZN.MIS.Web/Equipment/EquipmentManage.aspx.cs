﻿using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class EquipmentManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreEquipmentView";
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
            CheckPowerWithButton("CoreEquipmentEdit", btnChangeEnableUsers);
            CheckPowerWithButton("CoreEquipmentNew", btnNew);

            Inits();
            //绑定商品类型信息
            BindCostType();

            btnNew.OnClientClick = Window1.GetShowReference("~/Equipment/EquipmentEdit.aspx", "新增商品商品");

            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();

            //加载商品信息
            BindGrid();
        }
        #endregion

        #region 绑定商品类型
        private void BindCostType()
        {
            List<EquipmentTypeInfo> myList = new List<EquipmentTypeInfo>();

            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("IsUsed", "1"));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<EquipmentTypeInfo> list = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetAllByKeys(qryList, orderList);

            Grid2.DataSource = list;
            Grid2.DataBind();
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
            int classID = GetSelectedDataKeyID(Grid2);

            if (!string.IsNullOrEmpty(qryName))
            {
                qryList.Add(Expression.Like("EquipmentName", qryName, MatchMode.Anywhere));
            }
            if (ddlIsUsed.SelectedValue != "0")
            {
                qryList.Add(Expression.Eq("IsUsed", ddlIsUsed.SelectedValue));
            }
            if (classID > 0)
            {
                qryList.Add(Expression.Eq("EquipmentTypeID", classID));
            }
            Order[] orderList = new Order[1];
            Order orderli = new Order(Grid1.SortField, Grid1.SortDirection == "ASC" ? true : false);
            orderList[0] = orderli;
            int count = 0;
            IList<EquipmentInfo> list = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
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
                case "0":
                    i = "停用";
                    break;
                case "1":
                    i = "启用";
                    break;
            }
            return i;
        }

        //获取分类名称
        public string GetType(string typeID)
        {
            EquipmentTypeInfo objType = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(int.Parse(typeID));
            return objType != null ? objType.TypeName : "";
        }

        //获取单位
        public string GetUnitName(string state)
        {
            string i = "";
            switch (state)
            {
                case "1":
                    i = "樘";
                    break;
                case "2":
                    i = "米";
                    break;
                case "3":
                    i = "平方米";
                    break;
            }
            return i;
        }

        //获取单位
        public string GetCalcUnitName(string state)
        {
            string i = "";
            switch (state)
            {
                case "1":
                    i = "单位数量";
                    break;
                case "2":
                    i = "三方周长";
                    break;
                case "3":
                    i = "三方周长";
                    break;
                case "4":
                    i = "面积";
                    break;
            }
            return i;
        }

        //获取单位
        public string GetPassUnit(string state)
        {
            string i = "";
            switch (state)
            {
                case "1":
                    i = "按公分计算";
                    break;
                case "2":
                    i = "单价加价";
                    break;
                case "3":
                    i = "面积加价";
                    break;
            }
            return i;
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
            CheckPowerWithWindowField("CoreEquipmentEdit", Grid1, "editField");
            CheckPowerWithLinkButtonField("CoreEquipmentDelete", Grid1, "deleteField");
        }

        protected void btnDisableUsers_Click(object sender, EventArgs e)
        {
            SetSelectedUsersEnableStatus(false);
        }

        private void SetSelectedUsersEnableStatus(bool enabled)
        {
            string isUsed = enabled ? "1" : "0";
            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            // 执行数据库操作
            foreach (int ID in ids)
            {
                EquipmentInfo entity = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(ID);
                entity.IsUsed = isUsed;
                Core.Container.Instance.Resolve<IServiceEquipmentInfo>().Update(entity);
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
            object[] values = Grid1.DataKeys[e.RowIndex];
            int goodsID = Convert.ToInt32(values[0]);

            int ID = GetSelectedDataKeyID(Grid1);
            if (e.CommandName == "Delete")
            {
                Core.Container.Instance.Resolve<IServiceEquipmentInfo>().Delete(ID);
                BindGrid();
            }
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            foreach (int id in ids)
            {
                Core.Container.Instance.Resolve<IServiceEquipmentInfo>().Delete(id);
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

        protected void Grid2_RowClick(object sender, FineUIPro.GridRowClickEventArgs e)
        {
            int classID = GetSelectedDataKeyID(Grid2);
            Session["EquipmentInfoType"] = classID;
            BindGrid();
        }

        protected void Grid1_RowDoubleClick(object sender, GridRowClickEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            PageContext.RegisterStartupScript(Window1.GetShowReference(string.Format("~/Equipment/EquipmentEdit.aspx?id={0}", ID)
                , "编辑商品商品"));

        }

        #endregion

    }
}