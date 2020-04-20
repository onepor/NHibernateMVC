using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZAJCZN.MIS.Web
{
    public class MenuTree : ICustomTree, IKeyID, ICloneable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string NavigateUrl { get; set; }
        public string Remark { get; set; }
        public int SortIndex { get; set; }
        public int ParentID { get; set; }
        public int ViewPowerID { get; set; }

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

        public object Clone()
        {
            MenuTree menu = new MenuTree
            {
                ID = ID,
                Name = Name,
                ImageUrl = ImageUrl,
                NavigateUrl = NavigateUrl,
                Remark = Remark,
                SortIndex = SortIndex,
                TreeLevel = TreeLevel,
                Enabled = Enabled,
                IsTreeLeaf = IsTreeLeaf
            };
            return menu;
        }
    }
}