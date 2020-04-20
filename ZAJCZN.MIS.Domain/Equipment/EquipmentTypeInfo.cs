using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class EquipmentTypeInfo : BaseEntity<EquipmentTypeInfo>
    {
        /// <summary>
        /// 类别名称
        /// </summary>
        [Property]
        public string TypeName { get; set; }

        /// <summary>
        /// 是否启用	0：停用  1：启用（默认）
        /// </summary>
        [Property]
        public string IsUsed { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 类别类型 1:实木门  2：合金门 3：其他
        /// </summary>
        [Property]
        public string TypeClass { get; set; }

    }
}
