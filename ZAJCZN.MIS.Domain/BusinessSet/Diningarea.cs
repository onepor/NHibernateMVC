using Castle.ActiveRecord;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 餐区信息
    /// </summary>
    [ActiveRecord]
    public partial class tm_Diningarea : BaseEntity<tm_Diningarea>
    {
        /// <summary>
        /// 餐区名称
        /// </summary>
        [Property]
        public string AreaName { get; set; }

        /// <summary>
        /// 服务费
        /// </summary>
        [Property]
        public decimal? Fee { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Property]
        public int? Sort { get; set; }

        /// <summary>
        /// 餐区对应的餐台集合
        /// </summary>
        //一对多，对应Tabie表的Diningarea_Tabie属性
        [HasMany(typeof(tm_Tabie), Table = "tm_Tabie", ColumnKey = "DiningAreaID", Cascade = ManyRelationCascadeEnum.None, Inverse = false, Lazy = false)]
        public IList<tm_Tabie> Tabie_Diningarea { get; set; }
    }
}
