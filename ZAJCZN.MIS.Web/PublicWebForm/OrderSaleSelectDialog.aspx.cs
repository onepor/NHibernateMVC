using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Data;

namespace ZAJCZN.MIS.Web
{
    public partial class OrderSaleSelectDialog : PageBase
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

        #region retquest param

        private int OrderID
        {
            get { return ViewState["OrderID"] != null ? int.Parse(ViewState["OrderID"].ToString()) : 0; }
            set { ViewState["OrderID"] = value; }
        }
        private int ContractID
        {
            get { return ViewState["ContractID"] != null ? int.Parse(ViewState["ContractID"].ToString()) : 0; }
            set { ViewState["ContractID"] = value; }
        }
        private string OrderNO
        {
            get { return GetQueryValue("rowid"); }
        }
        private int OrderType
        {
            get { return ViewState["OrderType"] != null ? int.Parse(ViewState["OrderType"].ToString()) : 0; }
            set { ViewState["OrderType"] = value; }
        }


        #endregion retquest param

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Grid1.PageSize = ConfigHelper.PageSize;
                ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
                //绑定发货单信息
                BindOrderInfo();
                //绑定库房
                BindWH();
                //绑定物品类型信息
                BindCostType();
                //绑定物品列表
                BindGrid();
            }
        }

        #region 绑定数据

        private void BindGrid()
        {
            //获取物品规格信息
            IList<ICriterion> qryList = new List<ICriterion>();
            string qryName = ttbSearchMessage.Text.Trim();
            if (!string.IsNullOrEmpty(qryName))
            {
                qryList.Add(Expression.Like("GoodsName", qryName, MatchMode.Anywhere)
                            || Expression.Like("GoodsCode", qryName.ToUpper(), MatchMode.Anywhere));
            }
            qryList.Add(Expression.Eq("WareHouseID", int.Parse(ddlWH.SelectedValue)));
            if (ddlCostType.SelectedValue != "0")
            {
                qryList.Add(Expression.Eq("GoodsTypeID", int.Parse(ddlCostType.SelectedValue)));
            }

            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;

            //从默认的销售库房中获取当前库存大于0的物品信息
            IList<WHGoodsDetail> list = Core.Container.Instance.Resolve<IServiceWHGoodsDetail>().GetAllByKeys(qryList, orderList);
            foreach (WHGoodsDetail detail in list)
            {
                detail.GoodsInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(detail.GoodsID);
            }
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        #endregion

        #region 绑定库房

        private void BindWH()
        {
            IList<WareHouse> list = Core.Container.Instance.Resolve<IServiceWareHouse>().GetAll();

            ddlWH.DataSource = list;
            ddlWH.DataBind();
            ddlWH.SelectedIndex = 0;
            if (OrderType == 2)
            {
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("IsDefault", 1));
                WareHouse wareHouse = Core.Container.Instance.Resolve<IServiceWareHouse>().GetEntityByFields(qryList);
                ddlWH.SelectedValue = wareHouse.ID.ToString();
                ddlWH.Enabled = false;
            }
        }

        #endregion 绑定库房

        #region 绑定物品类型

        private void BindCostType()
        {
            List<JQueryFeature> myList = new List<JQueryFeature>();

            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("IsUsed", "1"));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<EquipmentTypeInfo> list = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetAllByKeys(qryList, orderList);

            foreach (EquipmentTypeInfo good in list)
            {
                myList.Add(new JQueryFeature(good.ID.ToString(), good.TypeName, 1, true));
            }
            myList.Insert(0, new JQueryFeature("0", "全部分类", 1, true));
            ddlCostType.DataSource = myList;
            ddlCostType.DataBind();
            ddlCostType.SelectedIndex = 0;
        }
        #endregion 绑定物品类型

        #region 绑定发货单数据
        private void BindOrderInfo()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));

            ContractOrderInfo orderInfo = Core.Container.Instance.Resolve<IServiceContractOrderInfo>().GetEntityByFields(qryList);
            OrderID = orderInfo != null ? orderInfo.ID : 0;
            ContractID = orderInfo != null ? orderInfo.ContractInfo.ID : 0;
            OrderType = orderInfo != null ? orderInfo.OrderType : 1;
            lblTitle.Text = string.Format("发货单号：{0}", OrderNO);
        }

        #endregion 绑定发货单数据

        #region 页面数据转换

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
        #endregion

        #region Events

        protected void ttbSearchMessage_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearchMessage.Text = String.Empty;
            ttbSearchMessage.ShowTrigger1 = false;
            BindGrid();
        }

        protected void ttbSearchMessage_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearchMessage.ShowTrigger1 = true;
            BindGrid();
        }

        protected void ddlCostType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }


        protected void ddlWH_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindGrid();
        }

        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();
        }

        private bool IsExists(int goodsID)
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            qryList.Add(Expression.Eq("GoodsID", goodsID));

            ContractOrderDetail objInfo = Core.Container.Instance.Resolve<IServiceContractOrderDetail>().GetEntityByFields(qryList);
            return objInfo != null ? true : false;
        }

        private void SaveItem()
        {
            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            WHGoodsDetail whGoodsEntity = new WHGoodsDetail();
            ContractOrderDetail dbEntity = new ContractOrderDetail();
            IList<ICriterion> qryList = new List<ICriterion>();
            PriceSetInfo priceSetInfo = new PriceSetInfo();

            //获取合同信息 
            ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(ContractID);

            // 执行数据库操作
            foreach (int ID in ids)
            {
                //获取物品信息
                whGoodsEntity = Core.Container.Instance.Resolve<IServiceWHGoodsDetail>().GetEntity(ID);
                if (whGoodsEntity != null)
                {
                    whGoodsEntity.GoodsInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(whGoodsEntity.GoodsID);
                    //判断是否已经添加改商品物品
                    if (whGoodsEntity.GoodsInfo != null && !IsExists(whGoodsEntity.GoodsID))
                    {
                        #region 主材

                        dbEntity = new ContractOrderDetail();
                        dbEntity.OrderNO = OrderNO;
                        dbEntity.OrderDate = DateTime.Now;
                        dbEntity.GoodsID = whGoodsEntity.GoodsID;
                        dbEntity.GoodTypeID = whGoodsEntity.GoodsTypeID;
                        dbEntity.GoodsNumber = 1;
                        dbEntity.FormerlyGoodsNumber = 1;
                        dbEntity.FixGoodsNumber = 0;
                        dbEntity.GoodsUnit = whGoodsEntity.InventoryUnit;
                        //dbEntity.GoodsCalcUnit = whGoodsEntity.GoodsInfo.CalcPriceUnit;
                        //dbEntity.NotOffsetNumber = dbEntity.GoodsNumber;
                        //dbEntity.GoodCalcPriceNumber = dbEntity.GoodsNumber * whGoodsEntity.GoodsInfo.Standard;
                        //dbEntity.PayUnit = whGoodsEntity.GoodsInfo.PayUnit;
                        /*-------------------获取价格-------------------*/
                        //获取主材类别，根据类别的报价策略获取价格
                        EquipmentTypeInfo equipmentTypeInfo = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(whGoodsEntity.GoodsInfo.EquipmentTypeID);
                        //dbEntity.IsStockByRepaired = equipmentTypeInfo.IsStockByRepaired;

                        //priceSetInfo = Core.Container.Instance.Resolve<IServicePriceSetInfo>().GetEntity(contractInfo.PriceSetID);
                        qryList = new List<ICriterion>();
                        qryList.Add(Expression.Eq("SetID", priceSetInfo.ID));
                        // 报价类型 1:按类别统计报价  2：按物品分别报价 3：不参与合同报价
                        //if (equipmentTypeInfo.PriceSetType == 1)
                        //{
                        //    qryList.Add(Expression.Eq("EquipmentID", whGoodsEntity.GoodsInfo.EquipmentTypeID));
                        //}
                        //if (equipmentTypeInfo.PriceSetType == 2)
                        //{
                        //    qryList.Add(Expression.Eq("EquipmentID", whGoodsEntity.GoodsID));
                        //}
                        PriceSetGoodsInfo priceSetGoodsInfo = Core.Container.Instance.Resolve<IServicePriceSetGoodsInfo>().GetEntityByFields(qryList);
                        if (priceSetGoodsInfo != null)
                        {
                            dbEntity.GoodsUnitPrice = priceSetGoodsInfo.DailyRents;
                            //判断物品费用是日租金还是现金买赔
                            //if (whGoodsEntity.GoodsInfo.IsPayNow == 1)
                            //{
                            //    dbEntity.GoodsUnitPrice = priceSetGoodsInfo.UnitPrice;
                            //}
                        }
                        else
                        {
                           // dbEntity.GoodsUnitPrice = whGoodsEntity.GoodsInfo.DailyRents;
                            //判断物品费用是日租金还是现金买赔
                            //if (whGoodsEntity.GoodsInfo.IsPayNow == 1)
                            //{
                            //    dbEntity.GoodsUnitPrice = whGoodsEntity.GoodsInfo.UnitPrice;
                            //}
                        }
                        dbEntity.NotOffsetNumber = 1;
                        dbEntity.WareHouseID = int.Parse(ddlWH.SelectedValue);
                        //保存领用主材信息
                        Core.Container.Instance.Resolve<IServiceContractOrderDetail>().Create(dbEntity);

                        #endregion 主材

                        //辅材列表
                        if (OrderType == 1)
                        {
                            //创建发货单辅材信息
                            CreateFHSecondaryDetail(dbEntity, priceSetInfo, whGoodsEntity);
                        }
                        else
                        {
                            //创建收货单辅材信息
                            CreateSHSecondaryDetail(dbEntity, priceSetInfo, whGoodsEntity);
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 创建发货单辅材信息
        /// </summary>
        /// <param name="dbEntity">发货单主材信息</param>
        /// <param name="priceSetInfo">合同价格套系</param>
        /// <param name="whGoodsEntity">库存物品信息</param>
        private void CreateFHSecondaryDetail(ContractOrderDetail dbEntity, PriceSetInfo priceSetInfo, WHGoodsDetail whGoodsEntity)
        {
            IList<ICriterion> qryList = new List<ICriterion>();

            //获取主材相关辅材信息
            qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ParentEquipmentID", whGoodsEntity.GoodsID));
            qryList.Add(Expression.Eq("EquipmentType", "2"));
            //IList<EquipmentAssortInfo> assortInfolist = Core.Container.Instance.Resolve<IServiceEquipmentAssortInfo>().GetAllByKeys(qryList);
            //计算并保存辅材信息
            //foreach (EquipmentAssortInfo assortInfo in assortInfolist)
            //{
            //    //判断辅材是否出库要算量
            //    if (assortInfo.IsOutCalcNumber == 1)
            //    {
            //        //获取辅材物品基本信息
            //        EquipmentInfo equipmentInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(assortInfo.EquipmentID);
            //        //创建辅材信息
            //        ContractOrderSecondaryDetail detail = new ContractOrderSecondaryDetail();
            //        detail.GoodsID = assortInfo.EquipmentID;
            //        detail.MainGoodsID = whGoodsEntity.GoodsID;
            //        detail.GoodsNumber = Math.Floor(1 / assortInfo.EquipmentCount) * assortInfo.AssortCount;
            //        detail.GoodsUnit = equipmentInfo.EquipmentUnit;
            //        detail.FormerlyGoodsNumber = Math.Floor(1 / assortInfo.EquipmentCount) * assortInfo.AssortCount;
            //        detail.PayForNumber = 0;
            //        detail.GoodsUnitPrice = 0;
            //        detail.IsShow = 0;
            //        //判断辅材出库是否要计算价格
            //        if (assortInfo.IsOutCalcPrice == 1)
            //        {
            //            /*-------------------获取价格-------------------*/
            //            //获取主材类别，根据类别的报价策略获取价格
            //            EquipmentTypeInfo equipmentTypeInfo = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(equipmentInfo.EquipmentTypeID);
            //            //获取价格 
            //            qryList = new List<ICriterion>();
            //            qryList.Add(Expression.Eq("SetID", priceSetInfo.ID));
            //            // 报价类型 1:按类别统计报价  2：按物品分别报价 3：不参与合同报价
            //            //if (equipmentTypeInfo.PriceSetType == 1)
            //            //{
            //            //    qryList.Add(Expression.Eq("EquipmentID", equipmentInfo.EquipmentTypeID));
            //            //}
            //            //if (equipmentTypeInfo.PriceSetType == 2)
            //            //{
            //            //    qryList.Add(Expression.Eq("EquipmentID", equipmentInfo.ID));
            //            //}
            //            //获取合同价格设置
            //            PriceSetGoodsInfo priceSetGoodsInfo1 = Core.Container.Instance.Resolve<IServicePriceSetGoodsInfo>().GetEntityByFields(qryList);
            //            if (priceSetGoodsInfo1 != null)
            //            {
            //                detail.GoodsUnitPrice = priceSetGoodsInfo1.UnitPrice;
            //            }
            //            else
            //            {
            //                detail.GoodsUnitPrice = equipmentInfo.UnitPrice;
            //            }
            //            detail.IsShow = 1;
            //        }
            //        detail.GoodsTotalPrice = detail.GoodsNumber * detail.GoodsUnitPrice;
            //        detail.IsCalcNumber = assortInfo.IsOutCalcNumber;
            //        detail.IsCalcPrice = assortInfo.IsOutCalcPrice;
            //        detail.OrderDate = dbEntity.OrderDate;
            //        detail.OrderNO = OrderNO;
            //        detail.OrderType = OrderType;
            //        detail.WareHouseID = int.Parse(ddlWH.SelectedValue);
            //        //保存发货辅材信息
            //        Core.Container.Instance.Resolve<IServiceContractOrderSecondaryDetail>().Create(detail);
            //    }
            //}
        }

        /// <summary>
        /// 创建收货单辅材信息
        /// </summary>
        /// <param name="dbEntity">收货单主材信息</param>
        /// <param name="priceSetInfo">合同价格套系</param>
        /// <param name="whGoodsEntity">库存物品信息</param>
        private void CreateSHSecondaryDetail(ContractOrderDetail dbEntity, PriceSetInfo priceSetInfo, WHGoodsDetail whGoodsEntity)
        {
            IList<ICriterion> qryList = new List<ICriterion>();

            //获取主材相关辅材信息
            qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ParentEquipmentID", whGoodsEntity.GoodsID));
            qryList.Add(Expression.Eq("EquipmentType", "2"));
            //IList<EquipmentAssortInfo> assortInfolist = Core.Container.Instance.Resolve<IServiceEquipmentAssortInfo>().GetAllByKeys(qryList);
            ////计算并保存辅材信息
            //foreach (EquipmentAssortInfo assortInfo in assortInfolist)
            //{
            //    //判断辅材是否入库要算量
            //    if (assortInfo.IsInCalcNumber == 1)
            //    {
            //        //获取辅材物品基本信息
            //        EquipmentInfo equipmentInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(assortInfo.EquipmentID);
            //        //创建辅材信息
            //        ContractOrderSecondaryDetail detail = new ContractOrderSecondaryDetail();
            //        detail.GoodsID = assortInfo.EquipmentID;
            //        detail.MainGoodsID = whGoodsEntity.GoodsID;
            //        detail.GoodsNumber = Math.Floor(1 / assortInfo.EquipmentCount) * assortInfo.AssortCount;
            //        detail.FormerlyGoodsNumber = Math.Floor(1 / assortInfo.EquipmentCount) * assortInfo.AssortCount;
            //        detail.PayForNumber = 0;
            //        detail.GoodsUnit = equipmentInfo.EquipmentUnit;
            //        detail.GoodsUnitPrice = 0;
            //        detail.IsShow = 0;
            //        //判断辅材收货时是否计算金额买赔
            //        if (assortInfo.IsInCalcPrice == 1)
            //        {
            //            /*-------------------获取价格-------------------*/
            //            //获取主材类别，根据类别的报价策略获取价格
            //            EquipmentTypeInfo equipmentTypeInfo = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(equipmentInfo.EquipmentTypeID);
            //            //获取价格 
            //            qryList = new List<ICriterion>();
            //            qryList.Add(Expression.Eq("SetID", priceSetInfo.ID));
            //            // 报价类型 1:按类别统计报价  2：按物品分别报价 3：不参与合同报价
            //            //if (equipmentTypeInfo.PriceSetType == 1)
            //            //{
            //            //    qryList.Add(Expression.Eq("EquipmentID", equipmentInfo.EquipmentTypeID));
            //            //}
            //            //if (equipmentTypeInfo.PriceSetType == 2)
            //            //{
            //            //    qryList.Add(Expression.Eq("EquipmentID", equipmentInfo.ID));
            //            //}
            //            //获取合同价格设置
            //            PriceSetGoodsInfo priceSetGoodsInfo1 = Core.Container.Instance.Resolve<IServicePriceSetGoodsInfo>().GetEntityByFields(qryList);
            //            if (priceSetGoodsInfo1 != null)
            //            {
            //                detail.GoodsUnitPrice = priceSetGoodsInfo1.UnitPrice;
            //            }
            //            else
            //            {
            //                detail.GoodsUnitPrice = equipmentInfo.UnitPrice;
            //            }
            //            detail.IsShow = 1;
            //        }
            //        detail.GoodsTotalPrice = detail.GoodsNumber * detail.GoodsUnitPrice;
            //        detail.IsCalcNumber = assortInfo.IsInCalcNumber;
            //        detail.IsCalcPrice = assortInfo.IsInCalcPrice;
            //        detail.OrderDate = dbEntity.OrderDate;
            //        detail.OrderNO = OrderNO;
            //        detail.OrderType = OrderType;
            //        detail.WareHouseID = int.Parse(ddlWH.SelectedValue);
            //        //保存收货辅材信息
            //        Core.Container.Instance.Resolve<IServiceContractOrderSecondaryDetail>().Create(detail);
            //    }
            //}
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();
            Alert.Show("物品添加成功!");
            //BindGrid();
            //PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion Events

    }
}