using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 在线支付成功记录
    /// </summary>
    [ActiveRecord]
    public partial class tm_OnlinePayInfo : BaseEntity<tm_OnlinePayInfo>
    {
        /// <summary>
        /// 支付类型 010微信，020 支付宝，060qq钱包，080京东钱包，090口碑，100翼支付，110银联二维码
        /// </summary>
        [Property]
        public string pay_type { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        [Property]
        public string merchant_name { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        [Property]
        public string merchant_no { get; set; }

        /// <summary>
        /// 终端号
        /// </summary>
        [Property]
        public string terminal_id { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [Property]
        public string terminal_trace { get; set; }

        /// <summary>
        /// 终端交易时间
        /// </summary>
        [Property]
        public string terminal_time { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        [Property]
        public string total_fee { get; set; }

        /// <summary>
        /// 支付完成时间
        /// </summary>
        [Property]
        public string end_time { get; set; }

        /// <summary>
        /// 利楚唯一订单号
        /// </summary>
        [Property]
        public string out_trade_no { get; set; }

        /// <summary>
        /// 通道订单号，微信订单号、支付宝订单号等
        /// </summary>
        [Property]
        public string channel_trade_no { get; set; }

        /// <summary>
        /// 银行渠道订单号，微信支付时显示在支付成功页面的条码，可用作扫码查询和扫码退款时匹配
        /// </summary>
        [Property]
        public string channel_order_no { get; set; }

        /// <summary>
        /// 付款方用户id，“微信openid”、“支付宝账户”、“qq号”等
        /// </summary>
        [Property]
        public string user_id { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }
    }
}