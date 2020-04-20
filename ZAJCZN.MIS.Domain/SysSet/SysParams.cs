using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 异常原因信息
    /// </summary>
    [ActiveRecord]
    public partial class Sys_Paras : BaseEntity<Sys_Paras>
    {
        /// <summary>
        /// 参数中文名称
        /// </summary>
        [Property]
        public string ParaName { get; set; }

        /// <summary>
        /// 参数编号
        /// </summary>
        [Property]
        public string ParaKey { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        [Property]
        public string ParaValue { get; set; }

        /// <summary>
        /// 参数备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 参数分组
        /// </summary>
        [Property]
        public string ParaGroup { get; set; }


    }
}

