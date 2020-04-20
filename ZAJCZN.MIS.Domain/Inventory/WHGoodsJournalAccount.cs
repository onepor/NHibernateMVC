using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class WHGoodsJournalAccount : BaseEntity<WHGoodsJournalAccount>
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        [Property]
        public int GoodsID { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Property]
        public string GoodsName { get; set; }

        /// <summary>
        ///  商品编码
        /// </summary>
        [Property]
        public string GoodsCode { get; set; }

        /// <summary>
        /// 库房ID
        /// </summary>
        [Property]
        public int WareHouseID { get; set; }

        /// <summary>
        /// 业务单号
        /// </summary>
        [Property]
        public string OrderNO { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        [Property]
        public string OrderType { get; set; }

        /// <summary>
        /// 入库\出库标志 1:进 2：出
        /// </summary>
        [Property]
        public int InOutFlag { get; set; }

        /// <summary>
        /// 期初库存数量
        /// </summary>
        [Property]
        public decimal BeginNumber { get; set; }

        /// <summary>
        /// 期初库存单价
        /// </summary>
        [Property]
        public decimal BeginUnitPrice { get; set; }

        /// <summary>
        /// 期初库存金额
        /// </summary>
        [Property]
        public decimal BeginAmount { get; set; }

        /// <summary>
        /// 发生库存数量
        /// </summary>
        [Property]
        public decimal OccurNumber { get; set; }

        /// <summary>
        /// 发生库存单价
        /// </summary>
        [Property]
        public decimal OccurUnitPrice { get; set; }

        /// <summary>
        /// 发生库存金额
        /// </summary>
        [Property]
        public decimal OccurAmount { get; set; }

        /// <summary>
        /// 期末库存数量
        /// </summary>
        [Property]
        public decimal TerminalNumber { get; set; }

        /// <summary>
        /// 期末库存单价
        /// </summary>
        [Property]
        public decimal TerminalUnitPrice { get; set; }

        /// <summary>
        /// 期末库存金额
        /// </summary>
        [Property]
        public decimal TerminalAmount { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        [Property]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 入库批次号
        /// </summary>
        [Property]
        public string BatchNO { get; set; }
    }
}
