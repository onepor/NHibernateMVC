using Castle.ActiveRecord;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public partial class tm_Mealtime : BaseEntity<tm_Mealtime>
    {
        /// <summary>
        /// 餐段名称
        /// </summary>
        [Property]
        public string MealsName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Property]
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Property]
        public string EndTime { get; set; }

        /// <summary>
        /// 是否次日
        /// </summary>
        [Property]
        public string IsTomorrow { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }
    }
}
