using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class ContractReceivingOrderInfo : BaseEntity<ContractReceivingOrderInfo>
    {
        /// <summary>
        /// 合同信息
        /// </summary>
        //多对一，对应ContractInfo的ID属性
        [BelongsTo(Lazy = FetchWhen.OnInvoke, Column = "ContractID")]
        public ContractInfo ContractInfo { get; set; }

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
        /// 订单单号
        /// </summary>
        [Property]
        public string OrderNO { get; set; }

        /// <summary>
        /// 收货商品数量
        /// </summary>
        [Property]
        public decimal OrderNumber { get; set; }

        /// <summary>
        /// 收货商品金额
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

    }
}
