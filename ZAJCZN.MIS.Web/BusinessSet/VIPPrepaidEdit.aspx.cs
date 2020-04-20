using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class VIPPrepaidEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreVIPPrepaidView";
            }
        }

        #endregion

        #region Page_Load

        protected int _id
        {
            get
            {
                return GetQueryIntValue("id");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        #endregion

        #region 绑定数据

        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("VipID", _id));
            if (!string.IsNullOrEmpty(dpkSatrt.Text.Trim()))
                qryList.Add(Expression.Ge("PrepaidDate",DateTime.Parse(dpkSatrt.Text.Trim())));
            if (!string.IsNullOrEmpty(dpkEnd.Text.Trim()))
                qryList.Add(Expression.Lt("PrepaidDate", DateTime.Parse(dpkEnd.Text.Trim()).AddDays(1)));
            Order[] orderList = new Order[1];
            Order orderli = new Order(Grid1.SortField, Grid1.SortDirection == "DESC" ? true : false);
            orderList[0] = orderli;
            int count = 0;
            IList<tm_VIPPrepaid> list = Core.Container.Instance.Resolve<IServiceVIPPrepaid>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected string GetType(string PrepaidWay)
        {
            string i = "";
            switch (PrepaidWay)
            {
                case "1":
                    i = "刷卡";
                    break;
                case "2":
                    i = "现金";
                    break;
                case "3":
                    i = "微信";
                    break;
                case "4":
                    i = "支付宝";
                    break;
            }
            return i;
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

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindGrid();
        }

        #endregion

        
    }
}