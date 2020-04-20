using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class roles : BaseEntity<roles>
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [Property]
        public string Name { get; set; }

        /// <summary>
        ///  备注
        /// </summary>
        [Property]
        public string Remark { get; set; } 

    }
}