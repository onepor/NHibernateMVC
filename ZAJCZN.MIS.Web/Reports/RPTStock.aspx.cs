using FineUIPro;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Helpers;
using ZAJCZN.MIS.Service;
using NHibernate.Criterion;
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
    public partial class RPTStock : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreReportStock";
            }
        }

        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //绑定出库库房
                BindWH();
            }
        }

        #endregion

        #region 绑定数据

        private void BindWH()
        {
            IList<WareHouse> list = Core.Container.Instance.Resolve<IServiceWareHouse>().GetAll();
            ddlWH.DataSource = list;
            ddlWH.DataBind();
        }

        //获取分类名称
        public string GetType(string typeID)
        {
            tm_GoodsType objType = Core.Container.Instance.Resolve<IServiceGoodsType>().GetEntity(int.Parse(typeID));
            return objType != null ? objType.TypeName : "";
        }
        //获取单位
        public string GetUnitName(string state)
        {
            return GetSystemEnumValue("WPDW", state);
        }

        protected void BindGrid()
        {
            IList<ICriterion> qryListDetail = new List<ICriterion>();
            qryListDetail.Add(Expression.Eq("WareHouseID", int.Parse(ddlWH.SelectedValue)));
            IList<WHGoodsDetail> listWHGoodsDetail = Core.Container.Instance.Resolve<IServiceWHGoodsDetail>().GetAllByKeys(qryListDetail);
            Grid1.DataSource = listWHGoodsDetail;
            Grid1.DataBind();
        }

        #endregion

        #region 导出Excel

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string htmls = GetAllTableHtml();
            if (htmls != "")
            {
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=实时库存报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                Response.ContentType = "application/excel";
                Response.ContentEncoding = Encoding.UTF8;
                Response.Write(htmls);
                Response.End();
            }
        }

        private string GetAllTableHtml()
        {
            IList<ICriterion> qryListDetail = new List<ICriterion>();
            qryListDetail.Add(Expression.Eq("WareHouseID", int.Parse(ddlWH.SelectedValue)));
            IList<WHGoodsDetail> listWHGoodsDetail = Core.Container.Instance.Resolve<IServiceWHGoodsDetail>().GetAllByKeys(qryListDetail);

            if (listWHGoodsDetail != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

                #region - 拼凑主订单导出结果 -
                sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

                #region - 拼凑导出的列名 -
                sb.Append("<tr>");
                sb.AppendFormat("<td colspan=\"5\" style=\"text-align:center\">{0}</td>", DateTime.Now.ToString("yyyy-MM-dd"));
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td style=\"text-align:center\">序号</td>");
                sb.Append("<td style=\"text-align:center\">商品名称</td>");
                sb.Append("<td style=\"text-align:center\">库存数量</td>");
                sb.Append("</tr>");

                #endregion

                #region - 拼凑导出的数据行 -
                int recordIndex1 = 1;
                foreach (WHGoodsDetail row in listWHGoodsDetail)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", recordIndex1);
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", row.GoodsName);
                    sb.AppendFormat("<td style=\"text-align:center\">{0}</td>", row.InventoryCount);
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