using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class LossOrder : BaseEntity<LossOrder>
    {
        /// <summary>
        /// 损耗单日期
        /// </summary>
        [Property]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 损耗单号
        /// </summary>
        [Property]
        public string OrderNO { get; set; }

        /// <summary>
        /// 损耗商品数量
        /// </summary>
        [Property]
        public decimal OrderNumber { get; set; }

        /// <summary>
        /// 损耗商品金额
        /// </summary>
        [Property]
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        [Property]
        public string Operator { get; set; }

        /// <summary>
        /// 报损人
        /// </summary>
        [Property]
        public string UserName { get; set; }

        /// <summary>
        /// 报损人ID
        /// </summary>
        [Property]
        public int UserID { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        [Property]
        public int WareHouseID { get; set; }

        /// <summary>
        /// 仓库信息
        /// </summary> 
        public WareHouse WareHouseInfo { get; set; }

        /// <summary>
        /// 临时订单标志 0：正式  1：临时
        /// </summary>
        [Property]
        public int IsTemp { get; set; }

        /// <summary>
        /// 订单说明
        /// </summary>
        [Property]
        public string Remark { get; set; }
    }
}
