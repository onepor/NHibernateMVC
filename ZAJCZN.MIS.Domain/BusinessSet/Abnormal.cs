using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 异常原因信息
    /// </summary>
    [ActiveRecord] 
    public partial class Tm_Abnormal : BaseEntity<Tm_Abnormal>
    {
        /// <summary>
        /// 异常原因名称
        /// </summary>
        [Property]
        public string AbnormalName { get; set; }

        /// <summary>
        /// 异常原因类型
        /// </summary>
        [Property]
        public string AbnormalType { get; set; }

        /// <summary>
        /// 拼音助记码
        /// </summary>
        [Property]
        public string AbnormalPY { get; set; }

        /// <summary>
        /// 数字助记码
        /// </summary>
        [Property]
        public string AbnormalCode { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Property]
        public int IsUsed { get; set; }
      
    }
}

