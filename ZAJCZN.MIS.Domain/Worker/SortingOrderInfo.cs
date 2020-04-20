using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class SortingOrderInfo : BaseEntity<SortingOrderInfo>
    {
        /// <summary>
        /// 派工日期
        /// </summary>
        [Property]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 单号(系统)
        /// </summary>
        [Property]
        public string OrderNO { get; set; }

        /// <summary>
        /// 维修数量
        /// </summary>
        [Property]
        public decimal OrderNumber { get; set; }

        /// <summary>
        /// 工人人数
        /// </summary>
        [Property]
        public int WorkerNumber { get; set; }

        /// <summary>
        /// 工作总小时
        /// </summary>
        [Property]
        public decimal WorkeHours { get; set; }

        /// <summary>
        /// 工资
        /// </summary>
        [Property]
        public decimal WorkeSalary { get; set; }

        /// <summary>
        /// 订单说明
        /// </summary>
        [Property]
        public string Remark { get; set; }

    }
}
