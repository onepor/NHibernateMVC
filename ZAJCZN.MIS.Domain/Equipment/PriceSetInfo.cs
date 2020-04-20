using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class PriceSetInfo : BaseEntity<PriceSetInfo>
    {
        /// <summary>
        /// 价格套系名称
        /// </summary>
        [Property]
        public string SetName { get; set; }

        /// <summary>
        /// 是否启用	0：停用  1：启用（默认）
        /// </summary>
        [Property]
        public string IsUsed { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }
        
    }
}
