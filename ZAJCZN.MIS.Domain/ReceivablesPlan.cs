using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 收款计划信息
    /// </summary>
    [ActiveRecord]
    public partial class ReceivablesPlan:BaseEntity<ReceivablesPlan>
	{
        /// <summary>
        /// 计划名称
        /// </summary>
        [Property]
        public string PlanName { get; set; }

        /// <summary>
        /// 合同金额比例
        /// </summary>
        [Property]
        public decimal? ContractRatio { get; set; }

        /// <summary>
        /// 待收金额
        /// </summary>
        [Property]
        public decimal? ReceivedAmount { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        [Property]
        public decimal? CollectedAmount { get; set; }

        /// <summary>
        /// 偏差金额
        /// </summary>
        [Property]
        public decimal? DeviationAmount { get; set; }

        /// <summary>
        /// 发票开票编号
        /// </summary>
        [Property]
        public string InvoiceNO { get; set; }

        /// <summary>
        /// 发票开票金额
        /// </summary>
        [Property]
        public decimal? InvoiceAmount { get; set; }

        /// <summary>
        /// 发票存根地址
        /// </summary>
        [Property]
        public string InvoicePicture { get; set; }

        /// <summary>
        /// 发票开票时间
        /// </summary>
        [Property]
        public  string InvoiceDate { get; set; }

        /// <summary>
        /// 计划时间
        /// </summary>
        [Property]
        public string PlanDate { get; set; }

        /// <summary>
        /// 计划状态
        /// </summary>
        [Property]
        public string PlanState { get; set; }

       
        //多对一，对应ProjectInfo的receivablesPlans_ProjectInfo属性
        [BelongsTo(Lazy = FetchWhen.OnInvoke, Column = "ProjectID")]
        public ProjectInfo projectInfo_ReceivablesPlan { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }
    }
}

