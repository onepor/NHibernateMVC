using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 异常原因信息
    /// </summary>
    [ActiveRecord]
    public partial class Tm_EnumType : BaseEntity<Tm_EnumType>
    {
        /// <summary>
        /// 枚举类型名称编号
        /// </summary>
        [Property]
        public string TypeCode { get; set; }

        /// <summary>
        /// 枚举类型名称
        /// </summary>
        [Property]
        public string TypeName { get; set; }

        /// <summary>
        /// 枚举项集合
        /// </summary>
        //一对多，对应Tm_Enum表的EnumTypeID属性
        [HasMany(typeof(Tm_Enum), Table = "Tm_Enum", ColumnKey = "EnumTypeID", Cascade = ManyRelationCascadeEnum.None, Inverse = false, Lazy = false)]
        public IList<Tm_Enum> EnumList { get; set; }

    }
}

