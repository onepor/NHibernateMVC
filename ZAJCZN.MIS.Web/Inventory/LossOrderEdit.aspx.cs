using FineUIPro;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ZAJCZN.MIS.Web
{
    public partial class LossOrderEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreLossOrderEdit";
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
                //绑定领用人
                BindSupplier();
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

                btnReturn.ConfirmText = "确定要取消当前报损单信息吗？";

                btnNew.OnClientClick = Window1.GetShowReference(string.Format("~/PublicWebForm/OrderLossSelectDialog.aspx?rowid={0}", OrderNO), "添加领用商品");
                // 删除选中单元格的客户端脚本 
                btnDeleteSelected.OnClientClick = Grid1.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
                btnDeleteSelected.ConfirmText = String.Format("确定要删除选中的<span class=\"highlight\"><script>{0}</script></span>项记录吗？", Grid1.GetSelectedCountReference());
                btnDeleteSelected.ConfirmTarget = FineUIPro.Target.Top;
                                    
                //绑定表格
                BindGrid();
            }
        }

        #region 绑定数据

        private void BindSupplier()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("IsUsed", "1"));
            //获取当前订单信息

            IList<EmployeeInfo> list = Core.Container.Instance.Resolve<IServiceEmployeeInfo>().GetAllByKeys(qryList);
            ddlSuplier.DataSource = list;
            ddlSuplier.DataBind();
            ddlSuplier.SelectedIndex = 0;
        }

        private void BindWH()
        {
            IList<WareHouse> list = Core.Container.Instance.Resolve<IServiceWareHouse>().GetAll();
            ddlWH.DataSource = list;
            ddlWH.DataBind();
            ddlWH.SelectedIndex = 0;
        }

        private void CreateOrderInfo()
        {
            LossOrder order = new LossOrder();
            order.OrderAmount = 0;
            order.OrderDate = DateTime.Now;
            order.OrderNO = string.Format("BS{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
            order.OrderNumber = 0;
            order.Remark = "";
            order.UserID = 0;
            order.WareHouseID = int.Parse(ddlWH.SelectedValue);
            order.Operator = User.Identity.Name;
            Core.Container.Instance.Resolve<IServiceLossOrder>().Create(order);

            OrderNO = order.OrderNO;

            //初始化页面数据
            lblOrderNo.Text = OrderNO;
            dpStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void GetOrderInfo()
        {
            LossOrder order = Core.Container.Instance.Resolve<IServiceLossOrder>().GetEntity(OrderID);
            OrderNO = order.OrderNO;

            //初始化页面数据
            dpStartDate.Text = order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss");
            txtRemark.Text = order.Remark;
            lblOrderNo.Text = OrderNO;
            ddlSuplier.SelectedValue = order.UserID.ToString();
            ddlWH.SelectedValue = order.WareHouseID.ToString();
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
            IList<LossOrderDetail> list = Core.Container.Instance.Resolve<IServiceLossOrderDetail>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);

            foreach (LossOrderDetail detail in list)
            {
                goodsNumber += detail.GoodsNumber;
                goodsAmount += detail.GoodTotalPrice;

                detail.GoodsInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(detail.GoodsID);
            }
            lblCount.Text = goodsNumber.ToString();
            lblAmount.Text = goodsAmount.ToString();

            Grid1.DataSource = list;
            Grid1.DataBind();

        }

        #endregion

        #region 页面数据转化

        //获取分类名称
        public string GetType(string typeID)
        {
            EquipmentTypeInfo objType = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(int.Parse(typeID));
            return objType != null ? objType.TypeName : "";
        }

        //获取单位
        public string GetUnitName(string state)
        {
            return GetSystemEnumValue("WPDW", state);
        }

        #endregion 页面数据转化

        #region Events

        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            Dictionary<int, Dictionary<string, object>> modifiedDict = Grid1.GetModifiedDict();
            foreach (int rowIndex in modifiedDict.Keys)
            {
                int rowID = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
                LossOrderDetail objInfo = Core.Container.Instance.Resolve<IServiceLossOrderDetail>().GetEntity(rowID);
                if (modifiedDict[rowIndex].Keys.Contains("GoodsUnitPrice"))
                {
                    objInfo.GoodsUnitPrice = Convert.ToDecimal(modifiedDict[rowIndex]["GoodsUnitPrice"]);
                }
                if (modifiedDict[rowIndex].Keys.Contains("GoodsNumber"))
                {
                    objInfo.GoodsNumber = Convert.ToDecimal(modifiedDict[rowIndex]["GoodsNumber"]);
                }
                objInfo.GoodTotalPrice = objInfo.GoodsUnitPrice * objInfo.GoodsNumber;

                Core.Container.Instance.Resolve<IServiceLossOrderDetail>().Update(objInfo);
            }

            BindGrid();
        }

        public void btnReturn_Click(object sender, EventArgs e)
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            //获取当前订单信息
            LossOrder orderInfo = Core.Container.Instance.Resolve<IServiceLossOrder>().GetEntityByFields(qryList);

            //删除临时订单商品信息
            if (orderInfo != null && orderInfo.IsTemp == 1)
            {
                IList<LossOrderDetail> goodsList = Core.Container.Instance.Resolve<IServiceLossOrderDetail>().GetAllByKeys(qryList);
                foreach (LossOrderDetail goods in goodsList)
                {
                    Core.Container.Instance.Resolve<IServiceLossOrderDetail>().Delete(goods);
                }
                //删除临时订单信息               
                Core.Container.Instance.Resolve<IServiceLossOrder>().Delete(orderInfo);
            }
            //返回订单管理页面
            PageContext.Redirect("~/Inventory/LossOrderManager.aspx");
        }

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            CheckPowerWithLinkButtonField("CoreLossOrderEdit", Grid1, "deleteField");
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
                Core.Container.Instance.Resolve<IServiceLossOrderDetail>().Delete(id);
            }
            BindGrid();
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            if (e.CommandName == "Delete")
            {
                Core.Container.Instance.Resolve<IServiceLossOrderDetail>().Delete(ID);
                BindGrid();
            }
        }

        protected void ddlWH_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveItem(1);
        }

        protected void btnSaveTemp_Click(object sender, EventArgs e)
        {
            SaveItem(1);
            PageContext.Redirect("~/Inventory/LossOrderManager.aspx");
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem(0);
            PageContext.Redirect("~/Inventory/LossOrderManager.aspx");
        }

        #endregion

        private void SaveItem(int isTemp)
        {
            bool IsStock = bool.Parse(ConfigurationManager.AppSettings["IsStock"]);
            //更新订单状态为正式订单
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            LossOrder orderInfo = Core.Container.Instance.Resolve<IServiceLossOrder>().GetEntityByFields(qryList);

            orderInfo.OrderDate = DateTime.Parse(dpStartDate.Text);
            orderInfo.OrderAmount = decimal.Parse(lblAmount.Text);
            orderInfo.IsTemp = isTemp;
            orderInfo.OrderNumber = decimal.Parse(lblCount.Text);
            orderInfo.UserID = int.Parse(ddlSuplier.SelectedValue);
            orderInfo.UserName = ddlSuplier.SelectedText;
            orderInfo.WareHouseID = int.Parse(ddlWH.SelectedValue);
            orderInfo.Operator = User.Identity.Name;
            Core.Container.Instance.Resolve<IServiceLossOrder>().Update(orderInfo);

            //正式订单，更新入库信息及流水信息等
            if (isTemp == 0)
            {
                #region  出库单信息
                if (IsStock)
                {
                    // 出库单信息
                    WHOutBoundOrder storageOrder = new WHOutBoundOrder();
                    storageOrder.BOrderNO = OrderNO;
                    storageOrder.Operator = User.Identity.Name;
                    storageOrder.OrderAmount = orderInfo.OrderAmount;
                    storageOrder.OrderNumber = orderInfo.OrderNumber;
                    storageOrder.OrderDate = orderInfo.OrderDate;
                    storageOrder.OrderNO = string.Format("CK{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    storageOrder.OrderType = 4;
                    storageOrder.OutOrderNO = "";
                    storageOrder.Remark = "报损出库";
                    storageOrder.SuplierID = orderInfo.UserID;
                    storageOrder.WareHouseID = orderInfo.WareHouseID;

                    // 出库商品明细信息
                    IList<ICriterion> qryListDetail = new List<ICriterion>();
                    qryListDetail.Add(Expression.Eq("OrderNO", OrderNO));
                    Order[] orderList = new Order[1];
                    Order orderli = new Order("ID", true);
                    orderList[0] = orderli;
                    IList<LossOrderDetail> list = Core.Container.Instance.Resolve<IServiceLossOrderDetail>().GetAllByKeys(qryList, orderList);
                    //写入出库商品明细
                    foreach (LossOrderDetail detail in list)
                    {
                        decimal amount = detail.GoodsNumber;
                        decimal goodsAmount = 0;

                        IList<ICriterion> qryWHList = new List<ICriterion>();
                        qryWHList.Add(Expression.Eq("GoodsID", detail.GoodsID));
                        qryWHList.Add(Expression.Eq("WareHouseID", storageOrder.WareHouseID));
                        WHGoodsDetail whGoodsDetail = Core.Container.Instance.Resolve<IServiceWHGoodsDetail>().GetEntityByFields(qryWHList);

                        WHOrderGoodsDetail orderDetail = new WHOrderGoodsDetail();
                        orderDetail.GoodsID = detail.GoodsID;
                        orderDetail.GoodsUnit = detail.GoodsUnit;
                        orderDetail.OrderDate = storageOrder.OrderDate;
                        orderDetail.OrderNO = storageOrder.OrderNO;
                        orderDetail.GoodsNumber = amount;
                        //获取出库单价和金额
                        orderDetail.GoodsUnitPrice = whGoodsDetail.InventoryUnitPrice;
                        orderDetail.GoodTotalPrice = Math.Round(orderDetail.GoodsNumber * orderDetail.GoodsUnitPrice, 2);

                        orderDetail.TaxAmount = 0;
                        orderDetail.TaxPoint = 0;
                        orderDetail.TotalPriceNoTax = 0;
                        orderDetail.UnitPriceNoTax = 0;

                        //保存出库明细信息
                        Core.Container.Instance.Resolve<IServiceWHOrderGoodsDetail>().Create(orderDetail);
                        //更新商品变动明细信息(出库)
                        new InventoryHelper().UpdateGoodsJournal(storageOrder.WareHouseID, detail.GoodsID, OrderNO, "LY", 2
                                                                 , -orderDetail.GoodsNumber, orderDetail.GoodsUnitPrice
                                                                 , -orderDetail.GoodTotalPrice, ""
                                                                 , orderInfo.OrderDate);

                        //累计计算订单成本金额
                        goodsAmount += orderDetail.GoodTotalPrice;

                        //更新商品库存信息
                        new InventoryHelper().UpdateWareHouseStock(storageOrder.WareHouseID, detail.GoodsID, -detail.GoodsNumber, 0, -goodsAmount, 0);
                    }
                    //创建出库单信息
                    Core.Container.Instance.Resolve<IServiceWHOutBoundOrder>().Create(storageOrder);
                }

                #endregion  入库单信息
            }
        }      
    }
}

