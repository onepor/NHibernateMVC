using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class ReceiveOrderManager : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreReceiveOrderView";
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
            // 权限检查 
            CheckPowerWithButton("CoreReceiveOrderNew", btnNew);

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
            if (!string.IsNullOrEmpty(qryName))
            {
                qryList.Add(Expression.Disjunction()
                 .Add(Expression.Like("OrderNO", qryName, MatchMode.Anywhere))
                 .Add(Expression.Like("UserName", qryName, MatchMode.Anywhere))
                 );
            }
            if (!string.IsNullOrEmpty(dpStartDate.Text))
            {
                qryList.Add(Expression.Ge("OrderDate", DateTime.Parse(dpStartDate.Text)));
            }
            if (!string.IsNullOrEmpty(dpEndDate.Text))
            {
                qryList.Add(Expression.Le("OrderDate", DateTime.Parse(dpEndDate.Text)));
            }
            if (!string.IsNullOrEmpty(ddlState.SelectedValue))
            {
                qryList.Add(Expression.Eq("IsTemp", int.Parse(ddlState.SelectedValue)));
            }
            Order[] orderList = new Order[1];
            Order orderli = new Order("OrderDate", false);
            orderList[0] = orderli;
            int count = 0;
            IList<ReceiveOrder> list = Core.Container.Instance.Resolve<IServiceReceiveOrder>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
        }
        #endregion

        #region 页面数据转换
        public string GetWareHouseID(string ID)
        {
            WareHouse entity = Core.Container.Instance.Resolve<IServiceWareHouse>().GetEntity(Int32.Parse(ID));
            return entity == null ? "" : entity.WHName;
        }

        public string GetSuplierID(string ID)
        {
            SupplierInfo entity = Core.Container.Instance.Resolve<IServiceSupplierInfo>().GetEntity(Int32.Parse(ID));
            return entity == null ? "" : entity.SupplierName;
        }
        #endregion

        #region Events

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreReceiveOrderEdit", Grid1, "editField");
            CheckPowerWithLinkButtonField("CoreReceiveOrderDelete", Grid1, "deleteField");
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
            ReceiveOrder orderInfo = Core.Container.Instance.Resolve<IServiceReceiveOrder>().GetEntity(ID);
            if (e.CommandName == "Delete")
            {
                if (orderInfo != null)
                {
                    if (orderInfo.IsTemp == 1)
                    {
                        //删除临时订单商品信息
                        IList<ICriterion> qryList = new List<ICriterion>();
                        qryList.Add(Expression.Eq("OrderNO", orderInfo.OrderNO));
                        IList<ReceiveOrderDetail> goodsList = Core.Container.Instance.Resolve<IServiceReceiveOrderDetail>().GetAllByKeys(qryList);
                        foreach (ReceiveOrderDetail goods in goodsList)
                        {
                            Core.Container.Instance.Resolve<IServiceReceiveOrderDetail>().Delete(goods);
                        }
                        //删除临时订单信息          
                        Core.Container.Instance.Resolve<IServiceReceiveOrder>().Delete(orderInfo);
                        BindGrid();
                    }
                    else
                    {
                        Alert.Show("正式订单不能删除！");
                    }
                }
            }
            if (e.CommandName == "editField")
            {
                if (orderInfo != null)
                {
                    if (orderInfo.IsTemp == 1)
                    {
                        PageContext.Redirect(string.Format("~/Inventory/ReceiveOrderEdit.aspx?id={0}", ID));
                    }
                    else
                    {
                        Alert.Show("正式订单不能修改！");
                    }
                }
            }
            if (e.CommandName == "viewField")
            {
                PageContext.Redirect(string.Format("~/Inventory/ReceiveOrderView.aspx?id={0}", ID));
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

        #endregion

        protected void btnNew_Click(object sender, EventArgs e)
        {
            PageContext.Redirect("~/Inventory/ReceiveOrderEdit.aspx");
        }

    }
}