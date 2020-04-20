using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DTcms.DBUtility;

namespace DTcms.BLL
{
    public class BalBase
    {
        private readonly DTcms.DAL.DalBase dal = new DTcms.DAL.DalBase();

        /// <summary>
        /// 获取分页列表；需先调用 GetRecordCount 来获取总数
        /// </summary>
        /// <param name="strSelect"></param>
        /// <param name="strWhere"></param>
        /// <param name="orderBy"></param>
        /// <param name="startIndex"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        public DataTable GetListByPage_MySql(string strSelect, string strWhere, string orderBy, int startIndex, int PageCount)
        {
            return dal.GetListByPage_MySql(strSelect, strWhere, orderBy, startIndex, PageCount);
        }

        /// <summary>
        /// 事务执行数据库语句
        /// </summary>
        /// <param name="sqlList"></param>
        /// <returns></returns>
        public int ExecuteSqlTran(List<string> sqlList)
        {
            return DbHelperMySQL.ExecuteSqlTran(sqlList);
        }


        /// <summary>
        /// 生成销售订单编号(停用)
        /// </summary>
        /// <returns></returns>
        public string CreateSaleOrderNO()
        {
            string frontStr = DTcms.Helpers.ConfigHelper.GetConfigString("saleOrderNoStart");

            DateTime curTime = DateTime.Now;
            string curStr = curTime.ToString("yyyyMMddHHmmssfff");
            return frontStr + curStr;
            //return frontStr + ZHUAO.DBUtility.DbHelperMySQL.Query("call CreateSaleBillNo()").Tables[0].Rows[0][0].ToString();
        }
        /// <summary>
        /// 生成进货订单编号
        /// </summary>
        /// <returns></returns>
        public string CreateStockOrderNO()
        {
            string frontStr = DTcms.Helpers.ConfigHelper.GetConfigString("stockOrderNoStart");

            DateTime curTime = DateTime.Now;
            string curStr = curTime.ToString("yyyyMMddHHmmssfff");
            return frontStr + curStr;
            //return frontStr + ZHUAO.DBUtility.DbHelperMySQL.Query("call CreateSaleBillNo()").Tables[0].Rows[0][0].ToString();
        }
        /// <summary>
        /// 生成服务中心订单编号(停用)
        /// </summary>
        /// <returns></returns>
        public string CreateProductStockOrderNO()
        {
            string frontStr = DTcms.Helpers.ConfigHelper.GetConfigString("ProductStockOrder");

            DateTime curTime = DateTime.Now;
            string curStr = curTime.ToString("yyyyMMddHHmmssfff");
            return frontStr + curStr;
            //return frontStr + ZHUAO.DBUtility.DbHelperMySQL.Query("call CreateSaleBillNo()").Tables[0].Rows[0][0].ToString();
        }
        /// <summary>
        /// 生成退货订单编号(停用)
        /// </summary>
        /// <returns></returns>
        public string CreateReGoodsOrderNo()
        {
            string frontStr = DTcms.Helpers.ConfigHelper.GetConfigString("stockRegoodsNoStart");

            DateTime curTime = DateTime.Now;
            string curStr = curTime.ToString("yyyyMMddHHmmssfff");
            return frontStr + curStr;

        }
        /// <summary>
        /// 生成充值订单编号
        /// </summary>
        /// <returns></returns>
        public string CreatePurchaseNO()
        {
            string frontStr = DTcms.Helpers.ConfigHelper.GetConfigString("PurchaseNO");

            DateTime curTime = DateTime.Now;
            string curStr = curTime.ToString("yyyyMMddHHmmssfff");
            return frontStr + curStr;
            //return frontStr + ZHUAO.DBUtility.DbHelperMySQL.Query("call CreateSaleBillNo()").Tables[0].Rows[0][0].ToString();
        }
    }
}
