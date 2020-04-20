using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUIPro;
using ZAJCZN.MIS.Helpers;
using System.Data;
using System.Configuration;

namespace ZAJCZN.MIS.Web
{
    public partial class DMergePay : PageBase
    {
        protected string MergeNO
        {
            get { return GetQueryValue("id"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GPPayWay.Hidden = false;
                GPvip.Hidden = false;
                GPFreeReason.Hidden = true;
                GPChargeReason.Hidden = true;

                //设置结算时间
                dltCloseTabie.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //获取合并付款餐桌就餐信息
                BindPayInfo();
            }
        }

        #region 页面数据绑定

        /// <summary>
        /// 绑定免单原因
        /// </summary>
        protected void BindFreeReason()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("AbnormalType", "4"));
            IList<Tm_Abnormal> list = Core.Container.Instance.Resolve<IServiceAbnormal>().Query(qryList);
            ddlFreeReason.DataSource = list;
            ddlFreeReason.DataBind();
            ddlFreeReason.SelectedIndex = 0; ;

        }

        /// <summary>
        /// 绑定挂账原因
        /// </summary>
        protected void BindChargeReason()
        {
            IList<tm_Charge> list = Core.Container.Instance.Resolve<IServiceCharge>().GetAll().ToList();
            list.Insert(0, new tm_Charge { ID = 0, ChargeName = "--请选择--" });
            ddlChargeReason.DataSource = list;
            ddlChargeReason.DataBind();
            ddlChargeReason.SelectedIndex = 0;
        }

        /// <summary>
        /// 折扣授权码处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbxKey_Blur(object sender, EventArgs e)
        {
            StringBuilder tabieUsingIDs = new StringBuilder();

            //获取合并餐台信息
            List<ICriterion> qrylist = new List<ICriterion>();
            qrylist.Add(Expression.Eq("MergeNO", MergeNO));
            IList<tm_TabieUsingInfo> list = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Query(qrylist);

            foreach (tm_TabieUsingInfo usingInfo in list)
            {
                tabieUsingIDs.AppendFormat("{0},", usingInfo.ID);
                //更新每个餐台的打折比例
                usingInfo.DisPoint = decimal.Parse(ddlDiscount.SelectedValue);
                Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(usingInfo);
            }
            //获取所有点菜参与打折菜品金额信息
            string sql = string.Format("SELECT SUM(Moneys) as Moneys FROM tm_tabiedishesinfo WHERE TabieUsingID in ({0}) and DishesType=1 AND IsFree=0 AND IsDiscount=1 ", tabieUsingIDs.ToString().TrimEnd(','));
            DataSet dsMoneys = DbHelperMySQL.Query(sql);
            if (dsMoneys.Tables[0] != null)
            {
                decimal Discount = (1 - decimal.Parse(ddlDiscount.SelectedValue)) * decimal.Parse(dsMoneys.Tables[0].Rows[0]["Moneys"].ToString());
                decimal FactPrice = decimal.Parse(labFactPrice.Text) - Discount;
                txtDiscount.Text = Math.Round(Discount, 2).ToString();
                labFactPrice.Text = Math.Round(FactPrice, 2).ToString();
            }
            else
            {
                Alert.Show("菜品消费总金额获取失败");
            }
        }

