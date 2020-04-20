using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class ContractOrderInfo : BaseEntity<ContractOrderInfo>
    {
        /// <summary>
        /// 合同信息
        /// </summary>
        //多对一，对应ContractInfo的ID属性
        [BelongsTo(Lazy = FetchWhen.OnInvoke, Column = "ContractID")]
        public ContractInfo ContractInfo { get; set; }

        /// <summary>
        ///  客户名称
        /// </summary>
        [Property]
        public string CustomerName { get; set; }

        /// <summary>
        ///  合同编号
        /// </summary>
        [Property]
        public string ContractNO { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        [Property]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 订单计价日期
        /// </summary>
        [Property]
        public string ValuationDate { get; set; }

        /// <summary>
        /// 订单单号(系统)
        /// </summary>
        [Property]
        public string OrderNO { get; set; }

        /// <summary>
        /// 订单单号(手工)
        /// </summary>
        [Property]
        public string ManualNO { get; set; }

        /// <summary>
        /// 发货商品数量
        /// </summary>
        [Property]
        public decimal OrderNumber { get; set; }

        /// <summary>
        /// 发货商品金额
        /// </summary>
        [Property]
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 结清时间
        /// </summary>
        [Property]
        public DateTime? FinishDate { get; set; }

        /// <summary>
        /// 是否结算标志  1:执行中 2:已结算
        /// </summary>
        [Property]
        public string OrderState { get; set; }

        /// <summary>
        /// 运输车牌号编号
        /// </summary>
        [Property]
        public int CarID { get; set; }

        /// <summary>
        /// 运输车牌号
        /// </summary>
        [Property]
        public string CarNO { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        [Property]
        public string Operator { get; set; }

        /// <summary>
        /// 订单说明
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 临时订单标志 
        /// </summary>
        [Property]
        public int IsTemp { get; set; }

        /// <summary>
        /// 是否调整  1：是  0 ：否 
        /// </summary>
        [Property]
        public int IsFix { get; set; }

        /// <summary>
        /// 订单类型  1:发货  2：收货 
        /// </summary>
        [Property]
        public int OrderType { get; set; }

    }
}
