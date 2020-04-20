using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class roleusers : BaseEntity<roleusers>
    {
        /// <summary>
        /// 角色编号
        /// </summary>
        [Property]
        public int RoleID { get; set; }
        /// <summary>
        ///  用户编号
        /// </summary>
        [Property]
        public int UserID { get; set; } 

    }
}