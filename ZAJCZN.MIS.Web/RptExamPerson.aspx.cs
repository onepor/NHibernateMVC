using FineUIPro;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XGZDAPP.Web.Exam.Report
{
    public partial class RptExamPerson : ZAJCZN.MIS.Web.PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        //public override string ViewPower
        //{
        //    get
        //    {
        //        return "CoreExamSubjectView";
        //    }
        //}

        #endregion

        #region - 变量 -

        //XXPP.BLL.ExamSubjectBll examSubjectBLL = new XXPP.BLL.ExamSubjectBll();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSubjectInfo();

                BindSubjectExam();
            }
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


        #region - 私有方法 -

        #region 数据绑定


        //查询SQL

        string sql = @" from dbo.tm_PersonExam pe left join dbo.Users us on pe.UserID=us.ID 
                        left join dbo.ExternalDepts ed on us.DeptID =ed.ID left join dbo.ExternalUserInfo eui on us.IdentityCard = eui.ID 
                        left join dbo.tm_ExamSubject es on pe.SubjectID=es.id left join dbo.tm_ExamPlanInfo epi on pe.ExamPlanID=epi.ID
                        where us.UserType=3 and epi.ExamType=2 ";// and epi.ExamType=2

        /// <summary>
        /// 数据绑定
        /// </summary>
        private void BindGrid()
        {

            string sqlColumns = " ROW_NUMBER() OVER(ORDER BY pe.id) AS RowNum,pe.id ID,us.Name Name,ed.Name WorkPlace,eui.Position Position,epi.ExamName ExamName,es.SubjectName SubjectName, pe.Score Score,pe.ExamState ExamState ";
            string sqlEnd = sql;

            //加载查询条件
            string textSubject = ddlSubject.Text;
            string textExam = ddlExam.Text;
            if (!string.IsNullOrEmpty(textSubject))
            {
                if (sql_inj(textSubject))
                {
                    //存在SQL注入风险
                }
                else
                {
                    sqlEnd = sql + " and es.SubjectName like '" + textSubject + "'";
                }
            }
            if (!string.IsNullOrEmpty(textExam))
            {
                if (sql_inj(textExam))
                {
                    //存在SQL注入风险
                }
                else
                {
                    sqlEnd = sql + " and epi.ExamName like '" + textExam + "'";
                }
            }

            //数据量  条数
            int count = ZAJCZN.MIS.Helpers.DbHelperSQL.Query("select pe.id " + sqlEnd).Tables[0].Rows.Count;

            //排序判断
            if (Grid1.SortDirection == "ASC")
            {
                sqlEnd += " order by " + Grid1.SortField + " ASC";
            }
            else
            {
                sqlEnd += " order by " + Grid1.SortField + " desc";
            }

            sqlEnd = "select " + sqlColumns + sqlEnd;

            //数据加载及绑定
            System.Data.DataSet ds = ZAJCZN.MIS.Helpers.DbHelperSQL.Query(sqlEnd);
            System.Data.DataTable data = ds.Tables[0];
            Grid1.RecordCount = count;
            //int pageIndexMax = (Grid1.PageIndex + 1) * Grid1.PageSize;
            //int pageIndexmin = pageIndexMax - Grid1.PageSize;
            Grid1.DataSource = data;//data.Select("" + pageIndexmin + " < RowNum and RowNum <=" + pageIndexMax + "");
            Grid1.DataBind();

        }


        #endregion

        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
            btnExpor.Enabled = true;
        }

        protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSubjectExam();
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                int deptID = GetSelectedDataKeyID(Grid1);
                string addUrl = String.Format("~/Exam/Report/FincalReport/RptExamPersonDetail.aspx?planid={0}&deptid={1}", ddlExam.SelectedValue, deptID);

                PageContext.RegisterStartupScript(Window1.GetShowReference(addUrl, "部门参考人员考试详情"));
            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {

        }



        #region 导出

        protected void btnExpor_Click(object sender, EventArgs e)
        {
            try
            {
                Response.ClearContent();
                Response.AddHeader("content-disposition", string.Format("attachment; filename=KSCJ_{0}.xls", DateTime.Now.ToString("yyyyMMddHHssmm")));
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
                string ExporSqlColumns = " us.Name Name,ed.Name WorkPlace,eui.Position Position,epi.ExamName ExamName,es.SubjectName SubjectName, pe.Score Score,pe.ExamState ExamState ";

                string ExporSql = "select " + ExporSqlColumns + sql;

                //加载查询条件
                string textSubject = ddlSubject.Text;
                string textExam = ddlExam.Text;
                if (!string.IsNullOrEmpty(textSubject))
                {
                    if (sql_inj(textSubject))
                    {
                        //存在SQL注入风险
                    }
                    else
                    {
                        ExporSql = sql + " and es.SubjectName like '" + textSubject + "'";
                    }
                }
                if (!string.IsNullOrEmpty(textExam))
                {
                    if (sql_inj(textExam))
                    {
                        //存在SQL注入风险
                    }
                    else
                    {
                        ExporSql = sql + " and epi.ExamName like '" + textExam + "'";
                    }
                }

                System.Data.DataSet ds = ZAJCZN.MIS.Helpers.DbHelperSQL.Query(ExporSql);
                System.Data.DataTable data = ds.Tables[0];

                if (data != null || data.Rows.Count > 0)
                {

                    //订单信息
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", "序号");
                    sb.AppendFormat("<td>{0}</td>", "姓名");
                    sb.AppendFormat("<td>{0}</td>", "工作单位");
                    sb.AppendFormat("<td>{0}</td>", "职务");
                    sb.AppendFormat("<td>{0}</td>", "考试计划表");
                    sb.AppendFormat("<td>{0}</td>", "科目");
                    sb.AppendFormat("<td>{0}</td>", "成绩");
                    //sb.AppendFormat("<td>{0}</td>", "考试状态");
                    sb.Append("</tr>");

                    for (int i = 0; i < data.Rows.Count; i++)
                    {

                        sb.Append("<tr>");
                        sb.AppendFormat("<td>{0}</td>", i + 1);
                        sb.AppendFormat("<td>{0}</td>", data.Rows[i]["Name"]);
                        sb.AppendFormat("<td>{0}</td>", data.Rows[i]["WorkPlace"]);
                        sb.AppendFormat("<td>{0}</td>", data.Rows[i]["Position"]);
                        sb.AppendFormat("<td>{0}</td>", data.Rows[i]["ExamName"]);
                        sb.AppendFormat("<td>{0}</td>", data.Rows[i]["SubjectName"]);
                        sb.AppendFormat("<td>{0}</td>", data.Rows[i]["ExamState"].ToString() == "4" ? "缺考" : data.Rows[i]["Score"]);
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


        /// <summary>
        /// SQL注入字符串验证
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool sql_inj(String str)
        {
            String inj_str = @"'| and | exec | insert | select | delete | update |
            count | *|%| chr | mid | master | truncate | char | declare |;| or | -| +|,";

            String[] inj_stra = inj_str.Split('|');
            for (int i = 0; i < inj_stra.Length; i++)
            {
                if (str.IndexOf(inj_stra[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}