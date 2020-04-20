using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class menus : BaseEntity<menus>
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        [Property]
        public string Name { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [Property]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 菜单地址
        /// </summary>
        [Property]
        public string NavigateUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Property]
        public int SortIndex { get; set; }

        /// <summary>
        /// 上级菜单ID
        /// </summary>
        [Property]
        public int ParentID { get; set; }

        /// <summary>
        /// 菜单权限ID
        /// </summary>
        [Property]
        public int ViewPowerID { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string ViewPowerName { get; set; }
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