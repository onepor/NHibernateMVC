using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class tm_Goods : BaseEntity<tm_Goods>
    {
        /// <summary>
        /// 商品类别编号
        /// </summary>
        [Property]
        public int GoodsTypeID { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Property]
        public string GoodsName { get; set; }

        /// <summary>
        ///  商品编码
        /// </summary>
        [Property]
        public int GoodsCode { get; set; }

        /// <summary>
        /// 商品单位
        /// </summary>
        [Property]
        public string GoodsUnit { get; set; }

        /// <summary>
        /// 拼音助记码
        /// </summary>
        [Property]
        public string GoodsPY { get; set; }

        /// <summary>
        /// 商品单价(进货价)
        /// </summary>
        [Property]
        public decimal GoodsPrice { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        [Property]
        public string GoodsFormat { get; set; }

        /// <summary>
        /// 盘点类型
        /// </summary>
        [Property]
        public string InventoryType { get; set; }

        /// <summary>
        /// 税率
        /// </summary>
        [Property]
        public int TaxPoint { get; set; }

        /// <summary>
        /// 消耗换算值
        /// </summary>
        [Property]
        public decimal ConsumeNum { get; set; }

        /// <summary>
        /// 标准与消耗换算值单位
        /// </summary>
        [Property]
        public string ConsumeUnit { get; set; }

        /// <summary>
        /// 采购换算值
        /// </summary>
        [Property]
        public decimal PurchaseNum { get; set; }

        /// <summary>
        /// 标准与采购换算值单位
        /// </summary>
        [Property]
        public string PurchaseUnit { get; set; }

        /// <summary>
        /// 订货换算值
        /// </summary>
        [Property]
        public decimal OrderNum { get; set; }

        /// <summary>
        /// 标准与订货换算值单位
        /// </summary>
        [Property]
        public string OrderUnit { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Property]
        public int IsUsed { get; set; }

        /// <summary>
        /// 商品条码
        /// </summary>
        [Property]
        public string GoodsBarCode { get; set; }

    }
}
