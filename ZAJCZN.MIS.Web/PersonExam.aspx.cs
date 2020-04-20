using FineUIPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZAJCZN.MIS.Web
{
    public partial class PersonExam : PageBase//System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            //BindData();
        }

        #region 数据绑定


        //查询SQL

        string sql = @" from dbo.tm_PersonExam pe left join dbo.Users us on pe.UserID=us.ID 
                        left join dbo.ExternalDepts ed on us.DeptID =ed.ID left join dbo.ExternalUserInfo eui on us.IdentityCard = eui.ID 
                        left join dbo.tm_ExamSubject es on pe.SubjectID=es.id left join dbo.tm_ExamPlanInfo epi on pe.ExamPlanID=epi.ID
                        where us.UserType=3 ";

        /// <summary>
        /// 数据绑定
        /// </summary>
        private void BindData()
        {
            try
            {

                string sqlColumns = " ROW_NUMBER() OVER(ORDER BY pe.id) AS RowNum,pe.id ID,us.Name Name,ed.Name WorkPlace,eui.Position Position,epi.ExamName ExamName,es.SubjectName SubjectName, pe.Score Score,pe.ExamState ExamState ";
                string sqlEnd = sql;

                //加载查询条件
                string textSearch = ttbSearchMessage.Text;
                if (!string.IsNullOrEmpty(textSearch))
                {
                    if (sql_inj(textSearch))
                    {
                        //存在SQL注入风险
                    }
                    else
                    {
                        sqlEnd = sql + " and( us.Name like '" + textSearch + "' or ed.Name like '" + textSearch + "' or eui.Position like '" + textSearch + "' or epi.ExamName like '" + textSearch + "' or es.SubjectName like '" + textSearch + "')";
                    }
                }

                //数据量  条数
                int count = Helpers.DbHelperSQL.Query("select pe.id " + sqlEnd).Tables[0].Rows.Count;

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
                System.Data.DataSet ds = Helpers.DbHelperSQL.Query(sqlEnd);
                System.Data.DataTable data = ds.Tables[0];
                Grid1.RecordCount = count;

                int pageIndexMax = (Grid1.PageIndex + 1) * Grid1.PageSize;
                int pageIndexmin = pageIndexMax - Grid1.PageSize;
                Grid1.DataSource = data.Select("" + pageIndexmin + " < RowNum and RowNum <=" + pageIndexMax + "");
                Grid1.DataBind();
                
                ds.Dispose();
                data.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region 排序-分页

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindData();
        }
        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreSupplierEdit", Grid1, "editField");
            CheckPowerWithLinkButtonField("CoreSupplierDelete", Grid1, "deleteField");
        }

        //分页（上下页）
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindData();
        }

        //页数更新
        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindData();
        }

        #endregion

        #region 搜索

        //搜索
        protected void ttbSearchMessage_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearchMessage.Text = String.Empty;
            ttbSearchMessage.ShowTrigger1 = false;
            BindData();
        }

        //搜索
        protected void ttbSearchMessage_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearchMessage.ShowTrigger1 = true;
            BindData();
        }


        protected void Window1_Close(object sender, EventArgs e)
        {
            MenuHelper.Reload();
            BindData();
        }


        #endregion

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
            finally
            {
                Dispose();
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
                string textSearch = ttbSearchMessage.Text;
                if (!string.IsNullOrEmpty(textSearch))
                {
                    if (sql_inj(textSearch))
                    {
                        //存在SQL注入风险
                    }
                    else
                    {
                        ExporSql += " and( us.Name like '" + textSearch + "' or ed.Name like '" + textSearch + "' or eui.Position like '" + textSearch + "' or epi.ExamName like '" + textSearch + "' or es.SubjectName like '" + textSearch + "')";
                    }
                }

                System.Data.DataSet ds = Helpers.DbHelperSQL.Query(ExporSql);
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
                    sb.AppendFormat("<td>{0}</td>", "考试状态");
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
                        sb.AppendFormat("<td>{0}</td>", data.Rows[i]["Score"]);
                        sb.AppendFormat("<td>{0}</td>", data.Rows[i]["ExamState"]);
                        sb.Append("</tr>");

                    }
                    sb.Append("</table>");

                    //Response.ClearContent();
                    //Response.AddHeader("content-disposition", string.Format("attachment; filename=KSCJ_{0}.xls", DateTime.Now.ToString("yyyyMMddHHssmm")));
                    //Response.ContentType = "application/excel";
                    //Response.ContentEncoding = System.Text.Encoding.UTF8;
                    //Response.Write(sb);
                    //Response.End();

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