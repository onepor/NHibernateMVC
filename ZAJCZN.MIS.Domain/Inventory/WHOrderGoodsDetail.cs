using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class WHOrderGoodsDetail : BaseEntity<WHOrderGoodsDetail>
    {
        /// <summary>
        /// 入库日期
        /// </summary>
        [Property]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 出库\入库单号
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
        /// 转换商品数量（主要是用于进货和消耗兑换)
        /// </summary>
        [Property]
        public decimal ChangeNumber { get; set; }

        /// <summary>
        /// 商品标准单位
        /// </summary>
        [Property]
        public string GoodsUnit { get; set; }

        /// <summary>
        /// 转换商品单位（主要是用于进货和消耗兑换)
        /// </summary>
        [Property]
        public string ChangeUnit { get; set; }

        /// <summary>
        ///  商品信息
        /// </summary> 
        public EquipmentInfo GoodsInfo { get; set; }
    }
}
