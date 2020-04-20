using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class GoodsOrderDetail : BaseEntity<GoodsOrderDetail>
    {
        /// <summary>
        /// 进货日期
        /// </summary>
        [Property]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 进货单号
        /// </summary>
        [Property]
        public string OrderNO { get; set; }

        /// <summary>
        ///  商品ID
        /// </summary>
        [Property]
        public int GoodsID { get; set; }

        /// <summary>
        /// 商品总数
        /// </summary>
        [Property]
        public decimal GoodsNumber { get; set; }

        /// <summary>
        /// 商品单价(含税)
        /// </summary>
        [Property]
        public decimal GoodsUnitPrice { get; set; }

        /// <summary>
        ///  商品总金额(含税)
        /// </summary>
        [Property]
        public decimal GoodTotalPrice { get; set; }

        /// <summary>
        /// 税率
        /// </summary>
        [Property]
        public decimal TaxPoint { get; set; }

        /// <summary>
        /// 税金
        /// </summary>
        [Property]
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// 商品单价(不含税)
        /// </summary>
        [Property]
        public decimal UnitPriceNoTax { get; set; }

        /// <summary>
        /// 商品总金额(不含税)
        /// </summary>
        [Property]
        public decimal TotalPriceNoTax { get; set; }
 
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
