using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using KZKJ.IOA.Domain;
using KZKJ.IOA.Service;

namespace KZKJ.IOA.Web
{
    public partial class ExpiredGoods : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreExpiredView";
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
            BindGrid();
        }
        #endregion

        #region 绑定数据

        private void BindGrid()
        {
            List<ICriterion> qrylist = new List<ICriterion>();
            if (!string.IsNullOrEmpty(dpEndDate.Text.Trim()))
                qrylist.Add(Expression.Le("GuaranteeDate", dpEndDate.Text.Trim()));
            else
                qrylist.Add(Expression.Le("GuaranteeDate", DateTime.Now.AddDays(30).ToString()));//30天内过期
            Order[] orderList = new Order[1];
            Order orderli = new Order(Grid1.SortField, Grid1.SortDirection == "ASC" ? true : false);
            orderList[0] = orderli;
            int count = 0;
            IList<tm_GoodsOrderDetail> list = Core.Container.Instance.Resolve<IServiceGoodsOrderDetail>().GetPaged(qrylist, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        #endregion

        //#region 退货处理

        //protected void btnReturns_Click(object sender, EventArgs e)
        //{
        //    List<int> ids = GetSelectedDataKeyIDs(Grid1);
        //    decimal OrderNumber = 0;
        //    IList<tm_ExpiredGoods> list = new List<tm_ExpiredGoods>();
        //    foreach (int id in ids)
        //    {
        //        tm_ExpiredGoods entity = Core.Container.Instance.Resolve<IServiceExpiredGoods>().GetEntity(id);
        //        list.Add(entity);
        //        OrderNumber += entity.GoodsNumber;
        //    }
        //    #region  出库单信息

        //    // 出库单信息
        //    tm_WHOutBoundOrder storageOrder = new tm_WHOutBoundOrder();
        //    string time = DateTime.Now.ToString("yyyyMMddHHmmss");
        //    storageOrder.BOrderNO = "TH" + time;
        //    storageOrder.OperatorID = 0;
        //    storageOrder.OrderAmount = 0;
        //    storageOrder.OrderNumber = OrderNumber;
        //    storageOrder.OrderDate = DateTime.Parse(time);
        //    storageOrder.OrderNO = string.Format("CK{0}", time);
        //    storageOrder.OrderType = 1;
        //    storageOrder.OutOrderNO = "";
        //    storageOrder.Remark = "退货出库";
        //    tm_ExpiredGoods entitys = Core.Container.Instance.Resolve<IServiceExpiredGoods>().GetEntity(ids[0]);
        //    storageOrder.SuplierID = entitys.SuplierID;
        //    storageOrder.WareHouseID = entitys.WareHouseID;
        //    Core.Container.Instance.Resolve<IServiceWHOutBoundOrder>().Create(storageOrder);

        //    // 出库商品明细信息            
        //    IList<ICriterion> qryList1 = new List<ICriterion>();
        //    qryList1.Add(Expression.Eq("OrderNO", storageOrder.OrderNO));
        //    tm_WHOutBoundOrder storageOrderNew = Core.Container.Instance.Resolve<IServiceWHOutBoundOrder>().GetEntityByFields(qryList1);
        //    if (storageOrderNew != null)
        //    {
        //        //写入出库商品明细
        //        foreach (tm_ExpiredGoods detail in list)
        //        {
        //            tm_WHOrderGoodsDetail orderDetail = new tm_WHOrderGoodsDetail();
        //            orderDetail.GoodsID = detail.GoodsID;
        //            //orderDetail.ChangeNumber = detail.ChangeNumber;
        //            orderDetail.GoodsNumber = detail.GoodsNumber;
        //            //orderDetail.GoodsUnit = detail.GoodsUnit;
        //            //orderDetail.GoodsUnitPrice = detail.GoodsUnitPrice;
        //            //orderDetail.GoodTotalPrice = detail.GoodTotalPrice;
        //            orderDetail.OrderDate = storageOrderNew.OrderDate;
        //            orderDetail.OrderNO = storageOrderNew.OrderNO;
        //            //orderDetail.ChangeUnit = detail.ChangeUnit;
        //            //orderDetail.TaxAmount = detail.TaxAmount;
        //            //orderDetail.TaxPoint = detail.TaxPoint;
        //            //orderDetail.TotalPriceNoTax = detail.TotalPriceNoTax;
        //            //orderDetail.UnitPriceNoTax = detail.UnitPriceNoTax;
        //            Core.Container.Instance.Resolve<IServiceWHOrderGoodsDetail>().Create(orderDetail);
        //        }
        //    }
        //    #endregion
        //}

        //#endregion

        #region 页面数据转换
        public string GetWareHouseID(string order)
        {
            List<ICriterion> qrylist = new List<ICriterion>();
            qrylist.Add(Expression.Eq("OrderNO", order));
            tm_GoodsOrder GoodsOrder = Core.Container.Instance.Resolve<IServiceGoodsOrder>().GetEntityByFields(qrylist);
            tm_WareHouse entity = Core.Container.Instance.Resolve<IServiceWareHouse>().GetEntity(GoodsOrder.WareHouseID);
            return entity == null ? "" : entity.WHName;
        }

        public string GetGoodsName(string ID)
        {
            tm_Goods entity = Core.Container.Instance.Resolve<IServiceGoods>().GetEntity(Int32.Parse(ID));
            return entity == null ? "" : entity.GoodsName;
        }

        public string GetSuplierID(string order)
        {
            List<ICriterion> qrylist = new List<ICriterion>();
            qrylist.Add(Expression.Eq("OrderNO", order));
            tm_GoodsOrder GoodsOrder = Core.Container.Instance.Resolve<IServiceGoodsOrder>().GetEntityByFields(qrylist);
            tm_SupplierInfo entity = Core.Container.Instance.Resolve<IServiceSupplierInfo>().GetEntity(GoodsOrder.SuplierID);
            return entity == null ? "" : entity.SupplierName;
        }

        public string GetDays(string termofvalidity)
        {
            DateTime Termofvalidity = DateTime.Parse(termofvalidity);
            TimeSpan time = Termofvalidity.Subtract(DateTime.Now);
            return time.Days.ToString() + "天" + time.Hours.ToString() + "小时" + time.Minutes.ToString() + "分" + time.Seconds.ToString() + "秒";
        }
        #endregion

        #region Events
        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreExpiredEdit", Grid1, "editField");
            CheckPowerWithLinkButtonField("CoreExpiredEdit", Grid1, "deleteField");
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

            //int ID = GetSelectedDataKeyID(Grid1);
            //if (e.CommandName == "Delete")
            //{
            //    Core.Container.Instance.Resolve<IServiceExpiredGoods>().Delete(ID);
            //    BindGrid();
            //}
        }


        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindGrid();
        }

        protected void dpEndDate_TextChanged(object sender, EventArgs e)
        {
            BindGrid();
        }
        #endregion


    }
}