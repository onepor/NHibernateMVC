using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Configuration;
using System.Data;
using ZAJCZN.MIS.Helpers;

namespace ZAJCZN.MIS.Web
{
    public partial class DinnerOrderManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreDinnerOrderView";
            }
        }

        #endregion

        #region 加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dpEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                dpStartDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
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

            qryList.Add(Expression.Gt("OrderState", "1"));

            if (!string.IsNullOrEmpty(qryName))
            {
                qryList.Add(Expression.Like("OrderNO", qryName, MatchMode.Anywhere));
            }
            if (!string.IsNullOrEmpty(dpStartDate.Text))
            {
                qryList.Add(Expression.Ge("OpenTime", DateTime.Parse(dpStartDate.Text)));
            }
            if (!string.IsNullOrEmpty(dpEndDate.Text))
            {
                qryList.Add(Expression.Lt("OpenTime", DateTime.Parse((DateTime.Parse(dpEndDate.Text).AddDays(1).ToString("yyyy-MM-dd")))));
            }
            Order[] orderList = new Order[1];
            Order orderli = new Order("OpenTime", false);
            orderList[0] = orderli;
            int count = 0;
            IList<tm_TabieUsingInfo> list = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
        }
        #endregion

        #region 页面数据转换
        public string GetPayType(string id)
        {
            string strState = string.Empty;
            tm_TabieUsingInfo tabieUsing = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(int.Parse(id));
            if (tabieUsing != null)
            {
                switch (tabieUsing.OrderState)
                {
                    case "1":
                        strState = "就餐中";
                        break;
                    case "2":
                        strState = "已结账";
                        break;
                    case "3":
                        strState = string.Format("【免单】{0}", tabieUsing.FreeReason);
                        break;
                    case "4":
                        strState = string.Format("【挂账】{0}", tabieUsing.Charge);
                        break;
                    default:
                        strState = "就餐中";
                        break;
                }
            }
            return strState;
        }

        //获取单位
        public string GetTabieName(string id)
        {
            tm_Tabie supplierInfo = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(int.Parse(id));
            return supplierInfo != null ? supplierInfo.TabieName : "";
        }

        #endregion

        #region Events

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
            if (e.CommandName == "viewField")
            {
                PageContext.Redirect(string.Format("~/Reports/DinnerOrderEdit.aspx?id={0}", ID));
            }
            if (e.CommandName == "stockField")
            {
                if (CheckPower("CoreCateringEdit"))
                {
                    //库存冲减
                    StockProcess(ID);
                }
                else
                {
                    Alert.Show("您没有反结算的权限！");
                }
            }
            if (e.CommandName == "stockBack")
            {
                if (CheckPower("CoreCateringEdit"))
                {
                    tm_TabieUsingInfo tm_tabieUsingInfo = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(ID);

                    //反结算-更新餐台信息状态
                    tm_Tabie tabieInfo = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(tm_tabieUsingInfo.TabieID);
                    if (tabieInfo.TabieState == 1)
                    {
                        tabieInfo.TabieState = 4;
                        tabieInfo.CurrentUsingID = tm_tabieUsingInfo.ID;
                        Core.Container.Instance.Resolve<IServiceTabie>().Update(tabieInfo);
                        //反结算-更新就餐开台信息状态
                        tm_tabieUsingInfo.OrderState = "1";
                        Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(tm_tabieUsingInfo);

                        //反结算-删除结算信息
                        IList<ICriterion> qryList = new List<ICriterion>();
                        qryList.Add(Expression.Eq("TabieUsingID", ID));
                        tm_TabiePayInfo payInfo = Core.Container.Instance.Resolve<IServiceTabiePayInfo>().GetEntityByFields(qryList);
                        if (payInfo != null)
                        {
                            Core.Container.Instance.Resolve<IServiceTabiePayInfo>().Delete(payInfo);
                        }
                        Alert.Show("请返回营业管理进行反结算处理！");
                        //刷新
                        BindGrid();
                    }
                    else
                    {
                        Alert.Show("餐台当前使用中，不能进行反结算！");
                    }
                }
                else
                {
                    Alert.Show("您没有反结算的权限！");
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

        #endregion

        #region 消费商品出库处理

        /// <summary>
        /// 库存冲减
        /// </summary>
        protected void StockProcess(int TabieUsingID)
        {
            decimal useCount = 0;
            List<int> listWHID = new List<int>();
            IList<ICriterion> qryList = new List<ICriterion>();
            //获取消费单所有菜品信息和数量
            string sql = string.Format("SELECT DishesID,sum(DishesCount) as DishesCount FROM tm_tabiedishesinfo WHERE TabieUsingID={0}  GROUP BY DishesID "
                                      , TabieUsingID);
            DataSet ds = DbHelperSQL.Query(sql);

            if (ds.Tables[0] != null)
            {
                //清空临时表
               // new InventoryHelper().DeleteTempDishesBatching();
                //获取菜品对应库存物品消耗配料信息
                foreach (DataRow dishesInfo in ds.Tables[0].Rows)
                {
                    //获取菜品配料信息
                    sql = string.Format("SELECT GoodsID,WareHouseID,UsingCount FROM tm_dishesbatching WHERE DishesID ={0} "
                                      , dishesInfo["DishesID"].ToString());
                    DataSet dsBatch = DbHelperSQL.Query(sql);
                    if (dsBatch.Tables[0] != null && dsBatch.Tables[0].Rows.Count > 0)
                    {
                        //获取使用数量
                        foreach (DataRow batching in dsBatch.Tables[0].Rows)
                        {
                            //计算菜品原料使用情况
                            useCount = Math.Round(decimal.Parse(batching["UsingCount"].ToString()) * decimal.Parse(dishesInfo["DishesCount"].ToString()), 2);
                            //保存数据到临时表
                            //new InventoryHelper().InserTempDishesBatching(TabieUsingID
                            //                                            , batching["GoodsID"].ToString()
                            //                                            , batching["WareHouseID"].ToString()
                            //                                            , useCount);
                            //保存使用到的库房编号
                            if (!listWHID.Contains(int.Parse(batching["WareHouseID"].ToString())))
                            {
                                listWHID.Add(int.Parse(batching["WareHouseID"].ToString()));
                            }
                        }
                    }
                }
                //如果有配料信息，冲减库存
                if (listWHID.Count > 0)
                {
                    foreach (int whID in listWHID)
                    {
                        //冲减库存,商品出库处理
                        SaveItem(whID, TabieUsingID);
                    }
                }
            }
        }

        /// <summary>
        /// 商品出库处理
        /// </summary>
        /// <param name="whID">库房编号</param>
        /// <returns></returns>
        private bool SaveItem(int whID, int TabieUsingID)
        {
            //获取消费订单信息
            tm_TabieUsingInfo tabieUsingInfo = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(TabieUsingID);
            //检查系统设置是否做库存处理
            bool IsStock = bool.Parse(ConfigurationManager.AppSettings["IsStock"]);

            if (IsStock)
            {
                //获取销售仓库信息 
                WareHouse houseInfo = Core.Container.Instance.Resolve<IServiceWareHouse>().GetEntity(whID);
                //如果仓库存在
                if (houseInfo != null)
                {
                    #region  出库单信息

                    // 出库单信息
                    WHOutBoundOrder storageOrder = new WHOutBoundOrder();
                    storageOrder.BOrderNO = tabieUsingInfo.OrderNO;
                    storageOrder.Operator = User.Identity.Name;
                    storageOrder.OrderAmount = 0;
                    storageOrder.OrderNumber = 0;
                    storageOrder.OrderDate = (DateTime)tabieUsingInfo.ClearTime;
                    storageOrder.OrderNO = string.Format("CK{0}", tabieUsingInfo.OpenTime.ToString("yyyyMMddHHmmss"));
                    storageOrder.OrderType = 6;
                    storageOrder.WareHouseID = houseInfo.ID;
                    storageOrder.OutOrderNO = "";
                    storageOrder.Remark = "消费出库";

                    //获取订单商品明细,生成出库商品明细信息 
                    string sql = string.Format("SELECT GoodsID,sum(UsedCount) as UsedCount FROM tm_dinnerTemp WHERE TabieUsingID={0} and WHID={1}  GROUP BY TabieUsingID,WHID,GoodsID HAVING sum(UsedCount)>0  "
                                      , TabieUsingID, whID);
                    DataSet dsGoods = DbHelperSQL.Query(sql);
                    if (dsGoods.Tables[0] != null)
                    {
                        //写入出库商品明细
                        foreach (DataRow detail in dsGoods.Tables[0].Rows)
                        {
                            decimal goodsAmount = 0;
                            decimal goodsCount = 0;
                            int goodsID = int.Parse(detail["GoodsID"].ToString());
                            //获取商品信息
                            tm_Goods goodsInfo = Core.Container.Instance.Resolve<IServiceGoods>().GetEntity(goodsID);
                            //计算消耗转换标准的数量
                            decimal amount = Math.Round(decimal.Parse(detail["UsedCount"].ToString()) / goodsInfo.ConsumeNum, 2);
                            goodsCount = amount;
                            storageOrder.OrderNumber += amount;
                            ////根据批次号获取出库商品信息 
                            //List<tm_whgoodsorderbatch> batchList = new InventoryHelper().GetGoodsByBatchInfo(houseInfo.ID, goodsID);
                            ////按照批次依次出库
                            //foreach (tm_whgoodsorderbatch batchInfo in batchList)
                            //{
                            //    tm_WHOrderGoodsDetail orderDetail = new tm_WHOrderGoodsDetail();
                            //    orderDetail.GoodsID = goodsID;
                            //    orderDetail.GoodsUnit = goodsInfo.GoodsUnit;
                            //    orderDetail.OrderDate = storageOrder.OrderDate;
                            //    orderDetail.OrderNO = storageOrder.OrderNO;
                            //    orderDetail.TaxAmount = 0;
                            //    orderDetail.TaxPoint = goodsInfo.TaxPoint;
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
                            //    new InventoryHelper().UpdateGoodsBatchInfo(houseInfo.ID, goodsID, batchInfo.BatchNO, orderDetail.GoodsNumber);
                            //    //更新商品变动明细信息(出库)
                            //    new InventoryHelper().UpdateGoodsJournal(houseInfo.ID, goodsID, storageOrder.OrderNO, "XF", 2
                            //                                             , -orderDetail.GoodsNumber, batchInfo.OrderUnitPrice
                            //                                             , -orderDetail.GoodTotalPrice, batchInfo.BatchNO
                            //                                             , tabieUsingInfo.OpenTime);

                            //    //累计计算订单成本金额
                            //    goodsAmount += orderDetail.GoodTotalPrice;
                            //    storageOrder.OrderAmount += orderDetail.GoodTotalPrice;

                            //    //判断该商品是否完成订单数量的出库
                            //    if (amount <= 0)
                            //    {
                            //        break;
                            //    }
                            //}
                            //更新商品库存信息
                            new InventoryHelper().UpdateWareHouseStock(houseInfo.ID, goodsID, -goodsCount, 0, -goodsAmount, 0);
                        }
                        //创建出库单信息
                        Core.Container.Instance.Resolve<IServiceWHOutBoundOrder>().Create(storageOrder);
                    }

                    #endregion
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        #endregion 消费商品出库处理

    }
}