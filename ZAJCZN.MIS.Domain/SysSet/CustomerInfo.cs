using Castle.ActiveRecord;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 客户信息
    /// </summary>
    [ActiveRecord]
    public partial class CustomerInfo : BaseEntity<CustomerInfo>
    {
        /// <summary>
        /// 客户编号
        /// </summary>
        [Property]
        public int CustomerNumber { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [Property]
        public string CustomerName { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Property]
        public string LinkMan { get; set; }

        /// <summary>
        /// 客户联系电话
        /// </summary>
        [Property]
        public string ContractPhone { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        [Property]
        public string ContractAddress { get; set; }

        /// <summary>
        /// 状态 0：停用  1：启用
        /// </summary>
        [Property]
        public string IsUsed { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

    }
}

