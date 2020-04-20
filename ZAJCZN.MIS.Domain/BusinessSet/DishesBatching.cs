using Castle.ActiveRecord;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 菜品信息
    /// </summary>
    [ActiveRecord]
    public partial class tm_DishesBatching : BaseEntity<tm_DishesBatching>
    {
        /// <summary>
        /// 配料名称
        /// </summary>
        [Property]
        public string BatchingName { get; set; }

        /// <summary>
        /// 配料物品ID
        /// </summary>
        [Property]
        public int GoodsID { get; set; }

        /// <summary>
        ///物品编号
        /// </summary>
        [Property]
        public string GoodsCode { get; set; }

        /// <summary>
        /// 消耗数量
        /// </summary>
        [Property]
        public decimal UsingCount { get; set; }

        /// <summary>
        /// 消耗单位
        /// </summary>
        [Property]
        public string UsingUnit { get; set; }

        /// <summary>
        /// 消耗单价
        /// </summary>
        [Property]
        public decimal UsingUnitPrice { get; set; }

        /// <summary>
        /// 配料成本
        /// </summary>
        [Property]
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 是否冲减库存
        /// </summary>
        [Property]
        public int IsOffset { get; set; }

        /// <summary>
        /// 菜品信息
        /// </summary>
        //多对一，对应tm_DishesBatching的DishesID属性
        [BelongsTo(Lazy = FetchWhen.OnInvoke, Column = "DishesID")]
        public tm_Dishes DishesInfo { get; set; }

        /// <summary>
        /// 出库库房ID
        /// </summary>
        [Property]
        public int WareHouseID { get; set; }

    }
}

