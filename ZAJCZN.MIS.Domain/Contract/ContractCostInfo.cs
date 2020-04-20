using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 项目信息
    /// </summary>
    [ActiveRecord]
    public partial class ContractCostInfo : BaseEntity<ContractCostInfo>
    {
        /// <summary>
        /// 合同信息
        /// </summary>
        [Property]
        public int ContractID { get; set; }

        /// <summary>
        /// 厂家编号
        /// </summary>
        [Property]
        public int SuppilerID { get; set; }

        /// <summary>
        /// 厂家名称
        /// </summary>
        [Property]
        public string SuppilerName { get; set; }

        /// <summary> 
        ///  类型成本  1：门  2：柜子 
        /// </summary>
        [Property]
        public int CostType { get; set; }

        /// <summary>
        /// 成本金额
        /// </summary>
        [Property]
        public decimal CostAmount { get; set; }

        /// <summary>
        /// 已付金额
        /// </summary>
        [Property]
        public decimal PayAmount { get; set; }

        /// <summary> 
        ///  厂商状态 1:申请付款 2：已付款
        /// </summary>
        [Property]
        public int SuppilerState { get; set; }

        /// <summary> 
        ///  订单生产状态 1:生产中 2：已完成
        /// </summary>
        [Property]
        public int ProduceState { get; set; }

        /// <summary>
        /// 订单生产状态备注
        /// </summary>
        [Property]
        public string ProduceRemark { get; set; }

        /// <summary> 
        ///  订单送货状态 0:未送货 1：已送货不安装  2：已送货可安装 3:待安装 4：已安装
        /// </summary>
        [Property]
        public int SendingState { get; set; }

        /// <summary>
        /// 送货人员
        /// </summary>
        [Property]
        public int SendPerson { get; set; }

        /// <summary>
        /// 安装人员
        /// </summary>
        [Property]
        public int InstallPerson { get; set; }
        
        /// <summary>
        /// 送货日期
        /// </summary>
        [Property]
        public string SendDate { get; set; }
         
        /// <summary>
        /// 安装日期
        /// </summary>
        [Property]
        public string InstalDate { get; set; }
    }
}

