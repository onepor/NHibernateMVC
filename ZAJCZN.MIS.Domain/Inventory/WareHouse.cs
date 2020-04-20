using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class WareHouse : BaseEntity<WareHouse>
    {
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Property]
        public string WHName { get; set; }

        /// <summary>
        ///  仓库编码
        /// </summary>
        [Property]
        public string WHCode { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Property]
        public int IsUsed { get; set; }

        /// <summary>
        ///  默认仓库
        /// </summary>
        [Property]
        public int IsDefault { get; set; }

    }
}
