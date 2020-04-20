using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 发货、收货单费用信息
    /// </summary>
    [ActiveRecord]
    public partial class ContractOrderCostInfo : BaseEntity<ContractOrderCostInfo>
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Property]
        public string OrderNO { get; set; }

        /// <summary>
        /// 费用项ID
        /// </summary>
        [Property]
        public int CostID { get; set; }

        /// <summary>
        /// 费用名称
        /// </summary>
        [Property]
        public string CostName { get; set; }

        /// <summary>
        /// 计费单位
        /// </summary>
        [Property]
        public string PayUnit { get; set; }

        /// <summary>
        /// 计费数量
        /// </summary>
        [Property]
        public decimal OrderNumber { get; set; }

        /// <summary>
        /// 计费单价
        /// </summary>
        [Property]
        public decimal PayPrice { get; set; }

        /// <summary>
        /// 费用总金额
        /// </summary>
        [Property]
        public decimal CostAmount { get; set; }

        /// <summary>
        /// 费用项目类型  1：员工费用  2：客户费用  3：司机费用
        /// </summary>
        [Property]
        public int CostType { get; set; }

        /// <summary>
        /// 备注描述
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 是否已经结算  1：已结算  0：未结算（默认值）
        /// </summary>
        [Property]
        public int IsSettle { get; set; }

        /// <summary>
        /// 发货收货标志  1：发货  2：收货
        /// </summary>
        [Property]
        public int InOutFlag { get; set; }


    }
}

