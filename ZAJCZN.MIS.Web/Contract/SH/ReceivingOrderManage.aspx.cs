using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class ReceivingOrderManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreReceivingOrderView";
            }
        }

        #endregion

        #region 加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
            //加载物品信息
            BindGrid();
        }
        #endregion

        #region 绑定数据
        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            string qryName = txtOrderNo.Text.Trim();
            qryList.Add(Expression.Eq("OrderType", 2));
            qryList.Add(Expression.Eq("IsTemp", 0));
            if (!string.IsNullOrEmpty(qryName))
            {
                qryList.Add(Expression.Like("ManualNO", qryName, MatchMode.Anywhere)
             || Expression.Like("CustomerName", qryName, MatchMode.Anywhere)
             || Expression.Like("OrderNO", qryName, MatchMode.Anywhere));
            }
            if (!string.IsNullOrEmpty(dpStartDate.Text))
            {
                qryList.Add(Expression.Ge("ValuationDate", dpStartDate.Text));
            }
            if (!string.IsNullOrEmpty(dpEndDate.Text))
            {
                qryList.Add(Expression.Le("ValuationDate", dpEndDate.Text));
            }
            if (!string.IsNullOrEmpty(ddlState.SelectedValue))
            {
                qryList.Add(Expression.Eq("OrderState", ddlState.SelectedValue));
            }

            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", false);
            orderList[0] = orderli;
            int count = 0;
            IList<ContractOrderInfo> list = Core.Container.Instance.Resolve<IServiceContractOrderInfo>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
        }
        #endregion

        #region Events

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreReceivingOrderEdit", Grid1, "editField");
            CheckPowerWithLinkButtonField("CoreReceivingOrderEdit", Grid1, "deleteField");
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

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            //获取当前选中记录信息
            ContractOrderInfo orderInfo = Core.Container.Instance.Resolve<IServiceContractOrderInfo>().GetEntity(ID);

            if (e.CommandName == "editField")
            {
                if (orderInfo != null)
                {
                    PageContext.Redirect(string.Format("~/Contract/SH/ReceivingOrderEdit.aspx?id={0}", ID));
                }
            }
        }

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void Grid1_PreRowDataBound(object sender, GridPreRowEventArgs e)
        {
            ContractOrderInfo row = e.DataItem as ContractOrderInfo;
            int isTemp = Convert.ToInt32(row.OrderState);
            LinkButtonField lbtnEditField = Grid1.FindColumn("lbtnEditField") as LinkButtonField;
            lbtnEditField.Enabled = isTemp == 1;
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        #endregion

    }
}