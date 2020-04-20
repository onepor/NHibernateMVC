using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreContractView";
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
            // 权限检查
            CheckPowerWithButton("CoreContractNew", btnNew);
            btnNew.OnClientClick = Window1.GetShowReference("~/Contract/ContractEdit.aspx", "新增订单信息");
            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
            //绑定数据
            BindGrid();
        }

        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            string qryName = txtSearch.Text.Trim();
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
            qryList.Add(Expression.Eq("ContractState", 1) || Expression.Eq("ContractState", 0));

            Order[] orderList = new Order[1];
            Order orderli = new Order("IsUrgent", false);
            orderList[0] = orderli;
            int count = 0;
            IList<ContractInfo> list = Core.Container.Instance.Resolve<IServiceContractInfo>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        public string GetOrderState(string state)
        {
            /// 项目进度情况 0:登记中 1:已付定金 2:待测量 3:测量完成 4:生产中 5:生产完成  
            /// 6：送货中 7：送货完成 8：待安装 9：安装完成  10:质保中 11:售后中 12:质保结束 
            string i = "";
            switch (state)
            {
                case "0":
                    i = "登记中";
                    break;
                case "1":
                    i = "已收定金";
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

        public string GetPerson(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                EmployeeInfo info = Core.Container.Instance.Resolve<IServiceEmployeeInfo>().GetEntity(int.Parse(id));
                return info != null ? info.EmployeeName : "";
            }
            return "";
        }

        #endregion

        #region Events

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreContractEdit", Grid1, "editField");
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreContractEdit", Grid1, "lbtnEditMoneyField");
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);

            if (e.CommandName == "Delete")
            {
                // 在操作之前进行权限检查
                if (!CheckPower("CoreContractDelete"))
                {
                    CheckPowerFailWithAlert();
                    return;
                }
                //检查订单状态，已付定金的登记订单不能删除
                ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(ID);
                if (contractInfo.ContractState == 0)
                {
                    //删除类型
                    Core.Container.Instance.Resolve<IServiceContractInfo>().Delete(ID);
                    //更新页面数据
                    BindGrid();
                }
                else
                {
                    Alert.Show("订单已付定金，不能删除！");
                }
            }
            //收定金
            if (e.CommandName == "editFieldMoney")
            {
                PageContext.Redirect(string.Format("~/Contract/ContractPayManage.aspx?id={0}", ID));
            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void Grid1_RowDoubleClick(object sender, GridRowClickEventArgs e)
        {
            // 在操作之前进行权限检查
            if (!CheckPower("CoreContractEdit"))
            {
                int ID = GetSelectedDataKeyID(Grid1);
                PageContext.RegisterStartupScript(Window1.GetShowReference(string.Format("~/Contract/ContractEdit.aspx?id={0}", ID)
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

        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            int rowIndex = Grid1.SelectedRowIndex;
            hdNO.Text = "0";
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("PayType", 1));
            qryList.Add(Expression.Eq("ContractID", int.Parse(Grid1.DataKeys[rowIndex][0].ToString())));

            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;

            ContractPayInfo payInfo = Core.Container.Instance.Resolve<IServiceContractPayInfo>().GetFirstEntityByFields(qryList, orderList);
            if (payInfo != null)
            {
                hdNO.Text = payInfo.ID.ToString();
            }
        }

        #endregion
    }
}
