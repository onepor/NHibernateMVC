using Castle.ActiveRecord;
using System.ComponentModel;

namespace ZAJCZN.MIS.Domain
{
	/// <summary>
	/// 项目人员信息
	/// </summary>
    [ActiveRecord]
	public class ProjectStaffInfo:BaseEntity<ProjectStaffInfo>
	{     
        /// <summary>
        /// 姓名
        /// </summary>
        [Property]
        public string EmployeeName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Property]
        public string EmployeePhone { get; set; }

       
        //多对一，对应ProjectInfo的projectStaffInfos_ProjectInfo属性
        [BelongsTo(Lazy = FetchWhen.OnInvoke, Column = "ProjectID")]
        public ProjectInfo projectInfo_ProjectStaffInfo { get; set; }

        
        //多对一，对应EmployeeInfo的projectStaffInfos_EmployeeInfo属性
        [BelongsTo(Lazy = FetchWhen.OnInvoke, Column = "EmployeeID")]
        public EmployeeInfo employeeInfo_ProjectStaffInfo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }
    }
}

