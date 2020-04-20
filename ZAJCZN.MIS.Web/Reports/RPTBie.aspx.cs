using ZAJCZN.MIS.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;

namespace ZAJCZN.MIS.Web
{
    public partial class RPTBie : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreReportDishesPoint";
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnExcel.Enabled = false;
                dpkStart.Text = DateTime.Now.AddMonths(-1).ToString();
                dpkEnd.Text = DateTime.Now.ToString();
            }
        }

        #region 查询

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string satrtdate = dpkStart.Text;
            string enddate = dpkEnd.Text == "" ? DateTime.Now.ToString("yyyy-MM-dd") : dpkEnd.Text;
            string EndDate = enddate;
            enddate = DateTime.Parse(enddate).AddDays(1).ToString("yyyy-MM-dd");

            string sqlwhere = "SELECT sum(DishesCount) as DishesCount,tm_foodclass.ClassName " +
            "FROM tm_tabiedishesinfo " +
            "LEFT JOIN tm_dishes ON DishesID = tm_dishes.ID " +
            "LEFT JOIN tm_foodclass on ClassID=tm_foodclass.ID " +
            "WHERE TabieUsingID IN ( " +
            "SELECT ID " +
            "FROM tm_tabieusinginfo " +
            "WHERE " +
            "tm_tabieusinginfo.OpenTime >= '" + satrtdate + "' " +
            "AND tm_tabieusinginfo.OpenTime < '" + enddate + "' " +
            ") " +
            "GROUP BY ClassID";
            DataSet ds = DbHelperSQL.Query(sqlwhere);

            if (ds.Tables[0] != null)
            {
                string data = String.Empty;
                int x = 1;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (ds.Tables[0].Rows.Count == x)
                        data += "{\"value\":" + row["DishesCount"].ToString() + ",\"name\":\"" + row["ClassName"].ToString() + "\"}";
                    else
                        data += "{\"value\":" + row["DishesCount"].ToString() + ",\"name\":\"" + row["ClassName"].ToString() + "\"},";
                    x++;
                }
                hfBarOptions.Value = data;
            }
            hfTitle.Value = (satrtdate == "" ? "开始" : satrtdate) + " 至 " + EndDate;
            btnExcel.Enabled = true;
        }

        #endregion

        #region 导出Excel

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string htmls = GetAllTableHtml();
            if (htmls != "")
            {
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=菜品类别销售情况报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                Response.ContentType = "application/excel";
                Response.ContentEncoding = Encoding.UTF8;
                Response.Write(htmls);
                Response.End();
            }
        }

        private string GetAllTableHtml()
        {
            string satrtdate = dpkStart.Text;
            string enddate = dpkEnd.Text == "" ? DateTime.Now.ToString("yyyy-MM-dd") : dpkEnd.Text;
            enddate = DateTime.Parse(enddate).AddDays(1).ToString("yyyy-MM-dd");
            //查询
            string sqlwhere = "SELECT convert(sum(DishesCount) /(" +
                "SELECT sum(DishesCount) " +
                "FROM tm_tabiedishesinfo " +
                "WHERE TabieUsingID IN("+
                "SELECT ID FROM tm_tabieusinginfo WHERE "+
                "tm_tabieusinginfo.OpenTime >= '" + satrtdate + "' " +
                "AND tm_tabieusinginfo.OpenTime < '" + enddate + "'" +
                "))*100,decimal) as ratio," +
                "sum(DishesCount) as DishesCounts,tm_foodclass.ClassName " +
                "FROM tm_tabiedishesinfo " +
                "LEFT JOIN tm_dishes ON DishesID = tm_dishes.ID " +
                "LEFT JOIN tm_foodclass on ClassID=tm_foodclass.ID " +
                "WHERE TabieUsingID IN ( " +
                "SELECT ID " +
                "FROM tm_tabieusinginfo " +
                "WHERE " +
                "tm_tabieusinginfo.OpenTime >= '" + satrtdate + "' " +
                "AND tm_tabieusinginfo.OpenTime < '" + enddate + "' " +
                ") " +
                "GROUP BY ClassID ORDER BY DishesCounts DESC";
            DataSet ds = DbHelperSQL.Query(sqlwhere);
            if (ds.Tables[0].Rows.Count != 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

                #region - 拼凑主订单导出结果 -
                sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

                #region - 拼凑导出的列名 -
                sb.Append("<tr>");
                sb.AppendFormat("<td colspan=\"4\" style=\"text-align:center\">{0}</td>", satrtdate+"至"+ enddate);
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "销售排名");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "菜品分类");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "销售数量");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "销售比例");
                sb.Append("</tr>");

                #endregion

                #region - 拼凑导出的数据行 -
                int recordIndex1 = 1;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", recordIndex1);
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", row["ClassName"].ToString());
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", row["DishesCounts"].ToString());
                    sb.AppendFormat("<td style=\"text-align:center\">{0}%</td>", row["ratio"].ToString());
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


    }
}