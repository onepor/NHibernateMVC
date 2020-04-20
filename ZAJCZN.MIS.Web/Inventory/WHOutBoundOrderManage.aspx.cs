using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class WHOutBoundOrderManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreHWOrderOutView";
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
            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
            //加载物品信息
            BindGrid();
        }
        #endregion 

        #region 绑定数据
        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            string qryName = txtOrderNo.Text.Trim();
            if (!string.IsNullOrEmpty(qryName))
            {
                qryList.Add(Expression.Like("OrderNO", qryName, MatchMode.Anywhere) || Expression.Like("BOrderNO", qryName, MatchMode.Anywhere));
            }
            //if (ddlSuplier.SelectedValue != "0")
            //{
            //    qryList.Add(Expression.Eq("SuplierID", int.Parse(ddlSuplier.SelectedValue)));
            //}
            if (!string.IsNullOrEmpty(dpStartDate.Text))
            {
                qryList.Add(Expression.Ge("OrderDate", DateTime.Parse(dpStartDate.Text)));
            }
            if (!string.IsNullOrEmpty(dpEndDate.Text))
            {
                qryList.Add(Expression.Le("OrderDate", DateTime.Parse(dpEndDate.Text)));
            }
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", false);
            orderList[0] = orderli;
            int count = 0;
            IList<WHOutBoundOrder> list = Core.Container.Instance.Resolve<IServiceWHOutBoundOrder>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
        }
        #endregion

        #region 页面数据转换
        
        //获取库房名称
        public string GetWHName(string id)
        {
            WareHouse whInfo = Core.Container.Instance.Resolve<IServiceWareHouse>().GetEntity(int.Parse(id));
            return whInfo != null ? whInfo.WHName : "";
        }
        #endregion

        #region Events

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
            if (e.CommandName == "viewField")
            {
                PageContext.Redirect(string.Format("~/Inventory/WHOutBoundOrderView.aspx?id={0}", ID));
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

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        #endregion

    }
}