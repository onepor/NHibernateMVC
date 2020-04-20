using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public partial class tm_SalePayInfo : BaseEntity<tm_SalePayInfo>
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
        public decimal? CashMoneys { get; set; }


        /// <summary>
        /// 刷卡支付
        /// </summary>
        [Property]
        public string PayWayCredit { get; set; }

        /// <summary>
        /// 信用卡支付金额
        /// </summary>
        [Property]
        public decimal? CreditMoneys { get; set; }


        /// <summary>
        /// 会员卡支付
        /// </summary>
        [Property]
        public string PayWayVipcard { get; set; }

        /// <summary>
        /// 会员余额支付金额
        /// </summary>
        [Property]
        public decimal? VipcardMoneys { get; set; }

        /// <summary>
        /// 在线支付1.微信2.支付宝
        /// </summary>
        [Property]
        public string PayWayOnline { get; set; }


        /// <summary>
        /// 在线支付金额
        /// </summary>
        [Property]
        public decimal? OnlineMoneys { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        [Property]
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// SaleID
        /// </summary>
        [Property]
        public int? SaleOrderID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }
    }
}
