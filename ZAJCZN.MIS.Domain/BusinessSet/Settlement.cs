using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public partial class tm_Settlement : BaseEntity<tm_Settlement>
    {

        /// <summary>
        /// 结算时间
        /// </summary>
        [Property]
        public string SettlementDate { get; set; }

        /// <summary>
        /// 打印时间
        /// </summary>
        [Property]
        public DateTime PrintTime { get; set; }
        
        /// <summary>
        /// 客数
        /// </summary>
        [Property]
        public int CustomerCount { get; set; }

        /// <summary>
        /// 单数
        /// </summary>
        [Property]
        public int OrderCount { get; set; }
        
        /// <summary>
        /// 应收金额
        /// </summary>
        [Property]
        public decimal AmountReceivable { get; set; }

        /// <summary>
        /// 免单金额
        /// </summary>
        [Property]
        public decimal SingleAmount { get; set; }

        /// <summary>
        /// 挂账金额
        /// </summary>
        [Property]
        public decimal ChargeAmount { get; set; }

        /// <summary>
        /// 赠送金额
        /// </summary>
        [Property]
        public decimal DonationAmount { get; set; }

        /// <summary>
        /// 抹零金额
        /// </summary>
        [Property]
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// 折让金额
        /// </summary>
        [Property]
        public decimal DZAmount { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        [Property]
        public decimal AmountCollected { get; set; }

        /// <summary>
        /// 现金金额
        /// </summary>
        [Property]
        public decimal CashAmount { get; set; }

        /// <summary>
        /// 微信金额
        /// </summary>
        [Property]
        public decimal WXAmount { get; set; }

        /// <summary>
        /// 支付宝金额
        /// </summary>
        [Property]
        public decimal ZFBAmount { get; set; }

        /// <summary>
        /// 刷卡金额
        /// </summary>
        [Property]
        public decimal CardAmount { get; set; }

        /// <summary>
        /// 会员抵扣
        /// </summary>
        [Property]
        public decimal MemberAmount { get; set; }

        /// <summary>
        /// 团购券金额
        /// </summary>
        [Property]
        public decimal GroupAmount { get; set; }

        /// <summary>
        /// 退菜金额
        /// </summary>
        [Property]
        public decimal BackAmount { get; set; }

        /// <summary>
        /// 现金金额(实盘)
        /// </summary>
        [Property]
        public decimal ACCashAmount { get; set; }

        /// <summary>
        /// 微信金额(实盘)
        /// </summary>
        [Property]
        public decimal ACWXAmount { get; set; }

        /// <summary>
        /// 支付宝金额(实盘)
        /// </summary>
        [Property]
        public decimal ACZFBAmount { get; set; }

        /// <summary>
        /// 刷卡金额(实盘)
        /// </summary>
        [Property]
        public decimal ACCardAmount { get; set; }

        /// <summary>
        /// 会员抵扣(实盘)
        /// </summary>
        [Property]
        public decimal ACMemberAmount { get; set; }

        /// <summary>
        /// 团购券金额(实盘)
        /// </summary>
        [Property]
        public decimal ACGroupAmount { get; set; }
    }
}
