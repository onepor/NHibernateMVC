using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class RepairProjectManager : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreRepairProjectView";
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
            //权限检查
            CheckPowerWithButton("CoreRepairProjectNew", btnNew);

            btnNew.OnClientClick = Window1.GetShowReference("~/SysSet/RepairProjectEdit.aspx?action=add", "新增维修项信息");

            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
            BindGrid();
        }
        #endregion

        #region 绑定数据

        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            if (!string.IsNullOrEmpty(ddlState.SelectedValue))
            {
                qryList.Add(Expression.Eq("ProjectType", int.Parse(ddlState.SelectedValue)));
            }
            if (!string.IsNullOrEmpty(ddlUseRange.SelectedValue))
            {
                qryList.Add(Expression.Eq("UsingType", int.Parse(ddlUseRange.SelectedValue)));
            }
            Order[] orderList = new Order[1];
            Order orderli = new Order(Grid1.SortField, Grid1.SortDirection == "ASC" ? true : false);
            orderList[0] = orderli;
            int count = 0;
            IList<RepairProjectInfo> list = Core.Container.Instance.Resolve<IServiceRepairProjectInfo>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

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

        public string GetProjectType(string type)
        {
            string i = "";
            switch (type)
            {
                case "1":
                    i = "员工费用";
                    break;
                case "2":
                    i = "客户费用";
                    break;
                case "3":
                    i = "司机费用";
                    break;
            }
            return i;
        }

        public string GetUsingType(string type)
        {
            string i = "";
            switch (type)
            {
                case "1":
                    i = "场内";
                    break;
                case "2":
                    i = "发货";
                    break;
                case "3":
                    i = "收货";
                    break;
            }
            return i;
        }

        public string GetPriceSourceType(string type)
        {
            string i = "";
            // 费用价格获取类型  0：自定义  1：合同客户运费  2：合同司机运费  3：合同单价  4：合同维修单价
            switch (type)
            {
                case "0":
                    i = "";
                    break;
                case "1":
                    i = "合同客户运费";
                    break;
                case "2":
                    i = "合同司机运费";
                    break;
                case "3":
                    i = "合同单价";
                    break;
                case "4":
                    i = "合同维修单价";
                    break;
            }
            return i;
        }

        public string GetGoodsType(string type)
        {
            string i = "";
            EquipmentTypeInfo equipmentTypeInfo = new EquipmentTypeInfo();
            if (!string.IsNullOrEmpty(type))
            {
                string[] goodsIDs = type.Split(',');
                foreach (string id in goodsIDs)
                {
                    equipmentTypeInfo = new EquipmentTypeInfo();
                    equipmentTypeInfo = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(int.Parse(id));
                    i += string.Format("{0},", equipmentTypeInfo.TypeName);
                }
            }
            return i;
        }

        //获取单位
        public string GetUnitName(string state)
        {
            return GetSystemEnumValue("FYDW", state);

        }
        #endregion

        #region Events
        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreRepairProjectEdit", Grid1, "editField");
            CheckPowerWithLinkButtonField("CoreRepairProjectDelete", Grid1, "deleteField");
        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
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

            if (e.CommandName == "editField")
            {
                if (!CheckPower("CoreRepairProjectEdit"))
                    Alert.ShowInTop("您没有该操作权限，请从管理员处获取！", MessageBoxIcon.Information);
            }
            if (e.CommandName == "Delete")
            {
                if (!CheckPower("CoreRepairProjectDelete"))
                    Alert.ShowInTop("您没有该操作权限，请从管理员处获取！", MessageBoxIcon.Information);
                else
                {
                    Core.Container.Instance.Resolve<IServiceRepairProjectInfo>().Delete(ID);
                    BindGrid();
                }
            }
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

        protected void Grid1_RowDoubleClick(object sender, GridRowClickEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            PageContext.RegisterStartupScript(Window1.GetShowReference(string.Format("~/SysSet/RepairProjectEdit.aspx?id={0}&action=edit", ID)
                , "编辑维修项目信息"));

        }
        #endregion

    }
}