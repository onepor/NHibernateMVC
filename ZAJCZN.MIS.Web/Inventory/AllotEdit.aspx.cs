using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Configuration;

namespace ZAJCZN.MIS.Web
{
    public partial class AllotEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreAllotEdit";
            }
        }

        #endregion

        private string OrderNO
        {
            get { return ViewState["OrderNO"].ToString(); }
            set
            {
                ViewState["OrderNO"] = value;

            }
        }

        private int OrderID
        {
            get { return GetQueryIntValue("id"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //绑定库房
                BindWH();
                if (OrderID <= 0)
                {
                    //生成订单信息
                    CreateOrderInfo();
                }
                else
                {
                    GetOrderInfo();
                }
                btnReturn.ConfirmText = "确定要取消当前调拨单信息吗？";

                btnNew.OnClientClick = Window1.GetShowReference(string.Format("~/PublicWebForm/AllotSelectDialog.aspx?rowid={0}", OrderNO), "添加调拨商品");
                // 删除选中单元格的客户端脚本 
                btnDeleteSelected.OnClientClick = Grid1.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
                btnDeleteSelected.ConfirmText = String.Format("确定要删除选中的<span class=\"highlight\"><script>{0}</script></span>项配料记录吗？", Grid1.GetSelectedCountReference());
                btnDeleteSelected.ConfirmTarget = FineUIPro.Target.Top;

                //绑定表格
                BindGrid();
            }
        }

        #region 绑定数据
        private void BindWH()
        {
            IList<WareHouse> list = Core.Container.Instance.Resolve<IServiceWareHouse>().GetAll();

            ddlCKWareHouseID.DataSource = list;
            ddlCKWareHouseID.DataBind();
            ddlCKWareHouseID.SelectedIndex = 0;

            ddlRKWareHouseID.DataSource = list;
            ddlRKWareHouseID.DataBind();
            ddlRKWareHouseID.SelectedIndex = 0;
        }

        private void CreateOrderInfo()
        {
            tm_GoodsAllocationBill allot = new tm_GoodsAllocationBill();
            allot.OrderNO = string.Format("DB{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
            allot.CKWareHouseID = 0;
            allot.RKWareHouseID = 0;
            allot.OrderDate = DateTime.Now;
            allot.Operator = User.Identity.Name;
            allot.AllotCount = 0;
            allot.AllotAmount = 0;
            allot.CKWareHouseID = int.Parse(ddlCKWareHouseID.SelectedValue);
            allot.RKWareHouseID = int.Parse(ddlRKWareHouseID.SelectedValue);
            allot.Remark = "";
            allot.IsTemp = 1;
            Core.Container.Instance.Resolve<IServiceGoodsAllocationBill>().Create(allot);

            OrderNO = allot.OrderNO;

            //初始化页面数据
            lblOrderNo.Text = OrderNO;
            dpStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void GetOrderInfo()
        {
            tm_GoodsAllocationBill order = Core.Container.Instance.Resolve<IServiceGoodsAllocationBill>().GetEntity(OrderID);
            OrderNO = order.OrderNO;

            //初始化页面数据
            dpStartDate.Text = order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss");
            txtRemark.Text = order.Remark;
            lblOrderNo.Text = OrderNO;
            ddlCKWareHouseID.SelectedValue = order.CKWareHouseID.ToString();
            ddlRKWareHouseID.SelectedValue = order.RKWareHouseID.ToString();
        }

        #endregion 绑定数据

        #region BindGrid

        private void BindGrid()
        {
            decimal goodsNumber = 0;
            decimal goodsAmount = 0;
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));

            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            int count = 0;
            IList<tm_GoodsAllocationBillDetail> list = Core.Container.Instance.Resolve<IServiceGoodsAllocationBillDetail>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);

            Grid1.DataSource = list;
            Grid1.DataBind();

            foreach (tm_GoodsAllocationBillDetail detail in list)
            {
                goodsNumber += detail.GoodsNumber;
                goodsAmount += detail.GoodsAmount ?? 0;
            }
            lblCount.Text = goodsNumber.ToString();
            lblAmount.Text = goodsAmount.ToString();
        }

        #endregion

        #region 页面数据转化

        //获取单位
        public string GetUnitName(string id)
        {
            tm_Goods supplierInfo = Core.Container.Instance.Resolve<IServiceGoods>().GetEntity(int.Parse(id));
            return GetSystemEnumValue("WPDW", supplierInfo.GoodsUnit);
        }

        //获取单位
        public string GetGoodsName(string id)
        {
            tm_Goods supplierInfo = Core.Container.Instance.Resolve<IServiceGoods>().GetEntity(int.Parse(id));
            return supplierInfo != null ? supplierInfo.GoodsName : "";
        }

        //获取单位
        public string GetGoodCode(string id)
        {
            tm_Goods supplierInfo = Core.Container.Instance.Resolve<IServiceGoods>().GetEntity(int.Parse(id));
            return supplierInfo != null ? supplierInfo.GoodsCode.ToString() : "";
        }

        #endregion 页面数据转化

        #region Events

        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            Dictionary<int, Dictionary<string, object>> modifiedDict = Grid1.GetModifiedDict();
            foreach (int rowIndex in modifiedDict.Keys)
            {
                int rowID = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
                tm_GoodsAllocationBillDetail objInfo = Core.Container.Instance.Resolve<IServiceGoodsAllocationBillDetail>().GetEntity(rowID);
                if (modifiedDict[rowIndex].Keys.Contains("GoodsPrice"))
                {
                    objInfo.GoodsPrice = Convert.ToDecimal(modifiedDict[rowIndex]["GoodsPrice"]);
                }
                if (modifiedDict[rowIndex].Keys.Contains("GoodsNumber"))
                {
                    objInfo.GoodsNumber = Convert.ToDecimal(modifiedDict[rowIndex]["GoodsNumber"]);
                }
                objInfo.GoodsAmount = objInfo.GoodsPrice * objInfo.GoodsNumber;

                Core.Container.Instance.Resolve<IServiceGoodsAllocationBillDetail>().Update(objInfo);
            }

            BindGrid();
        }

        public void btnReturn_Click(object sender, EventArgs e)
        {
            PageContext.Redirect("~/Inventory/AllotManager.aspx");
        }

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            CheckPowerWithLinkButtonField("CoreAllotEdit", Grid1, "deleteField");
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            foreach (int id in ids)
            {
                Core.Container.Instance.Resolve<IServiceGoodsAllocationBillDetail>().Delete(id);
            }
            BindGrid();
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            if (e.CommandName == "Delete")
            {
                Core.Container.Instance.Resolve<IServiceGoodsAllocationBillDetail>().Delete(ID);
                BindGrid();
            }
        }

        protected void btnSaveTemp_Click(object sender, EventArgs e)
        {
            if (SaveItem(1))
            {
                PageContext.Redirect("~/Inventory/AllotManager.aspx");
            }
            else
            {
                Alert.Show("调拨单创建失败！");
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem(0);
            PageContext.Redirect("~/Inventory/AllotManager.aspx");
        }

        private bool SaveItem(int isTemp)
        {
            bool IsStock = bool.Parse(ConfigurationManager.AppSettings["IsStock"]);
            decimal costAmount = 0;
            //更新订单状态为正式订单
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            tm_GoodsAllocationBill orderInfo = Core.Container.Instance.Resolve<IServiceGoodsAllocationBill>().GetEntityByFields(qryList);

            orderInfo.OrderDate = DateTime.Parse(dpStartDate.Text);
            orderInfo.CKWareHouseID = Int32.Parse(ddlCKWareHouseID.SelectedValue);
            orderInfo.RKWareHouseID = Int32.Parse(ddlRKWareHouseID.SelectedValue);
            orderInfo.AllotCount = decimal.Parse(lblCount.Text);
            orderInfo.AllotAmount = 0;
            orderInfo.IsTemp = isTemp;
            Core.Container.Instance.Resolve<IServiceGoodsAllocationBill>().Update(orderInfo);

            string WHOutBoundOrderNO = string.Format("CK{0}", DateTime.Parse(dpStartDate.Text).ToString("yyyyMMddHHmmss"));
            string WHStorageOrderNO = string.Format("RK{0}", DateTime.Parse(dpStartDate.Text).ToString("yyyyMMddHHmmss"));
            if (IsStock)
            {
                //正式订单，更新出库信息及流水信息等
                if (isTemp == 0)
                {
                    //调拨出库处理
                    if (WHOutBound(WHOutBoundOrderNO, WHStorageOrderNO, orderInfo, out costAmount))
                    {
                        //调拨入库处理 
                        if (WHStorage(WHOutBoundOrderNO, WHStorageOrderNO, orderInfo))
                        {
                            //更新调拨单成本金额
                            orderInfo.AllotAmount = costAmount;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            //更新调拨单信息
            Core.Container.Instance.Resolve<IServiceGoodsAllocationBill>().Update(orderInfo);

            return true;
        }

        /// <summary>
        /// 调拨出库处理
        /// </summary>
        /// <param name="WHOutBoundOrderNO">出库单号</param>
        /// <param name="WHStorageOrderNO">入库单号</param>
        /// <param name="orderInfo">调拨单信息</param>
        /// <returns></returns>
        private bool WHOutBound(string WHOutBoundOrderNO, string WHStorageOrderNO, tm_GoodsAllocationBill orderInfo, out decimal costAmount)
        {
            costAmount = 0;
            //获取出库仓库信息 
            WareHouse houseInfo = Core.Container.Instance.Resolve<IServiceWareHouse>().GetEntity(orderInfo.CKWareHouseID);
            //如果出库仓库存在
            if (houseInfo != null)
            {
                //出库单信息
                WHOutBoundOrder outBoundOrder = new WHOutBoundOrder();
                outBoundOrder.BOrderNO = OrderNO;
                outBoundOrder.Operator = User.Identity.Name;
                outBoundOrder.OrderAmount = 0;
                outBoundOrder.OrderNumber = orderInfo.AllotCount;
                outBoundOrder.OrderDate = orderInfo.OrderDate;
                outBoundOrder.OrderNO = WHOutBoundOrderNO;
                outBoundOrder.OrderType = 2;   //2：调拨出库
                outBoundOrder.OutOrderNO = WHStorageOrderNO;
                outBoundOrder.Remark = "调拨出库";
                outBoundOrder.SuplierID = 0;
                outBoundOrder.WareHouseID = orderInfo.CKWareHouseID;
                Core.Container.Instance.Resolve<IServiceWHOutBoundOrder>().Create(outBoundOrder);

                //获取调拨商品明细,生成出库商品明细信息 
                IList<ICriterion> qryListDetail = new List<ICriterion>();
                qryListDetail.Add(Expression.Eq("OrderNO", OrderNO));
                Order[] orderList = new Order[1];
                Order orderli = new Order("ID", true);
                orderList[0] = orderli;
                IList<tm_GoodsAllocationBillDetail> list = Core.Container.Instance.Resolve<IServiceGoodsAllocationBillDetail>().GetAllByKeys(qryListDetail, orderList);
                //写入出库商品明细
                foreach (tm_GoodsAllocationBillDetail detail in list)
                {
                    decimal amount = detail.GoodsNumber;
                    decimal goodsAmount = 0;
                    //根据批次号获取出库商品信息 
                    //List<tm_whgoodsorderbatch> batchList = new InventoryHelper().GetGoodsByBatchInfo(houseInfo.ID, detail.GoodsID);
                    ////按照批次依次出库
                    //foreach (tm_whgoodsorderbatch batchInfo in batchList)
                    //{
                    //    tm_WHOrderGoodsDetail orderDetail = new tm_WHOrderGoodsDetail();
                    //    orderDetail.GoodsID = detail.GoodsID;
                    //    orderDetail.OrderDate = outBoundOrder.OrderDate;
                    //    orderDetail.OrderNO = WHOutBoundOrderNO;
                    //    //获取商品信息
                    //    tm_Goods goosInfo = Core.Container.Instance.Resolve<IServiceGoods>().GetEntity(detail.GoodsID);
                    //    orderDetail.GoodsUnit = goosInfo.GoodsUnit;
                    //    orderDetail.ChangeUnit = goosInfo.PurchaseUnit;
                    //    orderDetail.ChangeNumber = goosInfo.PurchaseNum;
                    //    orderDetail.TaxAmount = 0;
                    //    orderDetail.TaxPoint = goosInfo.TaxPoint;
                    //    orderDetail.TotalPriceNoTax = 0;
                    //    orderDetail.UnitPriceNoTax = 0;
                    //    //如果当前批次剩余库存大于等于销售商品的销售数量
                    //    if (batchInfo.CurrentNumber >= amount)
                    //    {
                    //        orderDetail.GoodsNumber = amount;
                    //        amount = 0;
                    //    }
                    //    else
                    //    {
                    //        orderDetail.GoodsNumber = batchInfo.CurrentNumber;
                    //        amount -= batchInfo.CurrentNumber;
                    //    }
                    //    //获取出库单价和金额
                    //    orderDetail.GoodsUnitPrice = batchInfo.OrderUnitPrice;
                    //    orderDetail.GoodTotalPrice = Math.Round(orderDetail.GoodsNumber * orderDetail.GoodsUnitPrice, 2);
                    //    //保存出库明细信息
                    //    Core.Container.Instance.Resolve<IServiceWHOrderGoodsDetail>().Create(orderDetail);

                    //    //更新进货批次库存信息
                    //    new InventoryHelper().UpdateGoodsBatchInfo(houseInfo.ID, detail.GoodsID, batchInfo.BatchNO, orderDetail.GoodsNumber);
                    //    //更新商品变动明细信息(出库)
                    //    new InventoryHelper().UpdateGoodsJournal(houseInfo.ID, detail.GoodsID, OrderNO, "DC", 2
                    //                                             , -orderDetail.GoodsNumber, batchInfo.OrderUnitPrice
                    //                                             , -orderDetail.GoodTotalPrice, batchInfo.BatchNO
                    //                                             , outBoundOrder.OrderDate);

                    //    //累计计算订单成本金额
                    //    goodsAmount += orderDetail.GoodTotalPrice;
                    //    outBoundOrder.OrderAmount += orderDetail.GoodTotalPrice;
                    //    //判断该商品是否完成订单数量的出库
                    //    if (amount <= 0)
                    //    {
                    //        break;
                    //    }
                    //}
                    //更新商品库存信息
                    new InventoryHelper().UpdateWareHouseStock(houseInfo.ID, detail.GoodsID, -detail.GoodsNumber, 0, -goodsAmount, 0);
                    //更新调拨出库商品成本信息
                    detail.GoodsAmount = goodsAmount;
                    Core.Container.Instance.Resolve<IServiceGoodsAllocationBillDetail>().Update(detail);
                }
                //返回出库单成本金额
                costAmount = outBoundOrder.OrderAmount;
                //创建出库单信息
                Core.Container.Instance.Resolve<IServiceWHOutBoundOrder>().Create(outBoundOrder);
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 调拨入库处理
        /// </summary>
        /// <param name="WHOutBoundOrderNO">出库单号</param>
        /// <param name="WHStorageOrderNO">入库单号</param>
        /// <param name="orderInfo">调拨单信息</param>
        /// <returns></returns>
        private bool WHStorage(string WHOutBoundOrderNO, string WHStorageOrderNO, tm_GoodsAllocationBill orderInfo)
        {
            //获取入库仓库信息 
            WareHouse houseInfo = Core.Container.Instance.Resolve<IServiceWareHouse>().GetEntity(orderInfo.RKWareHouseID);
            //如果入库仓库存在
            if (houseInfo != null)
            {
                // 入库单信息
                WHStorageOrder storageOrder = new WHStorageOrder();
                storageOrder.BOrderNO = OrderNO;
                storageOrder.Operator = User.Identity.Name;
                storageOrder.OrderAmount = orderInfo.AllotAmount;
                storageOrder.OrderNumber = orderInfo.AllotCount;
                storageOrder.OrderDate = orderInfo.OrderDate;
                storageOrder.OrderNO = WHStorageOrderNO;
                storageOrder.OrderType = 2;  //2：调拨入库
                storageOrder.OutOrderNO = WHOutBoundOrderNO;
                storageOrder.Remark = "调拨入库";
                storageOrder.SuplierID = 0;
                storageOrder.WareHouseID = orderInfo.RKWareHouseID;
                Core.Container.Instance.Resolve<IServiceWHStorageOrder>().Create(storageOrder);

                //根据出库单号获取出库单物品明细
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("OrderNO", WHOutBoundOrderNO));
                IList<WHOrderGoodsDetail> WHOrderGoodsList = Core.Container.Instance.Resolve<IServiceWHOrderGoodsDetail>().GetAllByKeys(qryList);
                //写入入库商品明细
                foreach (WHOrderGoodsDetail detail in WHOrderGoodsList)
                {
                    #region 写入入库商品明细
                    WHOrderGoodsDetail orderDetail = new WHOrderGoodsDetail();
                    orderDetail.GoodsID = detail.GoodsID;
                    orderDetail.ChangeNumber = detail.ChangeNumber;
                    orderDetail.GoodsNumber = detail.GoodsNumber;
                    orderDetail.GoodsUnit = detail.GoodsUnit;
                    orderDetail.GoodsUnitPrice = detail.GoodsUnitPrice;
                    orderDetail.GoodTotalPrice = detail.GoodTotalPrice;
                    orderDetail.OrderDate = storageOrder.OrderDate;
                    orderDetail.OrderNO = storageOrder.OrderNO;
                    orderDetail.ChangeUnit = detail.ChangeUnit;
                    orderDetail.TaxAmount = detail.TaxAmount;
                    orderDetail.TaxPoint = detail.TaxPoint;
                    orderDetail.TotalPriceNoTax = detail.TotalPriceNoTax;
                    orderDetail.UnitPriceNoTax = detail.UnitPriceNoTax;
                    Core.Container.Instance.Resolve<IServiceWHOrderGoodsDetail>().Create(orderDetail);

                    #endregion  写入入库商品明细

                    #region 更新商品库存以及流水信息

                    //更新商品库存信息
                    new InventoryHelper().UpdateWareHouseStock(orderInfo.RKWareHouseID, detail.GoodsID, detail.GoodsNumber
                                                              , detail.GoodsUnitPrice, detail.GoodTotalPrice, 0);
                    //更新商品变动明细信息(入库)
                    new InventoryHelper().UpdateGoodsJournal(orderInfo.RKWareHouseID, detail.GoodsID, OrderNO, "DR", 1
                                                             , detail.GoodsNumber, detail.GoodsUnitPrice, detail.GoodTotalPrice
                                                             , "", storageOrder.OrderDate);
                     #endregion 更新商品库存以及流水信息
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        protected void ddlCKWareHouseID_SelectedIndexChanged(object sender, EventArgs e)
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            tm_GoodsAllocationBill orderInfo = Core.Container.Instance.Resolve<IServiceGoodsAllocationBill>().GetEntityByFields(qryList);
            orderInfo.CKWareHouseID = Int32.Parse(ddlCKWareHouseID.SelectedValue);
            orderInfo.RKWareHouseID = Int32.Parse(ddlRKWareHouseID.SelectedValue);
            Core.Container.Instance.Resolve<IServiceGoodsAllocationBill>().Update(orderInfo);
        }

        #endregion

    }
}
