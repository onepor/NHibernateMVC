using Castle.ActiveRecord;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 费用项
    /// </summary>
    [ActiveRecord]
    public partial class CostItemInfo:BaseEntity<CostItemInfo>
	{
        /// <summary>
        /// 费用项目名称
        /// </summary>
        [Property]
        public string CostName { get; set; }

        /// <summary>
        /// 费用类型
        /// </summary>
        [Property]
        public string CostType { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Property]
        public string IsUsed { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 费用项管理对应的项目费用项集合
        /// </summary>
        //一对多，对用ProjectCostItemInfo的costItemInfo_ProjectCostItemInfo属性
        [HasMany(typeof(ProjectCostItemInfo), Table = "ProjectCostItemInfo", ColumnKey = "CostItemID", Cascade = ManyRelationCascadeEnum.None, Inverse = false, Lazy = true)]
        public IList<ProjectCostItemInfo> projectCostItemInfos_CostItemInfo { get; set; }
    }
}

