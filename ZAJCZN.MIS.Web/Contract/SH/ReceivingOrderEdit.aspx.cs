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
using ZAJCZN.MIS.Helpers;

namespace ZAJCZN.MIS.Web
{
    public partial class ReceivingOrderEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreReceivingOrderEdit";
            }
        }

        #endregion

        #region request param

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

        #endregion request param

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Grid1.AutoScroll = true;
                GridSecondDetail.AutoScroll = true;
                gdCostInfo.AutoScroll = true;
                gdWeightInfo.AutoScroll = true;
                //从配置文件中获取明细信息框高度
                int height = int.Parse(ConfigurationManager.AppSettings["TabStripHight"]);
                tsDetail.Height = height;
                gdCostInfo.Height = height - 100;
                gdWeightInfo.Height = height - 100;
                //检查是否显示费用清单
                if (!CheckPower("CoreReceivingOrderCost"))
                {
                    tabCustomerCost.Hidden = true;
                }
                //绑定合同信息
                BindContractInfo(); 
                if (OrderID <= 0)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误，订单号不存在！", String.Empty, ActiveWindow.GetHideReference());
                }
                else
                {
                    GetOrderInfo();
                }

                // 绑定表格
                BindGrid();
            }
        }

        #region 页面初始数据绑定

        private void GetOrderInfo()
        {
            ContractOrderInfo order = Core.Container.Instance.Resolve<IServiceContractOrderInfo>().GetEntity(OrderID);
            OrderNO = order.OrderNO;

            //初始化页面数据
            lblDate.Text = order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss");
            txtRemark.Text = order.Remark;
            lblOrderNo.Text = OrderNO;
            tbManualNO.Text = order.ManualNO;
            dpStartDate.Text = order.ValuationDate;

            ddlContract.SelectedValue = order.ContractInfo.ID.ToString(); 
        }

        /// <summary>
        /// 绑定合同信息
        /// </summary>
        private void BindContractInfo()
        {
            List<ContractShowObj> listShow = new List<ContractShowObj>();

            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractState", "1"));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;

            IList<ContractInfo> list = Core.Container.Instance.Resolve<IServiceContractInfo>().GetAllByKeys(qryList, orderList);

            foreach (ContractInfo info in list)
            {
                ////获取客户信息
                //CustomerInfo customer = Core.Container.Instance.Resolve<IServiceCustomerInfo>().GetEntity(info.CustomerID);
                //listShow.Add(new ContractShowObj
                //{
                //    ID = info.ID,
                //    ContarctName = string.Format("客户名称【{0}】|客户编号【{5}】|合同号【{1}】|客户合同号【{2}】|运费单价【{3}元】|保底【{4}吨】"
                //                                 , info.CustomerName, info.ContractNO, info.CustContractNO
                //                                 , info.CarCostPrice, info.CarMinTong, customer.CustomerNumber)
                //});
            }
            ddlContract.DataSource = listShow;
            ddlContract.DataBind();

        }

        #endregion 页面初始数据绑定

        #region 收货信息绑定显示

        private void BindGrid()
        {
            //绑定主材列表
            BindMainGoodsInfo();
            //绑定主材列表
            BindSecondaryGoodsInfo();
            //绑定费用列表
            BindCostsInfo();
            //检查是否显示价格
            if (!CheckPower("CoreReceivingOrderPrice"))
            {
                //检测权限，是否显示价格
                GridColumn column = Grid1.FindColumn("GoodsUnitPrice");
                GridColumn clGoodsUnitPrice = GridSecondDetail.FindColumn("GoodsUnitPrice");
                GridColumn clGoodsTotalPrice = GridSecondDetail.FindColumn("GoodsTotalPrice");

                column.Hidden = !column.Hidden;
                clGoodsUnitPrice.Hidden = !clGoodsUnitPrice.Hidden;
                clGoodsTotalPrice.Hidden = !clGoodsTotalPrice.Hidden;
            }
        }

        /// <summary>
        /// 绑定主材列表
        /// </summary>
        private void BindMainGoodsInfo()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));

            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<ContractOrderDetail> list = Core.Container.Instance.Resolve<IServiceContractOrderDetail>().GetAllByKeys(qryList, orderList);

            foreach (ContractOrderDetail detail in list)
            {
                detail.GoodsInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(detail.GoodsID);
            }
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        /// <summary>
        /// 绑定辅材列表
        /// </summary>
        private void BindSecondaryGoodsInfo()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            qryList.Add(Expression.Gt("GoodsNumber", 0M));

            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<ContractOrderSecondaryDetail> list = Core.Container.Instance.Resolve<IServiceContractOrderSecondaryDetail>().GetAllByKeys(qryList, orderList);

            foreach (ContractOrderSecondaryDetail detail in list)
            {
                detail.GoodsInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(detail.GoodsID);
                detail.MainGoodsInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(detail.MainGoodsID);

                qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("OrderNO", OrderNO));
                qryList.Add(Expression.Eq("GoodsID", detail.MainGoodsID));
                detail.MainGoodsOrderInfo = Core.Container.Instance.Resolve<IServiceContractOrderDetail>().GetEntityByFields(qryList);
            }
            GridSecondDetail.DataSource = list;
            GridSecondDetail.DataBind();
        }

        /// <summary>
        /// 绑定费用列表
        /// </summary>
        private void BindCostsInfo()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            if (!ddlSHCostType.SelectedValue.Equals("0"))
            {
                qryList.Add(Expression.Eq("CostType", int.Parse(ddlSHCostType.SelectedValue)));
            }
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<ContractOrderCostInfo> list = Core.Container.Instance.Resolve<IServiceContractOrderCostInfo>().GetAllByKeys(qryList, orderList);

            gdCostInfo.DataSource = list;
            gdCostInfo.DataBind();
        }

        #endregion 收货信息绑定显示

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


        //获取单位
        public string GetFYUnitName(string state)
        {
            return GetSystemEnumValue("FYDW", state);
        }

        //获取单位
        public string GetWHName(string whID)
        {
            WareHouse houseObj = Core.Container.Instance.Resolve<IServiceWareHouse>().GetEntity(int.Parse(whID));
            return houseObj != null ? houseObj.WHName : "";
        }

        public string GetPayUnit(string state)
        {
            string i = "";
            switch (state)
            {
                case "1":
                    i = "按计价单位";
                    break;
                case "2":
                    i = "按出库数量";
                    break;
            }
            return i;
        }

        public string GetCostType(string type)
        {
            string i = "";
            // 1：员工费用  2：客户费用  3：司机费用
            switch (type)
            {
                case "1":
                    i = "员工费用";
                    break;
                case "2":
                    i = "客户费用";
                    break;
                case "3":
                    i = "司机费用";
                    break;
            }
            return i;
        }

        public string GetNowPay(string state)
        {
            string i = "";
            switch (state)
            {
                case "1":
                    i = "现金买赔";
                    break;
                case "0":
                    i = "日租金";
                    break;
            }
            return i;
        }

        #endregion 页面数据转化

        #region 收货单费用计算

        /// <summary>
        /// 更新费用信息
        /// </summary>
        private void CalcCost()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            //获取费用明细项目
            IList<ContractOrderCostInfo> costList = Core.Container.Instance.Resolve<IServiceContractOrderCostInfo>().GetAllByKeys(qryList);
            //更新费用明细项目
            foreach (ContractOrderCostInfo costInfo in costList)
            {
                //计算费用
                CalcCost(costInfo);
            }
            //绑定费用记录
            BindCostsInfo();
        }

        /// <summary>
        /// 各类费用计算
        /// </summary>
        /// <param name="costInfo">费用项信息</param>
        private void CalcCost(ContractOrderCostInfo costInfo)
        {
            //获取费用项信息
            RepairProjectInfo costProjectInfo = Core.Container.Instance.Resolve<IServiceRepairProjectInfo>().GetEntity(costInfo.CostID);
            //获取费用项适用范围
            string[] ids = costProjectInfo.UsingGoods.Split(',');
            //获取订单信息
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            ContractOrderInfo orderInfo = Core.Container.Instance.Resolve<IServiceContractOrderInfo>().GetEntityByFields(qryList);

            #region 获取费用项单价

            costInfo.PayPrice = costProjectInfo.PayPrice;
            //如果费用单价是从合同获取，根据合同获取单价【费用价格获取类型  0：自定义  1：合同客户运费  2：合同司机运费  3：合同单价  4：合同维修单价】
            if (costProjectInfo.PriceSourceType > 0)
            {
                switch (costProjectInfo.PriceSourceType)
                {
                    //合同客户运费(获取合同设定运费)
                    case 1:
                        //costInfo.PayPrice = orderInfo.ContractInfo.CarCostPrice;
                        break;
                    //合同司机运费(获取订单选择的车辆在合同中设定运费)
                    case 2:
                        //获取合同车辆信息
                        qryList = new List<ICriterion>();
                        qryList.Add(Expression.Eq("CarID", orderInfo.CarID));
                        ContractCarPriceSetInfo carPriceSetInfo = Core.Container.Instance.Resolve<IServiceContractCarPriceSetInfo>().GetEntityByFields(qryList);
                        costInfo.PayPrice = carPriceSetInfo.TonPayPrice;
                        break;
                    //合同单价(获取租赁物品在合同中设定的单价)
                    case 3:
                        if (ids.Length > 0)
                        {
                            qryList = new List<ICriterion>();
                            //qryList.Add(Expression.Eq("SetID", orderInfo.ContractInfo.PriceSetID));
                            qryList.Add(Expression.Eq("GoodsTypeID", ids[0]));
                            Order[] orderList = new Order[1];
                            Order orderli = new Order("ID", true);
                            orderList[0] = orderli;
                            //获取价格套系中物品设定信息
                            PriceSetGoodsInfo goodsInfo = Core.Container.Instance.Resolve<IServicePriceSetGoodsInfo>().GetFirstEntityByFields(qryList, orderList);
                            //获取物品单价
                            costInfo.PayPrice = goodsInfo.UnitPrice;
                        }
                        break;
                    //合同维修单价(获取租赁物品在合同中设定的维修价)
                    case 4:
                        if (ids.Length > 0)
                        {
                            qryList = new List<ICriterion>();
                            //qryList.Add(Expression.Eq("SetID", orderInfo.ContractInfo.PriceSetID));
                            qryList.Add(Expression.Eq("GoodsTypeID", ids[0]));
                            Order[] orderList = new Order[1];
                            Order orderli = new Order("ID", true);
                            orderList[0] = orderli;
                            //获取价格套系中物品设定信息
                            PriceSetGoodsInfo goodsInfo = Core.Container.Instance.Resolve<IServicePriceSetGoodsInfo>().GetFirstEntityByFields(qryList, orderList);
                            //获取物品维修单价
                            costInfo.PayPrice = goodsInfo.FixPrice;
                        }
                        break;
                    default:
                        break;
                }
            }

            #endregion 获取费用项单价

            #region 获取费用项计价数量

            string sql = string.Empty;
            //1:数量 2:计价单位 3:客户吨位 4:员工吨位 5:司机吨位 6:其他
            switch (costProjectInfo.PayUnit)
            {
                //收货出库数量,收货明细表：GoodsNumber
                case "1":
                    //获取运送获取重量信息
                    if (!string.IsNullOrEmpty(costProjectInfo.UsingGoods))
                    {
                        sql = string.Format(@"select sum(GoodsNumber) as GoodsNumber from ContractOrderDetail where OrderNO ='{0}' and GoodTypeID in ({1}) ", OrderNO, costProjectInfo.UsingGoods.TrimEnd(','));
                        DataSet ds = DbHelperSQL.Query(sql);
                        if (ds.Tables[0] != null)
                        {
                            costInfo.OrderNumber = decimal.Parse(ds.Tables[0].Rows[0]["GoodsNumber"].ToString());
                        }
                    }
                    break;
                //收货计价单位，例如米,收货明细表：GoodCalcPriceNumber
                case "2":
                    //获取运送获取重量信息
                    if (!string.IsNullOrEmpty(costProjectInfo.UsingGoods))
                    {
                        sql = string.Format(@"select isnull(sum(GoodCalcPriceNumber),0) as GoodsNumber from ContractOrderDetail where OrderNO ='{0}' and GoodTypeID in ({1}) ", OrderNO, costProjectInfo.UsingGoods.TrimEnd(','));
                        DataSet ds = DbHelperSQL.Query(sql);
                        if (ds.Tables[0] != null)
                        {
                            costInfo.OrderNumber = decimal.Parse(ds.Tables[0].Rows[0]["GoodsNumber"].ToString());
                        }
                    }
                    break;
                //客户吨位,收货明细表：GoodsCustomerWeight
                case "3":
                    //获取运送获取重量信息
                    if (!string.IsNullOrEmpty(costProjectInfo.UsingGoods))
                    {
                        sql = string.Format(@"select isnull(sum(GoodsCustomerWeight),0) as GoodsNumber from ContractOrderDetail where OrderNO ='{0}' and GoodTypeID in ({1}) ", OrderNO, costProjectInfo.UsingGoods.TrimEnd(','));
                        DataSet ds = DbHelperSQL.Query(sql);
                        if (ds.Tables[0] != null)
                        {
                            costInfo.OrderNumber = decimal.Parse(ds.Tables[0].Rows[0]["GoodsNumber"].ToString());
                        }
                    }
                    break;
                //员工吨位,收货明细表：GoodsStaffWeight
                case "4":
                    //获取运送获取重量信息
                    if (!string.IsNullOrEmpty(costProjectInfo.UsingGoods))
                    {
                        sql = string.Format(@"select isnull(sum(GoodsStaffWeight),0) as GoodsNumber from ContractOrderDetail where OrderNO ='{0}' and GoodTypeID in ({1}) ", OrderNO, costProjectInfo.UsingGoods.TrimEnd(','));
                        DataSet ds = DbHelperSQL.Query(sql);
                        if (ds.Tables[0] != null)
                        {
                            costInfo.OrderNumber = decimal.Parse(ds.Tables[0].Rows[0]["GoodsNumber"].ToString());
                        }
                    }
                    break;
                //司机吨位,收货明细表：GoodsDriverWeight
                case "5":
                    //获取运送获取重量信息
                    if (!string.IsNullOrEmpty(costProjectInfo.UsingGoods))
                    {
                        sql = string.Format(@"select isnull(sum(GoodsDriverWeight),0) as GoodsNumber from ContractOrderDetail where OrderNO ='{0}' and GoodTypeID in ({1}) ", OrderNO, costProjectInfo.UsingGoods.TrimEnd(','));
                        DataSet ds = DbHelperSQL.Query(sql);
                        if (ds.Tables[0] != null)
                        {
                            costInfo.OrderNumber = decimal.Parse(ds.Tables[0].Rows[0]["GoodsNumber"].ToString());
                        }
                    }
                    break;
                //其他,默认1
                case "6":
                    costInfo.OrderNumber = 1;
                    break;
                default:
                    costInfo.OrderNumber = 1;
                    break;

            }

            #endregion 获取费用项计价数量

            //更新费用项信息
            costInfo.CostAmount = costInfo.PayPrice * costInfo.OrderNumber;
            Core.Container.Instance.Resolve<IServiceContractOrderCostInfo>().Update(costInfo); 
        }

        /// <summary>
        /// 相关费用单价及数量编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gdCostInfo_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            Dictionary<int, Dictionary<string, object>> modifiedDict = gdCostInfo.GetModifiedDict();
            foreach (int rowIndex in modifiedDict.Keys)
            {
                int rowID = Convert.ToInt32(gdCostInfo.DataKeys[rowIndex][0]);
                ContractOrderCostInfo objInfo = Core.Container.Instance.Resolve<IServiceContractOrderCostInfo>().GetEntity(rowID);

                if (modifiedDict[rowIndex].Keys.Contains("PayPrice"))
                {
                    objInfo.PayPrice = Convert.ToDecimal(modifiedDict[rowIndex]["PayPrice"]);
                }
                if (modifiedDict[rowIndex].Keys.Contains("OrderNumber"))
                {
                    objInfo.OrderNumber = Convert.ToDecimal(modifiedDict[rowIndex]["OrderNumber"]);
                }

                objInfo.CostAmount = objInfo.PayPrice * objInfo.OrderNumber;

                Core.Container.Instance.Resolve<IServiceContractOrderCostInfo>().Update(objInfo);
            }
            BindCostsInfo();
        }

        #endregion 收货单费用计算

        #region 收货单保存处理

        /// <summary>
        /// 取消返回处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnReturn_Click(object sender, EventArgs e)
        {
            PageContext.Redirect("~/Contract/SH/ReceivingOrderManage.aspx");
        }

        /// <summary>
        /// 提交正式订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();
            PageContext.Redirect("~/Contract/SH/ReceivingOrderManage.aspx");
        }

        /// <summary>
        /// 保存收货单信息
        /// </summary>
        /// <returns></returns>
        private bool SaveItem()
        {
            bool IsStock = bool.Parse(ConfigurationManager.AppSettings["IsStock"]);
            //更新订单状态为正式订单
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            ContractOrderInfo orderInfo = Core.Container.Instance.Resolve<IServiceContractOrderInfo>().GetEntityByFields(qryList);
            orderInfo.Remark = txtRemark.Text;
            orderInfo.CarNO = "";
            orderInfo.CarID = 0;
            //检查订单收货明细是否调整
            qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            qryList.Add(Expression.Gt("FixGoodsNumber", 0M));
            int recordCount = Core.Container.Instance.Resolve<IServiceContractOrderDetail>().GetRecordCountByFields(qryList);
            orderInfo.IsFix = recordCount > 0 ? 1 : 0;
            //更新订单信息 
            Core.Container.Instance.Resolve<IServiceContractOrderInfo>().Update(orderInfo);
            return true;
        }

        #endregion 收货单保存处理

        #region 收货明细清单调整编辑

        /// <summary>
        /// 主材单件及数量编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            Dictionary<int, Dictionary<string, object>> modifiedDict = Grid1.GetModifiedDict();
            foreach (int rowIndex in modifiedDict.Keys)
            {
                int rowID = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
                ContractOrderDetail objInfo = Core.Container.Instance.Resolve<IServiceContractOrderDetail>().GetEntity(rowID);
                objInfo.GoodsInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(objInfo.GoodsID);

                if (modifiedDict[rowIndex].Keys.Contains("FixGoodsNumber"))
                {
                    objInfo.GoodsNumber = objInfo.FormerlyGoodsNumber + Convert.ToDecimal(modifiedDict[rowIndex]["FixGoodsNumber"]);
                    objInfo.FixGoodsNumber = Convert.ToDecimal(modifiedDict[rowIndex]["FixGoodsNumber"]);
                    objInfo.NotOffsetNumber = objInfo.FormerlyGoodsNumber + Convert.ToDecimal(modifiedDict[rowIndex]["FixGoodsNumber"]);
                }
                //计算并更新计价数量及重量
                if (objInfo.PayUnit == 1)
                {
                    //objInfo.GoodCalcPriceNumber = objInfo.GoodsInfo.Standard * objInfo.GoodsNumber;
                }
                else
                {
                    objInfo.GoodCalcPriceNumber = objInfo.GoodsNumber;
                }
                //更新订单明细
                Core.Container.Instance.Resolve<IServiceContractOrderDetail>().Update(objInfo);
            }
            // 绑定表格
            BindGrid();
        }

        #endregion 收货明细清单调整编辑

        #region Events

        /// <summary>
        /// 合同客户选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlContract_SelectedIndexChanged(object sender, EventArgs e)
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            //获取合同信息 
            ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(int.Parse(ddlContract.SelectedValue));
            //获取合同价格套系信息
           // PriceSetInfo priceSetInfo = Core.Container.Instance.Resolve<IServicePriceSetInfo>().GetEntity(contractInfo.PriceSetID);

            //根据选择的客户合同更新收货商品的价格
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            IList<ContractOrderDetail> orderGoodsList = Core.Container.Instance.Resolve<IServiceContractOrderDetail>().GetAllByKeys(qryList);
            foreach (ContractOrderDetail goodsInfo in orderGoodsList)
            {
                qryList = new List<ICriterion>();
                //qryList.Add(Expression.Eq("SetID", priceSetInfo.ID));
                qryList.Add(Expression.Eq("EquipmentID", goodsInfo.GoodsID));
                PriceSetGoodsInfo priceSetGoodsInfo = Core.Container.Instance.Resolve<IServicePriceSetGoodsInfo>().GetEntityByFields(qryList);
                if (priceSetGoodsInfo != null)
                {
                    goodsInfo.GoodsUnitPrice = priceSetGoodsInfo.DailyRents;
                    Core.Container.Instance.Resolve<IServiceContractOrderDetail>().Update(goodsInfo);
                }
            } 
            //更新费用信息
            CalcCost();
            //更新显示收货信息
            BindGrid();
        } 

        /// <summary>
        /// 收货明细信息查看切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabStrip1_TabIndexChanged(object sender, EventArgs e)
        {
            decimal customerWeight = 0, driverWeight = 0, staffWeight = 0;
            if (tsDetail.ActiveTabIndex == 2)
            {
                //获取运送获取重量信息
                string sql = string.Format(@"select GoodTypeID,GoodsCalcUnit,sum(GoodCalcPriceNumber) as GoodCalcPriceNumber
                              ,sum(GoodsCustomerWeight) as GoodsCustomerWeight
                              ,sum(GoodsDriverWeight) as GoodsDriverWeight,sum(GoodsStaffWeight) as GoodsStaffWeight 
                               from ContractOrderDetail where OrderNO ='{0}' group by GoodTypeID,GoodsCalcUnit ", OrderNO);

                DataSet ds = DbHelperSQL.Query(sql);
                if (ds.Tables[0] != null)
                {
                    gdWeightInfo.DataSource = ds.Tables[0];
                    gdWeightInfo.DataBind();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        customerWeight += decimal.Parse(row["GoodsCustomerWeight"].ToString());
                        driverWeight += decimal.Parse(row["GoodsDriverWeight"].ToString());
                        staffWeight += decimal.Parse(row["GoodsStaffWeight"].ToString());
                    }
                    lblTotalWeight.Text = string.Format("整车重量：客户【{0}吨】|司机【{1}吨】|员工【{2}吨】", customerWeight, driverWeight, staffWeight);
                }
                //获取费用信息
                BindCostsInfo();

            }
        }

        /// <summary>
        /// 费用清单列列表费用类型选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSHCostType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //获取费用信息
            BindCostsInfo();
        }

        #endregion
    }
}
