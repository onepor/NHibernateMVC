using FineUIPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZAJCZN.MIS.Web
{
    public partial class ExamBulletin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindGrid();
        }


        /// <summary>
        /// 绑定客户合同信息列表
        /// </summary>
        private void BindSubjectInfo()
        {
            //IList<ExamSubjectModel> list = examSubjectBLL.GetModelList("");

            //ddlSubject.DataTextField = "SubjectName";
            //ddlSubject.DataValueField = "id";
            //ddlSubject.DataSource = list;
            //ddlSubject.DataBind();
            //ddlSubject.SelectedIndex = 0;
        }

        /// <summary>
        /// 绑定客户合同信息列表
        /// </summary>
        private void BindSubjectExam()
        {
            //ddlExam.Items.Clear();
            //ddlExam.Enabled = false;
            //if (!string.IsNullOrEmpty(ddlSubject.SelectedValue))
            //{
            //    string sqlWhere = string.Format(" SubjectID='{0}' ", ddlSubject.SelectedValue);
            //    IList<ExamPlanInfoModel> list = new ExamPlanInfoBll().GetModelList(sqlWhere);

            //    ddlExam.DataTextField = "ExamName";
            //    ddlExam.DataValueField = "ID";
            //    ddlExam.DataSource = list;
            //    ddlExam.DataBind();
            //    ddlExam.Enabled = true;
            //}
        }
        

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
            btnExpor.Enabled = true;
        }


        #region 数据绑定


        //查询SQL
        
        string sql = @"  select es.SubjectName SubjectName, count(pe.ID) TotalCount
                        ,sum(case when pe.Score >= epi.PassedScore then 1 else 0 end) PassCount,
                         cast(case when count(pe.ID) >0 then (cast(sum(case when pe.Score >= epi.PassedScore then 1 else 0 end) as decimal(18,2))/cast(count(pe.ID)as decimal(18,2))) else 0 end as decimal(18,2)) PassRate
                        from dbo.tm_ExamSubject es left join dbo.tm_ExamPlanInfo epi on es.id = epi.SubjectID
                        left join dbo.tm_PersonExam pe on epi.ID = pe.ExamPlanID ";

        /// <summary>
        /// 数据绑定
        /// </summary>
        private void BindGrid()
        {

            string sqlEnd = sql;

            //加载查询条件
            string textSubject = "";//ddlSubject.Text;
            string textExam = "";//ddlExam.Text;
            if (!string.IsNullOrEmpty(textSubject))
            {
                sqlEnd = sql + " and es.SubjectName like '" + textSubject + "'";
            }
            if (!string.IsNullOrEmpty(textExam))
            {
                sqlEnd = sql + " and epi.ExamName like '" + textExam + "'";
            }

            //数据量  条数
            //int count = ZAJCZN.MIS.Helpers.DbHelperSQL.Query("select pe.id " + sqlEnd).Tables[0].Rows.Count;

            //排序判断
            //if (Grid1.SortDirection == "ASC")
            //{
            //    sqlEnd += " order by " + Grid1.SortField + " ASC";
            //}
            //else
            //{
            //    sqlEnd += " order by " + Grid1.SortField + " desc";
            //}

            sqlEnd = sqlEnd + "group by SubjectName";

            //数据加载及绑定
            System.Data.DataSet ds = ZAJCZN.MIS.Helpers.DbHelperSQL.Query(sqlEnd);
            System.Data.DataTable data = ds.Tables[0];

            System.Data.DataRow dr = data.NewRow();
            dr["SubjectName"] = "合计";
            dr["TotalCount"] = data.Compute("SUM(TotalCount)", "true");
            dr["PassCount"] = data.Compute("SUM(PassCount)", "true");
            dr["PassRate"] = data.Compute("SUM(PassRate)", "true");
            data.Rows.Add(dr);

            Grid1.RecordCount = 0;
            //int pageIndexMax = (Grid1.PageIndex + 1) * Grid1.PageSize;
            //int pageIndexmin = pageIndexMax - Grid1.PageSize;
            Grid1.DataSource = data;//data.Select("" + pageIndexmin + " < RowNum and RowNum <=" + pageIndexMax + "");
            Grid1.DataBind();

        }


        #endregion


        #region 导出

        protected void btnExpor_Click(object sender, EventArgs e)
        {
            try
            {
                Response.ClearContent();
                Response.AddHeader("content-disposition", string.Format("attachment; filename=KSCJTJ_{0}.xls", DateTime.Now.ToString("yyyyMMddHHssmm")));
                Response.ContentType = "application/excel";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write(ExporTable());
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 导出
        /// </summary>
        public System.Text.StringBuilder ExporTable()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");
            //单据头
            sb.Append("<tr>");
            sb.AppendFormat("<td colspan=\"4\" style=\"font-size :x-large; text-align:center;\">{0}</td>", string.Format("{0}考试成绩统计报表", DateTime.Now.ToString("yyyy-MM")));
            sb.Append("</tr>");

            try
            {
                string ExporSql = sql;

                //加载查询条件
                string textSubject = "";//ddlSubject.Text;
                string textExam = "";//ddlExam.Text;
                if (!string.IsNullOrEmpty(textSubject))
                {
                    ExporSql = ExporSql + " and es.SubjectName like '" + textSubject + "'";
                }
                if (!string.IsNullOrEmpty(textExam))
                {
                    ExporSql = ExporSql + " and epi.ExamName like '" + textExam + "'";
                }
                ExporSql = ExporSql + " group by SubjectName";

                System.Data.DataSet ds = ZAJCZN.MIS.Helpers.DbHelperSQL.Query(ExporSql);
                System.Data.DataTable data = ds.Tables[0];
                System.Data.DataRow dr = data.NewRow();
                dr["SubjectName"] = "合计";
                dr["TotalCount"] = data.Compute("SUM(TotalCount)", "true");
                dr["PassCount"] = data.Compute("SUM(PassCount)", "true");
                dr["PassRate"] = data.Compute("SUM(PassRate)", "true");
                data.Rows.Add(dr);

                if (data != null || data.Rows.Count > 0)
                {

                    //订单信息
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", "科目");
                    sb.AppendFormat("<td>{0}</td>", "考试人数");
                    sb.AppendFormat("<td>{0}</td>", "合格人数");
                    sb.AppendFormat("<td>{0}</td>", "合格率");
                    sb.Append("</tr>");

                    for (int i = 0; i < data.Rows.Count; i++)
                    {

                        sb.Append("<tr>");
                        //sb.AppendFormat("<td>{0}</td>", i + 1);
                        sb.AppendFormat("<td>{0}</td>", data.Rows[i]["SubjectName"]);
                        sb.AppendFormat("<td>{0}</td>", data.Rows[i]["TotalCount"]);
                        sb.AppendFormat("<td>{0}</td>", data.Rows[i]["PassCount"]);
                        sb.AppendFormat("<td>{0}</td>", (Convert.ToDecimal(data.Rows[i]["PassRate"])*100).ToString()+"%");
                        //sb.AppendFormat("<td>{0}</td>", data.Rows[i]["ExamState"]);
                        sb.Append("</tr>");

                    }
                    sb.Append("</table>");
                }
                else
                {
                    Alert.Show("暂无可以导出数据！");
                }

            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message);
            }
            return sb;
        }


        #endregion



        protected void Window1_Close(object sender, EventArgs e)
        {

        }
    }
}