﻿using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class ContractOrderSecondaryDetail : BaseEntity<ContractOrderSecondaryDetail>
    {
        /// <summary>
        ///  销售单号
        /// </summary>
        [Property]
        public string OrderNO { get; set; }

        /// <summary>
        /// 销售日期
        /// </summary>
        [Property]
        public DateTime OrderDate { get; set; }

        /// <summary>
        ///  商品ID
        /// </summary>
        [Property]
        public int GoodsID { get; set; }

        /// <summary>
        ///  主材商品ID
        /// </summary>
        [Property]
        public int MainGoodsID { get; set; }

        /// <summary>
        /// 商品出\入库总数(最终库存数据)
        /// </summary>
        [Property]
        public decimal GoodsNumber { get; set; }
                
        /// <summary>
        /// 商品出\入库总数(原始数据,按主材配套系统自动计算)
        /// </summary>
        [Property]
        public decimal FormerlyGoodsNumber { get; set; }
        
        /// <summary>
        /// 赔偿总数(入库使用)
        /// </summary>
        [Property]
        public decimal PayForNumber { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        [Property]
        public decimal GoodsUnitPrice { get; set; }

        /// <summary>
        /// 商品总价
        /// 出库：要收费辅材金额
        /// 入库：赔偿金额
        /// </summary>
        [Property]
        public decimal GoodsTotalPrice { get; set; }

        /// <summary>
        /// 商品标准单位
        /// </summary>
        [Property]
        public string GoodsUnit { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        [Property]
        public int WareHouseID { get; set; }

        /// <summary> 
        /// 单据是否计算数量  1:是 0：否
        /// </summary>
        [Property]
        public int IsCalcNumber { get; set; }

        /// <summary> 
        /// 单据是否计算金额  1:是 0：否
        /// </summary>
        [Property]
        public int IsCalcPrice { get; set; }

        /// <summary> 
        /// 单据类型  1:发货 2：收货
        /// </summary>
        [Property]
        public int OrderType { get; set; }

        /// <summary> 
        /// 是否在订单辅材信息中显示  1:显示 0：不显示
        /// </summary>
        [Property]
        public int IsShow { get; set; }

        /// <summary>
        ///  主材商品信息
        /// </summary> 
        public EquipmentInfo MainGoodsInfo { get; set; }

        /// <summary>
        ///  商品信息
        /// </summary> 
        public EquipmentInfo GoodsInfo { get; set; }

        /// <summary>
        ///  主材商品发货信息
        /// </summary> 
        public ContractOrderDetail MainGoodsOrderInfo { get; set; }

    }
}
