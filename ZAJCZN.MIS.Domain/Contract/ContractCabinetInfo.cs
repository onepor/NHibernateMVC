using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class ContractCabinetInfo : BaseEntity<ContractCabinetInfo>
    {
        /// <summary>
        /// 合同信息
        /// </summary>
        //多对一，对应ContractInfo的ID属性
        [BelongsTo(Lazy = FetchWhen.OnInvoke, Column = "ContractID")]
        public ContractInfo ContractInfo { get; set; }

        /// <summary>
        ///  柜体位置
        /// </summary>
        [Property]
        public string GoodsType { get; set; }

        /// <summary>
        ///  商品名称
        /// </summary>
        [Property]
        public string GoodsName { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        [Property]
        public decimal GHeight { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        [Property]
        public decimal GWide { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        [Property]
        public decimal OrderNumber { get; set; }

        /// <summary>
        /// 商品金额
        /// </summary>
        [Property]
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        [Property]
        public decimal GArea { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [Property]
        public decimal GPrice { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        [Property]
        public string OperatorName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 柜体颜色
        /// </summary>
        [Property]
        public string GColorOne { get; set; }

        /// <summary>
        /// 柜门颜色
        /// </summary>
        [Property]
        public string GColorTwo { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        [Property]
        public int SupplyID { get; set; }

    }
}
