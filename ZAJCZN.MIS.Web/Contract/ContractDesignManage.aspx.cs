using FineUIPro;
using Newtonsoft.Json.Linq;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Data;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Helpers;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractDesignManage : PageBase
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
                //绑定销售人员
                BindSaler();
                LoadData();
            }
        }

        /// <summary>
        /// 绑定销售人员
        /// </summary>
        private void BindSaler()
        {
            List<EmployeeInfo> listObj = new List<EmployeeInfo>();

            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("UserType", "销售人员"));
            IList<EmployeeInfo> list = Core.Container.Instance.Resolve<IServiceEmployeeInfo>().GetAllByKeys(qryList);
            listObj.AddRange(list);
            listObj.Insert(0, new EmployeeInfo { ID = 0, EmployeeName = "--全部--" });
            ddlSaler.DataSource = listObj;
            ddlSaler.DataBind();
            ddlSaler.SelectedIndex = 0;
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
            if (ddlSaler.SelectedValue != "0")
            {
                qryList.Add(Expression.Eq("SalePerson", int.Parse(ddlSaler.SelectedValue)));
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

            decimal doorAmount = 0M;
            decimal cabinetAmount = 0M;
            foreach (ContractInfo eqpInfo in listAll)
            {
                doorAmount += (decimal)eqpInfo.DoorAmount;
                cabinetAmount += (decimal)eqpInfo.CabinetAmount;
            }
            //绑定合计数据
            JObject summary = new JObject();
            summary.Add("DoorAmount", doorAmount);
            summary.Add("CabinetAmount", cabinetAmount);

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

        #endregion

        #region Events

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithLinkButtonField("CoreContractDoor", Grid1, "lbtnEditDoorField");
            CheckPowerWithBoundField("CoreContractDoor", Grid1, "DoorAmount");
            // 数据绑定之前，进行权限检查
            CheckPowerWithLinkButtonField("CoreContractCabinet", Grid1, "lbtnEditCabinetField");
            CheckPowerWithBoundField("CoreContractCabinet", Grid1, "CabinetAmount");
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreContractEdit", Grid1, "editField");
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreContraceTrack", Grid1, "wfSupplierField");
            // 数据绑定之前，进行权限检查
            CheckPowerWithLinkButtonField("CorePayment", Grid1, "lbtnEditMoneyField");
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreContraceCost", Grid1, "wfCostField");

        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            //门设计
            if (e.CommandName == "editFieldDoor")
            {
                PageContext.Redirect(string.Format("~/Contract/ContractDoorDetail.aspx?id={0}", ID));
            }
            //柜子设计
            if (e.CommandName == "editFieldCabinet")
            {
                PageContext.Redirect(string.Format("~/Contract/ContractCabinetEdit.aspx?id={0}", ID));
            }
            //收付款
            if (e.CommandName == "editFieldMoney")
            {
                PageContext.Redirect(string.Format("~/Contract/ContractPayManage.aspx?id={0}", ID));
            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
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

        #endregion
    }
}
