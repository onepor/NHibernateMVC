using FineUIPro;
using ZAJCZN.MIS.Helpers;
using ZAJCZN.MIS.Helpers.DEncrypt;
using System;
using System.Data;
using System.Text;

namespace ZAJCZN.MIS.Web
{
    public partial class RPTChargeDetail : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreReportView";
            }
        }

        #endregion

        #region Page_Load

        protected string _Charge
        {
            get { return GetQueryValue("Charge"); }
        }

        protected string _sql
        {
            get { return DESEncrypt.Decrypt(GetQueryValue("sql")); }
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

        protected void BindGrid()
        {
            string sql = "SELECT Charge,FactPrice,ClearTime " +
                         "FROM tm_tabieusinginfo " +
                         "where Charge='" + _Charge + "'" + _sql +
                         " ORDER BY FactPrice DESC ";
            DataSet ds = DbHelperSQL.Query(sql + " limit " + Grid1.PageIndex * Grid1.PageSize + "," + Grid1.PageSize);
            int count = Int32.Parse(DbHelperSQL.GetSingle("select count(1) from(" + sql + ") aa").ToString());
            if (ds.Tables[0] != null)
            {
                Grid1.RecordCount = count;
                Grid1.DataSource = ds.Tables[0];
                Grid1.DataBind();
            }
        }

        #endregion

        #region 导出Excel

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string htmls = GetAllTableHtml();
            if (htmls != "")
            {
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=挂账明细报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                Response.ContentType = "application/excel";
                Response.ContentEncoding = Encoding.UTF8;
                Response.Write(htmls);
                Response.End();
            }
        }

        private string GetAllTableHtml()
        {
            //查询
            string sql = "SELECT Charge,FactPrice,ClearTime " +
                         "FROM tm_tabieusinginfo " +
                         "where Charge='" + _Charge + "'" + _sql +
                         " ORDER BY FactPrice DESC ";
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0].Rows.Count != 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

                #region - 拼凑主订单导出结果 -
                sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

                #region - 拼凑导出的列名 -

                sb.Append("<tr>");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "挂账排名");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "挂账人");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "挂账金额");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "挂账时间");
                sb.Append("</tr>");

                #endregion

                #region - 拼凑导出的数据行 -
                int recordIndex1 = 1;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", recordIndex1);
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", row["Charge"].ToString());
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", row["FactPrice"].ToString());
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", row["ClearTime"].ToString());
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