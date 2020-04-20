using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class EquipmentInfo : BaseEntity<EquipmentInfo>
    {
        /// <summary>
        /// 类别编号
        /// </summary>
        [Property]
        public int EquipmentTypeID { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Property]
        public string EquipmentName { get; set; }

        /// <summary>
        /// 线条名称
        /// </summary>
        [Property]
        public string LineName { get; set; }

        /// <summary>
        /// 标准高
        /// </summary>
        [Property]
        public int EHeight { get; set; }

        /// <summary>
        /// 标准宽
        /// </summary>
        [Property]
        public int EWide { get; set; }

        /// <summary>
        /// 标准厚
        /// </summary>
        [Property]
        public int EThickness { get; set; }

        /// <summary>
        /// 商品单位 1:樘  2：米  3：平方米
        /// </summary>
        [Property]
        public string EquipmentUnit { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [Property]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 超高单价
        /// </summary>
        [Property]
        public decimal PassHeight { get; set; }

        /// <summary>
        /// 超宽单价
        /// </summary>
        [Property]
        public decimal PassWide { get; set; }

        /// <summary>
        /// 超厚单价 
        /// </summary>
        [Property]
        public decimal PassThckness { get; set; }

        /// <summary>
        /// 超面积单价 
        /// </summary>
        [Property]
        public decimal PassArea { get; set; }

        /// <summary>
        /// 是否启用 0：停用  1：启用（默认）
        /// </summary>
        [Property]
        public string IsUsed { get; set; }

        /// <summary>
        ///  备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 超标计算 1：按公分计算 2:单价加价  3：面积加价
        /// </summary>
        [Property]
        public int PassCalcType { get; set; }

        /// <summary>
        /// 安装运输费
        /// </summary>
        [Property]
        public decimal InstallCost { get; set; }

        /// <summary>
        ///  计算方式 1：单位数量 2:三方周长  3：四方周长 4：面积
        /// </summary>
        [Property]
        public int CalcUnitType { get; set; }

    }
}
