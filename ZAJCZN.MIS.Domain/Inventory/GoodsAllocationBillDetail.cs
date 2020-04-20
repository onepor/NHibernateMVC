using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class tm_GoodsAllocationBillDetail : BaseEntity<tm_GoodsAllocationBillDetail>
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        [Property]
        public int GoodsID { get; set; }

        /// <summary>
        /// 商品类别
        /// </summary>
        [Property]
        public int GoodsType { get; set; }

        /// <summary>
        ///  商品数量
        /// </summary>
        [Property]
        public decimal GoodsNumber { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        [Property]
        public decimal? GoodsPrice { get; set; }

        /// <summary>
        /// 商品总价
        /// </summary>
        [Property]
        public decimal? GoodsAmount { get; set; }

        /// <summary>
        /// 调拨单号
        /// </summary>
        [Property]
        public string OrderNO { get; set; }

        /// <summary>
        /// 调拨日期
        /// </summary>
        [Property]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

    }
}
