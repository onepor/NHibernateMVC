using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class WHStorageOrder : BaseEntity<WHStorageOrder>
    {
        /// <summary>
        /// 入库日期
        /// </summary>
        [Property]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 入库单号
        /// </summary>
        [Property]
        public string OrderNO { get; set; }

        /// <summary>
        ///  入库类型  1:进货入库  2：收货入库  3：销退入库
        /// </summary>
        [Property]
        public int OrderType { get; set; }

        /// <summary>
        /// 入库商品数量
        /// </summary>
        [Property]
        public decimal OrderNumber { get; set; }

        /// <summary>
        /// 入库商品金额
        /// </summary>
        [Property]
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        [Property]
        public int SuplierID { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        [Property]
        public int WareHouseID { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        [Property]
        public string Operator { get; set; }

        /// <summary>
        /// 入库说明
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 调拨出库单号(调拨单使用)
        /// </summary>
        [Property]
        public string OutOrderNO { get; set; }

        /// <summary>
        /// 业务单号
        /// </summary>
        [Property]
        public string BOrderNO { get; set; }

    }
}
