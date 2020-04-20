using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Data;

namespace ZAJCZN.MIS.Web
{
    public partial class GoodsOrderManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreGoodsOrderView";
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
            CheckPowerWithButton("CoreGoodsOrderNew", btnNew);
            //绑定供应商信息
            BindSupplier();

            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();

            //加载物品信息
            BindGrid();
        }
        #endregion

        #region 绑定供应商信息
        private void BindSupplier()
        {
            IList<SupplierInfo> list = Core.Container.Instance.Resolve<IServiceSupplierInfo>().GetAll();

            ddlSuplier.DataSource = list;
            ddlSuplier.DataBind();
            ddlSuplier.Items.Insert(0, new ListItem { Value = "0", Text = "--全部供应商--" });
            ddlSuplier.SelectedIndex = 0;
        }
        #endregion 绑定供应商信息

        #region 绑定数据
        private void BindGrid()
        {
            Grid1.DataSource = null;
            IList<ICriterion> qryList = new List<ICriterion>();
            string qryName = txtOrderNo.Text.Trim(); 
            if (!string.IsNullOrEmpty(qryName))
            {
                qryList.Add(Expression.Like("OrderNO", qryName, MatchMode.Anywhere));
            }
            if (ddlSuplier.SelectedValue != "0")
            {
                qryList.Add(Expression.Eq("SuplierID", int.Parse(ddlSuplier.SelectedValue)));
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
            IList<GoodsOrder> list = Core.Container.Instance.Resolve<IServiceGoodsOrder>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
        }
        #endregion

        #region 页面数据转换
        public string GetPayType(string state)
        {
            string i = "";
            switch (state)
            {
                case "1":
                    i = "现金";
                    break;
                case "2":
                    i = "挂账";
                    break;
            }
            return i;
        }

        //获取单位
        public string GetSupplierName(string id)
        {
            SupplierInfo supplierInfo = Core.Container.Instance.Resolve<IServiceSupplierInfo>().GetEntity(int.Parse(id));
            return supplierInfo != null ? supplierInfo.SupplierName : "";
        }
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
            CheckPowerWithWindowField("CoreGoodsOrderEdit", Grid1, "editField");
            CheckPowerWithLinkButtonField("CoreGoodsOrderDelete", Grid1, "deleteField");
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
            GoodsOrder orderInfo = Core.Container.Instance.Resolve<IServiceGoodsOrder>().GetEntity(ID);
            if (e.CommandName == "Delete")
            {
                if (orderInfo != null)
                {
                    if (orderInfo.IsTemp == 1)
                    {
                        //删除临时订单商品信息
                        IList<ICriterion> qryList = new List<ICriterion>();
                        qryList.Add(Expression.Eq("OrderNO", orderInfo.OrderNO));
                        IList<GoodsOrderDetail> goodsList = Core.Container.Instance.Resolve<IServiceGoodsOrderDetail>().GetAllByKeys(qryList);
                        foreach (GoodsOrderDetail goods in goodsList)
                        {
                            Core.Container.Instance.Resolve<IServiceGoodsOrderDetail>().Delete(goods);
                        }
                        //删除临时订单信息          
                        Core.Container.Instance.Resolve<IServiceGoodsOrder>().Delete(orderInfo);
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
                        PageContext.Redirect(string.Format("~/Inventory/GoodsOrderEdit.aspx?id={0}", ID));
                    }
                    else
                    {
                        Alert.Show("正式订单不能修改！");
                    }
                }
            }
            if (e.CommandName == "viewField")
            {
                PageContext.Redirect(string.Format("~/Inventory/GoodsOrderView.aspx?id={0}", ID));
            }
        }       

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindGrid();
        }

        protected void Grid1_PreRowDataBound(object sender, GridPreRowEventArgs e)
        {
            GoodsOrder row = e.DataItem as GoodsOrder;
            int isTemp = Convert.ToInt32(row.IsTemp);
            LinkButtonField lbtnEditField = Grid1.FindColumn("lbtnEditField") as LinkButtonField;
            LinkButtonField deleteField = Grid1.FindColumn("deleteField") as LinkButtonField;
            lbtnEditField.Enabled = isTemp > 0;
            deleteField.Enabled = isTemp > 0;
        }
        
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            PageContext.Redirect("~/Inventory/GoodsOrderEdit.aspx");
        }

        #endregion


         
    }
}