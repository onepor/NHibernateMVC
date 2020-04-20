using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 发货单发货重量信息
    /// </summary>
    [ActiveRecord]
    public partial class ContractOrderWeightInfo : BaseEntity<ContractOrderWeightInfo>
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Property]
        public string OrderNO { get; set; }

        /// <summary>
        /// 材料种类ID
        /// </summary>
        [Property]
        public int GoodsTypeID { get; set; }

        /// <summary>
        /// 材料种类名称
        /// </summary>
        [Property]
        public string GoodsTypeName { get; set; }

        /// <summary>
        /// 计费单位
        /// </summary>
        [Property]
        public string TypeUnit { get; set; }

        /// <summary>
        /// 计费数量
        /// </summary>
        [Property]
        public decimal CalcNumber { get; set; }

        /// <summary>
        /// 客户重量
        /// </summary>
        [Property]
        public decimal CustWeight { get; set; }

        /// <summary>
        /// 司机重量
        /// </summary>
        [Property]
        public decimal DriverWeight { get; set; }

        /// <summary>
        /// 员工重量
        /// </summary>
        [Property]
        public decimal StaffWeight { get; set; }


    }
}

