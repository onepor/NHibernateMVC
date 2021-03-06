﻿using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public partial class tm_TabiePayInfo : BaseEntity<tm_TabiePayInfo>
    {

        /// <summary>
        /// 现金支付
        /// </summary>
        [Property]
        public string PayWayCash { get; set; }


        /// <summary>
        /// 现金支付金额
        /// </summary>
        [Property]
        public decimal CashMoneys { get; set; }


        /// <summary>
        /// 刷卡支付
        /// </summary>
        [Property]
        public string PayWayCredit { get; set; }

        /// <summary>
        /// 信用卡支付金额
        /// </summary>
        [Property]
        public decimal CreditMoneys { get; set; }

        /// <summary>
        /// 会员卡支付
        /// </summary>
        [Property]
        public string PayWayVipcard { get; set; }

        /// <summary>
        /// 会员手机号
        /// </summary>
        [Property]
        public string VipCardNO { get; set; }

        /// <summary>
        /// 会员余额支付金额
        /// </summary>
        [Property]
        public decimal VipcardMoneys { get; set; }

        /// <summary>
        /// 在线支付(微信)
        /// </summary>
        [Property]
        public string PayWayOnline { get; set; }

        /// <summary>
        /// 在线支付金额(微信)
        /// </summary>
        [Property]
        public decimal OnlineMoneys { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        [Property]
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// UsingID
        /// </summary>
        [Property]
        public int TabieUsingID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 团购支付
        /// </summary>
        [Property]
        public string PayWayGroup { get; set; }

        /// <summary>
        /// 团购支付金额
        /// </summary>
        [Property]
        public decimal GroupMoneys { get; set; }

        /// <summary>
        /// 团购券号
        /// </summary>
        [Property]
        public string GroupCardNO { get; set; }

        /// <summary>
        /// 在线支付(支付宝)
        /// </summary>
        [Property]
        public string PayWayZFB { get; set; }

        /// <summary>
        /// 在线支付金额(支付宝)
        /// </summary>
        [Property]
        public decimal? ZFBMoneys { get; set; }

        /// <summary>
        /// 合并付款编号
        /// </summary>
        [Property]
        public string MergeNO { get; set; }

    }
}
