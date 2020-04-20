using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class ContractOrderDetail : BaseEntity<ContractOrderDetail>
    {
        /// <summary>
        /// 销售日期
        /// </summary>
        [Property]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        [Property]
        public int WareHouseID { get; set; }

        /// <summary>
        ///  销售单号
        /// </summary>
        [Property]
        public string OrderNO { get; set; }

        /// <summary>
        ///  商品类别ID
        /// </summary>
        [Property]
        public int GoodTypeID { get; set; }

        /// <summary>
        ///  商品ID
        /// </summary>
        [Property]
        public int GoodsID { get; set; }

        /// <summary>
        /// 商品标准单位
        /// </summary>
        [Property]
        public string GoodsUnit { get; set; }

        /// <summary>
        /// 商品出库总数(最终)
        /// </summary>
        [Property]
        public decimal GoodsNumber { get; set; }

        /// <summary>
        /// 商品出库总数（调整数）
        /// </summary>
        [Property]
        public decimal FixGoodsNumber { get; set; }

        /// <summary>
        /// 商品出库总数（出库单原始数）
        /// </summary>
        [Property]
        public decimal FormerlyGoodsNumber { get; set; }

        /// <summary>
        /// 商品计费标准单位
        /// </summary>
        [Property]
        public string GoodsCalcUnit { get; set; }

        /// <summary>
        /// 商品计价总数（换算为计费标准单位）
        /// </summary>
        [Property]
        public decimal GoodCalcPriceNumber { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        [Property]
        public decimal GoodsUnitPrice { get; set; }

        /// <summary>
        /// 商品待还总数（换算为计费标准单位）
        /// </summary>
        [Property]
        public decimal NotOffsetNumber { get; set; }

        /// <summary>
        /// 商品重量(对客户)（换算为计费标准单位后计算重量)
        /// </summary>
        [Property]
        public decimal GoodsCustomerWeight { get; set; }

        /// <summary>
        /// 商品重量(对司机)（换算为计费标准单位后计算重量)
        /// </summary>
        [Property]
        public decimal GoodsDriverWeight { get; set; }

        /// <summary>
        /// 商品重量(对员工)（换算为计费标准单位后计算重量)
        /// </summary>
        [Property]
        public decimal GoodsStaffWeight { get; set; }

        /// <summary> 
        /// 计价方式  1:L按计价单位 2：按出库数量
        /// </summary>
        [Property]
        public int PayUnit { get; set; }

        /// <summary> 
        /// 收货入库方式  1:维修分拣入库 0：直接入库
        /// </summary>
        [Property]
        public int IsStockByRepaired { get; set; }

        /// <summary>
        ///  商品信息
        /// </summary> 
        public EquipmentInfo GoodsInfo { get; set; }



    }
}
