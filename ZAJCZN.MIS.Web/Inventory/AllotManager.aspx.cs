using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class AllotManager : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreAllotView";
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
            CheckPowerWithButton("CoreAllotEdit", btnNew);
            //绑定库房信息
            BindWH();

            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();

            //加载物品信息
            BindGrid();
        }
        #endregion

        #region 绑定物品类型
        private void BindWH()
        {
            IList<WareHouse> list = Core.Container.Instance.Resolve<IServiceWareHouse>().GetAll();
            //出库
            ddlCKWareHouseID.DataSource = list;
            ddlCKWareHouseID.DataBind();
            ddlCKWareHouseID.Items.Insert(0, new ListItem { Text = "", Value = "" });
            ddlCKWareHouseID.SelectedIndex = 0;
            //入库
            ddlRKWareHouseID.DataSource = list;
            ddlRKWareHouseID.DataBind();
            ddlRKWareHouseID.Items.Insert(0, new ListItem { Text = "", Value = "" });
            ddlRKWareHouseID.SelectedIndex = 0;
        }
        #endregion

        #region 绑定数据
        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            string qryName = txtOrderNo.Text.Trim();
            if (!string.IsNullOrEmpty(qryName))
            {
                qryList.Add(Expression.Like("OrderNO", qryName, MatchMode.Anywhere));
            }
            if (!string.IsNullOrEmpty(ddlCKWareHouseID.SelectedValue))
            {
                qryList.Add(Expression.Eq("CKWareHouseID", int.Parse(ddlCKWareHouseID.SelectedValue)));
            }
            if (!string.IsNullOrEmpty(ddlRKWareHouseID.SelectedValue))
            {
                qryList.Add(Expression.Eq("RKWareHouseID", int.Parse(ddlRKWareHouseID.SelectedValue)));
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
            IList<tm_GoodsAllocationBill> list = Core.Container.Instance.Resolve<IServiceGoodsAllocationBill>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
        }
        #endregion

        #region 页面数据转换
        //获取税率名称
        public string GetWHName(string id)
        {
            WareHouse whInfo = Core.Container.Instance.Resolve<IServiceWareHouse>().GetEntity(int.Parse(id));
            return whInfo != null ? whInfo.WHName : "";
        }
        #endregion

        #region Events

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreAllotEdit", Grid1, "editField");
            CheckPowerWithLinkButtonField("CoreAllotEdit", Grid1, "deleteField");
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
            tm_GoodsAllocationBill orderInfo = Core.Container.Instance.Resolve<IServiceGoodsAllocationBill>().GetEntity(ID);
            if (e.CommandName == "Delete")
            {
                if (orderInfo != null)
                {
                    if (orderInfo.IsTemp == 1)
                    {
                        //删除临时订单商品信息
                        IList<ICriterion> qryList = new List<ICriterion>();
                        qryList.Add(Expression.Eq("OrderNO", orderInfo.OrderNO));
                        IList<tm_GoodsAllocationBillDetail> goodsList = Core.Container.Instance.Resolve<IServiceGoodsAllocationBillDetail>().GetAllByKeys(qryList);
                        foreach (tm_GoodsAllocationBillDetail goods in goodsList)
                        {
                            Core.Container.Instance.Resolve<IServiceGoodsAllocationBillDetail>().Delete(goods);
                        }
                        //删除临时订单信息          
                        Core.Container.Instance.Resolve<IServiceGoodsAllocationBill>().Delete(orderInfo);
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
                        PageContext.Redirect(string.Format("~/Inventory/AllotEdit.aspx?id={0}", ID));
                    }
                    else
                    {
                        Alert.Show("正式订单不能修改！");
                    }
                }
            }
            if (e.CommandName == "viewField")
            {
                PageContext.Redirect(string.Format("~/Inventory/AllotView.aspx?id={0}", ID));
            } 
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            foreach (int id in ids)
            {
                Core.Container.Instance.Resolve<IServiceGoodsAllocationBill>().Delete(id);
            }
            BindGrid();
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

        protected void btnNew_Click(object sender, EventArgs e)
        {
            PageContext.Redirect("~/Inventory/AllotEdit.aspx");
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }
        #endregion
    }
}