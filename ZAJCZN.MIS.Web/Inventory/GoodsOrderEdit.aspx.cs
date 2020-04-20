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
    public partial class GoodsOrderEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreGoodsOrderEdit";
            }
        }

        #endregion

        #region PageParam

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

        #endregion PageParam

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //绑定供应商
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
                btnReturn.ConfirmText = "确定要取消当前订单信息吗？";

                btnNew.OnClientClick = Window1.GetShowReference(string.Format("~/PublicWebForm/OrderGoodsSelectDialog.aspx?rowid={0}", OrderNO), "添加进货商品");
                // 删除选中单元格的客户端脚本 
                btnDeleteSelected.OnClientClick = Grid1.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
                btnDeleteSelected.ConfirmText = String.Format("确定要删除选中的<span class=\"highlight\"><script>{0}</script></span>项配料记录吗？", Grid1.GetSelectedCountReference());
                btnDeleteSelected.ConfirmTarget = FineUIPro.Target.Top;

                // 绑定表格
                BindGrid();
            }
        }

        #region 绑定数据
        private void BindSupplier()
        {
            IList<SupplierInfo> list = Core.Container.Instance.Resolve<IServiceSupplierInfo>().GetAll();

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
            dpStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            GoodsOrder order = new GoodsOrder();
            order.Operator = User.Identity.Name;
            order.OrderAmount = 0;
            order.OrderDate = DateTime.Now;
            order.OrderNO = string.Format("JH{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
            order.OrderNumber = 0;
            order.OrderPayType = 1;
            order.OrderType = 1;
            order.OriginalOrderNO = "";
            order.Remark = "";
            order.SuplierID = 0;
            order.WareHouseID = 0;
            order.IsTemp = 1;
            Core.Container.Instance.Resolve<IServiceGoodsOrder>().Create(order);

            OrderNO = order.OrderNO;

            //初始化页面数据
            lblOrderNo.Text = OrderNO;
        }

        private void GetOrderInfo()
        {
            GoodsOrder order = Core.Container.Instance.Resolve<IServiceGoodsOrder>().GetEntity(OrderID);
            OrderNO = order.OrderNO;

            //初始化页面数据
            dpStartDate.Text = order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss");
            txtRemark.Text = order.Remark;
            ddlSuplier.SelectedValue = order.SuplierID.ToString();
            ddlWH.SelectedValue = order.WareHouseID.ToString();
            ddlPay.SelectedValue = order.OrderPayType.ToString();
            lblOrderNo.Text = OrderNO;
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

            IList<GoodsOrderDetail> list = Core.Container.Instance.Resolve<IServiceGoodsOrderDetail>().GetAllByKeys(qryList, orderList);

            foreach (GoodsOrderDetail detail in list)
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
                GoodsOrderDetail objInfo = Core.Container.Instance.Resolve<IServiceGoodsOrderDetail>().GetEntity(rowID);
                if (modifiedDict[rowIndex].Keys.Contains("GoodsUnitPrice"))
                {
                    objInfo.GoodsUnitPrice = Convert.ToDecimal(modifiedDict[rowIndex]["GoodsUnitPrice"]);
                }
                if (modifiedDict[rowIndex].Keys.Contains("GoodsNumber"))
                {
                    objInfo.GoodsNumber = Convert.ToDecimal(modifiedDict[rowIndex]["GoodsNumber"]);
                }

                objInfo.GoodTotalPrice = objInfo.GoodsUnitPrice * objInfo.GoodsNumber;

                Core.Container.Instance.Resolve<IServiceGoodsOrderDetail>().Update(objInfo);
            }

            BindGrid();
        }

        public void btnReturn_Click(object sender, EventArgs e)
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            //获取当前订单信息
            GoodsOrder orderInfo = Core.Container.Instance.Resolve<IServiceGoodsOrder>().GetEntityByFields(qryList);

            //删除临时订单商品信息
            if (orderInfo != null && orderInfo.IsTemp == 1)
            {
                IList<GoodsOrderDetail> goodsList = Core.Container.Instance.Resolve<IServiceGoodsOrderDetail>().GetAllByKeys(qryList);
                foreach (GoodsOrderDetail goods in goodsList)
                {
                    Core.Container.Instance.Resolve<IServiceGoodsOrderDetail>().Delete(goods);
                }
                //删除临时订单信息               
                Core.Container.Instance.Resolve<IServiceGoodsOrder>().Delete(orderInfo);
            }
            //返回订单管理页面
            PageContext.Redirect("~/Inventory/GoodsOrderManage.aspx");
        }

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            CheckPowerWithLinkButtonField("CoreGoodsOrderEdit", Grid1, "deleteField");
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
                Core.Container.Instance.Resolve<IServiceGoodsOrderDetail>().Delete(id);
            }
            BindGrid();
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            if (e.CommandName == "Delete")
            {
                Core.Container.Instance.Resolve<IServiceGoodsOrderDetail>().Delete(ID);
                BindGrid();
            }
        }

        protected void btnSaveTemp_Click(object sender, EventArgs e)
        {
            SaveItem(1);
            PageContext.Redirect("~/Inventory/GoodsOrderManage.aspx");
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem(0);
            PageContext.Redirect("~/Inventory/GoodsOrderManage.aspx");
        }

        private void SaveItem(int isTemp)
        {
            bool IsStock = bool.Parse(ConfigurationManager.AppSettings["IsStock"]);
            //更新订单状态为正式订单
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            GoodsOrder orderInfo = Core.Container.Instance.Resolve<IServiceGoodsOrder>().GetEntityByFields(qryList);

            orderInfo.OrderDate = DateTime.Parse(dpStartDate.Text);
            orderInfo.IsTemp = isTemp;
            orderInfo.OrderAmount = decimal.Parse(lblAmount.Text);
            orderInfo.OrderNumber = decimal.Parse(lblCount.Text);
            orderInfo.SuplierID = int.Parse(ddlSuplier.SelectedValue);
            orderInfo.WareHouseID = int.Parse(ddlWH.SelectedValue);
            orderInfo.OrderPayType = int.Parse(ddlPay.SelectedValue);
            orderInfo.Remark = txtRemark.Text;
            orderInfo.Operator = User.Identity.Name;
            Core.Container.Instance.Resolve<IServiceGoodsOrder>().Update(orderInfo);

            //正式订单，更新入库信息及流水信息等
            if (isTemp == 0)
            {
                //获取进货商品明细
                IList<ICriterion> qryListDetail = new List<ICriterion>();
                qryListDetail.Add(Expression.Eq("OrderNO", OrderNO));
                Order[] orderList = new Order[1];
                Order orderli = new Order("ID", true);
                orderList[0] = orderli;
                IList<GoodsOrderDetail> list = Core.Container.Instance.Resolve<IServiceGoodsOrderDetail>().GetAllByKeys(qryList, orderList);

                #region  入库单信息
                if (IsStock)
                {
                    // 入库单信息
                    WHStorageOrder storageOrder = new WHStorageOrder();
                    storageOrder.BOrderNO = OrderNO;
                    storageOrder.Operator = User.Identity.Name;
                    storageOrder.OrderAmount = orderInfo.OrderAmount;
                    storageOrder.OrderNumber = orderInfo.OrderNumber;
                    storageOrder.OrderDate = orderInfo.OrderDate;
                    storageOrder.OrderNO = string.Format("RK{0}", DateTime.Parse(dpStartDate.Text).ToString("yyyyMMddHHmmss"));
                    storageOrder.OrderType = 1;
                    storageOrder.OutOrderNO = "";
                    storageOrder.Remark = "进货入库";
                    storageOrder.SuplierID = orderInfo.SuplierID;
                    storageOrder.WareHouseID = orderInfo.WareHouseID;
                    Core.Container.Instance.Resolve<IServiceWHStorageOrder>().Create(storageOrder);

                    //写入入库商品明细
                    foreach (GoodsOrderDetail detail in list)
                    {
                        WHOrderGoodsDetail orderDetail = new WHOrderGoodsDetail();
                        orderDetail.GoodsID = detail.GoodsID;
                        orderDetail.GoodsNumber = detail.GoodsNumber;
                        orderDetail.GoodsUnit = detail.GoodsUnit;
                        orderDetail.GoodsUnitPrice = detail.GoodsUnitPrice;
                        orderDetail.GoodTotalPrice = detail.GoodTotalPrice;
                        orderDetail.OrderDate = storageOrder.OrderDate;
                        orderDetail.OrderNO = storageOrder.OrderNO;
                        orderDetail.TaxAmount = detail.TaxAmount;
                        orderDetail.TaxPoint = detail.TaxPoint;
                        orderDetail.TotalPriceNoTax = detail.TotalPriceNoTax;
                        orderDetail.UnitPriceNoTax = detail.UnitPriceNoTax;
                        Core.Container.Instance.Resolve<IServiceWHOrderGoodsDetail>().Create(orderDetail);
                    }
                }
                #endregion  入库单信息

                #region 更新商品库存以及流水信息
                //写入入库商品明细
                foreach (GoodsOrderDetail detail in list)
                {
                    //更新商品库存信息
                    new InventoryHelper().UpdateWareHouseStock(orderInfo.WareHouseID, detail.GoodsID, detail.GoodsNumber
                                                        , detail.GoodsUnitPrice, detail.GoodTotalPrice, 1);

                    //更新商品变动明细信息(入库)
                    new InventoryHelper().UpdateGoodsJournal(orderInfo.WareHouseID, detail.GoodsID, OrderNO, "JH", 1
                                                             , detail.GoodsNumber, detail.GoodsUnitPrice, detail.GoodTotalPrice
                                                             , "", orderInfo.OrderDate);
                }

                #endregion 更新商品库存以及流水信息

            }
        }
        #endregion

    }
}
