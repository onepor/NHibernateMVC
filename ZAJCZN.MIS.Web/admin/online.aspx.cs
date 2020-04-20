using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using FineUIPro;
using System.Data.Entity;
using NHibernate.Criterion;
using ZAJCZN.MIS.Service;
using ZAJCZN.MIS.Domain;


namespace ZAJCZN.MIS.Web.admin
{
    public partial class online : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreOnlineView";
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
            // 每页记录数
            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();

            BindGrid();
        }

        private void BindGrid()
        {
            DateTime lastD = DateTime.Now.AddHours(-2);
            // 在用户名中搜索
            string searchText = ttbSearchMessage.Text.Trim();
            IList<ICriterion> qryList = new List<ICriterion>();
            if (!String.IsNullOrEmpty(searchText))
            {
                qryList.Add(Expression.Like("UserName", searchText, MatchMode.Anywhere));
            }
            qryList.Add(Expression.Gt("UpdateTime", lastD));

            Order[] orderList = new Order[1];
            Order orderli = new Order(Grid1.SortField, Grid1.SortDirection == "ASC" ? true : false);
            orderList[0] = orderli;
            int count = 0;
            IList<onlines> objList = Core.Container.Instance.Resolve<IServiceOnlines>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);

            // 在添加条件之后，排序和分页之前获取总记录数
            Grid1.RecordCount = count;
            // 排列和数据库分页
            //q = SortAndPage<Online>(q, Grid1);
            Grid1.DataSource = objList;
            Grid1.DataBind();
        }

        #endregion

        #region Events

        protected void ttbSearchMessage_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearchMessage.ShowTrigger1 = true;
            BindGrid();
        }

        protected void ttbSearchMessage_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearchMessage.Text = String.Empty;
            ttbSearchMessage.ShowTrigger1 = false;
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


        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid();
        }

        #endregion

    }
}
