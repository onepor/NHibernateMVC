using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 挂账
    /// </summary>
    [ActiveRecord]
    public partial class tm_Charge : BaseEntity<tm_Charge>
    {
        /// <summary>
        /// 挂账人
        /// </summary>
        [Property]
        public string ChargeName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        [Property]
        public string CompanyName { get; set; }
    }
}

