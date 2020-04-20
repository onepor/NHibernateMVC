using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class SortingGoodsDetail : BaseEntity<SortingGoodsDetail>
    {
        /// <summary>
        /// 销售日期
        /// </summary>
        [Property]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 物品类别编号
        /// </summary>
        [Property]
        public int GoodTypeID { get; set; }

        /// <summary>
        ///  销售单号
        /// </summary>
        [Property]
        public string OrderNO { get; set; }

        /// <summary>
        /// 商品总数 
        /// </summary>
        [Property]
        public decimal GoodsNumber { get; set; }

        /// <summary>
        /// 变化类型 1:收货新增 2：维修分拣减少
        /// </summary>
        [Property]
        public int OrderType { get; set; }

    }
}
