using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 项目信息
    /// </summary>
    [ActiveRecord]
    public partial class ContractInfo : BaseEntity<ContractInfo>
    {
        /// <summary>
        /// 客户地址名称
        /// </summary>
        [Property]
        public string ProjectName { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        [Property]
        public string ContractNO { get; set; }

        /// <summary>
        /// 合同签订时间
        /// </summary>
        [Property]
        public string ContractDate { get; set; }

        /// <summary> 
        /// 客户名称
        /// </summary>
        [Property]
        public string CustomerName { get; set; }

        /// <summary>
        /// 客户联系电话
        /// </summary>
        [Property]
        public string ContactPhone { get; set; }

        /// <summary> 
        /// 测量日期
        /// </summary>
        [Property]
        public string MeasureDate { get; set; }

        /// <summary>
        /// 送货日期
        /// </summary>
        [Property]
        public string SendDate { get; set; }

        /// <summary>
        /// 预约送货日期
        /// </summary>
        [Property]
        public string PerSendDate { get; set; }

        /// <summary>
        /// 安装日期
        /// </summary>
        [Property]
        public string InstalDate { get; set; }

        /// <summary>
        /// 预约安装日期
        /// </summary>
        [Property]
        public string PerInstalDate { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        [Property]
        public string FinishDate { get; set; }

        /// <summary>
        /// 合同金额(门)
        /// </summary>
        [Property]
        public decimal? DoorAmount { get; set; }

        /// <summary>
        /// 合同金额（柜子)
        /// </summary>
        [Property]
        public decimal? CabinetAmount { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        [Property]
        public decimal? DiscountAmount { get; set; }

        /// <summary>
        /// 合同金额
        /// </summary>
        [Property]
        public decimal? TotalAmount { get; set; }

        /// <summary>
        /// 待收金额(不用)
        /// </summary>
        [Property]
        public decimal? WaitingPaymentMoney { get; set; }

        /// <summary>
        /// 门成本
        /// </summary>
        [Property]
        public decimal DoorCost { get; set; }

        /// <summary>
        /// 柜子成本
        /// </summary>
        [Property]
        public decimal CabinetCost { get; set; }

        /// <summary>
        /// 运费成本
        /// </summary>
        [Property]
        public decimal SendCost { get; set; }

        /// <summary>
        /// 五金成本
        /// </summary>
        [Property]
        public decimal HandWareCost { get; set; }

        /// <summary>
        /// 其他成本(不用)
        /// </summary>
        [Property]
        public decimal? OtherCost { get; set; }

        /// <summary>
        /// 售后成本
        /// </summary>
        [Property]
        public decimal? AfterSaleCost { get; set; }

        /// <summary>
        /// 利润
        /// </summary>
        [Property]
        public decimal? ProfitMoney { get; set; }
        
        /// <summary>
        /// 回款金额
        /// </summary>
        [Property]
        public decimal ReturnMoney { get; set; }

        /// <summary>
        /// 已付成本
        /// </summary>
        [Property]
        public decimal PayCostMoney { get; set; }

        /// <summary>    
        /// 项目进度情况 0:登记中 1:已付定金 2:待测量 3:测量完成 4:生产中 5:生产完成  
        /// 6：送货中 7：送货完成 8：待安装 9：安装完成  10:质保中 11:售后中 12:质保结束 
        /// </summary>
        [Property]
        public int ContractState { get; set; }

        /// <summary>
        /// 质保时间
        /// </summary>
        [Property]
        public int QualityYear { get; set; }

        /// <summary>
        /// 项目备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 是否紧急  0:否 1：是
        /// </summary>
        [Property]
        public int IsUrgent { get; set; }

        /// <summary>
        /// 销售人员
        /// </summary>
        [Property]
        public int SalePerson { get; set; }

        /// <summary>
        /// 测量人员
        /// </summary>
        [Property]
        public int MeasurePerson { get; set; }

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
        /// 登记人员
        /// </summary>
        [Property]
        public string Operator { get; set; }

        /// <summary>
        /// 项目对应的付款记录
        /// </summary>
        [HasMany(typeof(ReceivablesInfo), Table = "ReceivablesInfo", ColumnKey = "ContractID", Cascade = ManyRelationCascadeEnum.None, Inverse = false, Lazy = true)]
        public IList<ReceivablesInfo> ReceivablesList { get; set; }

        /// <summary>
        /// 合同对应的订单
        /// </summary>
        [HasMany(typeof(ContractCabinetInfo), Table = "ContractCabinetInfo", ColumnKey = "ContractID", Cascade = ManyRelationCascadeEnum.None, Inverse = false, Lazy = true)]
        public IList<ContractCabinetInfo> ContractCabinetInfoList { get; set; }

        /// <summary>
        /// 合同对应的订单
        /// </summary>
        [HasMany(typeof(ContractDoorInfo), Table = "ContractDoorInfo", ColumnKey = "ContractID", Cascade = ManyRelationCascadeEnum.None, Inverse = false, Lazy = true)]
        public IList<ContractDoorInfo> ContractDoorInfoList { get; set; }

        /// <summary>
        /// 合同对应的附件文件
        /// </summary>
        [HasMany(typeof(ContractFiles), Table = "ContractFiles", ColumnKey = "ContractID", Cascade = ManyRelationCascadeEnum.None, Inverse = false, Lazy = true)]
        public IList<ContractFiles> ContractFilesList { get; set; }

        ///// <summary>
        ///// 项目对应的结算记录
        ///// </summary>
        //[HasMany(typeof(ProjectStaffInfo), Table = "ProjectStaffInfo", ColumnKey = "ProjectID", Cascade = ManyRelationCascadeEnum.None, Inverse = false, Lazy = true)]
        //public IList<ProjectStaffInfo> projectStaffInfos_ProjectInfo { get; set; }

    }
}

