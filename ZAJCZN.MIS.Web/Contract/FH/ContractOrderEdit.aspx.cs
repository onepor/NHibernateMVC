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
    public partial class ContractOrderEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreSaleOrderEdit";
            }
        }

        #endregion

        #region request param

        /// <summary>
        /// 订单编号
        /// </summary>
        private string OrderNO
        {
            get { return ViewState["OrderNO"].ToString(); }
            set
            {
                ViewState["OrderNO"] = value;

            }
        }
        //订单ID（传入参数）
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
                if (!CheckPower("CoreSaleOrderCost"))
                {
                    tabCustomerCost.Hidden = true;
                }
                //根据结算单计算单据录入限制日期
                CalcOrderDate();
                //绑定合同信息
                BindContractInfo();
                //绑定货车信息
                BindCarInfo();
                if (OrderID <= 0)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误，订单号不存在！", String.Empty, ActiveWindow.GetHideReference());
                }
                else
                {
                    GetOrderInfo();
                }
                //获取订单发货材料各项明细
                BindOrderDetail();
            }
        }

        #region 页面初始数据绑定

        /// <summary>
        /// 根据计算单计算单据录入限制日期
        /// </summary>
        private void CalcOrderDate()
        {
            //获取最近一次结算日期

            //设置单据录入限制日期
            //dpStartDate.MinDate = DateTime.Now;
        }

        /// <summary>
        /// 根据订单号，获取订单信息
        /// </summary>
        private void GetOrderInfo()
        {
            //获取订单信息
            ContractOrderInfo order = Core.Container.Instance.Resolve<IServiceContractOrderInfo>().GetEntity(OrderID);
            OrderNO = order.OrderNO;
            //初始化页面数据
            lblDate.Text = order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss");
            txtRemark.Text = order.Remark;
            lblOrderNo.Text = OrderNO;
            tbManualNO.Text = order.ManualNO;
            dpStartDate.Text = order.ValuationDate;
            ddlContract.SelectedValue = order.ContractInfo.ID.ToString();
            ddlCar.SelectedValue = order.CarID.ToString();
        }

        /// <summary>
        /// 绑定合同信息
        /// </summary>
        private void BindContractInfo()
        {
            List<ContractShowObj> listShow = new List<ContractShowObj>();
            //获取执行中的合同列表
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractState", "1"));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<ContractInfo> list = Core.Container.Instance.Resolve<IServiceContractInfo>().GetAllByKeys(qryList, orderList);
            //转换显示格式
            foreach (ContractInfo info in list)
            {
                //获取客户信息
                //CustomerInfo customer = Core.Container.Instance.Resolve<IServiceCustomerInfo>().GetEntity(info.CustomerID);
                //listShow.Add(new ContractShowObj
                //{
                //    ID = info.ID,
                //    ContarctName = string.Format("客户名称【{0}】|客户编号【{5}】|合同号【{1}】|客户合同号【{2}】|运费单价【{3}元】|保底【{4}吨】"
                //                                  , info.CustomerName, info.ContractNO, info.CustContractNO
                //                                  , info.CarCostPrice, info.CarMinTong, customer.CustomerNumber)
                //});
            }
            ddlContract.DataSource = listShow;
            ddlContract.DataBind();
        }

        /// <summary>
        /// 绑定货车信息
        /// </summary>
        private void BindCarInfo()
        {
            List<ContractCarShowObj> listShow = new List<ContractCarShowObj>();
            //获取合同货车价格套系信息
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractID", int.Parse(ddlContract.SelectedValue)));
            IList<ContractCarPriceSetInfo> list = Core.Container.Instance.Resolve<IServiceContractCarPriceSetInfo>().GetAllByKeys(qryList);
            //转换显示格式
            foreach (ContractCarPriceSetInfo info in list)
            {
                info.CarInfo = Core.Container.Instance.Resolve<IServiceCarInfo>().GetEntity(info.CarID);
                listShow.Add(new ContractCarShowObj { ID = info.CarID, CarName = string.Format("车牌号【{0}】|单价【{1}元】|保底【{2}吨】", info.CarInfo.CarNO, info.TonPayPrice, info.MinTon) });
            }
            ddlCar.DataSource = listShow;
            ddlCar.DataBind();
        }

        #endregion 页面初始数据绑定

        #region 发货信息绑定显示

        /// <summary>
        /// 获取订单发货材料各项明细
        /// </summary>
        private void BindOrderDetail()
        {
            //绑定主材列表
            BindMainGoodsInfo();
            //绑定主材列表
            BindSecondaryGoodsInfo();
            //绑定费用列表
            BindCostsInfo();
            //检查是否显示价格
            if (!CheckPower("CoreSaleOrderPrice"))
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
            //根据订单号获取发货主材信息
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<ContractOrderDetail> list = Core.Container.Instance.Resolve<IServiceContractOrderDetail>().GetAllByKeys(qryList, orderList);
            //获取材料基本信息
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
            //根据订单号获取发货辅材信息，数量为0的不显示
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            qryList.Add(Expression.Gt("GoodsNumber", 0M));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<ContractOrderSecondaryDetail> list = Core.Container.Instance.Resolve<IServiceContractOrderSecondaryDetail>().GetAllByKeys(qryList, orderList);
            //获取辅材相关的主材信息，材料基本信息,发货信息
            foreach (ContractOrderSecondaryDetail detail in list)
            {
                //辅材材料基本信息
                detail.GoodsInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(detail.GoodsID);
                //主材材料基本信息
                detail.MainGoodsInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(detail.MainGoodsID);
                //订单主材发货信息
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
            //根据订单号获取发货费用信息
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

        #endregion 发货信息绑定显示

        #region 页面数据转化

        /// <summary>
        /// 获取材料分类名称
        /// </summary>
        /// <param name="typeID">分类ID</param>
        /// <returns>分类名称</returns>
        public string GetType(string typeID)
        {
            EquipmentTypeInfo objType = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(int.Parse(typeID));
            return objType != null ? objType.TypeName : "";
        }

        /// <summary>
        /// 获取材料单位
        /// </summary>
        /// <param name="unitCode">单位代码</param>
        /// <returns>材料单位名称</returns>
        public string GetUnitName(string unitCode)
        {
            return GetSystemEnumValue("WPDW", unitCode);
        }

        /// <summary>
        /// 获取计价单位名称
        /// </summary>
        /// <param name="unitCode">单位代码</param>
        /// <returns>计价单位名称</returns>
        public string GetFYUnitName(string unitCode)
        {
            return GetSystemEnumValue("FYDW", unitCode);
        }

        /// <summary>
        /// 获取出库仓库名称
        /// </summary>
        /// <param name="whID">仓库ID</param>
        /// <returns>仓库名称</returns>
        public string GetWHName(string whID)
        {
            WareHouse houseObj = Core.Container.Instance.Resolve<IServiceWareHouse>().GetEntity(int.Parse(whID));
            return houseObj != null ? houseObj.WHName : "";
        }

        /// <summary>
        /// 获取计价方式
        /// </summary>
        /// <param name="payUnitCode">计价方式代码</param>
        /// <returns>计价方式</returns>
        public string GetPayUnit(string payUnitCode)
        {
            string i = "";
            switch (payUnitCode)
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

        /// <summary>
        /// 获取费用类别名称
        /// </summary>
        /// <param name="costCode">费用类别代码</param>
        /// <returns>费用类别名称</returns>
        public string GetCostType(string costCode)
        {
            string i = "";
            // 1：员工费用  2：客户费用  3：司机费用
            switch (costCode)
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

        /// <summary>
        /// 获取费用支付方式名称
        /// </summary>
        /// <param name="payCode">支付方式编码</param>
        /// <returns>支付方式名称</returns>
        public string GetNowPay(string payCode)
        {
            string i = "";
            switch (payCode)
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

        #region 发货明细清单调整编辑

        /// <summary>
        /// 主材单价及发货数量编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            Dictionary<int, Dictionary<string, object>> modifiedDict = Grid1.GetModifiedDict();
            foreach (int rowIndex in modifiedDict.Keys)
            {
                //根据绑定列的记录编号，获取发货物品信息和物品基本信息
                int rowID = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
                ContractOrderDetail objInfo = Core.Container.Instance.Resolve<IServiceContractOrderDetail>().GetEntity(rowID);
                objInfo.GoodsInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(objInfo.GoodsID);
                //修改发货数量
                if (modifiedDict[rowIndex].Keys.Contains("FixGoodsNumber"))
                {
                    objInfo.GoodsNumber = objInfo.FormerlyGoodsNumber + Convert.ToDecimal(modifiedDict[rowIndex]["FixGoodsNumber"]);
                    objInfo.FixGoodsNumber = Convert.ToDecimal(modifiedDict[rowIndex]["FixGoodsNumber"]);
                    objInfo.NotOffsetNumber = objInfo.FormerlyGoodsNumber + Convert.ToDecimal(modifiedDict[rowIndex]["FixGoodsNumber"]);
                }
                //计算并更新计价数量及重量
                if (objInfo.PayUnit == 1)
                {
                    //按计价单位计价，计价数量=商品规格*发货数量
                   // objInfo.GoodCalcPriceNumber = objInfo.GoodsInfo.Standard * objInfo.GoodsNumber;
                }
                else
                {
                    //按出库数量计价，计价数量=发货数量
                    objInfo.GoodCalcPriceNumber = objInfo.GoodsNumber;
                }
                //更新订单明细
                Core.Container.Instance.Resolve<IServiceContractOrderDetail>().Update(objInfo);
            }
            //重新加载订单发货信息
            BindOrderDetail();
        }

        #endregion 发货明细清单调整编辑

        #region 发货单费用明细编辑计算

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
                //根据绑定列的记录编号，获取发货费用信息和费用项基本信息
                int rowID = Convert.ToInt32(gdCostInfo.DataKeys[rowIndex][0]);
                ContractOrderCostInfo objInfo = Core.Container.Instance.Resolve<IServiceContractOrderCostInfo>().GetEntity(rowID);
                //修改费用计价单价
                if (modifiedDict[rowIndex].Keys.Contains("PayPrice"))
                {
                    objInfo.PayPrice = Convert.ToDecimal(modifiedDict[rowIndex]["PayPrice"]);
                }
                //修改费用计价数量
                if (modifiedDict[rowIndex].Keys.Contains("OrderNumber"))
                {
                    objInfo.OrderNumber = Convert.ToDecimal(modifiedDict[rowIndex]["OrderNumber"]);
                }
                //计算费用金额
                objInfo.CostAmount = objInfo.PayPrice * objInfo.OrderNumber;
                //更新费用项信息
                Core.Container.Instance.Resolve<IServiceContractOrderCostInfo>().Update(objInfo);
            }
            //重新加载订单费用信息
            BindCostsInfo();
        }

        #endregion 发货单费用明细编辑计算

        #region 发货单保存处理

        /// <summary>
        /// 返回订单列表页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnReturn_Click(object sender, EventArgs e)
        {
            //返回订单列表页面
            PageContext.Redirect("~/Contract/FH/ContractOrderManage.aspx");
        }

        /// <summary>
        /// 提交正式订单，返回订单列表页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            //保存正式订单信息
            if (SaveOrderInfo())
            {
                //返回订单列表页面
                PageContext.Redirect("~/Contract/FH/ContractOrderManage.aspx");
            }
        }

        /// <summary>
        /// 保存发货单信息
        /// </summary>
        /// <returns></returns>
        private bool SaveOrderInfo()
        {
            //从配置文件获取订单提交是否进行库存处理
            bool IsStock = bool.Parse(ConfigurationManager.AppSettings["IsStock"]);
            //更新订单状态为正式订单
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            ContractOrderInfo orderInfo = Core.Container.Instance.Resolve<IServiceContractOrderInfo>().GetEntityByFields(qryList);
            orderInfo.Remark = txtRemark.Text;
            orderInfo.ManualNO = tbManualNO.Text;
            orderInfo.ValuationDate = dpStartDate.Text;
            //发货车辆信息
            CarInfo carInfo = Core.Container.Instance.Resolve<IServiceCarInfo>().GetEntity(int.Parse(ddlCar.SelectedValue));
            orderInfo.CarNO = carInfo.CarNO;
            orderInfo.CarID = int.Parse(ddlCar.SelectedValue);
            //检查订单发货明细是否调整
            qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            qryList.Add(Expression.Gt("FixGoodsNumber", 0M));
            int recordCount = Core.Container.Instance.Resolve<IServiceContractOrderDetail>().GetRecordCountByFields(qryList);
            orderInfo.IsFix = recordCount > 0 ? 1 : 0;
            //更新订单信息 
            Core.Container.Instance.Resolve<IServiceContractOrderInfo>().Update(orderInfo);
            return true;
        }

        #endregion 发货单保存处理

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
            //PriceSetInfo priceSetInfo = Core.Container.Instance.Resolve<IServicePriceSetInfo>().GetEntity(contractInfo.PriceSetID);
            //根据选择的客户合同更新发货商品的价格
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            IList<ContractOrderDetail> orderGoodsList = Core.Container.Instance.Resolve<IServiceContractOrderDetail>().GetAllByKeys(qryList);
            //获取订单信息并更新订单合同信息
            ContractOrderInfo orderInfo = Core.Container.Instance.Resolve<IServiceContractOrderInfo>().GetEntityByFields(qryList);
            orderInfo.ContractInfo = contractInfo;
            Core.Container.Instance.Resolve<IServiceContractOrderInfo>().Update(orderInfo);
            //更新订单材料单价信息
            foreach (ContractOrderDetail goodsInfo in orderGoodsList)
            {
                //根据价格套系编号和商品ID获取合同商品价格信息
                qryList = new List<ICriterion>();
                //qryList.Add(Expression.Eq("SetID", priceSetInfo.ID));
                qryList.Add(Expression.Eq("EquipmentID", goodsInfo.GoodsID));
                PriceSetGoodsInfo priceSetGoodsInfo = Core.Container.Instance.Resolve<IServicePriceSetGoodsInfo>().GetEntityByFields(qryList);
                if (priceSetGoodsInfo != null)
                {
                    goodsInfo.GoodsUnitPrice = priceSetGoodsInfo.DailyRents;
                    //获取材料信息
                    EquipmentInfo equipmentInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(goodsInfo.GoodsID);
                    //判断物品费用是日租金还是现金买赔
                    //if (equipmentInfo.IsPayNow == 1)
                    //{
                    //    goodsInfo.GoodsUnitPrice = priceSetGoodsInfo.UnitPrice;
                    //}
                    Core.Container.Instance.Resolve<IServiceContractOrderDetail>().Update(goodsInfo);
                }
            }
            //绑定车辆价格套系
            BindCarInfo();
            ddlCar.SelectedValue = orderInfo.CarID.ToString();
            //更新费用信息     
            new ContractOrderBase().CalcOrderCost(OrderNO);
            //更新显示发货信息
            BindOrderDetail();
        }

        protected void ddlCar_SelectedIndexChanged(object sender, EventArgs e)
        {
            //获取订单信息
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            ContractOrderInfo orderInfo = Core.Container.Instance.Resolve<IServiceContractOrderInfo>().GetEntityByFields(qryList);
            //更新发货单送货车辆信息
            orderInfo.CarID = int.Parse(ddlCar.SelectedValue);
            Core.Container.Instance.Resolve<IServiceContractOrderInfo>().Update(orderInfo);
            //更新费用信息    
            new ContractOrderBase().CalcOrderCost(OrderNO);
        }

        /// <summary>
        /// 发货明细信息查看切换
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
