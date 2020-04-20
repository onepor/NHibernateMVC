using Castle.ActiveRecord;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 系统配置项
    /// </summary>
    [ActiveRecord]
    public partial class configs : BaseEntity<configs>
	{
        /// <summary>
        /// 配置项标识
        /// </summary>
        [Property]
        public string ConfigKey { get; set; }

        /// <summary>
        /// 配置项值
        /// </summary>
        [Property]
        public string ConfigValue { get; set; }

        /// <summary>
        /// 是否启备注用
        /// </summary>
        [Property]
        public string Remark { get; set; } 
     
    }
}

