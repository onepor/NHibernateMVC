using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 项目信息
    /// </summary>
    [ActiveRecord]
    public partial class ProjectInfo : BaseEntity<ProjectInfo>
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        [Property]
        public string ProjectName { get; set; }

        /// <summary>
        /// 合同金额
        /// </summary>
        [Property]
        public decimal? ContractAmount { get; set; }

        /// <summary>
        /// 项目进度情况
        /// </summary>
        [Property]
        public string ProjectState { get; set; }

        /// <summary>
        /// 合同签订时间
        /// </summary>
        [Property]
        public string ContractDate { get; set; }

        /// <summary>
        /// 项目开始时间
        /// </summary>
        [Property]
        public string StartDate { get; set; }

        /// <summary>
        /// 项目计划周期
        /// </summary>
        [Property]
        public decimal? DevelopmentCycle { get; set; }

        /// <summary>
        /// 客户姓名
        /// </summary>
        [Property]
        public string CustomerName { get; set; }

        /// <summary>
        /// 结项时间
        /// </summary>
        [Property]
        public string FinishDate { get; set; }

        /// <summary>
        /// 项目成本
        /// </summary>
        [Property]
        public decimal? TotalCost { get; set; }

        /// <summary>
        /// 项目利润
        /// </summary>
        [Property]
        public decimal? PlanProfit { get; set; }

        /// <summary>
        /// 实际利润
        /// </summary>
        [Property]
        public decimal? RealProfit { get; set; }

        /// <summary>
        /// 已收款金额
        /// </summary>
        [Property]
        public decimal? FundRestream { get; set; }

        /// <summary>
        /// 待收金额
        /// </summary>
        [Property]
        public decimal? ReceivedAmount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 项目对应的付款管理集合
        /// </summary>
        //一对多，对应PayInfo表的projectInfo_PayInfo属性
        [HasMany(typeof(ReceivablesInfo), Table = "PayInfo", ColumnKey = "ProjectID", Cascade = ManyRelationCascadeEnum.None, Inverse = false, Lazy = true)]
        public IList<ReceivablesInfo> payInfos_ProjectInfo { get; set; }

        /// <summary>
        /// 项目对应的费用项集合
        /// </summary>
        //一对多，对应ProjectCostItemInfo表的projectInfo_ProjectCostItemInfo属性
        [HasMany(typeof(ProjectCostItemInfo), Table = "ProjectCostItemInfo", ColumnKey = "ProjectID", Cascade = ManyRelationCascadeEnum.None, Inverse = false, Lazy = true)]
        public IList<ProjectCostItemInfo> projectCostItemInfos_ProjectInfo { get; set; }

        /// <summary>
        /// 项目对应的登记人员集合
        /// </summary>
        //一对多，对应ProjectStaffInfo表的projectInfo_ProjectStaffInfo属性
        [HasMany(typeof(ProjectStaffInfo), Table = "ProjectStaffInfo", ColumnKey = "ProjectID", Cascade = ManyRelationCascadeEnum.None, Inverse = false, Lazy = true)]
        public IList<ProjectStaffInfo> projectStaffInfos_ProjectInfo { get; set; }

        /// <summary>
        /// 项目对应的收款计划集合
        /// </summary>
        //一对多，对应ReceivablesPlan表的projectInfo_ReceivablesPlan属性
        [HasMany(typeof(ReceivablesPlan), Table = "ReceivablesPlan", ColumnKey = "ProjectID", Cascade = ManyRelationCascadeEnum.None, Inverse = false, Lazy = true)]
        public IList<ReceivablesPlan> receivablesPlans_ProjectInfo { get; set; }

        /// <summary>
        /// 项目对应的开发计划集合
        /// </summary>
        //一对多，对应ProjectPlan表的projectInfo_ProjectPlan属性
        [HasMany(typeof(ReceivablesPlan), Table = "ProjectPlan", ColumnKey = "ProjectID", Cascade = ManyRelationCascadeEnum.None, Inverse = false, Lazy = true)]
        public IList<ReceivablesPlan> projectPlan_ProjectInfo { get; set; }
    }
}

