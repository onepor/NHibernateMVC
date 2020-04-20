using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 项目信息
    /// </summary>
    [ActiveRecord]
    public partial class ContractPayInfo : BaseEntity<ContractPayInfo>
    {
        /// <summary>
        /// 合同ID
        /// </summary>
        [Property]
        public int ContractID { get; set; }

        /// <summary>
        /// 付款方式 1：收款  2：付款  3:售后
        /// </summary>
        [Property]
        public int PayType { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Property]
        public decimal PayMoney { get; set; }

        /// <summary> 
        /// 客户名称
        /// </summary>
        [Property]
        public string PayUser { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        [Property]
        public string PayWay { get; set; }

        /// <summary> 
        /// 支付日期
        /// </summary>
        [Property]
        public string ApplyDate { get; set; }

        /// <summary>
        /// 支付状态 1：待审核 2：审核通过 3：审核不通过
        /// </summary>
        [Property]
        public int ApplyState { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        [Property]
        public string AproveUser { get; set; }

        /// <summary>
        /// 审核日期
        /// </summary>
        [Property]
        public string AproveDate { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        [Property]
        public string Operator { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }
    }
}

