using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using FineUIPro;

namespace ZAJCZN.MIS.Web
{
    public partial class OnlinePayInfoList : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreOnlinePayInfoView";
            }
        }

        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        #endregion Page_Load

        #region BindGrid

        //支付类型
        protected string GetPayType(string _pay_type)
        {
            return _pay_type.Equals("010")?"微信":"支付宝";
        }

        protected string GetTime(string _end_time)
        {
            return DateTime.ParseExact(_end_time, "yyyyMMddHHmmss",null).ToString("yyyy-MM-dd HH:mm:ss");
        }

        protected void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            if (!string.IsNullOrEmpty(dpStartDate.Text.Trim()))
                qryList.Add(Expression.Ge("end_time", dpStartDate.Text.Trim().Replace("-","")));
            if (!string.IsNullOrEmpty(dpEndDate.Text.Trim()))
                qryList.Add(Expression.Lt("end_time",DateTime.Parse(dpEndDate.Text.Trim()).AddDays(1).ToString("yyyyMMdd")));
            if (ddlPayType.SelectedValue!="0")
                qryList.Add(Expression.Eq("pay_type", ddlPayType.SelectedValue));
            Order[] orderList = new Order[1];
            Order orderli = new Order(Grid1.SortField, Grid1.SortDirection == "DESC" ? true : false);
            orderList[0] = orderli;
            int count = 0;
            IList<tm_OnlinePayInfo> list = Core.Container.Instance.Resolve<IServiceOnlinePayInfo>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        #endregion BindGrid

        #region Evevts

        protected void btnSearch_Click(object sender, EventArgs e)
        {
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

        //protected void WindowOnlinePayInfo_Close(object sender, FineUIPro.WindowCloseEventArgs e)
        //{

        //}

        #endregion Evevts
    }
}