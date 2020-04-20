using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 会员充值记录信息
    /// </summary>
    [ActiveRecord]
    public partial class tm_VIPPrepaid : BaseEntity<tm_VIPPrepaid>
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        [Property]
        public int VipID { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Property]
        public int VIPPhone { get; set; }

        /// <summary>
        /// 充值时间
        /// </summary>
        [Property]
        public DateTime PrepaidDate { get; set; }

        /// <summary>
        /// 充值金额
        /// </summary>
        [Property]
        public decimal PrepaidAmount { get; set; }

        /// <summary>
        /// 赠送金额
        /// </summary>
        [Property]
        public decimal PresentationAmount { get; set; }

        /// <summary>
        /// 充值方式 1.刷卡2.现金3.微信4.支付宝
        /// </summary>
        [Property]
        public string PrepaidWay { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        [Property]
        public string Operator { get; set; }

        /// <summary>
        /// 充值订单号
        /// </summary>
        [Property]
        public string OrderNO { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

       
    }
}

