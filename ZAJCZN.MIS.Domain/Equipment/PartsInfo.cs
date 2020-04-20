using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class PartsInfo : BaseEntity<PartsInfo>
    {
        /// <summary>
        /// 类别编号
        /// </summary>
        [Property]
        public int PartsTypeID { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Property]
        public string PartsName { get; set; }

        /// <summary>
        /// 商品单位  
        /// </summary>
        [Property]
        public string PartsUnit { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [Property]
        public decimal UnitPrice { get; set; }

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
        /// 成本单价
        /// </summary>
        [Property]
        public decimal CostPrice { get; set; }


    }
}