        /// <summary>
        /// 修改开台时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dltCloseTabie_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MergeNO))
            {
                //获取合并餐台信息
                List<ICriterion> qrylist = new List<ICriterion>();
                qrylist.Add(Expression.Eq("MergeNO", MergeNO));
                IList<tm_TabieUsingInfo> list = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Query(qrylist);

                //获取整单交易和应付金额
                foreach (tm_TabieUsingInfo usingInfo in list)
                {
                    usingInfo.ClearTime = DateTime.Parse(DateTime.Parse(dltCloseTabie.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss"));
                    Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(usingInfo);
                }
            }
        }

        /// <summary>
        /// 折扣手工处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbxDiscount_Blur(object sender, EventArgs e)
        {
            //获取手工填写的折扣金额
            decimal Discount = !string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? decimal.Parse(txtDiscount.Text.Trim()) : 0;
            decimal FactPrice = decimal.Parse(labFactPrice.Text) - Discount;
            labFactPrice.Text = Math.Round(FactPrice, 2).ToString();
        }

        /// <summary>
        /// 会员号变化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtVip_Blur(object sender, EventArgs e)
        {
            lblVipMoney.Text = "0";
            if (!string.IsNullOrEmpty(MergeNO))
            {
                //获取合并餐台信息
                List<ICriterion> qrylist = new List<ICriterion>();
                qrylist.Add(Expression.Eq("MergeNO", MergeNO));
                IList<tm_TabieUsingInfo> list = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Query(qrylist);

                //获取会员信息
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("VIPPhone", txtVip.Text.Trim()));
                tm_vipinfo vipInfo = Core.Container.Instance.Resolve<IServiceVipInfo>().GetEntityByFields(qryList);

                if (vipInfo != null)
                {
                    //获取整单交易和应付金额
                    foreach (tm_TabieUsingInfo usingInfo in list)
                    {
                        lblVipMoney.Text = vipInfo.VIPCount.ToString();
                        //更新会员信息
                        usingInfo.VipID = txtVip.Text.Trim();
                        Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(usingInfo);
                    }
                }
                else
                {
                    Alert.Show("会员号不存在!");
                }
            }
        }

        /// <summary>
        /// 绑定交易信息
        /// </summary>
        private void BindPayInfo()
        {
            if (!string.IsNullOrEmpty(MergeNO))
            {
                //获取合并餐台信息
                List<ICriterion> qrylist = new List<ICriterion>();
                qrylist.Add(Expression.Eq("MergeNO", MergeNO));
                IList<tm_TabieUsingInfo> list = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Query(qrylist);
                gdTabieUsingInfo.DataSource = list;
                gdTabieUsingInfo.DataBind();

                decimal payMoneys = 0;
                decimal payPerMoneys = 0;
                decimal payFactMoneys = 0;

                //获取整单交易和应付金额
                foreach (tm_TabieUsingInfo usingInfo in list)
                {
                    payMoneys += usingInfo.Moneys;
                    payPerMoneys += usingInfo.PrePrice;
                    payFactMoneys += usingInfo.FactPrice;

                }
                labMoneys.Text = payMoneys.ToString();
                labPrePrice.Text = payPerMoneys.ToString();
                labFactPrice.Text = payFactMoneys.ToString();
            }
            else
            {
                Alert.Show("合台参数获取错误！");
                PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            }
        }

        public string GetTabieName(string tabieID)
        {
            tm_Tabie tabieInfo = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(int.Parse(tabieID));
            return tabieInfo != null ? tabieInfo.TabieName : "";

        }
        #endregion 页面数据绑定

        #region 账单结算处理

        /// <summary>
        /// 账单结算方式选择处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblPayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strPayType = rblPayType.SelectedValue;       //支付结算方式

