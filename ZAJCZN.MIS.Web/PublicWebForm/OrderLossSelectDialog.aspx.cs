using FineUIPro;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Web
{
    public partial class OrderLossSelectDialog : PageBase
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

        #region retquest param

        private int OrderID
        {
            get { return ViewState["OrderID"] != null ? int.Parse(ViewState["OrderID"].ToString()) : 0; }
            set { ViewState["OrderID"] = value; }
        }
        private int WHID
        {
            get { return ViewState["WHID"] != null ? int.Parse(ViewState["WHID"].ToString()) : 0; }
            set { ViewState["WHID"] = value; }
        }
        private string OrderNO
        {
            get { return GetQueryValue("rowid"); }
        }

        #endregion retquest param

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
                //绑定发货单信息
                BindOrderInfo();
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
            qryList.Add(Expression.Eq("WareHouseID", WHID));
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

        #region 绑定领用单数据
        private void BindOrderInfo()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));

            LossOrder orderInfo = Core.Container.Instance.Resolve<IServiceLossOrder>().GetEntityByFields(qryList);
            OrderID = orderInfo != null ? orderInfo.ID : 0;
            WHID = orderInfo != null ? orderInfo.WareHouseID : 0;
            lblTitle.Text = string.Format("领用单号：{0}", OrderNO);

            //获取出库库房信息
            WareHouse houseInfo = Core.Container.Instance.Resolve<IServiceWareHouse>().GetEntity(WHID);
            lblWHInfo.Text = string.Format("出库库房：{0}", houseInfo != null ? houseInfo.WHName : "");
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

            LossOrderDetail objInfo = Core.Container.Instance.Resolve<IServiceLossOrderDetail>().GetEntityByFields(qryList);
            return objInfo != null ? true : false;
        }

        private void SaveItem()
        {
            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            WHGoodsDetail whGoodsEntity = new WHGoodsDetail();
            EquipmentInfo goodsEntity = new EquipmentInfo();
            EquipmentTypeInfo goodsTypeEntity = new EquipmentTypeInfo();
            LossOrderDetail dbEntity = new LossOrderDetail();
            // 执行数据库操作
            foreach (int ID in ids)
            {
                //获取物品信息
                whGoodsEntity = Core.Container.Instance.Resolve<IServiceWHGoodsDetail>().GetEntity(ID);
                if (goodsEntity != null)
                {
                    whGoodsEntity.GoodsInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(whGoodsEntity.GoodsID);
                    //获取商品类别信息
                    goodsTypeEntity = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(whGoodsEntity.GoodsTypeID);
                    //判断是否已经添加改商品物品
                    if (!IsExists(ID))
                    {
                        dbEntity = new LossOrderDetail();
                        dbEntity.OrderNO = OrderNO;
                        dbEntity.OrderDate = DateTime.Now;
                        dbEntity.GoodsID = whGoodsEntity.GoodsID;
                        dbEntity.GoodsNumber = 1;
                        dbEntity.GoodsUnit = whGoodsEntity.InventoryUnit;
                        dbEntity.GoodsUnitPrice = whGoodsEntity.InventoryUnitPrice;
                        dbEntity.GoodTotalPrice = Math.Round(dbEntity.GoodsNumber * dbEntity.GoodsUnitPrice, 2);
                        //保存领用物品信息
                        Core.Container.Instance.Resolve<IServiceLossOrderDetail>().Create(dbEntity);
                    }
                }
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();
            Alert.Show("报损物品添加成功!");
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