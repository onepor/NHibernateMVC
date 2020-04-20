using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class SortingGoodsInfo : BaseEntity<SortingGoodsInfo>
    {
        /// <summary>
        ///  物品类别编号
        /// </summary>
        [Property]
        public int GoodTypeID { get; set; }

        /// <summary>
        ///  物品类别名称
        /// </summary>
        [Property]
        public string GoodsTypeName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Property]
        public decimal GoodsNumber { get; set; }       

        /// <summary>
        /// 最近更新时间
        /// </summary>
        [Property]
        public DateTime? UpdateDate { get; set; }       

    }
}
