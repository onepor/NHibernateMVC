using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Data;

namespace ZAJCZN.MIS.Web
{
    public partial class AllotSelectDialog : PageBase
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
            get { return GetQueryValue("rowid"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Grid1.PageSize = ConfigHelper.PageSize;
                ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
                //绑定物品信息
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
            //获取调拨仓库信息
            IList<ICriterion> qryListWH = new List<ICriterion>();
            qryListWH.Add(Expression.Eq("OrderNO", OrderNO));
            tm_GoodsAllocationBill orderInfo = Core.Container.Instance.Resolve<IServiceGoodsAllocationBill>().GetEntityByFields(qryListWH);
            if (orderInfo != null && orderInfo.CKWareHouseID > 0)
            {
                IList<ICriterion> qryList = new List<ICriterion>();
                string qryName = ttbSearchMessage.Text.Trim();
                if (!string.IsNullOrEmpty(qryName))
                {
                    qryList.Add(Expression.Like("GoodsName", qryName, MatchMode.Anywhere)
                                || Expression.Like("GoodsPY", qryName.ToUpper(), MatchMode.Anywhere) );
                }
                if (ddlCostType.SelectedValue != "0")
                {
                    qryList.Add(Expression.Eq("GoodsTypeID", int.Parse(ddlCostType.SelectedValue)));
                }
                qryList.Add(Expression.Eq("WareHouseID", orderInfo.CKWareHouseID));
                Order[] orderList = new Order[1];
                Order orderli = new Order("ID", true);
                orderList[0] = orderli;
                int count = 0;
                IList<WHGoodsDetail> list = Core.Container.Instance.Resolve<IServiceWHGoodsDetail>().GetAllByKeys(qryList, orderList);
                //Grid1.RecordCount = count;
                Grid1.DataSource = list;
                Grid1.DataBind();
            }
            else
            {
                Alert.Show("调拨单调拨出库仓库没有指定！");
            }
        }

        #endregion

        #region 绑定物品类型
        private void BindCostType()
        {
            List<JQueryFeature> myList = new List<JQueryFeature>();

            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ParentID", 0));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<tm_GoodsType> list = Core.Container.Instance.Resolve<IServiceGoodsType>().GetAllByKeys(qryList, orderList);

            foreach (tm_GoodsType good in list)
            {
                myList.Add(new JQueryFeature(good.ID.ToString(), good.TypeName, 1, true));

                IList<ICriterion> qryListChild = new List<ICriterion>();
                qryListChild.Add(Expression.Eq("ParentID", good.ID));
                Order[] orderListChild = new Order[1];
                Order orderliChild = new Order("ID", true);
                orderListChild[0] = orderliChild;
                IList<tm_GoodsType> listChild = Core.Container.Instance.Resolve<IServiceGoodsType>().GetAllByKeys(qryListChild, orderListChild);
                foreach (tm_GoodsType goodChild in listChild)
                {
                    myList.Add(new JQueryFeature(goodChild.ID.ToString(), goodChild.TypeName, 2, true));
                }
            }
            myList.Insert(0, new JQueryFeature("0", "全部分类", 1, true));
            ddlCostType.DataSource = myList;
            ddlCostType.DataBind();
            ddlCostType.SelectedIndex = 0;
        }
        #endregion

        #region 绑定菜品数据
        private void BindOrderInfo()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            tm_GoodsAllocationBill orderInfo = Core.Container.Instance.Resolve<IServiceGoodsAllocationBill>().GetEntityByFields(qryList);
    
            //获取仓库信息
            WareHouse houseInfo = Core.Container.Instance.Resolve<IServiceWareHouse>().GetEntity(orderInfo.CKWareHouseID);

            lblTitle.Text = string.Format("[{1}]调拨单号：{0}", OrderNO, houseInfo != null ? houseInfo.WHName : "");
        }

        #endregion 绑定菜品数据

        #region 页面数据转换

        //获取分类名称
        public string GetType(string typeID)
        {
            tm_GoodsType objType = Core.Container.Instance.Resolve<IServiceGoodsType>().GetEntity(int.Parse(typeID));
            return objType != null ? objType.TypeName : "";
        }
        //获取单位
        public string GetUnitName(string state)
        {
            return GetSystemEnumValue("WPDW", state);
        }
        //获取税率名称
        public string GetTaxName(string paramID)
        {
            Sys_Paras objType = Core.Container.Instance.Resolve<IServiceSysParams>().GetEntity(int.Parse(paramID));
            return objType != null ? objType.ParaName : "";
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

        private bool IsExists(int goodsID)
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            qryList.Add(Expression.Eq("GoodsID", goodsID));

            tm_GoodsAllocationBillDetail objInfo = Core.Container.Instance.Resolve<IServiceGoodsAllocationBillDetail>().GetEntityByFields(qryList);
            return objInfo != null ? true : false;
        }

        private void SaveItem()
        {
            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            tm_Goods goodsEntity = new tm_Goods();
            WHGoodsDetail whGoodsEntity = new WHGoodsDetail();
            tm_GoodsAllocationBillDetail dbEntity = new tm_GoodsAllocationBillDetail();
            // 执行数据库操作
            foreach (int ID in ids)
            {
                //获取库存物品信息
                whGoodsEntity = Core.Container.Instance.Resolve<IServiceWHGoodsDetail>().GetEntity(ID);
                if (whGoodsEntity != null)
                {
                    goodsEntity = Core.Container.Instance.Resolve<IServiceGoods>().GetEntity(whGoodsEntity.GoodsID);
                    //判断是否已经添加改商品物品
                    if (!IsExists(ID))
                    {
                        dbEntity = new tm_GoodsAllocationBillDetail();
                        dbEntity.OrderNO = OrderNO;
                        dbEntity.GoodsID = whGoodsEntity.GoodsID;
                        dbEntity.GoodsType = whGoodsEntity.GoodsTypeID;
                        dbEntity.GoodsNumber = 1;
                        dbEntity.GoodsPrice = goodsEntity.GoodsPrice;
                        dbEntity.GoodsAmount = goodsEntity.GoodsPrice;
                        dbEntity.OrderDate = DateTime.Now;

                        Core.Container.Instance.Resolve<IServiceGoodsAllocationBillDetail>().Create(dbEntity);
                    }
                }
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();
            Alert.Show("调拨商品添加成功!");
            BindGrid();
            //PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion Events

    }
}