using FineUIPro;
using ZAJCZN.MIS.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZAJCZN.MIS.Web
{
    public partial class RPTTableUsed : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreReportTableUsed";
            }
        }

        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dpkStarttime.Text = DateTime.Now.AddMonths(-1).ToString();
                dpkEndtime.Text = DateTime.Now.ToString();
            }
        }

        #endregion

        #region 绑定数据

        protected void BindGrid()
        {
            string sql = "SELECT tabie.TabieName," +
                "sum(Population) as Population," +
                "sum(Moneys) as Moneys," +
                "COUNT(tui.TabieID) as usecount " +
                "FROM tm_tabieusinginfo tui " +
                "LEFT JOIN tm_tabie tabie on(tui.TabieID=tabie.ID) " +
                GetSqlWhere() +
                " GROUP BY tui.TabieID " +
                "ORDER BY usecount DESC";
            DataSet ds = DbHelperMySQL.Query(sql + " limit " + Grid1.PageIndex * Grid1.PageSize + "," + Grid1.PageSize);
            int count = Int32.Parse(DbHelperMySQL.GetSingle("select count(1) from(" + sql + ") aa").ToString());
            if (ds.Tables[0] != null)
            {
                Grid1.RecordCount = count;
                Grid1.DataSource = ds.Tables[0];
                Grid1.DataBind();
            }
        }

        private string GetSqlWhere()
        {
            StringBuilder js = new StringBuilder();
            js.Append("where 1=1");
            if (!string.IsNullOrEmpty(dpkStarttime.Text.Trim()))
                js.AppendFormat(" and tui.OpenTime>='{0}'", dpkStarttime.Text.Trim());
            if (!string.IsNullOrEmpty(dpkEndtime.Text.Trim()))
                js.AppendFormat(" and tui.OpenTime<'{0}'", DateTime.Parse(dpkEndtime.Text.Trim()).AddDays(1).ToString("yyyy-MM-dd"));
            return js.ToString();
        }

        #endregion

        #region 导出Excel

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string htmls = GetAllTableHtml();
            if (htmls != "")
            {
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=餐台使用率报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                Response.ContentType = "application/excel";
                Response.ContentEncoding = Encoding.UTF8;
                Response.Write(htmls);
                Response.End();
            }
            btnExcel.Enabled = false;
        }

        private string GetAllTableHtml()
        {
            string satrtdate = dpkStarttime.Text;
            string enddate = dpkEndtime.Text;
            //查询
            string sql = "SELECT tabie.TabieName," +
                "sum(Population) as Population," +
                "sum(Moneys) as Moneys," +
                "COUNT(tui.TabieID) as usecount " +
                "FROM tm_tabieusinginfo tui " +
                "LEFT JOIN tm_tabie tabie on(tui.TabieID=tabie.ID) " +
                GetSqlWhere() +
                " GROUP BY tui.TabieID " +
                "ORDER BY usecount DESC";
            DataSet ds = DbHelperMySQL.Query(sql);
            if (ds.Tables[0].Rows.Count != 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

                #region - 拼凑主订单导出结果 -
                sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

                #region - 拼凑导出的列名 -
                sb.Append("<tr>");
                sb.AppendFormat("<td colspan=\"5\" style=\"text-align:center\">{0}</td>", satrtdate + "至" + enddate);
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "使用排名");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "餐台名称");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "使用次数");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "使用人数");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "总消费金额");
                sb.Append("</tr>");

                #endregion

                #region - 拼凑导出的数据行 -
                int recordIndex1 = 1;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", recordIndex1);
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", row["TabieName"].ToString());
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", row["usecount"].ToString());
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", row["Population"].ToString());
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", row["Moneys"].ToString());
                    sb.Append("</tr>");
                    recordIndex1++;
                }
                #endregion

                sb.Append("</table>");

                #endregion

                return sb.ToString();
            }
            else
                return "";
        }

        #endregion

        #region Events

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            btnExcel.Enabled = true;
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