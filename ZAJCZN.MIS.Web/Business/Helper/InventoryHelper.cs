using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Data.Entity;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using NHibernate.Criterion;
using ZAJCZN.MIS.Helpers;
using System.Data;

namespace ZAJCZN.MIS.Web
{
    public class InventoryHelper
    {
        #region 库房商品库存更新

        /// <summary>
        /// 更新商品库存信息
        /// </summary>
        /// <param name="wareHouseID">库房ID</param>
        /// <param name="goodsID">商品ID</param>
        /// <param name="goodsCount">商品数量(入库为正数，出库为负数)</param>
        /// <param name="goodsCount">商品单价</param>
        /// <param name="goodsAmount">进货金额(入库为正数，出库为负数)</param>
        /// <param name="isUnitChange">是否进行单位换算</param>
        public void UpdateWareHouseStock(int wareHouseID, int goodsID, decimal goodsCount, decimal goodsPrice, decimal goodsAmount, int isUnitChange)
        {
            //根据库房编号和商品编号获取库存信息
            IList<ICriterion> qryListDetail = new List<ICriterion>();
            qryListDetail.Add(Expression.Eq("GoodsID", goodsID));
            qryListDetail.Add(Expression.Eq("WareHouseID", wareHouseID));
            WHGoodsDetail whGoodsInfo = Core.Container.Instance.Resolve<IServiceWHGoodsDetail>().GetEntityByFields(qryListDetail);

            //获取商品信息
            EquipmentInfo goodsInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(goodsID);
            if (goodsInfo != null)
            {
                //如果有商品库存信息，更新数量、金额和单价
                if (whGoodsInfo != null)
                {
                    whGoodsInfo.InventoryCount += goodsCount;
                    whGoodsInfo.InventoryAmount += goodsAmount;
                    if (whGoodsInfo.InventoryCount > 0)
                    {
                        whGoodsInfo.InventoryUnitPrice = Math.Round(whGoodsInfo.InventoryAmount / whGoodsInfo.InventoryCount, 2);
                    }
                    else
                    {
                        whGoodsInfo.InventoryUnitPrice = 0;
                    }
                    Core.Container.Instance.Resolve<IServiceWHGoodsDetail>().Update(whGoodsInfo);
                }
                else
                {
                    //如果库房没有商品信息，新增商品
                    whGoodsInfo = new WHGoodsDetail();
                    whGoodsInfo.InventoryUnit = goodsInfo.EquipmentUnit;
                    whGoodsInfo.InventoryCount += goodsCount;
                    whGoodsInfo.InventoryAmount = goodsAmount;
                    whGoodsInfo.InventoryUnitPrice = Math.Round(whGoodsInfo.InventoryAmount / goodsCount, 2);
                   // whGoodsInfo.GoodsCode = goodsInfo.EquipmentCode;
                    whGoodsInfo.GoodsID = goodsID;
                    whGoodsInfo.GoodsTypeID = goodsInfo.EquipmentTypeID;
                    whGoodsInfo.GoodsName = goodsInfo.EquipmentName;
                    whGoodsInfo.WareHouseID = wareHouseID;

                    Core.Container.Instance.Resolve<IServiceWHGoodsDetail>().Create(whGoodsInfo);
                }
            }
        }

        #endregion 库房商品库存更新

        #region 库房商品库存变动明细维护

