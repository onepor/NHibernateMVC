using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class onlines : BaseEntity<onlines>
    {
        /// <summary>
        /// 登录IP
        /// </summary>
        [Property]
        public string IPAdddress { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        [Property]
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Property]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [Property]
        public int UserID { get; set; }
         /// <summary>
        /// 用户名称
        /// </summary>
        [Property]
        public string UserName { get; set; }
    }
}