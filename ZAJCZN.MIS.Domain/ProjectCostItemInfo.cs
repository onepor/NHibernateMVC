using Castle.ActiveRecord;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 项目费用项
    /// </summary>
    [ActiveRecord]
    public partial class ProjectCostItemInfo : BaseEntity<ProjectCostItemInfo>
    {

        //多对一，对应CostItemInfo的projectCostItemInfos_CostItemInfo属性
        [BelongsTo(Lazy = FetchWhen.OnInvoke, Column = "CostItemID")]
        public CostItemInfo costItemInfo_ProjectCostItemInfo { get; set; }

        /// <summary>
        /// 费用项名称
        /// </summary>
        [Property]
        public string CostItemName { get; set; }

        /// <summary>
        /// 费用类型
        /// </summary>
        [Property]
        public string CostType { get; set; }


        //多对一，对应ProjectInfo的projectCostItemInfos属性
        [BelongsTo(Lazy = FetchWhen.OnInvoke, Column = "ProjectID")]
        public ProjectInfo projectInfo_ProjectCostItemInfo { get; set; }

        /// <summary>
        /// 费用金额
        /// </summary>
        [Property]
        public decimal? CostMoney { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }
    }
}

