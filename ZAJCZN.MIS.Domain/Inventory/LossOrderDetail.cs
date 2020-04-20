using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class LossOrderDetail : BaseEntity<LossOrderDetail>
    {
        /// <summary>
        /// 报损日期
        /// </summary>
        [Property]
        public DateTime OrderDate { get; set; }

        /// <summary>
        ///  报损单号
        /// </summary>
        [Property]
        public string OrderNO { get; set; }

        /// <summary>
        ///  商品ID
        /// </summary>
        [Property]
        public int GoodsID { get; set; }

        /// <summary>
        /// 报损总数
        /// </summary>
        [Property]
        public decimal GoodsNumber { get; set; }

        /// <summary>
        /// 商品单价(含税)
        /// </summary>
        [Property]
        public decimal GoodsUnitPrice { get; set; }

        /// <summary>
        ///  报损总金额(含税)
        /// </summary>
        [Property]
        public decimal GoodTotalPrice { get; set; }

        /// <summary>
        /// 商品标准单位
        /// </summary>
        [Property]
        public string GoodsUnit { get; set; }

        /// <summary>
        ///  商品信息
        /// </summary> 
        public EquipmentInfo GoodsInfo { get; set; }
    }
}
