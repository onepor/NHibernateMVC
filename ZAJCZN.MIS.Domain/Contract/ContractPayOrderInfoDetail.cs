using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class ContractPayOrderDetail : BaseEntity<ContractPayOrderDetail>
    {
        /// <summary>
        /// 销售日期
        /// </summary>
        [Property]
        public DateTime OrderDate { get; set; }

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
        /// 商品单价
        /// </summary>
        [Property]
        public decimal GoodsUnitPrice { get; set; }

        /// <summary>
        /// 赔偿总价
        /// </summary>
        [Property]
        public decimal PayAmount { get; set; }      

        /// <summary>
        ///  商品信息
        /// </summary> 
        public EquipmentTypeInfo GoodsTypeInfo { get; set; }



    }
}
