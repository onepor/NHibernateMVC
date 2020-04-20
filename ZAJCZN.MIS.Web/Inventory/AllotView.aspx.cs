using FineUIPro;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Web
{
    public partial class AllotView : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreAllotView";
            }
        }

        #endregion

        private int OrderID
        {
            get { return GetQueryIntValue("id"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //绑定供应商
                //绑定库房
                BindWH();
                //生成订单信息
                GetOrderInfo();
            }
        }

        #region 绑定数据

        private void BindWH()
        {
            IList<WareHouse> list = Core.Container.Instance.Resolve<IServiceWareHouse>().GetAll();

            ddlCKWareHouseID.DataSource = list;
            ddlCKWareHouseID.DataBind();
            ddlCKWareHouseID.SelectedIndex = 0;

            ddlRKWareHouseID.DataSource = list;
            ddlRKWareHouseID.DataBind();
            ddlRKWareHouseID.SelectedIndex = 0;
        }

        private void GetOrderInfo()
        {
            tm_GoodsAllocationBill orderInfo = Core.Container.Instance.Resolve<IServiceGoodsAllocationBill>().GetEntity(OrderID);
            lblAmount.Text = orderInfo.AllotAmount.ToString();
            lblCount.Text = orderInfo.AllotCount.ToString();
            lblOrderNo.Text = orderInfo.OrderNO;
            lblStartDate.Text = orderInfo.OrderDate.ToString();
            ddlRKWareHouseID.SelectedValue = orderInfo.RKWareHouseID.ToString();
            ddlCKWareHouseID.SelectedValue = orderInfo.CKWareHouseID.ToString();
            //绑定商品明细
            BindGrid(orderInfo.OrderNO);
        }

        #endregion 绑定数据

        #region BindGrid

        private void BindGrid(string orderNO)
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", orderNO));

            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            int count = 0;
            IList<tm_GoodsAllocationBillDetail> list = Core.Container.Instance.Resolve<IServiceGoodsAllocationBillDetail>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);

            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        #endregion

        #region 页面数据转化

        //获取单位
        public string GetUnitName(string id)
        {
            tm_Goods supplierInfo = Core.Container.Instance.Resolve<IServiceGoods>().GetEntity(int.Parse(id));
            return GetSystemEnumValue("WPDW", supplierInfo.GoodsUnit);
        }

        //获取单位
        public string GetGoodsName(string id)
        {
            tm_Goods supplierInfo = Core.Container.Instance.Resolve<IServiceGoods>().GetEntity(int.Parse(id));
            return supplierInfo != null ? supplierInfo.GoodsName : "";
        }

        //获取单位
        public string GetGoodCode(string id)
        {
            tm_Goods supplierInfo = Core.Container.Instance.Resolve<IServiceGoods>().GetEntity(int.Parse(id));
            return supplierInfo != null ? supplierInfo.GoodsCode.ToString() : "";
        }



        #endregion 页面数据转化

        #region Events
        public void btnReturn_Click(object sender, EventArgs e)
        {
            PageContext.Redirect("~/Inventory/AllotManager.aspx");
        }
        #endregion


    }
}
