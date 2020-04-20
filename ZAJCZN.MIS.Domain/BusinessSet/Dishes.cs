using Castle.ActiveRecord;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 菜品信息
    /// </summary>
    [ActiveRecord]
    public partial class tm_Dishes : BaseEntity<tm_Dishes>
    {
        /// <summary>
        /// 菜品名称
        /// </summary>
        [Property]
        public string DishesName { get; set; }

        /// <summary>
        /// 菜品类型
        /// </summary>
        [Property]
        public int ClassID { get; set; }

        /// <summary>
        /// 拼音助记码
        /// </summary>
        [Property]
        public string DishesPY { get; set; }

        /// <summary>
        /// 菜品编号
        /// </summary>
        [Property]
        public int DishesCode { get; set; }

        /// <summary>
        /// 菜品单位
        /// </summary>
        [Property]
        public int DishesUnit { get; set; }

        /// <summary>
        /// 菜品售价 
        /// </summary>
        [Property]
        public decimal SellPrice { get; set; }

        /// <summary>
        /// 菜品会员价
        /// </summary>
        [Property]
        public decimal MemberPrice { get; set; }

        /// <summary>
        /// 菜品显示顺序
        /// </summary>
        [Property]
        public int ShowIndex { get; set; }

        /// <summary>
        /// 菜品图片
        /// </summary>
        [Property]
        public string DishesPicture { get; set; }

        /// <summary>
        /// 是否参与积分
        /// </summary>
        [Property]
        public int IsScore { get; set; }

        /// <summary>
        /// 是否参与折扣
        /// </summary>
        [Property]
        public int IsDiscount { get; set; }

        /// <summary>
        /// 是否参与排名
        /// </summary>
        [Property]
        public int IsRanking { get; set; }

        /// <summary>
        /// 菜品是否称重
        /// </summary>
        [Property]
        public int IsWeigh { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Property]
        public int IsUsed { get; set; }

        // <summary>
        /// 打印机ID
        /// </summary>
        [Property]
        public int PrinterID { get; set; }
        
        ///// <summary>
        ///// 菜品对于的配料信息
        ///// </summary>
        ////一对多，对应tm_DishesBatching表的DishesID属性
        //[HasMany(typeof(tm_DishesBatching), Table = "tm_DishesBatching", ColumnKey = "DishesID", Cascade = ManyRelationCascadeEnum.None, Inverse = false, Lazy = false)]
        //public IList<tm_DishesBatching> DishesBatchingList { get; set; }

    }
}

