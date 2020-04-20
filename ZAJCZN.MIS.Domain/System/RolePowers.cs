using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class rolepowers : BaseEntity<rolepowers>
    {
        /// <summary>
        /// 角色编号
        /// </summary>
        [Property]
        public int RoleID { get; set; }
        /// <summary>
        /// 权限编号
        /// </summary>
        [Property]
        public int PowerID { get; set; }

    }
}