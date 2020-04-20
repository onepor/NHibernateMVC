using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class PriceSetGoodsInfo : BaseEntity<PriceSetGoodsInfo>
    {
        /// <summary>
        /// 价格套系编号
        /// </summary>
        [Property]
        public int SetID { get; set; }

        /// <summary>
        /// 价格套系
        /// </summary>
        [Property]
        public string SetDate { get; set; }

        /// <summary>
        ///  商品ID
        /// </summary>
        [Property]
        public int EquipmentID { get; set; }

        /// <summary>
        ///  商品类别ID
        /// </summary>
        [Property]
        public int GoodsTypeID { get; set; }

        /// <summary>
        /// 日租金
        /// </summary>
        [Property]
        public decimal DailyRents { get; set; }

        /// <summary>
        /// 单价（丢赔、折算、销售）
        /// </summary>
        [Property]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 吨位换算(客户)
        /// </summary>
        [Property]
        public decimal CustomerUnit { get; set; }

        /// <summary>
        /// 吨位换算(司机)
        /// </summary>
        [Property]
        public decimal DriverUnit { get; set; }

        /// <summary>
        /// 吨位换算(员工)
        /// </summary>
        [Property]
        public decimal StaffUnit { get; set; }

        /// <summary>
        /// 起租天数
        /// </summary>
        [Property]
        public int MinRentingDays { get; set; }

        /// <summary>
        /// 维修价格
        /// </summary>
        [Property]
        public decimal FixPrice { get; set; }

        /// <summary>
        /// 物品类型标识  1：类别  2：物品
        /// </summary>
        [Property]
        public int IsGoodType { get; set; }

        /// <summary>
        ///  商品信息
        /// </summary> 
        public EquipmentInfo GoodsInfo { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 商品规格
        /// </summary>
        public decimal Standard { get; set; }

        /// <summary>
        /// 商品单位
        /// </summary>
        public string EquipmentUnit { get; set; }

    }
}
