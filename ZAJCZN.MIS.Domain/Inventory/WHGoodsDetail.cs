using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class WHGoodsDetail : BaseEntity<WHGoodsDetail>
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        [Property]
        public int GoodsID { get; set; }

        /// <summary>
        /// 商品类别ID
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
        public string GoodsCode { get; set; }

        /// <summary>
        /// 库房ID
        /// </summary>
        [Property]
        public int WareHouseID { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        [Property]
        public decimal InventoryCount { get; set; }

        /// <summary>
        ///  库存金额
        /// </summary>
        [Property]
        public decimal InventoryAmount { get; set; }
  

        /// <summary>
        /// 库存单价(备用字段)
        /// </summary>
        [Property]
        public decimal InventoryUnitPrice { get; set; }

        /// <summary>
        /// 库存标准单位
        /// </summary>
        [Property]
        public string InventoryUnit { get; set; }

        /// <summary>
        /// 商品信息
        /// </summary> 
        public EquipmentInfo GoodsInfo { get; set; }



    }
}
