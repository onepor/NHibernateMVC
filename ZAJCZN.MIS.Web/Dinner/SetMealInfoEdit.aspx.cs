using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Data;
using System.Linq;

namespace ZAJCZN.MIS.Web
{
    public partial class SetMealInfoEdit : PageBase
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

        #region 加载

        protected int _id
        {
            get { return GetQueryIntValue("id"); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
                LoadData();
            }
        }
      

        private void LoadData()
        {
            //权限检查
            CheckPowerWithButton("CoreSetMealInfoEdit", btnNew);

            if (_id == 0)
            {
                tm_SetMealInfo entity = new tm_SetMealInfo();
                entity.SetTime = DateTime.Now.ToString("yyyy-MM-dd");
                Core.Container.Instance.Resolve<IServiceSetMealInfo>().Create(entity);
                txbhidden.Text = entity.ID.ToString(); 
                btnNew.OnClientClick = Window1.GetShowReference("~/Dinner/SetMealInfoEditSelect.aspx?setmealid=" + entity.ID, "添加菜品");
            }
            else
            {
                txbhidden.Text = _id.ToString();
                btnNew.OnClientClick = Window1.GetShowReference("~/Dinner/SetMealInfoEditSelect.aspx?setmealid=" + _id, "添加菜品");
                BindGrid();
                BindGrid2();
            }
            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
        }
        #endregion

        #region 绑定数据


        private void BindGrid2()
        {
            tm_SetMealInfo entity = Core.Container.Instance.Resolve<IServiceSetMealInfo>().GetEntity(Int32.Parse(txbhidden.Text));
            txbSetMealName.Text = entity.SetMealName; 
            numPreferentialPrice.Text = entity.PreferentialPrice.ToString();
            dateStart.Text = entity.StartTime;
            dateFinish.Text = entity.FinishTime;
            rblEnabled.SelectedValue = entity.IsEnabled;
        }

        //已修正

        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("SetMealID", Int32.Parse(txbhidden.Text)));
            Order[] orderList = new Order[1];
            Order orderli = new Order(Grid1.SortField, Grid1.SortDirection == "ASC" ? true : false);
            orderList[0] = orderli;
            int count = 0;
            IList<tm_SetMealDetail> list = Core.Container.Instance.Resolve<IServiceSetMealDetail>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
            decimal? price = 0;
            foreach (tm_SetMealDetail entity in list)
            {
                price += entity.TotalPrice;
            }
            labPrice.Text = "￥" + price.ToString();
        }


        public string GetDishesNmae(string id)
        {
            tm_Dishes entity = Core.Container.Instance.Resolve<IServiceDishes>().GetEntity(Int32.Parse(id));
            return entity == null ? "" : entity.DishesName;
        }


        public string GetPrice(string id)
        {
            tm_Dishes entity = Core.Container.Instance.Resolve<IServiceDishes>().GetEntity(Int32.Parse(id));
            return entity == null ? "" : entity.SellPrice.ToString();
        }

        #endregion

        #region Events

        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            Dictionary<int, Dictionary<string, object>> modifiedDict = Grid1.GetModifiedDict();
            foreach (int rowIndex in modifiedDict.Keys)
            {
                int rowID = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
                tm_SetMealDetail objInfo = Core.Container.Instance.Resolve<IServiceSetMealDetail>().GetEntity(rowID);
                objInfo.DishCount = Int32.Parse(modifiedDict[rowIndex]["DishCount"].ToString());
                objInfo.TotalPrice = objInfo.DishCount * objInfo.Price;
                Core.Container.Instance.Resolve<IServiceSetMealDetail>().Update(objInfo);
            }
            BindGrid();
        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();
        }


        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            if (e.CommandName == "Delete")
            {
                Core.Container.Instance.Resolve<IServiceSetMealDetail>().Delete(ID);
                BindGrid();
            }
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

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();

        }

        #endregion

        #region 保存

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            Save();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        private void Save()
        {
            tm_SetMealInfo entity = Core.Container.Instance.Resolve<IServiceSetMealInfo>().GetEntity(Int32.Parse(txbhidden.Text));
            entity.SetMealName = txbSetMealName.Text.Trim();
             entity.Price = decimal.Parse(labPrice.Text.Replace("￥", ""));
            entity.PreferentialPrice = numPreferentialPrice.Text==""?0: decimal.Parse(numPreferentialPrice.Text);
            entity.StartTime = dateStart.Text;
            entity.FinishTime = dateFinish.Text;
            entity.IsEnabled = rblEnabled.SelectedValue;
            Core.Container.Instance.Resolve<IServiceSetMealInfo>().Update(entity);
        }

        #endregion
    }
}