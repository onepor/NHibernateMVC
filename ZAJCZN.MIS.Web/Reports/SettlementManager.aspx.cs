using FineUIPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using NHibernate.Criterion;
using System.Text;

namespace ZAJCZN.MIS.Web
{
    public partial class SettlementManager : PageBase
    {

        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreCateringView";
            }
        }

        #endregion


        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dpStartDate.Text = DateTime.Now.AddMonths(-1).ToString();
                dpEndDate.Text = DateTime.Now.ToString();
            }
        }

        #endregion Page_Load

        #region BindGrid

        protected void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();

            if (!string.IsNullOrEmpty(dpStartDate.Text))
            {
                qryList.Add(Expression.Ge("SettlementDate", dpStartDate.Text));
            }
            if (!string.IsNullOrEmpty(dpEndDate.Text))
            {
                qryList.Add(Expression.Lt("SettlementDate", DateTime.Parse(dpEndDate.Text).AddDays(1).ToString("yyyy-MM-dd")));
            }
            Order[] orderList = new Order[1];
            Order orderli = new Order("SettlementDate", false);
            orderList[0] = orderli;
            int count = 0;
            IList<tm_Settlement> list = Core.Container.Instance.Resolve<IServiceSettlement>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        #endregion BindGrid

        #region Events

        protected void WindowSettlement_Close(object sender, WindowCloseEventArgs e)
        {
            BindGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
            btnExcel.Enabled = true;
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

        #endregion Events

        #region 导出Excel

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string htmls = GetAllTableHtml();
            if (htmls != "")
            {
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=交班信息报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                Response.ContentType = "application/excel";
                Response.ContentEncoding = Encoding.UTF8;
                Response.Write(htmls);
                Response.End();
            }
            btnExcel.Enabled = false;
        }

        private string GetAllTableHtml()
        {
            string satrtdate = dpStartDate.Text;
            string enddate = dpEndDate.Text;

            //查询
            IList<ICriterion> qryList = new List<ICriterion>();
            if (!string.IsNullOrEmpty(dpStartDate.Text))
            {
                qryList.Add(Expression.Ge("SettlementDate", dpStartDate.Text));
            }
            if (!string.IsNullOrEmpty(dpEndDate.Text))
            {
                qryList.Add(Expression.Lt("SettlementDate", DateTime.Parse(dpEndDate.Text).AddDays(1).ToString("yyyy-MM-dd")));
            }
            IList<tm_Settlement> list = Core.Container.Instance.Resolve<IServiceSettlement>().Query(qryList);

            
            StringBuilder sb = new StringBuilder();
            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

            #region - 拼凑主订单导出结果 -
            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

            #region - 拼凑导出的列名 -
            sb.Append("<tr>");
            sb.AppendFormat("<td colspan=\"24\" style=\"text-align:center\">{0}</td>", satrtdate + "至" + enddate);
            sb.Append("</tr>");

            sb.Append("<tr>");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "排序");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "结算时间");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "打印时间");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "客数");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "订单数");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "应收金额");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "免单金额");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "挂账金额");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "赠送金额");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "折让金额");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "实收金额");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "现金金额");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "微信金额");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "支付宝");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "刷卡金额");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "会员抵扣");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "团购卷");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "退菜金额");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "现金金额(实)");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "微信金额(实)");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "支付宝金额(实)");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "刷卡金额(实)");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "会员抵扣(实)");
            sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "团购卷金额(实)");
            sb.Append("</tr>");

            #endregion

            #region - 拼凑导出的数据行 -
            int recordIndex1 = 1;
            foreach (tm_Settlement entity in list)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", recordIndex1);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>",entity.SettlementDate);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.PrintTime);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.CustomerCount);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.OrderCount);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.AmountReceivable);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.SingleAmount);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.ChargeAmount);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.DonationAmount);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.DiscountAmount);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.AmountCollected);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.CashAmount);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.WXAmount);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.ZFBAmount);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.CardAmount);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.MemberAmount);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.GroupAmount);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.BackAmount);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>",entity.ACCashAmount );
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.ACWXAmount);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.ACZFBAmount);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.ACCardAmount);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.ACMemberAmount);
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", entity.ACGroupAmount);
                sb.Append("</tr>");
                recordIndex1++;
            }
            #endregion

            sb.Append("</table>");

            #endregion

            return sb.ToString();
        }

        #endregion
    }
}