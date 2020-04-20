using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class tm_GoodsType : BaseEntity<tm_GoodsType>
    {
        /// <summary>
        /// 商品类别名称
        /// </summary>
        [Property]
        public string TypeName { get; set; }

        /// <summary>
        /// 商品类别编号
        /// </summary>
        [Property]
        public string Typecode { get; set; }      

        /// <summary>
        ///  所属商品类别大类
        /// </summary>
        [Property]
        public int? ParentID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Property]
        public int IsUsed { get; set; }

        /// <summary>
        /// 是否参与成本核算
        /// </summary>
        [Property]
        public int IsCalc { get; set; }
    }
}
