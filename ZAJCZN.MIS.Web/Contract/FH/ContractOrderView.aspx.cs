﻿using FineUIPro;
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
using ZAJCZN.MIS.Helpers;
using System.Configuration;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractOrderView : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreSHOrderView";
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
                btnClose.OnClientClick = ActiveWindow.GetHideReference();

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
            lblOrderNo.Text = order.ManualNO;

            //获取合同客户信息
            ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(order.ContractInfo.ID);
            lblContract.Text = contractInfo.CustomerName;
            //获取送货车辆信息 
            CarInfo carInfo = Core.Container.Instance.Resolve<IServiceCarInfo>().GetEntity(order.CarID);
            lblCar.Text = carInfo.CarNO;
        }

        #endregion 页面初始数据绑定

        #region 发货信息绑定显示

        private void BindGrid()
        {
            //绑定主材列表
            BindMainGoodsInfo();
            //绑定辅材列表
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
            decimal customerWeight = 0, driverWeight = 0, staffWeight = 0;
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

        #endregion 发货信息绑定显示

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
        #endregion 页面数据转化
    }
}
