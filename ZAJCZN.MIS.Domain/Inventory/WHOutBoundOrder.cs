using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class WHOutBoundOrder : BaseEntity<WHOutBoundOrder>
    {
        /// <summary>
        /// 出库日期
        /// </summary>
        [Property]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 出库单编号
        /// </summary>
        [Property]
        public string OrderNO { get; set; }

        /// <summary>
        ///  出库类型 1:发货出库 2：领用出库  3：进货退货出库 4：损耗出库 
        /// </summary>
        [Property]
        public int OrderType { get; set; }

        /// <summary>
        /// 商品总数量
        /// </summary>
        [Property]
        public decimal OrderNumber { get; set; }

        /// <summary>
        /// 出库商品总金额
        /// </summary>
        [Property]
        public decimal OrderAmount { get; set; }
        
        /// <summary>
        /// 供货商ID（退货单使用）
        /// </summary>
        [Property]
        public int SuplierID { get; set; }

        /// <summary>
        /// 库房ID
        /// </summary>
        [Property]
        public int WareHouseID { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        [Property]
        public string Operator { get; set; }

        /// <summary>
        /// 出库备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 调拨目的数据库（调拨单出库使用）
        /// </summary>
        [Property]
        public int ToStockID { get; set; }

        /// <summary>
        /// 业务单号
        /// </summary>
        [Property]
        public string BOrderNO { get; set; }

        /// <summary>
        /// 调拨入库单号(调拨单使用)
        /// </summary>
        [Property]
        public string OutOrderNO { get; set; }

    }
}
