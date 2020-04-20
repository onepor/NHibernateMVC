using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 异常原因信息
    /// </summary>
    [ActiveRecord]
    public partial class Tm_Enum : BaseEntity<Tm_Enum>
    {
        /// <summary>
        /// 枚举键
        /// </summary>
        [Property]
        public string EnumKey { get; set; }

         /// <summary>
        /// 枚举值
        /// </summary>
        [Property]
        public string EnumValue { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [Property]
        public int ShowIndex { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 类别编号
        /// </summary>
        [Property]
        public string EnumTypeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //多对一，对应Tm_EnumType的ID属性
        [BelongsTo(Lazy = FetchWhen.OnInvoke, Column = "EnumTypeID")]
        public Tm_EnumType EnumTypeInfo { get; set; }
    }
}

