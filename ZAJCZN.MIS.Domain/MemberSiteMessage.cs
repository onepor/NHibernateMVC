using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain//ZABT.ERP.Domain
{
    /// <summary>
    /// 会员等级设置
    /// </summary>

    [ActiveRecord]
    public partial class MemberSiteMessage : BaseEntity<MemberSiteMessage>
    {
        /// <summary>
        /// 会员编号
        /// </summary>
        [Property]
        //[MyAttribute("会员编号", false)]
        public int UserID { get; set; }

        /// <summary>
        /// 信息标题
        /// </summary>
        [Property]
        //[MyAttribute("信息标题", false)]
        public string MesTitle { get; set; }
        
        /// <summary>
        /// 话题编号 第1条作为话题标题和第1信息，设为0
        /// </summary>
        [Property]
        //[MyAttribute("话题编号", false)]
        public int FirstID { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        [Property]
        //[MyAttribute("信息", false)]
        public string Message { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        [Property]
        //[MyAttribute("发送时间", false)]
        public DateTime MesDate { get; set; }

        /// <summary>
        /// 发送方  1：平台发送 2：自己回复
        /// </summary>
        [Property]
        //[MyAttribute("发送方", false)]
        public int SendType { get; set; }
         
        /// <summary>
        /// 是否已读  1：是  0：否
        /// </summary>
        [Property]
        //[MyAttribute("是否已读", false)]
        public int IsRead { get; set; } 
    }
}

