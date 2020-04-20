using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class powers : BaseEntity<powers>
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        [Property]       
        public string Name { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        [Property]
        public string GroupName { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        [Property]
        public string Title { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }
         
    }
}