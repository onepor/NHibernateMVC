using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KZKJ.IOA.Domain;
using KZKJ.IOA.Service;
using System.Text;

namespace KZKJ.IOA.Web
{
    public partial class TabieUsingEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreTabieUsingView";
            }
        }

        #endregion

        #region Page_Load

        protected int _id
        {
            get { return GetQueryIntValue("id"); }
        }


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

            // 每页记录数
            Grid2.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
            //绑定物品类别
            BindGrid2();
        }

        #endregion

        #region
        private void BindGrid2()
        {
            IList<tm_Dishes> list = Core.Container.Instance.Resolve<IServiceDishes>().GetAll();
            Grid2.DataSource = list;
            Grid2.DataBind();
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

        public string GetClassName(string id)
        {
            if (int.Parse(id) > 0)
            {
                tm_FoodClass objType = Core.Container.Instance.Resolve<IServiceFoodClass>().GetEntity(int.Parse(id));
                return objType.ClassName;
            }
            return "";
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedDataKeyIDs(Grid2);
            decimal? Amount = 0;
            foreach (int id in ids)
            {
                tm_Dishes entity = Core.Container.Instance.Resolve<IServiceDishes>().GetEntity(id);
                tm_TabieDishesInfo sb = new tm_TabieDishesInfo();
                sb.DishesID = entity.ID;
                sb.DishesCount = 1;
                sb.Price = entity.SellPrice;
                sb.Moneys = 1 * entity.SellPrice;
                sb.DishesType = "1";
                sb.IsFree = "1";
                sb.TabieUsingID = _id;
                sb.DishesName = entity.DishesName;
                sb.UnitName = GetSystemEnumValue("CPDW", entity.DishesUnit.ToString());
                sb.IsPrint = 0;
                sb.PrintID = entity.PrinterID;
                Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().Create(sb);
                Amount += sb.Moneys;
            }
            tm_TabieUsingInfo taieusing = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(_id);
            taieusing.Moneys += Amount;
            taieusing.FactPrice += Amount;
            Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(taieusing);
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
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

        protected void Grid1_RowClick(object sender, FineUIPro.GridRowClickEventArgs e)
        {
            BindGrid2();
        }

        #endregion

        #region Grid2 Events


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


        #endregion

    }
}
