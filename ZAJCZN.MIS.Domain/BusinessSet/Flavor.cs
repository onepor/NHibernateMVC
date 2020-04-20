using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 口味信息
    /// </summary>
    [ActiveRecord]
    public partial class Tm_FlavorInfo : BaseEntity<Tm_FlavorInfo>
    {
        /// <summary>
        /// 口味名称
        /// </summary>
        [Property]
        public string FlavorName { get; set; }

        /// <summary>
        /// 上级类型编号
        /// </summary>
        [Property]
        public int ParentID { get; set; }

        /// <summary>
        /// 拼音助记码
        /// </summary>
        [Property]
        public string FlavorPY { get; set; }

        /// <summary>
        /// 数字助记码
        /// </summary>
        [Property]
        public string FlavorCode { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Property]
        public int IsUsed { get; set; }
      
    }
}

