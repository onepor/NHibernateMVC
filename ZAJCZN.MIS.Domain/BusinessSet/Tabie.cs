using Castle.ActiveRecord;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public partial class tm_Tabie : BaseEntity<tm_Tabie>
    {
        /// <summary>
        /// 餐区
        /// </summary>
        //多对一，对应Diningarea的Tabie_Diningarea属性
        [BelongsTo(Lazy = FetchWhen.OnInvoke, Column = "DiningAreaID")]
        public tm_Diningarea Diningarea_Tabie { get; set; }

        /// <summary>
        /// 餐台名称
        /// </summary>
        [Property]
        public string TabieName { get; set; }

        /// <summary>
        /// 餐台编号
        /// </summary>
        [Property]
        public string TabieNumber { get; set; }

        /// <summary>
        /// 销售模式
        /// </summary>
        [Property]
        public string SalesModel { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Property]
        public int Sort { get; set; }

        /// <summary>
        /// 餐台状态 1.没开 2.已开 3.叫菜  4：就餐
        /// </summary>
        [Property]
        public int TabieState { get; set; }
 
        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 开台编号
        /// </summary>
        [Property]
        public int CurrentUsingID { get; set; }
    }
}
