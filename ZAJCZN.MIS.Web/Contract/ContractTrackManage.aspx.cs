using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Linq;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Data;
using ZAJCZN.MIS.Helpers;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractTrackManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreContraceTrack";
            }
        }

        #endregion

        #region Page_Load

        private int OrderID
        {
            get { return GetQueryIntValue("id"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHideReference();

                BindGrid();
            }
        }

        private void BindGrid()
        {
            if (CheckPower("CoreContractDoor") || CheckPower("CoreContractCabinet"))
            {
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("ContractID", OrderID));

                if (CheckPower("CoreContractDoor") && !CheckPower("CoreContractCabinet"))
                {
                    qryList.Add(Expression.Eq("CostType", 1));
                }
                if (CheckPower("CoreContractCabinet") && !CheckPower("CoreContractDoor"))
                {
                    qryList.Add(Expression.Eq("CostType", 2));
                }
                Order[] orderList = new Order[1];
                Order orderli = new Order("CostType", true);
                orderList[0] = orderli;
                IList<ContractCostInfo> list = Core.Container.Instance.Resolve<IServiceContractCostInfo>().GetAllByKeys(qryList, orderList);

                gdHandWare.DataSource = list;
                gdHandWare.DataBind();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// 五金明细编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gdHandWare_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            Dictionary<int, Dictionary<string, object>> modifiedDict = gdHandWare.GetModifiedDict();
            ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(OrderID);

            foreach (int rowIndex in modifiedDict.Keys)
            {
                //根据绑定列的记录编号，获取发货物品信息和物品基本信息
                int rowID = Convert.ToInt32(gdHandWare.DataKeys[rowIndex][0]);
                ContractCostInfo objInfo = Core.Container.Instance.Resolve<IServiceContractCostInfo>().GetEntity(rowID);
                //修改成本单价
                if (modifiedDict[rowIndex].Keys.Contains("CostAmount"))
                {
                    objInfo.CostAmount = Convert.ToDecimal(modifiedDict[rowIndex]["CostAmount"]);
                }
                //修改生产备注
                if (modifiedDict[rowIndex].Keys.Contains("ProduceRemark"))
                {
                    objInfo.ProduceRemark = modifiedDict[rowIndex]["ProduceRemark"].ToString();
                }
                //修改生产状态
                if (modifiedDict[rowIndex].Keys.Contains("ProduceState"))
                {
                    //判断订单状态是否是生产中或者生产完成，其他状态不能修改生产状态
                    if (contractInfo.ContractState == 4 || contractInfo.ContractState == 5)
                    {
                        objInfo.ProduceState = Convert.ToInt32(modifiedDict[rowIndex]["ProduceState"]);
                    }
                }
                //更新订单明细
                Core.Container.Instance.Resolve<IServiceContractCostInfo>().Update(objInfo);
            }
            //判断是否所有厂家产品都生产完成
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractID", OrderID));
            qryList.Add(Expression.Eq("ProduceState", 1));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<ContractCostInfo> list = Core.Container.Instance.Resolve<IServiceContractCostInfo>().GetAllByKeys(qryList, orderList);

            //更新合同整体进度，如果厂商都生产完成，合同整体进度为生产完成，否则为生产中
            contractInfo.ContractState = 4;
            if (list.Count == 0)
            {
                contractInfo.ContractState = 5;
            }
            Core.Container.Instance.Resolve<IServiceContractInfo>().Update(contractInfo);
            //重新加载
            BindGrid();
        }

       
        #endregion
    }
}
