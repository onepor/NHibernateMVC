using FineUIPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZAJCZN.MIS.Web
{
    public partial class BackMsgList : System.Web.UI.Page
    {


        private string RetId
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //获取传参
            RetId = Request.QueryString["ID"].ToString();
        }


        /// <summary>
        /// 绑定列表数据
        /// </summary>
        private void BindGid()
        {

            string Sql = @" select  ROW_NUMBER() OVER(ORDER BY mm.ID) AS RowNum,mm.ID ID,us.Name UserName,mm.MesTitle SendTitle,mm.Message SendContent,mm.MesDate SendTime,case when mm.IsRead =0 then '否' else '是' end IsBack 
                            from MemberSiteMessage mm left join dbo.Users us on mm.UserID=us.ID ";

            string ExecuteSql = Sql;

            //查询条件
            string starTimeWhere = dpStartDate.Text;
            string endTimeWhere = dpEndDate.Text;

            ExecuteSql += " where FirstID="+ RetId + "";
            if (!string.IsNullOrEmpty(starTimeWhere))
            {
                ExecuteSql += " and mm.MesDate>='" + starTimeWhere + "'";
            }
            if (!string.IsNullOrEmpty(endTimeWhere))
            {
                ExecuteSql += " and '" + endTimeWhere + "'> mm.MesDate";
            }

            //数据加载及绑定
            System.Data.DataSet ds = Helpers.DbHelperSQL.Query(ExecuteSql);
            System.Data.DataTable data = ds.Tables[0];
            Grid1.RecordCount = data.Rows.Count;

            int pageIndexMax = (Grid1.PageIndex + 1) * Grid1.PageSize;
            int pageIndexmin = pageIndexMax - Grid1.PageSize;
            Grid1.DataSource = data;// data.Select("" + pageIndexmin + " < RowNum and RowNum <=" + pageIndexMax + "");
            Grid1.DataBind();

        }


        //查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {

            BindGid();
        }


        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGid();
        }
        //分页（上下页）
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindGid();
        }

        //页数更新
        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindGid();
        }

    }
}