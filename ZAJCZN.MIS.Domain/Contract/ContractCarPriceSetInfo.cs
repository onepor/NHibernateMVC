using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class ContractCarPriceSetInfo : BaseEntity<ContractCarPriceSetInfo>
    {
        /// <summary>
        /// 车辆编号
        /// </summary>
        [Property]
        public int CarID { get; set; }

        /// <summary>
        /// 合同ID
        /// </summary>
        [Property]
        public int ContractID { get; set; }

        /// <summary>
        /// 每吨费用
        /// </summary>
        [Property]
        public decimal TonPayPrice { get; set; }

        /// <summary>
        /// 整车费用
        /// </summary>
        [Property]
        public decimal CarPayPrice { get; set; }

        /// <summary>
        /// 保底吨数
        /// </summary>
        [Property]
        public decimal MinTon { get; set; }

        /// <summary>
        /// 车辆信息
        /// </summary>         
        public CarInfo CarInfo { get; set; }

    }
}
