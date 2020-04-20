using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 供应商信息
    /// </summary>
    [ActiveRecord]
    public partial class SupplierInfo : BaseEntity<SupplierInfo>
    {
        /// <summary>
        /// 供应商公司名称
        /// </summary>
        [Property]
        public string SupplierName { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Property]
        public string LinkMan { get; set; }

        /// <summary>
        /// 客户联系电话
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
        /// 供应商描述
        /// </summary>
        [Property]
        public string Remark { get; set; }
         
        
    }
}

