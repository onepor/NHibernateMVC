using FineUIPro;
using Newtonsoft.Json.Linq;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Linq;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractView : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreContractALL";
            }
        }

        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            Grid1.PageSize = ConfigHelper.PageSize;
            Grid1.AutoScroll = true;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
            //绑定数据
            BindGrid();
        }

        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            string qryName = txtSearch.Text.Trim();
            qryList.Add(!Expression.Eq("ContractState", 1));
            if (!string.IsNullOrEmpty(qryName))
            {
                qryList.Add(Expression.Disjunction()
                    .Add(Expression.Like("ContractNO", qryName, MatchMode.Anywhere))
                    .Add(Expression.Like("CustomerName", qryName, MatchMode.Anywhere))
                    .Add(Expression.Like("ContactPhone", qryName, MatchMode.Anywhere))
                    .Add(Expression.Like("ProjectName", qryName, MatchMode.Anywhere))
                    );
            }
            if (!string.IsNullOrEmpty(dpStartDate.Text))
            {
                qryList.Add(Expression.Ge("ContractDate", DateTime.Parse(dpStartDate.Text)));
            }
            if (!string.IsNullOrEmpty(dpEndDate.Text))
            {
                qryList.Add(Expression.Le("ContractDate", DateTime.Parse(dpEndDate.Text)));
            }
            if (!string.IsNullOrEmpty(ddlState.SelectedValue))
            {
                qryList.Add(Expression.Eq("ContractState", int.Parse(ddlState.SelectedValue)));
            }

            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            int count = 0;
            IList<ContractInfo> list = Core.Container.Instance.Resolve<IServiceContractInfo>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
            IList<ContractInfo> listAll = Core.Container.Instance.Resolve<IServiceContractInfo>().GetAllByKeys(qryList, orderList);

            GetSummary(listAll);
        }

        private void GetSummary(IList<ContractInfo> list)
        {
            decimal doorAmount = 0M;
            decimal CabinetAmount = 0M;
            decimal TotalAmount = 0M;
            decimal CabinetCost = 0M;
            decimal DoorCost = 0M;
            decimal SendCost = 0M;
            decimal HandWareCost = 0M;
            decimal AfterSaleCost = 0M;
            decimal ReturnMoney = 0M;
            decimal PayCostMoney = 0M;
            decimal ProfitMoney = 0M;

            foreach (ContractInfo eqpInfo in list)
            {
                doorAmount += (decimal)eqpInfo.DoorAmount;
                CabinetAmount += (decimal)eqpInfo.CabinetAmount;
                TotalAmount += (decimal)eqpInfo.TotalAmount;
                CabinetCost += (decimal)eqpInfo.CabinetCost;
                DoorCost += (decimal)eqpInfo.DoorCost;
                SendCost += (decimal)eqpInfo.SendCost;
                HandWareCost += (decimal)eqpInfo.HandWareCost;
                AfterSaleCost += (decimal)eqpInfo.AfterSaleCost;
                ReturnMoney += (decimal)eqpInfo.ReturnMoney;
                PayCostMoney += (decimal)eqpInfo.PayCostMoney;
                ProfitMoney += (decimal)eqpInfo.ProfitMoney;
            }
            //绑定合计数据
            JObject summary = new JObject();
            summary.Add("DoorAmount", doorAmount);
            summary.Add("CabinetAmount", CabinetAmount);
            summary.Add("TotalAmount", TotalAmount);
            summary.Add("TotalAmount1", TotalAmount);
            summary.Add("CabinetCost", CabinetCost);
            summary.Add("DoorCost", DoorCost);
            summary.Add("SendCost", SendCost);
            summary.Add("HandWareCost", HandWareCost);
            summary.Add("AfterSaleCost", AfterSaleCost);
            summary.Add("ReturnMoney", ReturnMoney);
            summary.Add("PayCostMoney", PayCostMoney);
            summary.Add("ProfitMoney", ProfitMoney);

            Grid1.SummaryData = summary;

        }

        public string GetOrderState(string state)
        {
            /// <summary>
            /// 项目进度情况  1:登记中 2:待测量 3:测量完成 4:生产中 5:生产完成  
            /// 6：送货中 7：送货完成 8：待安装 9：安装完成  10:质保中 11:售后中 12:质保结束 
            string i = "";
            switch (state)
            {
                case "1":
                    i = "登记中";
                    break;
                case "2":
                    i = "待测量";
                    break;
                case "3":
                    i = "测量完成";
                    break;
                case "4":
                    i = "生产中";
                    break;
                case "5":
                    i = "生产完成";
                    break;
                case "6":
                    i = "送货中";
                    break;
                case "7":
                    i = "送货完成";
                    break;
                case "8":
                    i = "待安装";
                    break;
                case "9":
                    i = "安装完成";
                    break;
                case "10":
                    i = "质保中";
                    break;
                case "11":
                    i = "售后中";
                    break;
                case "12":
                    i = "质保结束";
                    break;
            }
            return i;
        }

        public System.Drawing.Color GetColor(string state)
        {
            /// <summary>
            /// 项目进度情况  1:登记中 2:待测量 3:测量完成 4:生产中 5:生产完成  
            /// 6：送货中 7：送货完成 8：待安装 9：安装完成  10:质保中 11:售后中 12:质保结束 
            System.Drawing.Color i = new System.Drawing.Color();
            switch (state)
            {
                case "1":
                    i = System.Drawing.Color.Black;
                    break;
                case "2":
                    i = System.Drawing.Color.DeepPink;
                    break;
                case "3":
                    i = System.Drawing.Color.DeepPink;
                    break;
                case "4":
                    i = System.Drawing.Color.DarkRed;
                    break;
                case "5":
                    i = System.Drawing.Color.DarkRed;
                    break;
                case "6":
                    i = System.Drawing.Color.Green;
                    break;
                case "7":
                    i = System.Drawing.Color.Green;
                    break;
                case "8":
                    i = System.Drawing.Color.Blue;
                    break;
                case "9":
                    i = System.Drawing.Color.Blue;
                    break;
                case "10":
                    i = System.Drawing.Color.Green;
                    break;
                case "11":
                    i = System.Drawing.Color.Orange;
                    break;
                default:
                    i = System.Drawing.Color.Red;
                    break;
            }
            return i;
        }

        public string GetPerson(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                EmployeeInfo info = Core.Container.Instance.Resolve<IServiceEmployeeInfo>().GetEntity(int.Parse(id));
                return info != null ? info.EmployeeName : "";
            }
            return "";

        }

        public string GetProfitMoney(string id)
        {
            decimal profitMoney = 0;
            if (!string.IsNullOrEmpty(id))
            {
                ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(int.Parse(id));
                if (contractInfo != null)
                {
                    contractInfo.ProfitMoney = 0;
                    contractInfo.ProfitMoney = contractInfo.TotalAmount
                                              - contractInfo.CabinetCost
                                              - contractInfo.DoorCost
                                              - contractInfo.SendCost
                                              - contractInfo.HandWareCost
                                              - contractInfo.AfterSaleCost;
                    profitMoney = (decimal)contractInfo.ProfitMoney;
                    Core.Container.Instance.Resolve<IServiceContractInfo>().Update(contractInfo);
                }
            }
            return profitMoney.ToString();
        }
        #endregion

        #region Events

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreContractALL", Grid1, "editField");
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            //编辑发货单
            if (e.CommandName == "editField")
            {
                PageContext.Redirect(string.Format("~/Contract/ContractViewEdit.aspx?id={0}", ID));
            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void Grid1_RowDoubleClick(object sender, GridRowClickEventArgs e)
        {
            // 在操作之前进行权限检查
            if (!CheckPower("CoreContractALL"))
            {
                int ID = GetSelectedDataKeyID(Grid1);
                PageContext.RegisterStartupScript(Window1.GetShowReference(string.Format("~/Contract/ContractViewEdit.aspx?id={0}", ID)
                    , "编辑订单信息"));
            }
        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindGrid();
        }

        /// <summary>
        /// 主材单价及发货数量编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            Dictionary<int, Dictionary<string, object>> modifiedDict = Grid1.GetModifiedDict();
            foreach (int rowIndex in modifiedDict.Keys)
            {
                //根据绑定列的记录编号，获取发货物品信息和物品基本信息
                int rowID = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
                ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(rowID);
                //修改高度
                if (modifiedDict[rowIndex].Keys.Contains("DoorAmount"))
                {
                    contractInfo.DoorAmount = Convert.ToDecimal(modifiedDict[rowIndex]["DoorAmount"]);
                }
                //修改宽度
                if (modifiedDict[rowIndex].Keys.Contains("CabinetAmount"))
                {
                    contractInfo.CabinetAmount = Convert.ToDecimal(modifiedDict[rowIndex]["CabinetAmount"]);
                }
                //修改厚度
                if (modifiedDict[rowIndex].Keys.Contains("CabinetCost"))
                {
                    contractInfo.CabinetCost = Convert.ToDecimal(modifiedDict[rowIndex]["CabinetCost"]);
                }
                //修改五金费用
                if (modifiedDict[rowIndex].Keys.Contains("DoorCost"))
                {
                    contractInfo.DoorCost = Convert.ToDecimal(modifiedDict[rowIndex]["DoorCost"]);
                }
                //修改超标加价
                if (modifiedDict[rowIndex].Keys.Contains("SendCost"))
                {
                    contractInfo.SendCost = Convert.ToDecimal(modifiedDict[rowIndex]["SendCost"]);
                }
                //修改标准单价
                if (modifiedDict[rowIndex].Keys.Contains("HandWareCost"))
                {
                    contractInfo.HandWareCost = Convert.ToDecimal(modifiedDict[rowIndex]["HandWareCost"]);
                }
                //修改最终单价
                if (modifiedDict[rowIndex].Keys.Contains("AfterSaleCost"))
                {
                    contractInfo.AfterSaleCost = Convert.ToDecimal(modifiedDict[rowIndex]["AfterSaleCost"]);
                }

                //计算商品总价
                contractInfo.TotalAmount = contractInfo.DoorAmount + contractInfo.CabinetAmount; 
                //更新订单明细
                Core.Container.Instance.Resolve<IServiceContractInfo>().Update(contractInfo);
            }

            BindGrid();
        }
        #endregion
    }
}
