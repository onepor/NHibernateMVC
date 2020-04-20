using Castle.ActiveRecord;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public partial class tm_SetMealDetail : BaseEntity<tm_SetMealDetail>
    {
        /// <summary>
        /// 套餐ID
        /// </summary>
        [Property]
        public int SetMealID { get; set; }

        /// <summary>
        /// 菜品ID
        /// </summary>
        [Property]
        public int DishID { get; set; }

        /// <summary>
        /// 菜品份数
        /// </summary>
        [Property]
        public int DishCount { get; set; }

        /// <summary>
        /// 菜品单价
        /// </summary>
        [Property]
        public decimal? Price { get; set; }

        /// <summary>
        /// 菜品总价
        /// </summary>
        [Property]
        public decimal? TotalPrice { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }
    }
}
