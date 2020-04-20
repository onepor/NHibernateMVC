using Castle.ActiveRecord;
using System;
using System.ComponentModel;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 收款信息
    /// </summary>
    [ActiveRecord]
    public class ReceivablesInfo : BaseEntity<ReceivablesInfo>
    { 
        /// <summary>
        /// 回款时间
        /// </summary>
        [Property]
        public string ReceiveDate { get; set; }

        /// <summary>
        /// 回款金额
        /// </summary>
        [Property]
        public decimal? ReceivablesAmount { get; set; }
                        
        /// <summary>
        /// 合同信息
        /// </summary> 
        [BelongsTo(Lazy = FetchWhen.OnInvoke, Column = "ContractID")]
        public ContractInfo ContractInfo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }


    }
}

