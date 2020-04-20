using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class EquipmentAssortInfo : BaseEntity<EquipmentAssortInfo>
    {
        /// <summary>
        /// 主器材编号
        /// </summary>
        [Property]
        public int ParentEquipmentID { get; set; }

        /// <summary>
        ///  器材数量
        /// </summary>
        [Property]
        public decimal EquipmentCount { get; set; }

        /// <summary>
        ///  配套器材数量
        /// </summary>
        [Property]
        public decimal AssortCount { get; set; }

        /// <summary>
        ///  类型  1：器材  2：规格
        /// </summary>
        [Property]
        public string EquipmentType { get; set; }

        /// <summary>
        ///  备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 出库是否计算数量 0：不计算  1：计算（默认）
        /// </summary>
        [Property]
        public int IsOutCalcNumber { get; set; }

        /// <summary>
        /// 入库是否计算数量 0：不计算  1：计算（默认）
        /// </summary>
        [Property]
        public int IsInCalcNumber { get; set; }

        /// <summary>
        /// 出库是否计算金额 0：不计算  1：计算（默认）
        /// </summary>
        [Property]
        public int IsOutCalcPrice { get; set; }

        /// <summary>
        /// 入库是否计算金额 0：不计算  1：计算（默认）
        /// </summary>
        [Property]
        public int IsInCalcPrice { get; set; }

        /// <summary>
        /// 辅助器材编号
        /// </summary>
        [Property]
        public int EquipmentID { get; set; }

        /// <summary>
        /// 器材信息
        /// </summary> 
        public EquipmentInfo EquipmentInfo { get; set; }

    }
}
