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
    public partial class SetMealInfoEditSelect : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreSetMealInfoEdit";
            }
        }

        #endregion

        #region Page_Load

        protected int _setmealid
        {
            get { return GetQueryIntValue("setmealid"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 每页记录数
                Grid2.PageSize = ConfigHelper.PageSize;
                ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
                BindFoodClass();
                LoadData();
            }
        }

        private void BindFoodClass()
        {
            IList<tm_FoodClass> classList = Core.Container.Instance.Resolve<IServiceFoodClass>().GetAll();
            ddlFoodClass.DataSource = classList;
            ddlFoodClass.DataBind();
        }

        private void LoadData()
        {
            //绑定物品类别
            BindGrid2();
        }

        #endregion

        #region
        private void BindGrid2()
        {
            IList<ICriterion> qrylist = new List<ICriterion>();
            qrylist.Add(Expression.Eq("ClassID", int.Parse(ddlFoodClass.SelectedValue)));
            qrylist.Add(Expression.Eq("IsUsed", 1));
            IList<tm_Dishes> list = Core.Container.Instance.Resolve<IServiceDishes>().GetAllByKeys(qrylist);
            Grid2.DataSource = list;
            Grid2.DataBind();
        }
        #endregion

        #region 条件查询

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedDataKeyIDs(Grid2);
            foreach (int id in ids)
            {
                tm_Dishes entity = Core.Container.Instance.Resolve<IServiceDishes>().GetEntity(id);
                if (entity.ID == 3)
                {
                    IList<ICriterion> qrylist = new List<ICriterion>();
                    qrylist.Add(Expression.Eq("DishID", id));
                    qrylist.Add(Expression.Eq("SetMealID", _setmealid));
                    IList<tm_SetMealDetail> lst = Core.Container.Instance.Resolve<IServiceSetMealDetail>().Query(qrylist);
                    if (lst.Count > 0)
                    {
                        Alert.ShowInTop("已添加菜品，请直接修改数量！", "错误操作", MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        tm_SetMealDetail sb = new tm_SetMealDetail();
                        sb.DishID = id;
                        sb.DishCount = 1;
                        sb.Price = entity.SellPrice;
                        sb.TotalPrice = entity.SellPrice;
                        sb.SetMealID = _setmealid;
                        Core.Container.Instance.Resolve<IServiceSetMealDetail>().Create(sb);
                    }
                }
                else
                {
                    tm_SetMealDetail sb = new tm_SetMealDetail();
                    sb.DishID = id;
                    sb.DishCount = 1;
                    sb.Price = entity.SellPrice;
                    sb.TotalPrice = entity.SellPrice;
                    sb.SetMealID = _setmealid;
                    Core.Container.Instance.Resolve<IServiceSetMealDetail>().Create(sb);
                }
            }
            Alert.ShowInTop("已添加菜品！", "操作成功", MessageBoxIcon.Success);
            LoadData();
            //PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        #endregion

        #region Events

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid2.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindGrid2();
        }

        protected void ddlFoodClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
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

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

    }
}
