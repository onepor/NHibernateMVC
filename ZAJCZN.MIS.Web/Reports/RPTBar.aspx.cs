using ZAJCZN.MIS.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;

namespace ZAJCZN.MIS.Web
{
    public partial class RPTBar : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreReportSalePoint";
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
            int searchWay = Int32.Parse(ddlSearchWay.SelectedValue);
            string search = "";
            if (searchWay == 1)
            {
                search = "%Y-%m-%d";
                hfBarOptionsUnit.Value = "金额（元）";
            }
            else if (searchWay == 30)
            {
                search = "%Y-%m";
                hfBarOptionsUnit.Value = "金额（万元）";
            }
            else if (searchWay == 365)
            {
                search = "%Y";
                hfBarOptionsUnit.Value = "金额（万元）";
            }


            string sqlwhere = "SELECT sum(Moneys)+sum(GroupMoneys) as Moneys," +
                "sum(FactPrice)+sum(GroupMoneys) as FactPrice," +
                "convert(DATE_FORMAT(OpenTime, '" + search + "')using utf8) as time " +
                "FROM tm_tabieusinginfo WHERE OpenTime >= " +
                "'" + satrtdate + "' AND OpenTime < '" + enddate + "' " +
                "GROUP BY DATE_FORMAT(OpenTime,'" + search + "') ";
            DataSet ds = DbHelperSQL.Query(sqlwhere);

            if (ds.Tables[0] != null)
            {
                string dates = String.Empty;
                string growth = String.Empty;
                string moneys = String.Empty;
                string factpre = String.Empty;
                int x = 1;
                decimal PastMoney = 0;
                decimal yMoney = 0;
                decimal yGrowthMax = 0;
                decimal yGrowthMin = 0;
                decimal eachGroeth = 0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    //图形y轴范围确定
                    eachGroeth = (decimal.Parse(row["Moneys"].ToString()) - PastMoney) / (PastMoney == 0 ? decimal.Parse(row["Moneys"].ToString()) : PastMoney) * 100;
                    yGrowthMax = eachGroeth > yGrowthMax ? eachGroeth : yGrowthMax;
                    yGrowthMin = eachGroeth > yGrowthMin ? yGrowthMin : eachGroeth;

                    //y轴最大金额确认
                    yMoney = decimal.Parse(row["Moneys"].ToString()) > yMoney ? decimal.Parse(row["Moneys"].ToString()) : yMoney;

                    if (ds.Tables[0].Rows.Count == x)
                    {

                        dates += "{\"value\":\"" + row["time"].ToString() + "\"}";

                        growth += eachGroeth.ToString("#.##");
                        PastMoney = decimal.Parse(row["Moneys"].ToString());

                        if (searchWay == 1 || searchWay == 7)
                        {
                            moneys += row["Moneys"].ToString();
                            factpre += row["FactPrice"].ToString();
                        }
                        else
                        {
                            moneys += (decimal.Parse(row["Moneys"].ToString()) / 10000).ToString("#.##");
                            factpre += (decimal.Parse(row["FactPrice"].ToString()) / 10000).ToString("#.##");
                            yMoney = yMoney / 10000;
                        }
                    }
                    else
                    {
                        dates += "{\"value\":\"" + row["time"].ToString() + "\"},";

                        growth += eachGroeth.ToString("#.##") + ",";
                        PastMoney = decimal.Parse(row["Moneys"].ToString());

                        if (searchWay == 1)
                        {
                            moneys += row["Moneys"].ToString() + ",";
                            factpre += row["FactPrice"].ToString() + ",";
                        }
                        else
                        {
                            moneys += (decimal.Parse(row["Moneys"].ToString()) / 10000).ToString("#.##") + ",";
                            factpre += (decimal.Parse(row["FactPrice"].ToString()) / 10000).ToString("#.##") + ",";
                            yMoney = yMoney / 10000;
                        }
                    }
                    x++;
                }

                hfBarOptionsMax.Value = (yMoney + yMoney / 4).ToString("#");
                hfBarOptionsyGrowthMax.Value = (yGrowthMax + (yGrowthMax - yGrowthMin) / 7).ToString("#");
                hfBarOptionsyGrowthMin.Value = (yGrowthMin > 0 ? (yGrowthMin + (yGrowthMax - yGrowthMin) / 7) : (yGrowthMin - (yGrowthMax - yGrowthMin) / 7)).ToString("#");
                hfBarOptionsDates.Value = dates;
                hfBarOptionsGrowth.Value = growth;
                hfBarOptionsMoneys.Value = moneys;
                hfBarOptionsFactpre.Value = factpre;
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
                Response.AddHeader("content-disposition", "attachment; filename=营业财务报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
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
            int searchWay = Int32.Parse(ddlSearchWay.SelectedValue);
            string search = "";
            if (searchWay == 1)
                search = "%Y-%m-%d";
            else if (searchWay == 30)
                search = "%Y-%m";
            else if (searchWay == 365)
                search = "%Y";

            enddate = DateTime.Parse(enddate).AddDays(1).ToString("yyyy-MM-dd");
            //查询
            string sqlwhere = "SELECT sum(Moneys)+sum(GroupMoneys) as Moneys," +
                "sum(FactPrice)+sum(GroupMoneys) as FactPrice," +
                "convert(DATE_FORMAT(OpenTime, '" + search + "')using utf8) as time " +
                "FROM tm_tabieusinginfo WHERE OpenTime >= " +
                "'" + satrtdate + "' AND OpenTime < '" + enddate + "' " +
                "GROUP BY DATE_FORMAT(OpenTime,'" + search + "') ";
            DataSet ds = DbHelperSQL.Query(sqlwhere);
            if (ds.Tables[0].Rows.Count != 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

                #region - 拼凑主订单导出结果 -
                sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

                #region - 拼凑导出的列名 -
                sb.Append("<tr>");
                sb.AppendFormat("<td colspan=\"3\" style=\"text-align:center\">{0}</td>", satrtdate + "至" + enddate);
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "日期");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "订单金额");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "实收金额");
                sb.Append("</tr>");

                #endregion

                #region - 拼凑导出的数据行 -
                int recordIndex1 = 1;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", row["time"].ToString());
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", row["Moneys"].ToString());
                    sb.AppendFormat("<td style=\"text-align:center\">{0}%</td>", row["FactPrice"].ToString());
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