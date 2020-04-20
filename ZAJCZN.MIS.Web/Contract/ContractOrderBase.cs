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
    public class ContractOrderBase
    {
        #region 发货单相关费用计算

        /// <summary>
        /// 更新发货单费用信息
        /// </summary>
        public void CalcOrderCost(string orderNO)
        {
            //获取订单费用明细项目
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", orderNO));
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
            qryList.Add(Expression.Eq("OrderNO", costInfo.OrderNO));
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
                        //payPrice = orderInfo.ContractInfo.CarCostPrice;
                        break;
                    //合同司机运费(获取订单选择的车辆在合同中设定运费)
                    case 2:
                        //获取合同车辆信息
                        qryList = new List<ICriterion>();
                        qryList.Add(Expression.Eq("CarID", orderInfo.CarID));
                        qryList.Add(Expression.Eq("ContractID", orderInfo.ContractInfo.ID));
                        ContractCarPriceSetInfo carPriceSetInfo = Core.Container.Instance.Resolve<IServiceContractCarPriceSetInfo>().GetEntityByFields(qryList);
                        payPrice = carPriceSetInfo.TonPayPrice;
                        break;
                    //合同单价(获取租赁物品在合同中设定的单价)
                    case 3:
                        if (ids.Length > 0)
                        {
                            qryList = new List<ICriterion>();
                           // qryList.Add(Expression.Eq("SetID", orderInfo.ContractInfo.PriceSetID));
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
                            //qryList.Add(Expression.Eq("SetID", orderInfo.ContractInfo.PriceSetID));
                            qryList.Add(Expression.Eq("GoodsTypeID", int.Parse(ids[0])));
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
            string orderNO = orderInfo.OrderNO;
            //1:数量 2:计价单位 3:客户吨位 4:员工吨位 5:司机吨位 6:其他
            switch (costProjectInfo.PayUnit)
            {
                //发货出库数量,发货明细表：GoodsNumber
                case "1":
                    //获取运送获取重量信息
                    if (!string.IsNullOrEmpty(costProjectInfo.UsingGoods))
                    {
                        sql = string.Format(@"select isnull(sum(GoodsNumber),0) as GoodsNumber from ContractOrderDetail where OrderNO ='{0}' and GoodTypeID in ({1}) ", orderNO, costProjectInfo.UsingGoods.TrimEnd(','));
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
                        sql = string.Format(@"select isnull(sum(GoodCalcPriceNumber),0) as GoodsNumber from ContractOrderDetail where OrderNO ='{0}' and GoodTypeID in ({1}) ", orderNO, costProjectInfo.UsingGoods.TrimEnd(','));
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
                        sql = string.Format(@"select isnull(sum(GoodsCustomerWeight),0) as GoodsNumber from ContractOrderDetail where OrderNO ='{0}' and GoodTypeID in ({1}) ", orderNO, costProjectInfo.UsingGoods.TrimEnd(','));
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
                        sql = string.Format(@"select isnull(sum(GoodsStaffWeight),0) as GoodsNumber from ContractOrderDetail where OrderNO ='{0}' and GoodTypeID in ({1}) ", orderNO, costProjectInfo.UsingGoods.TrimEnd(','));
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
                        sql = string.Format(@"select isnull(sum(GoodsDriverWeight),0) as GoodsNumber from ContractOrderDetail where OrderNO ='{0}' and GoodTypeID in ({1}) ", orderNO, costProjectInfo.UsingGoods.TrimEnd(','));
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

        #endregion 发货单相关费用计算

        #region 创建报价体系

        public bool CreateContractPriceSetInfo(ContractInfo curretnInfo)
        {
            //创建报价体系
            IList<ICriterion> qryList = new List<ICriterion>();
            PriceSetInfo newPriceSetInfo = new PriceSetInfo();
            newPriceSetInfo.ContractID = curretnInfo.ID;
            newPriceSetInfo.IsUsed = "1";
            newPriceSetInfo.Remark = "";
            newPriceSetInfo.SetDate = DateTime.Now.ToString("yyyyMMdd");
            Core.Container.Instance.Resolve<IServicePriceSetInfo>().Create(newPriceSetInfo);
            //获取当前创建套系的ID
            qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("SetDate", newPriceSetInfo.SetDate));
            qryList.Add(Expression.Eq("ContractID", curretnInfo.ID));
            PriceSetInfo priceSetInfo = Core.Container.Instance.Resolve<IServicePriceSetInfo>().GetEntityByFields(qryList);

            /*------------------创建物品价明细------------------------*/
            PriceSetGoodsInfo priceSetGoodsInfo = new PriceSetGoodsInfo();
            //获取物品类别，判断报价类型，不参与报价的不需要
            qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("IsUsed", "1"));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<EquipmentTypeInfo> listEquipmentType = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetAllByKeys(qryList, orderList);
            foreach (EquipmentTypeInfo typeInfo in listEquipmentType)
            {
                ////报价类型 1:按类别统计报价  
                //if (typeInfo.PriceSetType == 1)
                //{
                //    priceSetGoodsInfo = new PriceSetGoodsInfo();
                //    priceSetGoodsInfo.DailyRents = typeInfo.RentPrice;
                //    priceSetGoodsInfo.EquipmentID = typeInfo.ID;
                //    priceSetGoodsInfo.MinRentingDays = typeInfo.MinRentingDays;
                //    priceSetGoodsInfo.UnitPrice = typeInfo.PayForPrice;
                //    priceSetGoodsInfo.SetID = priceSetInfo.ID;
                //    priceSetGoodsInfo.SetDate = priceSetInfo.SetDate;
                //    priceSetGoodsInfo.FixPrice = typeInfo.FixPrice;
                //    priceSetGoodsInfo.GoodsTypeID = typeInfo.ID;
                //    priceSetGoodsInfo.CustomerUnit = typeInfo.CustomerUnit;
                //    priceSetGoodsInfo.DriverUnit = typeInfo.DriverUnit;
                //    priceSetGoodsInfo.StaffUnit = typeInfo.StaffUnit;
                //    priceSetGoodsInfo.IsGoodType = 1;
                //    Core.Container.Instance.Resolve<IServicePriceSetGoodsInfo>().Create(priceSetGoodsInfo);
                //}
                ////报价类型 2：按物品分别报价 
                //if (typeInfo.PriceSetType == 2)
                //{
                //    qryList = new List<ICriterion>();
                //    qryList.Add(Expression.Eq("IsUsed", "1"));
                //    qryList.Add(Expression.Eq("EquipmentTypeID", typeInfo.ID));
                //    IList<EquipmentInfo> listEquipment = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetAllByKeys(qryList, orderList);
                //    foreach (EquipmentInfo obj in listEquipment)
                //    {
                //        priceSetGoodsInfo = new PriceSetGoodsInfo();
                //        priceSetGoodsInfo.DailyRents = obj.DailyRents;
                //        priceSetGoodsInfo.EquipmentID = obj.ID;
                //        priceSetGoodsInfo.MinRentingDays = obj.MinRentingDays;
                //        priceSetGoodsInfo.UnitPrice = obj.UnitPrice;
                //        priceSetGoodsInfo.SetID = priceSetInfo.ID;
                //        priceSetGoodsInfo.SetDate = priceSetInfo.SetDate;
                //        priceSetGoodsInfo.FixPrice = obj.FixPrice;
                //        priceSetGoodsInfo.GoodsTypeID = obj.EquipmentTypeID;
                //        priceSetGoodsInfo.CustomerUnit = obj.CustomerUnit;
                //        priceSetGoodsInfo.DriverUnit = obj.DriverUnit;
                //        priceSetGoodsInfo.StaffUnit = obj.StaffUnit;
                //        priceSetGoodsInfo.IsGoodType = 2;
                //        Core.Container.Instance.Resolve<IServicePriceSetGoodsInfo>().Create(priceSetGoodsInfo);
                //    }
                //}
                ////报价类型 3：不参与合同报价
                //if (typeInfo.PriceSetType == 3)
                //{
                //    continue;
                //}
            }

            //更新合同价格套系信息
            //curretnInfo.PriceSetID = priceSetInfo.ID;
            Core.Container.Instance.Resolve<IServiceContractInfo>().Update(curretnInfo);

            return true;
        }

        #endregion 创建报价体系
    }

    public partial class ContractShowObj
    {
        public int ID { get; set; }
        public string ContarctName { get; set; }
    }

    public partial class ContractCarShowObj
    {
        public int ID { get; set; }
        public string CarName { get; set; }
    }
}