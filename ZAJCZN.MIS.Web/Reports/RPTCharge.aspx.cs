using FineUIPro;
using ZAJCZN.MIS.Helpers;
using ZAJCZN.MIS.Helpers.DEncrypt;
using System;
using System.Data;
using System.Text;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Web
{
    public partial class RPTCharge : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreReportCharge";
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
                ddlChargeBind();
            }
        }

        #endregion

        #region 绑定数据

        protected void ddlChargeBind()
        {
            IList<tm_Charge> chargesList = Core.Container.Instance.Resolve<IServiceCharge>().GetAll();
            ddlCharge.DataTextField = "ChargeName";
            ddlCharge.DataValueField = "ID";
            ddlCharge.DataSource = chargesList;
            ddlCharge.DataBind();
            ddlCharge.Items.Insert(0,new ListItem { Text = "--全部--", Value = "0" });
            ddlCharge.SelectedIndex = 0;
        }

        protected void BindGrid()
        {
            string sql = "SELECT Charge,sum(FactPrice) as Amount " +
                         "FROM tm_tabieusinginfo " +
                         "WHERE Charge IS NOT NULL AND Charge != '' " +
                         GetSqlWhere() +
                         " GROUP BY Charge ORDER BY Amount DESC";
            DataSet ds = DbHelperSQL.Query(sql + " limit " + Grid1.PageIndex * Grid1.PageSize + "," + Grid1.PageSize);
            int count = Int32.Parse(DbHelperSQL.GetSingle("select count(1) from(" + sql + ") aa").ToString());
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
            if (!string.IsNullOrEmpty(dpkStarttime.Text.Trim()))
                js.AppendFormat(" and ClearTime>='{0}'", dpkStarttime.Text.Trim());
            if (!string.IsNullOrEmpty(dpkEndtime.Text.Trim()))
                js.AppendFormat(" and ClearTime<'{0}'", DateTime.Parse(dpkEndtime.Text.Trim()).AddDays(1).ToString("yyyy-MM-dd"));
            if (!ddlCharge.SelectedValue.Equals("0"))
                js.AppendFormat(" and Charge='{0}'",ddlCharge.SelectedText);
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
                Response.AddHeader("content-disposition", "attachment; filename=挂账报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
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
            string sql = "SELECT Charge,sum(FactPrice) as Amount " +
                         "FROM tm_tabieusinginfo " +
                         "WHERE Charge IS NOT NULL AND Charge != '' " +
                         GetSqlWhere() +
                         " GROUP BY Charge ORDER BY Amount DESC";
            DataSet ds = DbHelperSQL.Query(sql);
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
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "挂账排名");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "挂账人");
                sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", "挂账总金额");
                sb.Append("</tr>");

                #endregion

                #region - 拼凑导出的数据行 -
                int recordIndex1 = 1;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", recordIndex1);
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", row["Charge"].ToString());
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", row["Amount"].ToString());
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

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            string sql = Grid1.SelectedRow.Values[1].ToString();
            string SqlWhere = DESEncrypt.Encrypt(GetSqlWhere());
            if (e.CommandName == "Detail")
            {
                PageContext.Redirect(string.Format("~/Reports/RPTChargeDetail.aspx?Charge={0}&sql={1}", sql,SqlWhere));
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
            btnExcel.Enabled = true;
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