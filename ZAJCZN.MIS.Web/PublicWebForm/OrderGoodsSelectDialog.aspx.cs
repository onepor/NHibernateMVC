using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Data;

namespace ZAJCZN.MIS.Web
{
    public partial class OrderGoodsSelectDialog : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreContractCabinet";
            }
        }

        #endregion

        private int OrderID
        {
            get { return int.Parse(GetQueryValue("id")); }
        }
        private int UserType
        {
            get { return int.Parse(GetQueryValue("tid")); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Grid1.PageSize = ConfigHelper.PageSize;
                ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
                Grid1.AutoScroll = true;
                //绑定物品类型信息
                BindCostType();
                //绑定物品列表
                BindGrid();
            }
        }

        #region 绑定数据
        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            string qryName = ttbSearchMessage.Text.Trim();
            qryList.Add(Expression.Eq("IsUsed", "1"));
            if (!string.IsNullOrEmpty(qryName))
            {
                qryList.Add(Expression.Like("PartsName", qryName, MatchMode.Anywhere));
            }
            if (ddlCostType.SelectedValue != "0")
            {
                qryList.Add(Expression.Eq("PartsTypeID", int.Parse(ddlCostType.SelectedValue)));
            }
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<PartsInfo> list = Core.Container.Instance.Resolve<IServicePartsInfo>().GetAllByKeys(qryList, orderList);

            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        #endregion

        #region 绑定物品类型
        private void BindCostType()
        {
            List<PartsTypeInfo> myList = new List<PartsTypeInfo>();

            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("IsUsed", "1"));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<PartsTypeInfo> list = Core.Container.Instance.Resolve<IServicePartsTypeInfo>().GetAllByKeys(qryList, orderList);

            myList.AddRange(list);
            myList.Insert(0, new PartsTypeInfo { ID = 0, TypeName = "全部分类" });
            ddlCostType.DataSource = myList;
            ddlCostType.DataBind();
            ddlCostType.SelectedIndex = 0;
        }
        #endregion

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

        private bool IsExists(int goodsID)
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractID", OrderID));
            qryList.Add(Expression.Eq("GoodsID", goodsID));
            qryList.Add(Expression.Eq("OrderType", UserType));

            ContractHandWareDetail objInfo = Core.Container.Instance.Resolve<IServiceContractHandWareDetail>().GetEntityByFields(qryList);
            return objInfo != null ? true : false;
        }

        private void SaveItem()
        {
            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            PartsInfo goodsEntity = new PartsInfo();
            ContractHandWareDetail dbEntity = new ContractHandWareDetail();
            // 执行数据库操作
            foreach (int ID in ids)
            {
                goodsEntity = Core.Container.Instance.Resolve<IServicePartsInfo>().GetEntity(ID);
                //判断是否已经添加改商品物品
                if (!IsExists(ID))
                {
                    dbEntity = new ContractHandWareDetail();
                    dbEntity.ContractID =  OrderID;
                    dbEntity.GoodTypeID = goodsEntity.PartsTypeID;
                    dbEntity.GoodsID = goodsEntity.ID;
                    dbEntity.GoodsNumber = 1;
                    dbEntity.GoodsUnit = goodsEntity.PartsUnit;
                    dbEntity.GoodsUnitPrice = goodsEntity.UnitPrice;
                    dbEntity.GoodAmount = goodsEntity.UnitPrice;
                    dbEntity.GoodsName = goodsEntity.PartsName;
                    dbEntity.CostPrice = goodsEntity.CostPrice;
                    dbEntity.CostAmount = goodsEntity.CostPrice;
                    dbEntity.IsFree = 1;
                    dbEntity.OrderType = UserType;

                    Core.Container.Instance.Resolve<IServiceContractHandWareDetail>().Create(dbEntity);
                }
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();
            Alert.Show("进货商品添加成功!");
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