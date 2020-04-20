using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class tm_whgoodsorderbatch : BaseEntity<tm_whgoodsorderbatch>
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
        /// 入库数量
        /// </summary>
        [Property]
        public decimal OrderNumber { get; set; }

        /// <summary>
        /// 入库单价
        /// </summary>
        [Property]
        public decimal OrderUnitPrice { get; set; }

        /// <summary>
        /// 入库金额
        /// </summary>
        [Property]
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 当前数量
        /// </summary>
        [Property]
        public decimal CurrentNumber { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        [Property]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 入库批次号
        /// </summary>
        [Property]
        public string BatchNO { get; set; }
    }
}
