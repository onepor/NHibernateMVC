using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 维修项信息
    /// </summary>
    [ActiveRecord]
    public partial class RepairProjectInfo : BaseEntity<RepairProjectInfo>
    {
        /// <summary>
        /// 维修项名称
        /// </summary>
        [Property]
        public string ProjectName { get; set; }

        /// <summary>
        /// 计费单位  1:数量 2:计价单位 3:客户吨位 4:员工吨位 5:司机吨位 6:其他
        /// </summary>
        [Property]
        public string PayUnit { get; set; }

        /// <summary>
        /// 计费单价
        /// </summary>
        [Property]
        public decimal PayPrice { get; set; }

        /// <summary>
        /// 状态 0：停用  1：启用
        /// </summary>
        [Property]
        public string IsUsed { get; set; }

        /// <summary>
        /// 备注描述
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 费用项目类型  1：员工费用  2：客户费用  3：司机费用
        /// </summary>
        [Property]
        public int ProjectType { get; set; }

        /// <summary>
        /// 费用项目使用类型  1：场内  2：发货  3：收货
        /// </summary>
        [Property]
        public int UsingType { get; set; }

        /// <summary>
        /// 费用关联器材类型
        /// </summary>
        [Property]
        public string UsingGoods { get; set; }

        /// <summary>
        /// 费用价格获取类型  0：自定义  1：合同客户运费  2：合同司机运费  3：合同单价  4：合同维修单价
        /// </summary>
        [Property]
        public int PriceSourceType { get; set; }

        /// <summary>
        /// 是否固定费用  0：否  1：固定费用
        /// </summary>
        [Property]
        public int IsRegular { get; set; }
        
        /// <summary>
        /// 是否生成工单  0：否  1：是
        /// </summary>
        [Property]
        public int IsCreateJob { get; set; }

          /// <summary>
        /// 是否二次分拣  0：否  1：是
        /// </summary>
        [Property]
        public int IsSorting { get; set; }


    }
}

