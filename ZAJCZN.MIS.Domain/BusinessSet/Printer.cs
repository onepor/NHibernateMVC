using Castle.ActiveRecord;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 打印机信息
    /// </summary>
    [ActiveRecord]
    public partial class tm_Printer : BaseEntity<tm_Printer>
    {
        /// <summary>
        /// 打印机名称
        /// </summary>
        [Property]
        public string PrinterName { get; set; }

        /// <summary>
        /// 打印机类型  1：前台  2：后厨
        /// </summary>
        [Property]
        public string PrinterType { get; set; }

        /// <summary>
        /// 是否蜂鸣
        /// </summary>
        [Property]
        public string IsSinging { get; set; }

        /// <summary>
        /// 纸张宽度
        /// </summary>
        [Property]
        public string Width { get; set; }

        /// <summary>
        /// 打印机端口
        /// </summary>
        [Property]
        public int Port { get; set; }

        /// <summary>
        /// 打印机IP
        /// </summary>
        [Property]
        public string IP { get; set; }

        /// <summary>
        /// 打印机位置
        /// </summary>
        [Property]
        public string Address { get; set; }

        /// <summary>
        /// 是否开钱箱
        /// </summary>
        [Property]
        public string IsOpenCashbox { get; set; }

        /// <summary>
        /// 是否打印标签
        /// </summary>
        [Property]
        public string IsPrintslabels { get; set; }

        /// <summary>
        /// 打印机配置
        /// </summary>
        [Property]
        public string Deploy { get; set; }

        /// <summary>
        /// 打印机序列号
        /// </summary>
        [Property]
        public string SerialNumber { get; set; }

    }
}

