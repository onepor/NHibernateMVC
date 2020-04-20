using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class ContractPayOrderInfo : BaseEntity<ContractPayOrderInfo>
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
        /// 买赔商品总金额
        /// </summary>
        [Property]
        public decimal OrderAmount { get; set; }             

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
       
    }
}
