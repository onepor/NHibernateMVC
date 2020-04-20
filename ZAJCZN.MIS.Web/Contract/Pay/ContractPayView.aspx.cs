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
using ZAJCZN.MIS.Helpers;
using System.Configuration;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractPayView : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CorePayOrderView";
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
            }
        }

        #region 页面初始数据绑定

        private void GetOrderInfo()
        {
            //获取订单信息
            ContractPayOrderInfo order = Core.Container.Instance.Resolve<IServiceContractPayOrderInfo>().GetEntity(OrderID);
            OrderNO = order.OrderNO;
            //初始化页面数据
            lblDate.Text = order.OrderDate.ToString("yyyy-MM-dd");
            txtRemark.Text = order.Remark;
            lblOrderNo.Text = OrderNO;
            lblManualNO.Text = order.ManualNO;
            lblAmount.Text = order.OrderAmount.ToString();
            //获取合同客户信息
            ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(order.ContractInfo.ID);
            lblContract.Text = contractInfo.CustomerName;
            //绑定主材列表
            BindMainGoodsInfo();
        }

        /// <summary>
        /// 绑定主材列表
        /// </summary>
        private void BindMainGoodsInfo()
        {
            //根据订单号获取买赔主材信息
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<ContractPayOrderDetail> list = Core.Container.Instance.Resolve<IServiceContractPayOrderDetail>().GetAllByKeys(qryList, orderList);
            //获取材料基本信息
            foreach (ContractPayOrderDetail detail in list)
            {
                detail.GoodsTypeInfo = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(detail.GoodTypeID);
            }
            gdGoodsDetail.DataSource = list;
            gdGoodsDetail.DataBind();
        }

        #endregion 页面初始数据绑定

        #region 页面数据转化

        /// <summary>
        /// 获取材料单位
        /// </summary>
        /// <param name="unitCode">单位代码</param>
        /// <returns>材料单位名称</returns>
        public string GetUnitName(string unitCode)
        {
            return GetSystemEnumValue("WPDW", unitCode);
        }

        #endregion 页面数据转化
    }
}
