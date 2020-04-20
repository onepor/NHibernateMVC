using Castle.ActiveRecord;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class depts : BaseEntity<depts>
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        [Property]  
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Property]
        public int SortIndex { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 上级部门ID
        /// </summary>
        [Property]
        public int ParentID { get; set; }

        /// <summary>
        /// 菜单在树形结构中的层级（从0开始）
        /// </summary> 
        public int TreeLevel { get; set; }

        /// <summary>
        /// 是否可用（默认true）,在模拟树的下拉列表中使用
        /// </summary> 
        public bool Enabled { get; set; }

        /// <summary>
        /// 是否叶子节点（默认true）
        /// </summary> 
        public bool IsTreeLeaf { get; set; }
    }
}