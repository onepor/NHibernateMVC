using Castle.ActiveRecord;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public partial class tm_Payment : BaseEntity<tm_Payment>
    {

        /// <summary>
        /// 支付方式名称
        /// </summary>
        [Property]
        public string PaymentName { get; set; }

        /// <summary>
        /// 是否抵现
        /// </summary>
        [Property]
        public string IsDeduction { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Property]
        public string IsUsed { get; set; }

        /// <summary>
        /// 是否用于会员充值
        /// </summary>
        [Property]
        public string IsVip { get; set; }

        /// <summary>
        /// 是否参与积分
        /// </summary>
        [Property]
        public string IsIntegral { get; set; }

        /// <summary>
        /// 计入实收占比
        /// </summary>
        [Property]
        public int Proportion { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Property]
        public int? Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }
    }
}
