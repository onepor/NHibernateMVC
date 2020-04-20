using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractOrderManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreSaleOrderView";
            }
        }

        #endregion

        #region 加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //页面数据初始加载
                LoadData();
            }
        }

        private void LoadData()
        {
            //设置记录分页记录数
            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
            //加载发货单信息
            BindGrid();
        }
        #endregion

        #region 绑定数据
        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            string qryName = txtOrderNo.Text.Trim();
            qryList.Add(Expression.Eq("OrderType", 1));
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
            if (!string.IsNullOrEmpty(ddlFix.SelectedValue))
            {
                qryList.Add(Expression.Eq("IsFix", int.Parse(ddlFix.SelectedValue)));
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

        /// <summary>
        /// 绑定记录前按钮权限判断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreSaleOrderEdit", Grid1, "editField");
            CheckPowerWithLinkButtonField("CoreSaleOrderEdit", Grid1, "deleteField");
        }
        
        /// <summary>
        /// 绑定记录前按钮权限判断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PreRowDataBound(object sender, GridPreRowEventArgs e)
        {
            ContractOrderInfo row = e.DataItem as ContractOrderInfo;
            int isTemp = Convert.ToInt32(row.OrderState);
            LinkButtonField lbtnEditField = Grid1.FindColumn("lbtnEditField") as LinkButtonField;
            lbtnEditField.Enabled = isTemp == 1;
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
            BindGrid();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        /// <summary>
        /// 分页每页记录数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindGrid();
        }

        /// <summary>
        /// 记录列表行记录操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            //获取当前选中记录信息
            ContractOrderInfo orderInfo = Core.Container.Instance.Resolve<IServiceContractOrderInfo>().GetEntity(ID);
            if (orderInfo != null)
            {
                //编辑发货单
                if (e.CommandName == "editField")
                {
                    PageContext.Redirect(string.Format("~/Contract/FH/ContractOrderEdit.aspx?id={0}", ID));
                }
            }
        }

        /// <summary>
        /// 查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //加载发货单信息
            BindGrid();
        }

        /// <summary>
        /// 订单信息查看弹出页面关闭处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window1_Close(object sender, EventArgs e)
        {
            //加载发货单信息
            BindGrid();
        }
        #endregion

    }
}