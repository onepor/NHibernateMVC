using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 员工信息
    /// </summary>

    [ActiveRecord]
    public partial class EmployeeInfo : BaseEntity<EmployeeInfo>
    {
        /// <summary>
        /// 员工姓名
        /// </summary>
        [Property]
        public string EmployeeName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Property]
        public string Sex { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Property]
        public string ContractPhone { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        [Property]
        public string ContractAddress { get; set; }

        /// <summary>
        /// 状态 0：停用  1：启用
        /// </summary>
        [Property]
        public string IsUsed { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 员工类别  设计人员、安装人员、测量人员、销售人员、售后服务
        /// </summary>
        [Property]
        public string UserType { get; set; }
 
    }
}

