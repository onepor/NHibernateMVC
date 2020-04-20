using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 异常原因信息
    /// </summary>
    [ActiveRecord]
    public partial class tm_vipinfo : BaseEntity<tm_vipinfo>
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Property]
        public string VIPName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Property]
        public string VIPPhone { get; set; }

        /// <summary>
        /// 账户金额
        /// </summary>
        [Property]
        public decimal VIPCount { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        [Property]
        public string VIPWXCode { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        [Property]
        public decimal VIPScore { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }
        
        /// <summary>
        /// 注册时间
        /// </summary>
        [Property]
        public DateTime RegisterDate { get; set; }
      
    }
}

