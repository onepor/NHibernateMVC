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

namespace ZAJCZN.MIS.Web
{
    public partial class GoodsOrderView : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreGoodsOrderView";
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
                BindSupplier();
                //绑定库房
                BindWH();
                //获取订单信息
                GetOrderInfo();
            }
        }

        #region 绑定数据

        /// <summary>
        /// 绑定供应商
        /// </summary>
        private void BindSupplier()
        {
            IList<SupplierInfo> list = Core.Container.Instance.Resolve<IServiceSupplierInfo>().GetAll();

            ddlSuplier.DataSource = list;
            ddlSuplier.DataBind();
            ddlSuplier.SelectedIndex = 0;
        }

        /// <summary>
        /// 绑定库房
        /// </summary>
        private void BindWH()
        {
            IList<WareHouse> list = Core.Container.Instance.Resolve<IServiceWareHouse>().GetAll();

            ddlWH.DataSource = list;
            ddlWH.DataBind();
            ddlWH.SelectedIndex = 0;
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        private void GetOrderInfo()
        {
            GoodsOrder orderInfo = Core.Container.Instance.Resolve<IServiceGoodsOrder>().GetEntity(OrderID);
            lblAmount.Text = orderInfo.OrderAmount.ToString();
            lblCount.Text = orderInfo.OrderNumber.ToString();
            lblOrderNo.Text = orderInfo.OrderNO;
            lblStartDate.Text = orderInfo.OrderDate.ToString();
            ddlPay.SelectedValue = orderInfo.OrderType.ToString();
            ddlSuplier.SelectedValue = orderInfo.SuplierID.ToString();
            ddlWH.SelectedValue = orderInfo.WareHouseID.ToString();
            txtRemark.Text = orderInfo.Remark;
            //绑定商品明细
            BindDetailGrid(orderInfo.OrderNO);
        }

        /// <summary>
        /// 绑定商品明细
        /// </summary>
        /// <param name="orderNO"></param>
        private void BindDetailGrid(string orderNO)
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", orderNO));

            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            int count = 0;
            IList<GoodsOrderDetail> list = Core.Container.Instance.Resolve<IServiceGoodsOrderDetail>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);

            foreach (GoodsOrderDetail detail in list)
            {
                detail.GoodsInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(detail.GoodsID);
            }

            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        #endregion 绑定数据

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

        #endregion 页面数据转化

        #region Events
        public void btnReturn_Click(object sender, EventArgs e)
        {
            PageContext.Redirect("~/Inventory/GoodsOrderManage.aspx");
        }
        #endregion


    }
}
