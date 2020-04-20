using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 员工合同信息
    /// </summary>

    [ActiveRecord]
    public partial class EmployeeContractInfo : BaseEntity<EmployeeContractInfo>
    {
        /// <summary>
        /// 人员合同编号
        /// </summary>
        [Property]
        public string ContractNO { get; set; }

        /// <summary>
        /// 合同开始日期
        /// </summary>
        [Property]
        public DateTime? ContractStartDate { get; set; }

        /// <summary>
        /// 合同结束日期
        /// </summary>
        [Property]
        public DateTime? ContractEndDate { get; set; }

        /// <summary>
        /// 合同附件地址
        /// </summary>
        [Property]
        public string ContractAccessory { get; set; }

        /// <summary>
        /// 合同类别
        /// </summary>
        [Property]
        public string ContractType { get; set; }

        /// <summary>
        /// 合同期限
        /// </summary>
        [Property]
        public int? ContractPeriod { get; set; }

        
        /// <summary>
        /// 合同对应的人员关系
        /// </summary>
        //多对一，对应EmployeeInfo的employeeContractInfos_EmployeeInfo属性
        [BelongsTo(Lazy =FetchWhen.OnInvoke, Column = "UserID")]
        public EmployeeInfo employeeInfo_EmployeeContractInfo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }
        
    }
}

