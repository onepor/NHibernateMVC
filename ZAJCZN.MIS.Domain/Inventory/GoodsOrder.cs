using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class GoodsOrder : BaseEntity<GoodsOrder>
    {
        /// <summary>
        /// 订单日期
        /// </summary>
        [Property]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 订单单号
        /// </summary>
        [Property]
        public string OrderNO { get; set; }

        /// <summary>
        ///  订单类型 1:进货  2：进退
        /// </summary>
        [Property]
        public int OrderType { get; set; }

        /// <summary>
        /// 订单商品数量
        /// </summary>
        [Property]
        public decimal OrderNumber { get; set; }

        /// <summary>
        /// 订单商品金额
        /// </summary>
        [Property]
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 订单说明
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 订单结算类型  1:现金  2：挂账
        /// </summary>
        [Property]
        public int OrderPayType { get; set; }

        /// <summary>
        /// 原始业务单号（用于进退）
        /// </summary>
        [Property]
        public string OriginalOrderNO { get; set; }

        /// <summary>
        /// 临时订单标志 0：正式  1：临时
        /// </summary>
        [Property]
        public int IsTemp { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        [Property]
        public int SuplierID { get; set; }

        /// <summary>
        /// 供应商信息
        /// </summary> 
        public SupplierInfo SupplierInfo { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        [Property]
        public int WareHouseID { get; set; }

        /// <summary>
        /// 仓库信息
        /// </summary> 
        public WareHouse WareHouseInfo { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        [Property]
        public string Operator { get; set; }

    }
}
