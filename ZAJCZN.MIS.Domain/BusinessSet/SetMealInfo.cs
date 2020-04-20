using Castle.ActiveRecord;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public partial class tm_SetMealInfo : BaseEntity<tm_SetMealInfo>
    {
        /// <summary>
        /// 套餐名
        /// </summary>
        [Property]
        public string SetMealName { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        [Property]
        public decimal? Price { get; set; }

        /// <summary>
        /// 套餐优惠价
        /// </summary>
        [Property]
        public decimal? PreferentialPrice { get; set; }

        /// <summary>
        /// 创建套餐时间
        /// </summary>
        [Property]
        public string SetTime { get; set; }

        /// <summary>
        /// 套餐开始时间
        /// </summary>
        [Property]
        public string StartTime { get; set; }

        /// <summary>
        /// 套餐结束时间
        /// </summary>
        [Property]
        public string FinishTime { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Property]
        public string IsEnabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 套餐对应的菜品明细
        /// </summary>
        [HasMany(typeof(tm_SetMealDetail), Table = "tm_SetMealDetail", ColumnKey = "SetMealID", Cascade = ManyRelationCascadeEnum.None, Inverse = false, Lazy = true)]
        public IList<tm_SetMealDetail> SetMealDetailList { get; set; }
    }
}
