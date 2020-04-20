using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class ContractDoorInfo : BaseEntity<ContractDoorInfo>
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
        public string GoodsLocation { get; set; }

        /// <summary>
        ///  商品编号
        /// </summary>
        [Property]
        public int GoodsID { get; set; }

        /// <summary>
        ///  商品名称
        /// </summary>
        [Property]
        public string GoodsName { get; set; }

        /// <summary>
        ///  商品型号
        /// </summary>
        [Property]
        public string ModelName { get; set; }

        /// <summary>
        ///  材料类型编号
        /// </summary>
        [Property]
        public int TypeClass { get; set; }

        /// <summary>
        ///  类别名称
        /// </summary>
        [Property]
        public string TypeName { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        [Property]
        public int GHeight { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        [Property]
        public int GWide { get; set; }

        /// <summary>
        /// 厚
        /// </summary>
        [Property]
        public int GThickness { get; set; }

        /// <summary>
        /// 超高
        /// </summary>
        [Property]
        public int GPassHeight { get; set; }

        /// <summary>
        /// 超宽
        /// </summary>
        [Property]
        public int GPassWide { get; set; }

        /// <summary>
        /// 超厚
        /// </summary>
        [Property]
        public int GPassThickness { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        [Property]
        public decimal GArea { get; set; }

        /// <summary>
        /// 超面积
        /// </summary>
        [Property]
        public decimal GPassArea { get; set; }

        /// <summary>
        /// 周长
        /// </summary>
        [Property]
        public decimal GPerimeter { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        [Property]
        public decimal OrderNumber { get; set; }

        /// <summary>
        /// 成交单价
        /// </summary>
        [Property]
        public decimal GPrice { get; set; }

        /// <summary>
        /// 标准单价
        /// </summary>
        [Property]
        public decimal GStandardPrice { get; set; }

        /// <summary>
        /// 单价加
        /// </summary>
        [Property]
        public decimal PassPriceAmount { get; set; }

        /// <summary>
        /// 其他金额
        /// </summary>
        [Property]
        public decimal OtherAmount { get; set; }

        /// <summary>
        /// 商品标准定价
        /// </summary>
        [Property]
        public decimal GoodsAmount { get; set; }

        /// <summary>
        /// 超高价
        /// </summary>
        [Property]
        public decimal GPassHeightAmount { get; set; }

        /// <summary>
        /// 超宽价
        /// </summary>
        [Property]
        public decimal GPassWideAmount { get; set; }

        /// <summary>
        /// 超厚价
        /// </summary>
        [Property]
        public decimal GPassThicknessAmount { get; set; }

        /// <summary>
        /// 超面积价
        /// </summary>
        [Property]
        public decimal GPassAreaAmount { get; set; }

        /// <summary>
        /// 商品总金额
        /// </summary>
        [Property]
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 测量员工编号
        /// </summary>
        [Property]
        public int MOperatorID { get; set; }

        /// <summary>
        /// 测量员工名称
        /// </summary>
        [Property]
        public string MOperatorName { get; set; }

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
        /// 锁编号
        /// </summary>
        [Property]
        public int LockID { get; set; }

        /// <summary>
        /// 安装运费
        /// </summary>
        [Property]
        public decimal InstallCost { get; set; }

        /// <summary>
        /// 商品单位
        /// </summary>
        [Property]
        public string GoodUnit { get; set; }

        /// <summary>
        /// 门颜色
        /// </summary>
        [Property]
        public string DoorColor { get; set; }

        /// <summary>
        /// 五金费用
        /// </summary>
        [Property]
        public decimal HardWareAmount { get; set; }
                
        /// <summary>
        /// 超标加价
        /// </summary>
        [Property]
        public decimal PassAmount { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        [Property]
        public int SupplyID { get; set; }
                
        /// <summary>
        /// 门方向
        /// </summary>
        [Property]
        public string DoorDirection { get; set; }

        /// <summary>
        /// 线条说明
        /// </summary>
        [Property]
        public string LineName { get; set; }

        /// <summary>
        /// 玻璃款式
        /// </summary>
        [Property]
        public string GlassRemark { get; set; }


    }
}