            if (strPayType == "1")                       //正常支付结算
            {
                GPPayWay.Hidden = false;
                GPvip.Hidden = false;
                GPFreeReason.Hidden = true;
                GPChargeReason.Hidden = true;
                ddlDiscount.Hidden = false;
                tbxKey.Hidden = false;
            }
            else if (strPayType == "2")                  //免单处理
            {
                GPPayWay.Hidden = true;
                GPvip.Hidden = true;
                GPFreeReason.Hidden = false;
                GPChargeReason.Hidden = true;
                ddlDiscount.Hidden = true;
                tbxKey.Hidden = true;
                //绑定免单原因
                BindFreeReason();
            }
            else                                   //挂账处理
            {
                GPPayWay.Hidden = true;
                GPvip.Hidden = true;
                GPFreeReason.Hidden = true;
                GPChargeReason.Hidden = false;
                ddlDiscount.Hidden = true;
                tbxKey.Hidden = true;
                //绑定挂账人
                BindChargeReason();
            }
        }

        /// <summary>
        /// 账单结算保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //获取合并餐台信息
            List<ICriterion> qrylist = new List<ICriterion>();
            qrylist.Add(Expression.Eq("MergeNO", MergeNO));
            IList<tm_TabieUsingInfo> listTabieUsingInfo = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Query(qrylist);

            string strPayType = rblPayType.SelectedValue;    //支付方式
            StringBuilder groupCard = new StringBuilder();
            decimal groupMoney = 0;
            StringBuilder groupName = new StringBuilder();
            decimal totalAmount = 0;

            //检查是否有原付款信息,有删除原付款信息
            qrylist = new List<ICriterion>();
            qrylist.Add(Expression.Eq("MergeNO", MergeNO));
            IList<tm_TabiePayInfo> PayEntityList = Core.Container.Instance.Resolve<IServiceTabiePayInfo>().GetAllByKeys(qrylist);
            foreach (tm_TabiePayInfo payObj in PayEntityList)
            {
                Core.Container.Instance.Resolve<IServiceTabiePayInfo>().Delete(payObj);
            }

            int index = 0;
            //记录付款信息
            foreach (tm_TabieUsingInfo tabieUsingInfo in listTabieUsingInfo)
            {
                //保存就餐结算完成时间
                tabieUsingInfo.ClearTime = DateTime.Parse(dltCloseTabie.Text);
                //结算处理
                if (strPayType == "1")                           //正常支付结算                   
                {
                    if (index == 0)
                    {
                        //保存抹零金额,计入第一个餐台信息中
                        if (!string.IsNullOrEmpty(nbML.Text) && decimal.Parse(nbML.Text) > 0)
                        {
                            tabieUsingInfo.Erasing = decimal.Parse(nbML.Text);
                        }
                    }
                    //设置就餐信息状态为已结账
                    tabieUsingInfo.OrderState = "2";
                }
                else if (strPayType == "2")                //免单处理
                {
                    tabieUsingInfo.FreeReason = ddlFreeReason.SelectedValue;
                    //设置就餐信息状态为免单
                    tabieUsingInfo.OrderState = "3";
                }
                else                                       //挂账处理
                {
                    if (ddlChargeReason.SelectedValue.Equals("0") && string.IsNullOrEmpty(txbChargeReason.Text))
                    {
                        Alert.ShowInTop("请选择或填入挂账人信息", "错误提示", MessageBoxIcon.Warning);
                        return;
                    }
                    //如果是手填的挂账人，保存
                    if (!string.IsNullOrEmpty(txbChargeReason.Text.Trim()))
                    {
                        tabieUsingInfo.Charge = txbChargeReason.Text.Trim();
                        //如果是填写的挂账人，保存到挂账人信息表
                        tm_Charge chargeInfo = new tm_Charge();
                        chargeInfo.ChargeName = txbChargeReason.Text.Trim();
                        chargeInfo.Remark = txbChargeReason.Text.Trim();
                        Core.Container.Instance.Resolve<IServiceCharge>().Create(chargeInfo);
                    }
                    else
                    {
                        tabieUsingInfo.Charge = ddlChargeReason.SelectedText;
                    }
                    //设置就餐信息状态为挂账
                    tabieUsingInfo.OrderState = "4";
                }
                Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(tabieUsingInfo);

                //团购金额
                if (!string.IsNullOrEmpty(tabieUsingInfo.GroupName))
                {
                    groupCard.AppendFormat("{0},", tabieUsingInfo.GroupCardNO);
                    groupMoney += tabieUsingInfo.GroupMoneys;
                    groupName.AppendFormat("{0},", tabieUsingInfo.GroupName);
                }
                ////计算应付总额
                //totalAmount += tabieUsingInfo.FactPrice;
                index++;
            }
            //计算应付总额
            totalAmount = decimal.Parse(labFactPrice.Text);
            //-----------------------------结算处理-----------------------------------
            if (strPayType == "1")                           //正常支付结算                   
            {
                //保存结算付款信息
                decimal? Amount = 0;
                tm_TabiePayInfo PayEntity = new tm_TabiePayInfo();
                PayEntity.PayTime = DateTime.Parse(dltCloseTabie.Text);
                PayEntity.TabieUsingID = 0;
                PayEntity.MergeNO = MergeNO;

                if (!string.IsNullOrEmpty(nbxVip.Text) && decimal.Parse(nbxVip.Text) > 0)   //会员卡支付
                {
                    //检查会员卡余额
                    if (decimal.Parse(nbxVip.Text) > decimal.Parse(lblVipMoney.Text))
                    {
                        Alert.Show("会员卡余额不足!");
                        return;
                    }

                    PayEntity.VipcardMoneys = decimal.Parse(nbxVip.Text);
                    PayEntity.PayWayVipcard = "1";
                    PayEntity.VipCardNO = txtVip.Text;
                    Amount += decimal.Parse(nbxVip.Text);
                }
                if (!string.IsNullOrEmpty(nbCash.Text) && decimal.Parse(nbCash.Text) > 0)   //现金支付
                {
                    PayEntity.CashMoneys = decimal.Parse(nbCash.Text);
                    PayEntity.PayWayCash = "1";
                    Amount += decimal.Parse(nbCash.Text);
                }
                if (!string.IsNullOrEmpty(nbCard.Text) && decimal.Parse(nbCard.Text) > 0)   //刷卡支付
                {
                    PayEntity.CreditMoneys = decimal.Parse(nbCard.Text);
                    PayEntity.PayWayCredit = "1";
                    Amount += decimal.Parse(nbCard.Text);
                }
                if (!string.IsNullOrEmpty(nbWX.Text) && decimal.Parse(nbWX.Text) > 0)       //微信支付
                {
                    PayEntity.OnlineMoneys = decimal.Parse(nbWX.Text);
                    PayEntity.PayWayOnline = "1";
                    Amount += decimal.Parse(nbWX.Text);
                }
                if (!string.IsNullOrEmpty(nbZFB.Text) && decimal.Parse(nbZFB.Text) > 0)     //支付宝支付
                {
                    PayEntity.ZFBMoneys = decimal.Parse(nbZFB.Text);
                    PayEntity.PayWayZFB = "1";
                    Amount += decimal.Parse(nbZFB.Text);
                }
                if (!string.IsNullOrEmpty(nbML.Text) && decimal.Parse(nbML.Text) > 0)       //抹零金额
                {
                    Amount += decimal.Parse(nbML.Text);
                }
                if (!string.IsNullOrEmpty(groupCard.ToString()))                        //团购金额
                {
                    PayEntity.GroupCardNO = groupCard.ToString();
                    PayEntity.PayWayGroup = "1";
                    PayEntity.GroupMoneys = groupMoney;
                }
                //判断填写的实际支付金额和应付金额是否一致
                if (Amount != totalAmount)
                {
                    Alert.ShowInTop("录入的金额总数与实付金额不匹配", "错误提示", MessageBoxIcon.Warning);
                    return;
                }
                //保存结算支付信息
                Core.Container.Instance.Resolve<IServiceTabiePayInfo>().Create(PayEntity);
            }
            //单元结算单
            LocalPrint(groupCard.ToString(), groupName.ToString());
            //库存冲减
            StockProcess();
            //清台并返回餐台列表
            CloseTabie();
        }

        /// <summary>
        /// 清台并返回餐台列表
        /// </summary>
        protected void CloseTabie()
        {
            //获取合并餐台信息
            List<ICriterion> qrylist = new List<ICriterion>();
            qrylist.Add(Expression.Eq("MergeNO", MergeNO));
            IList<tm_TabieUsingInfo> listTabieUsingInfo = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Query(qrylist);
            //设置餐台状态为未开台
            foreach (tm_TabieUsingInfo entity in listTabieUsingInfo)
            {
                tm_Tabie tabieInfo = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(entity.TabieID);
                tabieInfo.TabieState = 1;
                tabieInfo.CurrentUsingID = 0;
                Core.Container.Instance.Resolve<IServiceTabie>().Update(tabieInfo);

                //更新就餐信息菜品信息打印状态
                string sql = string.Format("UPDATE tm_tabiedishesinfo SET IsPrint=1 WHERE TabieUsingID={0} and IsPrint=0 ", entity.ID);
                DbHelperMySQL.ExecuteSql(sql);
            }

            //更新会员信息
            bool IsVip = bool.Parse(ConfigurationManager.AppSettings["IsVip"]);
            if (IsVip)
            {
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("VIPPhone", txtVip.Text.Trim()));
                tm_vipinfo vipInfo = Core.Container.Instance.Resolve<IServiceVipInfo>().GetEntityByFields(qryList);

                if (vipInfo != null)
                {
                    vipInfo.VIPCount -= decimal.Parse(nbxVip.Text);
                    Core.Container.Instance.Resolve<IServiceVipInfo>().Update(vipInfo);
                }
            }

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #region 消费商品出库处理

        /// <summary>
        /// 库存冲减
        /// </summary>
        protected void StockProcess()
        {
            decimal useCount = 0;
            List<int> listWHID = new List<int>();
            //获取合并餐台信息
            List<ICriterion> qrylist = new List<ICriterion>();
            qrylist.Add(Expression.Eq("MergeNO", MergeNO));
            IList<tm_TabieUsingInfo> listTabieUsingInfo = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Query(qrylist);

            //依次冲减每个订单的库存
            foreach (tm_TabieUsingInfo usingInfo in listTabieUsingInfo)
            {
                //获取消费单所有菜品信息和数量
                string sql = string.Format("SELECT DishesID,sum(DishesCount) as DishesCount FROM tm_tabiedishesinfo WHERE TabieUsingID={0}  GROUP BY DishesID HAVING sum(DishesCount)>0  "
                                          , usingInfo.ID);
                DataSet ds = DbHelperMySQL.Query(sql);

                if (ds.Tables[0] != null)
                {
                    //清空临时表
                    new InventoryHelper().DeleteTempDishesBatching();
                    //获取菜品对应库存物品消耗配料信息
                    foreach (DataRow dishesInfo in ds.Tables[0].Rows)
                    {
                        //获取菜品配料信息
                        sql = string.Format("SELECT GoodsID,WareHouseID,UsingCount FROM tm_dishesbatching WHERE DishesID ={0} "
                                          , dishesInfo["DishesID"].ToString());
                        DataSet dsBatch = DbHelperMySQL.Query(sql);
                        if (dsBatch.Tables[0] != null && dsBatch.Tables[0].Rows.Count > 0)
                        {
                            //获取使用数量
                            foreach (DataRow batching in dsBatch.Tables[0].Rows)
                            {
                                //计算菜品原料使用情况
                                useCount = Math.Round(decimal.Parse(batching["UsingCount"].ToString()) * decimal.Parse(dishesInfo["DishesCount"].ToString()), 2);
                                //保存数据到临时表
                                new InventoryHelper().InserTempDishesBatching(usingInfo.ID
                                                                            , batching["GoodsID"].ToString()
                                                                            , batching["WareHouseID"].ToString()
                                                                            , useCount);
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
                            SaveItem(whID);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 商品出库处理
        /// </summary>
        /// <param name="whID">库房编号</param>
        /// <returns></returns>
        private bool SaveItem(int whID)
        {
            //检查系统设置是否做库存处理
            bool IsStock = bool.Parse(ConfigurationManager.AppSettings["IsStock"]);
            //获取合并餐台信息
            List<ICriterion> qrylist = new List<ICriterion>();
            qrylist.Add(Expression.Eq("MergeNO", MergeNO));
            IList<tm_TabieUsingInfo> listTabieUsingInfo = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Query(qrylist);

            //依次冲减每个订单的库存
            foreach (tm_TabieUsingInfo usingInfo in listTabieUsingInfo)
            {
                if (IsStock)
                {
                    //获取销售仓库信息 
                    WareHouse houseInfo = Core.Container.Instance.Resolve<IServiceWareHouse>().GetEntity(whID);
                    //如果仓库存在
                    if (houseInfo != null)
                    {
                        #region  出库单信息

                        // 出库单信息
                        tm_WHOutBoundOrder storageOrder = new tm_WHOutBoundOrder();
                        storageOrder.BOrderNO = usingInfo.MergeNO;
                        storageOrder.Operator = User.Identity.Name;
                        storageOrder.OrderAmount = 0;
                        storageOrder.OrderNumber = 0;
                        storageOrder.OrderDate = (DateTime)usingInfo.ClearTime;
                        storageOrder.OrderNO = string.Format("CK{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                        storageOrder.OrderType = 6;
                        storageOrder.WareHouseID = houseInfo.ID;
                        storageOrder.OutOrderNO = "";
                        storageOrder.Remark = "消费出库";

                        //获取订单商品明细,生成出库商品明细信息 
                        string sql = string.Format("SELECT GoodsID,sum(UsedCount) as UsedCount FROM tm_dinnerTemp WHERE TabieUsingID={0} and WHID={1}  GROUP BY TabieUsingID,WHID,GoodsID "
                                          , usingInfo.ID, whID);
                        DataSet dsGoods = DbHelperMySQL.Query(sql);
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
                                //                                             , DateTime.Now);

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
            }
            return true;
        }

        #endregion 消费商品出库处理

        #endregion 账单结算处理

        #region 前台本地打印

        /// <summary>
        /// 结算留存单打印
        /// </summary>
        protected void LocalPrint(string groupCard, string groupMoney)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder count = new StringBuilder();
            StringBuilder price = new StringBuilder();
            StringBuilder tabieNames = new StringBuilder();

            decimal payMoneys = 0;
            decimal payPerMoneys = 0;
            decimal payErasingMoneys = 0;
            decimal payFactMoneys = 0;
            int PopulationCount = 0;
            decimal payDisCount = 0;
            decimal payDisPoint = 0;

            //获取合并餐台信息
            List<ICriterion> qrylist = new List<ICriterion>();
            qrylist.Add(Expression.Eq("MergeNO", MergeNO));
            IList<tm_TabieUsingInfo> listTabieUsingInfo = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Query(qrylist);

            //获取台号和整单交易和应付金额
            foreach (tm_TabieUsingInfo tabieUsingInfo in listTabieUsingInfo)
            {
                tm_Tabie tabieEntity = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(tabieUsingInfo.TabieID);
                tabieNames.AppendFormat("{0},", tabieEntity.TabieName);
                //获取整单交易和应付金额                
                payMoneys += tabieUsingInfo.Moneys;
                payPerMoneys += tabieUsingInfo.PrePrice;
                payErasingMoneys += tabieUsingInfo.Erasing;
                payFactMoneys += tabieUsingInfo.FactPrice;
                PopulationCount += tabieUsingInfo.Population;
                payDisPoint = tabieUsingInfo.DisPoint;
            }

            sb.Append("农投良品生活馆\n");
            sb.Append("结算单\n");
            sb.AppendFormat("桌  位:{0}\n", tabieNames.ToString());
            sb.AppendFormat("账单编号：{0}\n", listTabieUsingInfo[0].OrderNO);
            sb.AppendFormat("营业日期：{0}\n", DateTime.Now.ToShortDateString());
            sb.AppendFormat("开台时间：{0}\n", listTabieUsingInfo[0].OpenTime.ToString("yyyy-MM-dd hh:mm:ss"));
            sb.AppendFormat("结账时间：{0}\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            sb.AppendFormat("客  数：{0}\n", PopulationCount);
            sb.AppendFormat("收款机号 \n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            count.AppendFormat("收银员：{0} \n", User.Identity.Name);
            price.Append("\n");
            price.Append("\n");
            price.Append("\n");
            price.Append("\n");
            price.Append("\n");
            price.Append("\n");
            price.Append("\n");
            price.Append("\n");
            price.Append("\n");
            sb.Append("---------------------------------------------------------------------------\n");
            count.Append("\n");
            price.Append("\n");

            //设置餐台状态为未开台
            foreach (tm_TabieUsingInfo tabieUsingInfo in listTabieUsingInfo)
            {
                tm_Tabie tabieEntity = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(tabieUsingInfo.TabieID);
                sb.Append("\n");
                count.AppendFormat("【{0}】\n", tabieEntity.TabieName);
                price.Append("\n");
                sb.Append("---------------------------------------------------------------------------\n");
                count.Append("\n");
                price.Append("\n");
                sb.Append("菜品名称\n");
                count.Append("数量 \n");
                price.Append("金额 \n");
                sb.Append("---------------------------------------------------------------------------\n");
                count.Append("\n");
                price.Append("\n");

                //获取点菜信息
                string sql = string.Format("SELECT DishesName,sum(DishesCount) as DishesCount,SUM(Moneys) as Moneys,UnitName,IsFree,DishesType FROM tm_tabiedishesinfo WHERE TabieUsingID={0} GROUP BY DishesName,UnitName,IsFree,DishesType ORDER BY DishesName "
                                        , tabieUsingInfo.ID);
                DataSet ds = DbHelperMySQL.Query(sql);

                if (ds.Tables[0] != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        //判断赠送
                        if (!row["IsFree"].ToString().Equals("1"))
                        {
                            //判断退菜
                            if (row["DishesType"].ToString().Equals("1"))
                            {
                                sb.AppendFormat("{0}\n", row["DishesName"].ToString());
                            }
                            if (row["DishesType"].ToString().Equals("2"))
                            {
                                sb.AppendFormat("{0}[退菜]\n", row["DishesName"].ToString());
                            }
                            //团购菜品
                            if (row["DishesType"].ToString().Equals("3"))
                            {
                                sb.AppendFormat("{0}[套餐]\n", row["DishesName"].ToString());
                            }
                        }
                        else
                        {
                            sb.AppendFormat("{0}[赠送]\n", row["DishesName"].ToString());
                        }
                        count.AppendFormat("{0}{1}\n", row["DishesCount"].ToString(), row["UnitName"].ToString());
                        price.AppendFormat("{0}\n", row["Moneys"].ToString());
                    }
                }

                sb.Append("-----------------------------------------------------------------------------\n");
                count.Append("\n");
                price.Append("\n");
            }

            //根据合台编号获取付款信息
            qrylist = new List<ICriterion>();
            qrylist.Add(Expression.Eq("MergeNO", MergeNO));
            tm_TabiePayInfo PayEntity = Core.Container.Instance.Resolve<IServiceTabiePayInfo>().GetEntityByFields(qrylist);

            count.Append("\n");
            price.Append("\n");
            sb.Append("消费合计：\n");
            count.Append("\n");
            price.AppendFormat("{0}\n", payMoneys);
            sb.Append("赠送金额：\n");
            count.Append("\n");
            price.AppendFormat("{0}\n", payPerMoneys);
            sb.Append("抹零金额：\n");
            count.Append("\n");
            price.AppendFormat("{0}\n", payErasingMoneys);
            sb.Append("应付金额：\n");
            count.Append("\n");
            price.AppendFormat("{0}\n", payFactMoneys);
            sb.AppendFormat("折扣[{0}折]\n", payDisPoint);
            count.Append("\n");
            price.AppendFormat("{0}\n", payDisCount);
            sb.Append("-----------------------------------------------------------------------------\n");
            count.Append("\n");
            price.Append("\n");
            //买单支付
            if (rblPayType.SelectedValue == "1")
            {
                if (!string.IsNullOrEmpty(PayEntity.PayWayCash))
                {
                    sb.Append("现金支付：\n");
                    count.Append("\n");
                    price.AppendFormat("{0}\n", PayEntity.CashMoneys);
                }
                if (!string.IsNullOrEmpty(PayEntity.PayWayCredit))
                {
                    sb.Append("刷卡支付：\n");
                    count.Append("\n");
                    price.AppendFormat("{0}\n", PayEntity.CreditMoneys);
                }
                if (!string.IsNullOrEmpty(PayEntity.PayWayVipcard))
                {
                    sb.Append("会员卡支付：\n");
                    count.Append("\n");
                    price.AppendFormat("{0}\n", PayEntity.VipcardMoneys);

                    //获取会员卡信息和余额
                    IList<ICriterion> qryList = new List<ICriterion>();
                    qryList.Add(Expression.Eq("VIPPhone", PayEntity.VipCardNO));
                    tm_vipinfo vipInfo = Core.Container.Instance.Resolve<IServiceVipInfo>().GetEntityByFields(qryList);

                    if (vipInfo != null)
                    {
                        sb.AppendFormat("会员卡：{0}\n", PayEntity.VipCardNO);
                        count.AppendFormat("余额:{0}\n", vipInfo.VIPCount);
                        price.AppendFormat("\n");
                    }
                }
                if (!string.IsNullOrEmpty(PayEntity.PayWayOnline))
                {
                    sb.Append("微信支付：\n");
                    count.Append("\n");
                    price.AppendFormat("{0}\n", PayEntity.OnlineMoneys);
                }
                if (!string.IsNullOrEmpty(PayEntity.PayWayZFB))
                {
                    sb.Append("支付宝支付：\n");
                    count.Append("\n");
                    price.AppendFormat("{0}\n", PayEntity.ZFBMoneys);
                }
                sb.Append("-----------------------------------------------------------------------------\n");
                count.Append("\n");
                price.Append("\n");
            }
            if (!string.IsNullOrEmpty(groupCard) && PayEntity.PayWayGroup.Equals("1"))
            {
                sb.AppendFormat("美团【{0}】：\n", groupMoney);
                count.AppendFormat("{0}\n", groupCard);
                price.Append("\n");
                sb.Append("-----------------------------------------------------------------------------\n");
                count.Append("\n");
                price.Append("\n");
            }
            //免单
            if (rblPayType.SelectedValue == "2")
            {
                sb.Append("免单：\n");
                count.AppendFormat("{0}\n", listTabieUsingInfo[0].FreeReason);
                price.Append("\n");
                sb.Append("-----------------------------------------------------------------------------\n");
                count.Append("\n");
                price.Append("\n");
            }
            //挂账
            if (rblPayType.SelectedValue == "3")
            {
                sb.Append("挂账：\n");
                count.Append("\n");
                price.AppendFormat("{0}\n", listTabieUsingInfo[0].Charge);
                sb.Append("-----------------------------------------------------------------------------\n");
                count.Append("\n");
                price.Append("\n");
            }

            sb.Append("地址：渝北区冉家坝龙山路301号\n");
            count.Append(" \n");
            price.Append("\n");
            sb.Append("电话：02367364577\n");
            count.Append(" \n");
            price.Append("\n");
            sb.Append("欢迎光临\n");
            count.Append("\n");
            price.Append("\n");
            LocalPrint(sb.ToString(), count.ToString(), price.ToString());
        }

        #endregion 前台本地打印

        #region 在线支付部分

        protected void WindowOnlinePay_Close(object sender, WindowCloseEventArgs e)
        {
            //支付成功对支付时间更新
            if (hfdPayType.Text.Equals("0"))
            {
                //子页面传回的支付成功时间
                dltCloseTabie.Text = DateTime.ParseExact(hfdPayTime.Text, "yyyyMMddHHmmss", null).ToString("yyyy-MM-dd HH:mm:ss");
                labPayState.Text = "在线支付成功";
                imgPayState.ImageUrl = "../res/icon/accept.png";
                //btnSave.Enabled = true;
            }
            //支付失败
            else
            {
                //子页面传回的失败原因描述
                labPayState.Text = hfdPayTime.Text;
                imgPayState.ImageUrl = "../res/icon/cross.png";
                //btnSave.Enabled = false;
            }

            labPayState.Hidden = false;
            imgPayState.Hidden = false;
        }

        protected void nbWX_Blur(object sender, EventArgs e)
        {
            //指定子页面传回的值的去处并跳转至子页面
            PageContext.RegisterStartupScript(WindowOnlinePay.GetSaveStateReference(hfdPayTime.ClientID, hfdPayType.ClientID)
                     + WindowOnlinePay.GetShowReference("~/Dinner/DOnlinePay.aspx?money=" + nbWX.Text.Trim() + "&pay_type=010&terminal_trace=" + MergeNO));

        }

        protected void nbZFB_Blur(object sender, EventArgs e)
        {
            //指定子页面传回的值的去处并跳转至子页面
            PageContext.RegisterStartupScript(WindowOnlinePay.GetSaveStateReference(hfdPayTime.ClientID, hfdPayType.ClientID)
                   + WindowOnlinePay.GetShowReference("~/Dinner/DOnlinePay.aspx?money=" + nbZFB.Text.Trim() + "&pay_type=020&terminal_trace=" + MergeNO));

        }

        #endregion
    }
}