        /// <summary>
        /// 更新商品变动明细信息
        /// </summary>
        /// <param name="wareHouseID">库房ID</param>
        /// <param name="goodsID">商品ID</param>
        /// <param name="orderNO">业务单号</param>
        /// <param name="orderType">业务类型</param>
        /// <param name="inOutFlag">进出库标志 1：入库 2：出库</param>
        /// <param name="goodsCount">商品数量(入库为正数，出库为负数)</param>
        /// <param name="goodsCount">商品单价</param>
        /// <param name="goodsAmount">进货金额(入库为正数，出库为负数)</param>
        /// <param name="batchNO">进货批次号</param>
        public void UpdateGoodsJournal(int wareHouseID, int goodsID, string orderNO, string orderType, int inOutFlag
                                        , decimal goodsCount, decimal goodsPrice, decimal goodsAmount, string batchNO
                                        , DateTime OrderDate)
        {
            //根据库房编号和商品编号获取最近一条变动信息
            IList<ICriterion> qryListDetail = new List<ICriterion>();
            qryListDetail.Add(Expression.Eq("GoodsID", goodsID));
            qryListDetail.Add(Expression.Eq("WareHouseID", wareHouseID));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", false);
            orderList[0] = orderli;
            WHGoodsJournalAccount whGoodsInfo = Core.Container.Instance.Resolve<IServiceWHGoodsJournalAccount>().GetFirstEntityByFields(qryListDetail, orderList);

            //获取商品信息
            EquipmentInfo goodsInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(goodsID);
            if (goodsInfo != null)
            {
                WHGoodsJournalAccount currentInfo = new WHGoodsJournalAccount();
                //currentInfo.GoodsCode = goodsInfo.EquipmentCode;
                currentInfo.GoodsID = goodsID;
                currentInfo.GoodsName = goodsInfo.EquipmentName;
                currentInfo.InOutFlag = inOutFlag;
                currentInfo.OrderNO = orderNO;
                currentInfo.OrderType = orderType;
                currentInfo.WareHouseID = wareHouseID;
                currentInfo.UpdateDate = OrderDate;
                currentInfo.BatchNO = batchNO;
                //如果有历史商品变动信息，获取期初数量、金额和单价
                if (whGoodsInfo != null)
                {
                    //期初数
                    currentInfo.BeginAmount = whGoodsInfo.TerminalAmount;
                    currentInfo.BeginNumber = whGoodsInfo.TerminalNumber;
                    currentInfo.BeginUnitPrice = whGoodsInfo.TerminalUnitPrice;
                    //发生数
                    currentInfo.OccurAmount = goodsAmount;
                    currentInfo.OccurNumber = goodsCount;
                    currentInfo.OccurUnitPrice = Math.Round(currentInfo.OccurAmount / currentInfo.OccurNumber, 2);
                    //期末数
                    currentInfo.TerminalAmount = whGoodsInfo.TerminalAmount + goodsAmount;
                    currentInfo.TerminalNumber = whGoodsInfo.TerminalNumber + goodsCount;
                    currentInfo.TerminalUnitPrice = 0;
                    if (currentInfo.TerminalNumber > 0)
                    {
                        currentInfo.TerminalUnitPrice = Math.Round(currentInfo.TerminalAmount / currentInfo.TerminalNumber, 2);
                    }
                }
                else
                {
                    //期初数
                    currentInfo.BeginAmount = 0;
                    currentInfo.BeginNumber = 0;
                    currentInfo.BeginUnitPrice = 0;
                    //发生数
                    currentInfo.OccurAmount = goodsAmount;
                    currentInfo.OccurNumber = goodsCount;
                    currentInfo.OccurUnitPrice = Math.Round(currentInfo.OccurAmount / currentInfo.OccurNumber, 2);
                    //期末数
                    currentInfo.TerminalAmount = goodsAmount;
                    currentInfo.TerminalNumber = goodsCount;
                    currentInfo.TerminalUnitPrice = 0;
                    if (currentInfo.TerminalNumber > 0)
                    {
                        currentInfo.TerminalUnitPrice = Math.Round(currentInfo.TerminalAmount / currentInfo.TerminalNumber, 2);
                    }
                }
                Core.Container.Instance.Resolve<IServiceWHGoodsJournalAccount>().Create(currentInfo);
            }
        }

        #endregion 库房商品库存变动明细维护

        ///// <summary>
        ///// 清空配料消耗临时表
        ///// </summary>
        //public void DeleteTempDishesBatching()
        //{
        //    string sql = string.Format("delete from tm_dinnerTemp");
        //    DbHelperSQL.ExecuteSql(sql);
        //}


        /// <summary>
        /// 根据订单号，获取发货单中所有材料的出库仓库信息
        /// </summary>
        /// <param name="orderNO">发货单订单号</param>
        /// <ret列表urns>出库仓库ID列表</returns>
        public List<int> GetWHInfo(string orderNO)
        {
            List<int> listWH = new List<int>();

            string sql = string.Format(" select WareHouseID from ContractOrderDetail where OrderNO='{0}' union select WareHouseID from ContractOrderSecondaryDetail where OrderNO='{0}'", orderNO);
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0] != null)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    listWH.Add(int.Parse(row["WareHouseID"].ToString()));
                }
            }
            return listWH;
        }
    }
}
