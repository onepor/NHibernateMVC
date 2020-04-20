using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class ContractHandWareDetail : BaseEntity<ContractHandWareDetail>
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Property]
        public int ContractID { get; set; }

        /// <summary>
        /// 配件类别
        /// </summary>
        [Property]
        public int GoodTypeID { get; set; }

        /// <summary>
        ///  配件编号
        /// </summary>
        [Property]
        public int GoodsID { get; set; }

        /// <summary>
        ///  配件名称
        /// </summary>
        [Property]
        public string GoodsName { get; set; }

        /// <summary>
        ///  配件数量
        /// </summary>
        [Property]
        public int GoodsNumber { get; set; }

        /// <summary>
        ///  商品单价
        /// </summary>
        [Property]
        public decimal GoodsUnitPrice { get; set; }

        /// <summary>
        /// 商品标准单位
        /// </summary>
        [Property]
        public string GoodsUnit { get; set; }

        /// <summary> 
        /// 总价
        /// </summary>
        [Property]
        public decimal GoodAmount { get; set; }

        /// <summary>
        ///  商品成本单价
        /// </summary>
        [Property]
        public decimal CostPrice { get; set; }
         
        /// <summary> 
        /// 成本总价
        /// </summary>
        [Property]
        public decimal CostAmount { get; set; }

        /// <summary>
        ///  是否收费  1：收费  0：免费
        /// </summary> 
        [Property]
        public int IsFree { get; set; }

        /// <summary>
        ///  五金类型  1：门  2：柜子
        /// </summary>
        [Property]
        public int OrderType { get; set; }

    }
}
