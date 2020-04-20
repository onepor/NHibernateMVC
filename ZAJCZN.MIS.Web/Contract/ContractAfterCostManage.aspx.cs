using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Helpers;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractAfterCostManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreContractAfterSale";
            }
        }

        #endregion

        #region request param

        //订单ID（传入参数）
        private int OrderID
        {
            get { return GetQueryIntValue("id"); }
        }

        #endregion request param

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 权限检查
                CheckPowerWithButton("CoreContractAfterSale", btnNew);
                btnNew.OnClientClick = Window1.GetShowReference(string.Format("~/Contract/ContractAfterCostAdd.aspx?id={0}", OrderID), "新增售后费用");

                LoadData();
            }
        }

        private void LoadData()
        {
            //绑定数据
            BindGrid();
        }

        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractID", OrderID));
            qryList.Add(Expression.Eq("PayType", 3));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<ContractPayInfo> list = Core.Container.Instance.Resolve<IServiceContractPayInfo>().GetAllByKeys(qryList, orderList);

            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        #endregion

        #region Events

        protected void btnDeleteHW_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            ContractPayInfo payInfo = new ContractPayInfo();
            foreach (int id in ids)
            {
                payInfo = Core.Container.Instance.Resolve<IServiceContractPayInfo>().GetEntity(id);
                Core.Container.Instance.Resolve<IServiceContractPayInfo>().Delete(id);
            }
            BindGrid();
            //更新订单售后成本
            CalcCost();
        }
         
        /// <summary>
        /// 添加柜子返回处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
            //更新订单售后成本
            CalcCost();
        }

        /// <summary>
        /// 更新订单售后成本
        /// </summary>
        private void CalcCost()
        {
            ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(OrderID);
            //更新订单售后成本
            string sql = string.Format(@"select isnull(sum(PayMoney),0) as CostAmount from ContractPayInfo where ContractID ={0} AND PayType=3  ", OrderID);
            DataSet dsDoor = DbHelperSQL.Query(sql);
            if (dsDoor.Tables[0] != null)
            {
                contractInfo.AfterSaleCost = decimal.Parse(dsDoor.Tables[0].Rows[0]["CostAmount"].ToString());
            } 
            Core.Container.Instance.Resolve<IServiceContractInfo>().Update(contractInfo);
        }
        #endregion
    }
}
