using Castle.ActiveRecord;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class tm_FoodClass : BaseEntity<tm_FoodClass>
    {
        /// <summary>
        /// 菜名类别名称
        /// </summary>
        [Property]
        public string ClassName { get; set; }

        /// <summary>
        /// 菜品单位
        /// </summary>
        [Property]
        public string Unit { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Property]
        public int? SortIndex { get; set; }

        /// <summary>
        ///  所属菜品大类
        /// </summary>
        [Property]
        public int? ParentID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Property]
        public int IsUsed { get; set; }

        /// <summary>
        /// 打印机ID
        /// </summary>
        [Property]
        public int PrintID { get; set; }

        /// <summary>
        /// 出库库房ID
        /// </summary>
        [Property]
        public int WareHouseID { get; set; }
    }
}
