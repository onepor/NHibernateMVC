using Castle.ActiveRecord;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public partial class tm_TabieDishesInfo : BaseEntity<tm_TabieDishesInfo>
    {

        /// <summary>
        /// 使用情况id
        /// </summary>
        [Property]
        public int TabieUsingID { get; set; }

        /// <summary>
        /// 菜品id
        /// </summary>
        [Property]
        public int DishesID { get; set; }

        /// <summary>
        /// 菜品份数
        /// </summary>
        [Property]
        public decimal DishesCount { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [Property]
        public decimal Price { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Property]
        public decimal Moneys { get; set; }

        /// <summary>
        /// 菜品类型 1:正常  2：退菜 3:团购菜品
        /// </summary>
        [Property]
        public string DishesType { get; set; }

        /// <summary>
        /// 是否赠送
        /// </summary>
        [Property]
        public string IsFree { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [Property]
        public string UnitName { get; set; }

        /// <summary>
        /// 菜品名称
        /// </summary>
        [Property]
        public string DishesName { get; set; }

        /// <summary>
        /// 打印机Id
        /// </summary>
        [Property]
        public int PrintID { get; set; }

        /// <summary>
        /// 是否已打印
        /// </summary>
        [Property]
        public int IsPrint { get; set; }

        /// <summary>
        /// 是否打折
        /// </summary>
        [Property]
        public int IsDiscount { get; set; }
    }
}
