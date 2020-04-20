using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZAJCZN.MIS.Web
{
    public partial class ExamPassRate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }


        #region BindGrid

        string sql = @" select dpone.Name CityName,es.SubjectName SubjectName
                       ,isNULL(cast(case when DATENAME(year,pe.ExamEndDate)=DATENAME(year,GETDATE())-1 then avg(pe.Score) end as decimal(18,2)),0) TopAvgScore
                       ,isNULL(cast(case when DATENAME(year,pe.ExamEndDate)=DATENAME(year,GETDATE()) then avg(pe.Score) end as decimal(18,2)),0) NowAvgScore
                        from Users us
                        left join ExternalDepts dpfour on us.DeptID=dpfour.ID
                        left join ExternalDepts dpthree on dpfour.ParentID=dpthree.ID
                        left join ExternalDepts dptwo on dpthree.ParentID=dptwo.ID
                        left join ExternalDepts dpone on dptwo.ParentID=dpone.ID
                        left join tm_PersonExam pe on pe.UserID=us.ID 
                        left join dbo.tm_ExamSubject es on es.id = pe.SubjectID
                        where pe.ExamState != 4 and DATENAME(year,pe.ExamEndDate)=DATENAME(year,GETDATE()) 
                        and DATENAME(year,pe.ExamEndDate)=DATENAME(year,GETDATE()-1) ";
        
        private void BindGrid()
        {
            
            string Bindsql = sql + " group by dpone.Name,es.SubjectName,DATENAME(year,pe.ExamEndDate)";

            //数据加载及绑定
            System.Data.DataSet ds = ZAJCZN.MIS.Helpers.DbHelperSQL.Query(Bindsql);
            System.Data.DataTable data = ds.Tables[0];

            System.Data.DataRow dr = data.NewRow();
            dr["CityName"] = "合计";
            dr["SubjectName"] = "";
            dr["TopAvgScore"] = data.Compute("AVG(TopAvgScore)", "true");
            dr["NowAvgScore"] = data.Compute("AVG(NowAvgScore)", "true");
            data.Rows.Add(dr);

            Grid1.RecordCount = 0;
            //int pageIndexMax = (Grid1.PageIndex + 1) * Grid1.PageSize;
            //int pageIndexmin = pageIndexMax - Grid1.PageSize;
            Grid1.DataSource = data;//data.Select("" + pageIndexmin + " < RowNum and RowNum <=" + pageIndexMax + "");
            Grid1.DataBind();

        }



        #endregion
    }
}