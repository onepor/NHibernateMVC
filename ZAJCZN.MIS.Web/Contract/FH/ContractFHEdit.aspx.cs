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
    public partial class ContractFHEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreFHOrderEdit";
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
                //从配置文件中获取明细信息框高度
                tsDetail.Height = int.Parse(ConfigurationManager.AppSettings["TabStripHight"]);
                btnCancel.ConfirmText = "确定要取消当前发货单信息吗？";
                btnSaveClose.ConfirmText = "确定正式提交发货单信息吗，提交后不能再修改？";
                // 删除选中单元格的客户端脚本 
                btnDeleteSelected.OnClientClick = Grid1.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
                btnDeleteSelected.ConfirmText = String.Format("确定要删除选中的<span class=\"highlight\"><script>{0}</script></span>项记录吗？", Grid1.GetSelectedCountReference());
                btnDeleteSelected.ConfirmTarget = FineUIPro.Target.Top;
                //根据结算单计算单据录入限制日期
                CalcOrderDate();
                //绑定合同信息
                BindContractInfo();
                //绑定货车信息
                BindCarInfo();
                //判断是否有传入的订单编号，没有为新增订单
                if (OrderID <= 0)
                {
                    //生成订单信息
                    CreateOrderInfo();
                }
                else
                {
                    GetOrderInfo();
                }

                btnNew.OnClientClick = Window1.GetShowReference(string.Format("~/PublicWebForm/OrderSaleSelectDialog.aspx?rowid={0}", OrderNO), "添加发货物品");
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
        /// 创建新订单
        /// </summary>
        private void CreateOrderInfo()
        {
            dpStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //创建新订单信息，默认为临时订单
            ContractOrderInfo order = new ContractOrderInfo();
            order.Operator = User.Identity.Name;
            order.OrderAmount = 0;
            order.OrderDate = DateTime.Now;
            order.ValuationDate = DateTime.Now.ToString("yyyy-MM-dd");
            order.ManualNO = "";
            order.OrderNO = string.Format("FH{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
            order.OrderNumber = 0;
            order.Remark = "";
            order.IsTemp = 1;                    //默认临时订单
            order.OrderType = 1;
            order.IsFix = 0;                     //默认订单未调整
            order.ContractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(int.Parse(ddlContract.SelectedValue));
            order.ContractNO = order.ContractInfo.ContractNO;
            order.OrderState = "1";              //默认执行中
            order.CustomerName = order.ContractInfo.CustomerName;
            order.CarID = int.Parse(ddlCar.SelectedValue);
            //获取车辆信息
            CarInfo carInfo = Core.Container.Instance.Resolve<IServiceCarInfo>().GetEntity(order.CarID);
            order.CarNO = carInfo.CarNO;
            //创建临时单据
            Core.Container.Instance.Resolve<IServiceContractOrderInfo>().Create(order);
            //保存订单号到页面缓存
            OrderNO = order.OrderNO;
            //创建合同相关费用信息
            CreateCostInfo(order.OrderNO);
            //初始化页面数据
            lblOrderNo.Text = OrderNO;
        }

        /// <summary>
        /// 创建合同发货单相关费用信息
        /// </summary>
        /// <param name="orderNO">合同发货单号</param>
        private void CreateCostInfo(string orderNO)
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("IsUsed", "1"));
            qryList.Add(Expression.Eq("UsingType", 2));
            //获取发货相关费用项
            IList<RepairProjectInfo> costList = Core.Container.Instance.Resolve<IServiceRepairProjectInfo>().GetAllByKeys(qryList);
            //创建发货单相关费用项信息
            foreach (RepairProjectInfo costInfo in costList)
            {
                ContractOrderCostInfo orderCostInfo = new ContractOrderCostInfo();
                orderCostInfo.CostID = costInfo.ID;
                orderCostInfo.CostName = costInfo.ProjectName;
                orderCostInfo.CostType = costInfo.ProjectType;
                orderCostInfo.InOutFlag = 1;
                orderCostInfo.IsSettle = 0;
                orderCostInfo.OrderNO = orderNO;
                orderCostInfo.OrderNumber = costInfo.IsRegular > 0 ? 1 : 0;
                orderCostInfo.PayPrice = costInfo.PayPrice;
                orderCostInfo.PayUnit = costInfo.PayUnit;
                orderCostInfo.Remark = "";
                orderCostInfo.CostAmount = orderCostInfo.OrderNumber * orderCostInfo.PayPrice;
                //创建费用项
                Core.Container.Instance.Resolve<IServiceContractOrderCostInfo>().Create(orderCostInfo);
            }
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
            dpStartDate.Text = order.ValuationDate;
            txtRemark.Text = order.Remark;
            lblOrderNo.Text = OrderNO;
            tbManualNO.Text = order.ManualNO;
            ddlContract.SelectedValue = order.ContractInfo.ID.ToString();
            ddlCar.SelectedValue = order.CarID.ToString();
        }

        /// <summary>
        /// 绑定客户合同信息列表
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
                ////获取客户信息
                //CustomerInfo customer = Core.Container.Instance.Resolve<IServiceCustomerInfo>().GetEntity(info.CustomerID);
                //listShow.Add(new ContractShowObj
                //{
                //    ID = info.ID,
                //    ContarctName = string.Format("客户名称【{0}】|客户编号【{1}】|项目名称【{2}】"
                //                                  , info.CustomerName, customer.CustomerNumber, info.ProjectName)
                //});
            }
            ddlContract.DataSource = listShow;
            ddlContract.DataBind();
        }

        /// <summary>
        /// 获取合同中设置的货车信息
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
                listShow.Add(new ContractCarShowObj { ID = info.CarID, CarName = string.Format("车牌号【{0}】", info.CarInfo.CarNO) });
            }
            ddlCar.DataSource = listShow;
            ddlCar.DataBind();
            ddlCar.SelectedIndex = 0;
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
            //绑定辅材列表
            BindSecondaryGoodsInfo();
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
            qryList.Add(Expression.Eq("IsShow", 1));
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

        #endregion 页面数据转化

        #region 发货明细清单编辑

        /// <summary>
        /// 主材发货数量编辑
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
                if (modifiedDict[rowIndex].Keys.Contains("GoodsNumber"))
                {
                    objInfo.GoodsNumber = Convert.ToDecimal(modifiedDict[rowIndex]["GoodsNumber"]);             //最终发货数量
                    objInfo.FormerlyGoodsNumber = Convert.ToDecimal(modifiedDict[rowIndex]["GoodsNumber"]);     //原始发货数量
                    objInfo.NotOffsetNumber = Convert.ToDecimal(modifiedDict[rowIndex]["GoodsNumber"]);         //商品待还数量
                    //更新辅材信息
                    UpdateSecondaryDetail(objInfo);
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
                //计算客户、员工和司机的发货商品重量
                //objInfo.GoodsCustomerWeight = objInfo.GoodsInfo.Standard * objInfo.GoodsNumber / objInfo.GoodsInfo.CustomerUnit;
                //objInfo.GoodsDriverWeight = objInfo.GoodsInfo.Standard * objInfo.GoodsNumber / objInfo.GoodsInfo.DriverUnit;
                //objInfo.GoodsStaffWeight = objInfo.GoodsInfo.Standard * objInfo.GoodsNumber / objInfo.GoodsInfo.StaffUnit;
                ////更新订单明细
                Core.Container.Instance.Resolve<IServiceContractOrderDetail>().Update(objInfo);
                //更新费用信息
                new ContractOrderBase().CalcOrderCost(OrderNO);
                //CalcOrderCost();
            }
            //重新加载订单发货信息
            BindOrderDetail();
        }

        /// <summary>
        /// 更新辅材信息
        /// </summary>
        /// <param name="objInfo">发货主材信息</param>
        private void UpdateSecondaryDetail(ContractOrderDetail objInfo)
        {
            //获取主材相关辅材信息
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ParentEquipmentID", objInfo.GoodsID));
            qryList.Add(Expression.Eq("EquipmentType", "2"));
            //IList<EquipmentAssortInfo> assortInfolist = Core.Container.Instance.Resolve<IServiceEquipmentAssortInfo>().GetAllByKeys(qryList);
            ////计算并更新辅材信息
            //foreach (EquipmentAssortInfo assortInfo in assortInfolist)
            //{
            //    //获取发货辅材信息
            //    qryList = new List<ICriterion>();
            //    qryList.Add(Expression.Eq("OrderNO", OrderNO));
            //    qryList.Add(Expression.Eq("MainGoodsID", objInfo.GoodsID));
            //    qryList.Add(Expression.Eq("GoodsID", assortInfo.EquipmentID));
            //    ContractOrderSecondaryDetail detail = Core.Container.Instance.Resolve<IServiceContractOrderSecondaryDetail>().GetEntityByFields(qryList);
            //    if (detail != null)
            //    {
            //        //更新辅材信息
            //        detail.GoodsNumber = Math.Floor(objInfo.GoodsNumber / assortInfo.EquipmentCount) * assortInfo.AssortCount;
            //        detail.FormerlyGoodsNumber = Math.Floor(objInfo.GoodsNumber / assortInfo.EquipmentCount) * assortInfo.AssortCount;
            //        detail.GoodsTotalPrice = detail.GoodsNumber * detail.GoodsUnitPrice;
            //        Core.Container.Instance.Resolve<IServiceContractOrderSecondaryDetail>().Update(detail);
            //    }
            //}
        }

        #region 发货单相关费用计算
        /*
        /// <summary>
        /// 更新发货单费用信息
        /// </summary>
        private void CalcOrderCost()
        {
            //获取订单费用明细项目
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            IList<ContractOrderCostInfo> costList = Core.Container.Instance.Resolve<IServiceContractOrderCostInfo>().GetAllByKeys(qryList);
            //更新费用明细项目
            foreach (ContractOrderCostInfo costInfo in costList)
            {
                //计算费用明细项费用
                CalcCost(costInfo);
            }
        }

        /// <summary>
        /// 各类费用项费用计算
        /// </summary>
        /// <param name="costInfo">费用项信息</param>
        private void CalcCost(ContractOrderCostInfo costInfo)
        {
            //获取费用项信息
            RepairProjectInfo costProjectInfo = Core.Container.Instance.Resolve<IServiceRepairProjectInfo>().GetEntity(costInfo.CostID);
            //获取订单信息
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            ContractOrderInfo orderInfo = Core.Container.Instance.Resolve<IServiceContractOrderInfo>().GetEntityByFields(qryList);

            //获取费用项单价
            costInfo.PayPrice = GetCostPayPrice(costProjectInfo, orderInfo);
            //获取费用项计价数量
            costInfo.OrderNumber = GetCostPayNumber(costProjectInfo, orderInfo);
            //计算费用金额，更新费用项信息
            costInfo.CostAmount = costInfo.PayPrice * costInfo.OrderNumber;
            Core.Container.Instance.Resolve<IServiceContractOrderCostInfo>().Update(costInfo);
        }

        /// <summary>
        /// 根据费用项费用价格获取类型获取价格
        /// </summary>
        /// <param name="costProjectInfo">费用项信息</param>
        /// <param name="orderInfo">订单信息</param> 
        /// <returns>价格</returns>
        private decimal GetCostPayPrice(RepairProjectInfo costProjectInfo, ContractOrderInfo orderInfo)
        {
            decimal payPrice = costProjectInfo.PayPrice;
            IList<ICriterion> qryList = new List<ICriterion>();
            //获取费用项适用范围
            string[] ids = costProjectInfo.UsingGoods.Split(',');
            //如果费用单价是从合同获取，根据合同获取单价【费用价格获取类型  0：自定义  1：合同客户运费  2：合同司机运费  3：合同单价  4：合同维修单价】
            if (costProjectInfo.PriceSourceType > 0)
            {
                switch (costProjectInfo.PriceSourceType)
                {
                    //合同客户运费(获取合同设定运费)
                    case 1:
                        payPrice = orderInfo.ContractInfo.CarCostPrice;
                        break;
                    //合同司机运费(获取订单选择的车辆在合同中设定运费)
                    case 2:
                        //获取合同车辆信息
                        qryList = new List<ICriterion>();
                        qryList.Add(Expression.Eq("CarID", orderInfo.CarID));
                        ContractCarPriceSetInfo carPriceSetInfo = Core.Container.Instance.Resolve<IServiceContractCarPriceSetInfo>().GetEntityByFields(qryList);
                        payPrice = carPriceSetInfo.TonPayPrice;
                        break;
                    //合同单价(获取租赁物品在合同中设定的单价)
                    case 3:
                        if (ids.Length > 0)
                        {
                            qryList = new List<ICriterion>();
                            qryList.Add(Expression.Eq("SetID", orderInfo.ContractInfo.PriceSetID));
                            qryList.Add(Expression.Eq("GoodsTypeID", ids[0]));
                            Order[] orderList = new Order[1];
                            Order orderli = new Order("ID", true);
                            orderList[0] = orderli;
                            //获取价格套系中物品设定信息
                            PriceSetGoodsInfo goodsInfo = Core.Container.Instance.Resolve<IServicePriceSetGoodsInfo>().GetFirstEntityByFields(qryList, orderList);
                            //获取物品单价
                            payPrice = goodsInfo.UnitPrice;
                        }
                        break;
                    //合同维修单价(获取租赁物品在合同中设定的维修价)
                    case 4:
                        if (ids.Length > 0)
                        {
                            qryList = new List<ICriterion>();
                            qryList.Add(Expression.Eq("SetID", orderInfo.ContractInfo.PriceSetID));
                            qryList.Add(Expression.Eq("GoodsTypeID", ids[0]));
                            Order[] orderList = new Order[1];
                            Order orderli = new Order("ID", true);
                            orderList[0] = orderli;
                            //获取价格套系中物品设定信息
                            PriceSetGoodsInfo goodsInfo = Core.Container.Instance.Resolve<IServicePriceSetGoodsInfo>().GetFirstEntityByFields(qryList, orderList);
                            //获取物品维修单价
                            payPrice = goodsInfo.FixPrice;
                        }
                        break;
                    default:
                        break;
                }
            }
            return payPrice;
        }

        /// <summary>
        /// 根据费用项计费单位类型获取费用项计价数量
        /// </summary>
        /// <param name="costProjectInfo">费用项信息</param>
        /// <param name="orderInfo">订单信息</param> 
        /// <returns>价格</returns>
        private decimal GetCostPayNumber(RepairProjectInfo costProjectInfo, ContractOrderInfo orderInfo)
        {
            decimal payNumber = 0M;
            IList<ICriterion> qryList = new List<ICriterion>();
            string sql = string.Empty;
            //1:数量 2:计价单位 3:客户吨位 4:员工吨位 5:司机吨位 6:其他
            switch (costProjectInfo.PayUnit)
            {
                //发货出库数量,发货明细表：GoodsNumber
                case "1":
                    //获取运送获取重量信息
                    if (!string.IsNullOrEmpty(costProjectInfo.UsingGoods))
                    {
                        sql = string.Format(@"select isnull(sum(GoodsNumber),0)) as GoodsNumber from ContractOrderDetail where OrderNO ='{0}' and GoodTypeID in ({1}) ", OrderNO, costProjectInfo.UsingGoods.TrimEnd(','));
                        DataSet ds = DbHelperSQL.Query(sql);
                        if (ds.Tables[0] != null)
                        {
                            payNumber = decimal.Parse(ds.Tables[0].Rows[0]["GoodsNumber"].ToString());
                        }
                    }
                    break;
                //发货计价单位，例如米,发货明细表：GoodCalcPriceNumber
                case "2":
                    //获取运送获取重量信息
                    if (!string.IsNullOrEmpty(costProjectInfo.UsingGoods))
                    {
                        sql = string.Format(@"select isnull(sum(GoodCalcPriceNumber),0) as GoodsNumber from ContractOrderDetail where OrderNO ='{0}' and GoodTypeID in ({1}) ", OrderNO, costProjectInfo.UsingGoods.TrimEnd(','));
                        DataSet ds = DbHelperSQL.Query(sql);
                        if (ds.Tables[0] != null)
                        {
                            payNumber = decimal.Parse(ds.Tables[0].Rows[0]["GoodsNumber"].ToString());
                        }
                    }
                    break;
                //客户吨位,发货明细表：GoodsCustomerWeight
                case "3":
                    //获取运送获取重量信息
                    if (!string.IsNullOrEmpty(costProjectInfo.UsingGoods))
                    {
                        sql = string.Format(@"select isnull(sum(GoodsCustomerWeight),0) as GoodsNumber from ContractOrderDetail where OrderNO ='{0}' and GoodTypeID in ({1}) ", OrderNO, costProjectInfo.UsingGoods.TrimEnd(','));
                        DataSet ds = DbHelperSQL.Query(sql);
                        if (ds.Tables[0] != null)
                        {
                            payNumber = decimal.Parse(ds.Tables[0].Rows[0]["GoodsNumber"].ToString());
                        }
                    }
                    break;
                //员工吨位,发货明细表：GoodsStaffWeight
                case "4":
                    //获取运送获取重量信息
                    if (!string.IsNullOrEmpty(costProjectInfo.UsingGoods))
                    {
                        sql = string.Format(@"select isnull(sum(GoodsStaffWeight),0) as GoodsNumber from ContractOrderDetail where OrderNO ='{0}' and GoodTypeID in ({1}) ", OrderNO, costProjectInfo.UsingGoods.TrimEnd(','));
                        DataSet ds = DbHelperSQL.Query(sql);
                        if (ds.Tables[0] != null)
                        {
                            payNumber = decimal.Parse(ds.Tables[0].Rows[0]["GoodsNumber"].ToString());
                        }
                    }
                    break;
                //司机吨位,发货明细表：GoodsDriverWeight
                case "5":
                    //获取运送获取重量信息
                    if (!string.IsNullOrEmpty(costProjectInfo.UsingGoods))
                    {
                        sql = string.Format(@"select isnull(sum(GoodsDriverWeight),0) as GoodsNumber from ContractOrderDetail where OrderNO ='{0}' and GoodTypeID in ({1}) ", OrderNO, costProjectInfo.UsingGoods.TrimEnd(','));
                        DataSet ds = DbHelperSQL.Query(sql);
                        if (ds.Tables[0] != null)
                        {
                            payNumber = decimal.Parse(ds.Tables[0].Rows[0]["GoodsNumber"].ToString());
                        }
                    }
                    break;
                //其他,默认1
                case "6":
                    payNumber = 1;
                    break;
                default:
                    payNumber = 1;
                    break;

            }

            return payNumber;
        }
        */
        #endregion 发货单相关费用计算

        #endregion 发货明细清单编辑

        #region 发货单保存处理

        /// <summary>
        /// 返回订单列表页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnReturn_Click(object sender, EventArgs e)
        {
            PageContext.Redirect("~/Contract/FH/ContractFHManage.aspx");
        }

        /// <summary>
        /// 取消当前订单，返回订单列表页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnCancel_Click(object sender, EventArgs e)
        {
            //根据订单号嘛，删除订单信息和相关发货、费用信息
            if (!string.IsNullOrEmpty(OrderNO))
            {
                string sqlWhere = string.Format(" OrderNO='{0}' ", OrderNO);
                Core.Container.Instance.Resolve<IServiceContractOrderInfo>().DelelteAll(sqlWhere);
                Core.Container.Instance.Resolve<IServiceContractOrderDetail>().DelelteAll(sqlWhere);
                Core.Container.Instance.Resolve<IServiceContractOrderSecondaryDetail>().DelelteAll(sqlWhere);
                Core.Container.Instance.Resolve<IServiceContractOrderCostInfo>().DelelteAll(sqlWhere);
            }
            //返回订单列表页面
            PageContext.Redirect("~/Contract/FH/ContractFHManage.aspx");
        }

        /// <summary>
        /// 提交正式订单，返回订单列表页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            //保存正式订单信息
            if (SaveOrderInfo(0))
            {
                //返回订单列表页面
                PageContext.Redirect("~/Contract/FH/ContractFHManage.aspx");
            }
        }

        /// <summary>
        /// 保存临时订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveTemp_Click(object sender, EventArgs e)
        {
            //保存临时订单信息
            if (SaveOrderInfo(1))
            {
                //返回订单列表页面
                PageContext.Redirect("~/Contract/FH/ContractFHManage.aspx");
            }
        }

        /// <summary>
        /// 保存发货单信息
        /// </summary>
        /// <param name="isTemp">订单类型 0：正式订单 1：临时订单</param>
        /// <returns></returns>
        private bool SaveOrderInfo(int isTemp)
        {
            //从配置文件获取订单提交是否进行库存处理
            bool IsStock = bool.Parse(ConfigurationManager.AppSettings["IsStock"]);
            //通过订单号获取当前订单信息
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            ContractOrderInfo orderInfo = Core.Container.Instance.Resolve<IServiceContractOrderInfo>().GetEntityByFields(qryList);
            //更新订单信息
            orderInfo.IsTemp = isTemp;
            orderInfo.OrderAmount = decimal.Parse(lblAmount.Text);
            orderInfo.OrderNumber = 0;
            orderInfo.ManualNO = tbManualNO.Text;
            orderInfo.ValuationDate = dpStartDate.Text;
            orderInfo.Remark = txtRemark.Text;
            orderInfo.Operator = User.Identity.Name;
            orderInfo.ContractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(int.Parse(ddlContract.SelectedValue));
            orderInfo.ContractNO = orderInfo.ContractInfo.ContractNO;
            orderInfo.CustomerName = orderInfo.ContractInfo.CustomerName;
            //发货车辆信息
            CarInfo carInfo = Core.Container.Instance.Resolve<IServiceCarInfo>().GetEntity(int.Parse(ddlCar.SelectedValue));
            orderInfo.CarNO = carInfo.CarNO;
            orderInfo.CarID = int.Parse(ddlCar.SelectedValue);

            //正式订单处理，更新出库信息及流水信息等
            if (isTemp == 0)
            {
                //检查是否做出库处理
                if (IsStock)
                {
                    //根据订单号，获取订单中所有材料的出库仓库ID信息
                    List<int> listWH = new InventoryHelper().GetWHInfo(OrderNO);
                    //按出库仓库各个计算出库信息
                    foreach (int whID in listWH)
                    {
                        //订单材料出库处理
                        OrderStockOutPress(whID, orderInfo);
                    }
                }
            }
            //更新订单信息 
            Core.Container.Instance.Resolve<IServiceContractOrderInfo>().Update(orderInfo);
            //更新费用信息 
            new ContractOrderBase().CalcOrderCost(OrderNO);
            //CalcOrderCost();

            return true;
        }

        #region 出库单处理

        /// <summary>
        /// 订单材料出库处理
        /// </summary>
        /// <param name="whID">出库仓库ID</param>
        /// <param name="orderInfo">发货单信息</param>
        private void OrderStockOutPress(int whID, ContractOrderInfo orderInfo)
        { 
            //获取销售仓库信息 
            WareHouse houseInfo = Core.Container.Instance.Resolve<IServiceWareHouse>().GetEntity(whID);
            // 生成出库单信息
            WHOutBoundOrder storageOrder = new WHOutBoundOrder();
            storageOrder.BOrderNO = OrderNO;
            storageOrder.Operator = User.Identity.Name;
            storageOrder.OrderAmount = 0;
            storageOrder.OrderNumber = 0;
            storageOrder.OrderDate = DateTime.Now;
            storageOrder.OrderNO = string.Format("CK{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
            storageOrder.OrderType = 1;
            //storageOrder.SuplierID = orderInfo.ContractInfo.CustomerID;
            storageOrder.WareHouseID = houseInfo.ID;
            storageOrder.OutOrderNO = "";
            storageOrder.Remark = "发货出库";

            #region 出库商品出库明细处理

            /*---- 主材出库明细处理 ----*/
            //获取订单商品主材明细,生成出库商品明细信息
            IList<ICriterion> qryListDetail = new List<ICriterion>();
            qryListDetail.Add(Expression.Eq("OrderNO", OrderNO));
            qryListDetail.Add(Expression.Eq("WareHouseID", whID));
            IList<ContractOrderDetail> list = Core.Container.Instance.Resolve<IServiceContractOrderDetail>().GetAllByKeys(qryListDetail);
            //写入出库商品明细
            foreach (ContractOrderDetail detail in list)
            {
                //出库单出库明细处理
                CreateStockOutOrder(whID, detail.GoodsNumber, detail.GoodsID, detail.GoodsUnit, ref storageOrder);
            }

            /*---- 辅材出库明细处理 ----*/
            //获取订单商品辅材明细,生成出库商品明细信息
            string sql = string.Format(@"select GoodsID,GoodsUnit,sum(GoodsNumber) as GoodsNumber from ContractOrderSecondaryDetail where OrderNO ='{0}' and WareHouseID ={1} group by GoodsID,GoodsUnit "
                                        , OrderNO, whID);
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                //写入出库商品明细
                foreach (DataRow detail in ds.Tables[0].Rows)
                {
                    //出库单出库明细处理
                    CreateStockOutOrder(whID, decimal.Parse(detail["GoodsNumber"].ToString())
                                        , int.Parse(detail["GoodsID"].ToString())
                                        , detail["GoodsUnit"].ToString()
                                        , ref storageOrder);
                }
            }

            #endregion 出库商品出库明细处理

            //创建出库单信息
            Core.Container.Instance.Resolve<IServiceWHOutBoundOrder>().Create(storageOrder);
        }

        /// <summary>
        /// 出库单出库明细处理
        /// </summary>
        /// <param name="WHID">出库仓库ID</param>
        /// <param name="goodsNumber">出库商品数量</param>
        /// <param name="goodsID">商品ID</param>
        /// <param name="goodsUnit">商品单位</param>
        /// <param name="storageOrder">出库单信息</param>
        private void CreateStockOutOrder(int WHID, decimal goodsNumber, int goodsID, string goodsUnit, ref WHOutBoundOrder storageOrder)
        {
            //如果商品出库数量大于0
            if (goodsNumber > 0)
            {
                //根据发货商品ID和出库仓库ID获取库存物品信息
                IList<ICriterion> qryWHList = new List<ICriterion>();
                qryWHList.Add(Expression.Eq("GoodsID", goodsID));
                qryWHList.Add(Expression.Eq("WareHouseID", WHID));
                WHGoodsDetail whGoodsDetail = Core.Container.Instance.Resolve<IServiceWHGoodsDetail>().GetEntityByFields(qryWHList);
                //生成出库单商品出库明细
                WHOrderGoodsDetail orderDetail = new WHOrderGoodsDetail();
                orderDetail.GoodsID = goodsID;
                orderDetail.GoodsUnit = goodsUnit;
                orderDetail.OrderDate = storageOrder.OrderDate;
                orderDetail.OrderNO = storageOrder.OrderNO;
                orderDetail.GoodsNumber = goodsNumber;
                //获取出库单价和金额
                orderDetail.GoodsUnitPrice = whGoodsDetail.InventoryUnitPrice;
                orderDetail.GoodTotalPrice = Math.Round(orderDetail.GoodsNumber * orderDetail.GoodsUnitPrice, 2);
                //含税价格（暂时不用)
                orderDetail.TaxAmount = 0;
                orderDetail.TaxPoint = 0;
                orderDetail.TotalPriceNoTax = 0;
                orderDetail.UnitPriceNoTax = 0;
                //保存出库明细信息
                Core.Container.Instance.Resolve<IServiceWHOrderGoodsDetail>().Create(orderDetail);
                //更新商品变动明细信息(出库)
                new InventoryHelper().UpdateGoodsJournal(WHID, goodsID, OrderNO, "FH", 2
                                                         , -orderDetail.GoodsNumber, orderDetail.GoodsUnitPrice
                                                         , -orderDetail.GoodTotalPrice, ""
                                                         , orderDetail.OrderDate);
                //累计计算订单成本金额
                storageOrder.OrderAmount += orderDetail.GoodTotalPrice;
                storageOrder.OrderNumber += orderDetail.GoodsNumber;
                //更新商品库存信息
                new InventoryHelper().UpdateWareHouseStock(WHID, goodsID, -goodsNumber, 0, -orderDetail.GoodTotalPrice, 0);
            }
        }

        #endregion 出库单处理

        #endregion 发货单保存处理

        #region 删除发货物品信息

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            foreach (int id in ids)
            {
                //删除发货物品信息
                DeleteDetail(id);
            }
            //获取订单发货材料各项明细
            BindOrderDetail();
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            if (e.CommandName == "Delete")
            {
                //删除发货物品信息
                DeleteDetail(ID);
                //获取订单发货材料各项明细
                BindOrderDetail();
            }
        }

        /// <summary>
        /// 删除发货物品信息
        /// </summary>
        /// <param name="id">物品ID</param>
        private void DeleteDetail(int id)
        {
            ContractOrderDetail contractOrderDetail = Core.Container.Instance.Resolve<IServiceContractOrderDetail>().GetEntity(id);
            //删除主材信息
            Core.Container.Instance.Resolve<IServiceContractOrderDetail>().Delete(id);
            //删除辅材信息          
            string sqlWhere = string.Format(" OrderNO='{0}' AND MainGoodsID={1}  ", OrderNO, contractOrderDetail.GoodsID);
            Core.Container.Instance.Resolve<IServiceContractOrderSecondaryDetail>().DelelteAll(sqlWhere);

        }

        #endregion 删除发货物品信息

        #region Events

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            CheckPowerWithLinkButtonField("CoreFHOrderEdit", Grid1, "deleteField");
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindOrderDetail();
        }

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
            //获取订单发货材料各项明细
            BindOrderDetail();
        }

        #endregion
    }
}
