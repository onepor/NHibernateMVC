using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 车辆信息
    /// </summary>

    [ActiveRecord]
    public partial class CarInfo : BaseEntity<CarInfo>
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        [Property]
        public string CarNO { get; set; }

        /// <summary>
        /// 载重
        /// </summary>
        [Property]
        public string CarLoad { get; set; }

        /// <summary>
        /// 司机姓名
        /// </summary>
        [Property]
        public string DriverName { get; set; }

        /// <summary>
        /// 计费类型	1:按单位  2：整车  
        /// </summary>
        [Property]
        public string ChargingType { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Property]
        public string ContractPhone { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        [Property]
        public string ContractAddress { get; set; }

        /// <summary>
        /// 状态 0：停用  1：启用
        /// </summary>
        [Property]
        public string IsUsed { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 计费单价
        /// </summary>
        [Property]
        public decimal PayPrice { get; set; }
        
        /// <summary>
        /// 是否收费
        /// </summary>
        [Property]
        public int IsCalcPeice { get; set; }
 
    }
}

