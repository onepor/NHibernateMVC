using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public partial class tm_TabieUsingInfo : BaseEntity<tm_TabieUsingInfo>
    {

        /// <summary>
        /// 餐桌ID
        /// </summary>
        [Property]
        public int TabieID { get; set; }

        /// <summary>
        /// 就餐人数
        /// </summary>
        [Property]
        public int Population { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        [Property]
        public decimal Moneys { get; set; }

        /// <summary>
        /// 订单状态 1.开台 2.已结账 3.免单 4.挂账(原：1.开台2.备菜3.走菜4.清台5.结账6.免单7.挂账)
        /// </summary>
        [Property]
        public string OrderState { get; set; }

        /// <summary>
        /// vipID
        /// </summary>
        [Property]
        public string VipID { get; set; }

        /// <summary>
        /// 挂账原因
        /// </summary>
        [Property]
        public string Charge { get; set; }

        /// <summary>
        /// 免单原因
        /// </summary>
        [Property]
        public string FreeReason { get; set; }

        /// <summary>
        /// 赠送金额
        /// </summary>
        [Property]
        public decimal PrePrice { get; set; }

        /// <summary>
        /// 折让金额
        /// </summary>
        [Property]
        public decimal Discount { get; set; }

        /// <summary>
        /// 折扣比例
        /// </summary>
        [Property]
        public decimal DisPoint { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>
        [Property]
        public decimal FactPrice { get; set; }

        /// <summary>
        /// 开台时间
        /// </summary>
        [Property]
        public DateTime OpenTime { get; set; }

        /// <summary>
        /// 结算时间
        /// </summary>
        [Property]
        public DateTime? ClearTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 订单抹零金额
        /// </summary>
        [Property]
        public decimal Erasing { get; set; }

        /// <summary>
        /// 团购支付
        /// </summary>
        [Property]
        public string GroupName { get; set; }

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
        /// 合并付款编号
        /// </summary>
        [Property]
        public string MergeNO { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [Property]
        public string OrderNO { get; set; }

    }
}
