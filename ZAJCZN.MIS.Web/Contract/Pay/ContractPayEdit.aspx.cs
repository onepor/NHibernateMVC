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
    public partial class ContractPayEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CorePayOrderEdit";
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
                btnCancel.ConfirmText = "确定要取消当前买赔单吗？";
                //绑定合同信息
                BindContractInfo();
                //判断是否有传入的订单编号，没有为新增订单
                if (OrderID <= 0)
                {
                    CreateOrderInfo();
                }
                else
                {
                    GetOrderInfo();
                }
                //获取订单买赔材料各项明细
                BindMainGoodsInfo();
            }
        }

        #region 页面初始数据绑定

        /// <summary>
        /// 创建新订单
        /// </summary>
        private void CreateOrderInfo()
        {
            dpStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //创建新订单信息，默认为临时订单
            ContractPayOrderInfo order = new ContractPayOrderInfo();
            order.Operator = User.Identity.Name;
            order.OrderAmount = 0;
            order.OrderDate = DateTime.Now;
            order.ManualNO = "";
            order.OrderNO = string.Format("Pay{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
            order.Remark = "";
            order.ContractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(int.Parse(ddlContract.SelectedValue));
            order.ContractNO = order.ContractInfo.ContractNO;
            order.CustomerName = order.ContractInfo.CustomerName;
            //创建临时单据
            Core.Container.Instance.Resolve<IServiceContractPayOrderInfo>().Create(order);
            //保存订单号到页面缓存
            OrderNO = order.OrderNO;
            //创建买赔物品明细
            CreatePayGoodsInfo(order.OrderNO);
            //初始化页面数据
            lblOrderNo.Text = OrderNO;
        }

        /// <summary>
        /// 创建买赔物品明细
        /// </summary>
        /// <param name="orderNO">合同买赔单号</param>
        private void CreatePayGoodsInfo(string orderNO)
        {
            ContractPayOrderDetail payOrderDetail = new ContractPayOrderDetail();
            //获取合同信息 
            ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(int.Parse(ddlContract.SelectedValue));
            //获取合同价格套系信息
           // PriceSetInfo priceSetInfo = Core.Container.Instance.Resolve<IServicePriceSetInfo>().GetEntity(contractInfo.PriceSetID);
            //获取合同发货主材类别信息
            string sql = string.Format(@"select * from EquipmentTypeInfo where id in (select distinct(GoodTypeID) as GoodTypeID from ContractOrderDetail where OrderNO in (select OrderNO from ContractOrderInfo where ContractID ={0} and OrderType=1))", contractInfo.ID);
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0] != null)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    payOrderDetail = new ContractPayOrderDetail();
                    payOrderDetail.OrderNO = OrderNO;
                    payOrderDetail.OrderDate = DateTime.Now;
                    payOrderDetail.GoodTypeID = int.Parse(row["ID"].ToString());
                    payOrderDetail.GoodsNumber = 0;
                    payOrderDetail.GoodsUnit = row["TypeUnit"].ToString();
                    payOrderDetail.GoodsUnitPrice = decimal.Parse(row["PayForPrice"].ToString());
                    /*-------------------获取合同价格-------------------*/
                    EquipmentTypeInfo equipmentTypeInfo = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(payOrderDetail.GoodTypeID);
                    IList<ICriterion> qryList = new List<ICriterion>();
                    //qryList.Add(Expression.Eq("SetID", priceSetInfo.ID));
                    qryList.Add(Expression.Eq("EquipmentID", payOrderDetail.GoodTypeID));
                    PriceSetGoodsInfo priceSetGoodsInfo = Core.Container.Instance.Resolve<IServicePriceSetGoodsInfo>().GetEntityByFields(qryList);
                    if (priceSetGoodsInfo != null)
                    {
                        payOrderDetail.GoodsUnitPrice = priceSetGoodsInfo.UnitPrice;
                    }
                    payOrderDetail.PayAmount = payOrderDetail.GoodsNumber * payOrderDetail.GoodsUnitPrice;
                    //保存赔偿材料信息
                    Core.Container.Instance.Resolve<IServiceContractPayOrderDetail>().Create(payOrderDetail);
                }
            }
        }

        /// <summary>
        /// 根据订单号，获取订单信息
        /// </summary>
        private void GetOrderInfo()
        {
            //获取订单信息
            ContractPayOrderInfo order = Core.Container.Instance.Resolve<IServiceContractPayOrderInfo>().GetEntity(OrderID);
            OrderNO = order.OrderNO;
            //初始化页面数据
            dpStartDate.Text = order.OrderDate.ToString("yyyy-MM-dd");
            txtRemark.Text = order.Remark;
            lblOrderNo.Text = OrderNO;
            tbManualNO.Text = order.ManualNO;
            ddlContract.SelectedValue = order.ContractInfo.ID.ToString();
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
                //获取客户信息
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

        #endregion 页面初始数据绑定

        #region 买赔信息绑定显示

        /// <summary>
        /// 绑定买赔材料列表
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

        #endregion 买赔信息绑定显示

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

        #region 买赔明细清单编辑

        /// <summary>
        /// 主材买赔数量编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gdGoodsDetail_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            Dictionary<int, Dictionary<string, object>> modifiedDict = gdGoodsDetail.GetModifiedDict();
            foreach (int rowIndex in modifiedDict.Keys)
            {
                //根据绑定列的记录编号，获取买赔物品信息和物品基本信息
                int rowID = Convert.ToInt32(gdGoodsDetail.DataKeys[rowIndex][0]);
                ContractPayOrderDetail objInfo = Core.Container.Instance.Resolve<IServiceContractPayOrderDetail>().GetEntity(rowID);
                objInfo.GoodsTypeInfo = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(objInfo.GoodTypeID);
                //修改买赔数量
                if (modifiedDict[rowIndex].Keys.Contains("GoodsNumber"))
                {
                    objInfo.GoodsNumber = Convert.ToDecimal(modifiedDict[rowIndex]["GoodsNumber"]);             //最终买赔数量 
                }
                //修改买赔单价
                if (modifiedDict[rowIndex].Keys.Contains("GoodsUnitPrice"))
                {
                    objInfo.GoodsUnitPrice = Convert.ToDecimal(modifiedDict[rowIndex]["GoodsUnitPrice"]);       //最终买赔单价 
                }
                //计算并更新买赔金额
                objInfo.PayAmount = objInfo.GoodsNumber * objInfo.GoodsUnitPrice;
                //更新订单明细
                Core.Container.Instance.Resolve<IServiceContractPayOrderDetail>().Update(objInfo);
            }
            //重新加载订单买赔信息
            BindMainGoodsInfo();
        }

        #endregion 买赔明细清单编辑

        #region 买赔单保存处理

        /// <summary>
        /// 返回订单列表页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnReturn_Click(object sender, EventArgs e)
        {
            PageContext.Redirect("~/Contract/Pay/ContractPayManage.aspx");
        }

        /// <summary>
        /// 取消当前订单，返回订单列表页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnCancel_Click(object sender, EventArgs e)
        {
            //根据订单号嘛，删除订单信息和相关买赔、费用信息
            if (!string.IsNullOrEmpty(OrderNO))
            {
                string sqlWhere = string.Format(" OrderNO='{0}' ", OrderNO);
                Core.Container.Instance.Resolve<IServiceContractPayOrderDetail>().DelelteAll(sqlWhere);
                Core.Container.Instance.Resolve<IServiceContractPayOrderInfo>().DelelteAll(sqlWhere);
            }
            //返回订单列表页面
            PageContext.Redirect("~/Contract/Pay/ContractPayManage.aspx");
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
                PageContext.Redirect("~/Contract/Pay/ContractPayManage.aspx");
            }
        }

        /// <summary>
        /// 保存买赔单信息
        /// </summary>
        /// <returns></returns>
        private bool SaveOrderInfo()
        {
            //通过订单号获取当前订单信息
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            ContractPayOrderInfo orderInfo = Core.Container.Instance.Resolve<IServiceContractPayOrderInfo>().GetEntityByFields(qryList);
            /*------------更新订单信息----------------*/
            //获取买赔商品信息，更新买赔总价
            orderInfo.OrderAmount = 0;
            IList<ContractPayOrderDetail> objInfoList = Core.Container.Instance.Resolve<IServiceContractPayOrderDetail>().GetAllByKeys(qryList);
            foreach (ContractPayOrderDetail detail in objInfoList)
            {
                orderInfo.OrderAmount += detail.PayAmount;
            }
            orderInfo.ManualNO = tbManualNO.Text;
            orderInfo.Remark = txtRemark.Text;
            orderInfo.Operator = User.Identity.Name;
            orderInfo.ContractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(int.Parse(ddlContract.SelectedValue));
            orderInfo.ContractNO = orderInfo.ContractInfo.ContractNO;
            orderInfo.CustomerName = orderInfo.ContractInfo.CustomerName;
            //更新订单信息 
            Core.Container.Instance.Resolve<IServiceContractPayOrderInfo>().Update(orderInfo);
            return true;
        }

        #endregion 买赔单保存处理

        #region Events

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindMainGoodsInfo();
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
            //根据选择的客户合同更新买赔商品的价格
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            IList<ContractPayOrderDetail> orderGoodsList = Core.Container.Instance.Resolve<IServiceContractPayOrderDetail>().GetAllByKeys(qryList);
            //获取订单信息并更新订单合同信息
            ContractPayOrderInfo orderInfo = Core.Container.Instance.Resolve<IServiceContractPayOrderInfo>().GetEntityByFields(qryList);
            orderInfo.ContractInfo = contractInfo;
            Core.Container.Instance.Resolve<IServiceContractPayOrderInfo>().Update(orderInfo);
            //更新订单材料单价信息
            foreach (ContractPayOrderDetail goodsInfo in orderGoodsList)
            {
                //根据价格套系编号和商品ID获取合同商品价格信息
                qryList = new List<ICriterion>();
                //qryList.Add(Expression.Eq("SetID", priceSetInfo.ID));
                qryList.Add(Expression.Eq("EquipmentID", goodsInfo.GoodTypeID));
                PriceSetGoodsInfo priceSetGoodsInfo = Core.Container.Instance.Resolve<IServicePriceSetGoodsInfo>().GetEntityByFields(qryList);
                if (priceSetGoodsInfo != null)
                {
                    goodsInfo.GoodsUnitPrice = priceSetGoodsInfo.UnitPrice;
                    goodsInfo.PayAmount = goodsInfo.GoodsNumber * priceSetGoodsInfo.UnitPrice;
                    Core.Container.Instance.Resolve<IServiceContractPayOrderDetail>().Update(goodsInfo);
                }
            }
            //获取订单买赔材料各项明细
            BindMainGoodsInfo();
        }

        #endregion
    }
}
