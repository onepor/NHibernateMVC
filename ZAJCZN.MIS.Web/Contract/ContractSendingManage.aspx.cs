using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Data;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Helpers;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractSendingManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreContractSending";
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
            //绑定数据
            BindGrid();
        }

        private void BindGrid()
        {
            string sqlHW = string.Format(@" select * from ContractInfo where ID in(
                           select  ContractID from ContractCostInfo where ProduceState=2 and (SendingState=0 or SendingState=1)) order by ContractState");
            DataSet dsHW = DbHelperSQL.Query(sqlHW);
            if (dsHW.Tables[0] != null)
            {
                Grid1.DataSource = dsHW.Tables[0];
                Grid1.DataBind();
            }
        }

        public string GetOrderState(string state)
        {
            string i = "";
            switch (state)
            {
                /// 项目进度情况 0:登记中 1:已付定金 2:待测量 3:测量完成 4:生产中 5:生产完成  
                /// 6：送货中 7：送货完成 8：待安装 9：安装完成  10:质保中 11:售后中 12:质保结束 
                case "6":
                    i = "已派工";
                    break;
                default:
                    i = "未派工";
                    break;
            }
            return i;
        }

        public System.Drawing.Color GetColor(string state)
        {
            System.Drawing.Color i = new System.Drawing.Color();
            switch (state)
            { 
                case "6":
                    i = System.Drawing.Color.Black;
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
            CheckPowerWithWindowField("CoreContractSending", Grid1, "editField");
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void Grid1_RowDoubleClick(object sender, GridRowClickEventArgs e)
        {
            // 在操作之前进行权限检查
            if (!CheckPower("CoreContractSending"))
            {
                int ID = GetSelectedDataKeyID(Grid1);
                PageContext.RegisterStartupScript(Window1.GetShowReference(string.Format("~/Contract/ContractSendingEdit.aspx?id={0}", ID)
                    , "安排送货时间"));
            }
        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            foreach (int id in ids)
            {
                ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(id);
                if (contractInfo.ContractState == 6)
                {
                    contractInfo.ContractState = 7;
                    Core.Container.Instance.Resolve<IServiceContractInfo>().Update(contractInfo);
                }
            }
            BindGrid();
        }

        #endregion
    }
}
