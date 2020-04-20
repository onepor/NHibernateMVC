using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class tm_GoodsAllocationBill : BaseEntity<tm_GoodsAllocationBill>
    {
        /// <summary>
        /// 调拨单号
        /// </summary>
        [Property]
        public string OrderNO{ get; set; }

        /// <summary>
        /// 出库仓库ID
        /// </summary>
        [Property]
        public int CKWareHouseID { get; set; }

        /// <summary>
        /// 入库仓库ID
        /// </summary>
        [Property]
        public int RKWareHouseID { get; set; }

        /// <summary>
        ///  调拨日期
        /// </summary>
        [Property]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 操作员ID
        /// </summary>
        [Property]
        public string Operator { get; set; }

        /// <summary>
        /// 调拨总数
        /// </summary>
        [Property]
        public decimal AllotCount { get; set; }

        /// <summary>
        /// 调拨商品总金额
        /// </summary>
        [Property]
        public decimal AllotAmount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 临时订单标志 0：正式  1：临时
        /// </summary>
        [Property]
        public int IsTemp { get; set; }
    }
}
