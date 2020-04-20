using Castle.ActiveRecord;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 项目开发计划
    /// </summary>
    [ActiveRecord]
    public partial class ProjectPlan : BaseEntity<ProjectPlan>
    {
        /// <summary>
        /// 项目功能名称
        /// </summary>
        [Property]
        public string FunctionName { get; set; }

        /// <summary>
        /// 开发进度
        /// </summary>
        [Property]
        public string Status { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        [Property]
        public string PlanStartDate { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        [Property]
        public string PlanEndDate { get; set; }

        /// <summary>
        /// 计划工作量
        /// </summary>
        [Property]
        public string PlanWorkload { get; set; }

        /// <summary>
        /// 实际开始时间
        /// </summary>
        [Property]
        public string StartDate { get; set; }

        /// <summary>
        /// 实际结束时间
        /// </summary>
        [Property]
        public string EndDate { get; set; }

        /// <summary>
        /// 实际工作量
        /// </summary>
        [Property]
        public string Workload { get; set; }

        /// <summary>
        /// 员工ID
        /// </summary>
        [Property]
        public int? EmployeeID { get; set; }

        /// <summary>
        /// 所属项目
        /// </summary>
        [BelongsTo(Lazy = FetchWhen.OnInvoke, Column = "ProjectID")]
        public ProjectInfo projectInfo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }
    }
}
