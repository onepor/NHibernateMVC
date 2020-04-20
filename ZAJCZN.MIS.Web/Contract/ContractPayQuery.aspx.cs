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
using Newtonsoft.Json.Linq;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractPayQuery : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CorePayView";
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
                Grid1.PageSize = ConfigHelper.PageSize;
                Grid1.AutoScroll = true;
                ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
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
            if (!string.IsNullOrEmpty(ddlType.SelectedValue))
            {
                qryList.Add(Expression.Eq("PayType", int.Parse(ddlType.SelectedValue)));
            }
            if (!string.IsNullOrEmpty(dpStartDate.Text))
            {
                qryList.Add(Expression.Ge("ApplyDate", dpStartDate.Text));
            }
            if (!string.IsNullOrEmpty(dpEndDate.Text))
            {
                qryList.Add(Expression.Le("ApplyDate", dpEndDate.Text));
            }
            if (!string.IsNullOrEmpty(ddlPay.SelectedValue))
            {
                qryList.Add(Expression.Eq("ApplyState", int.Parse(ddlPay.SelectedValue)));
            }
            Order[] orderList = new Order[1];
            Order orderli = new Order("ApplyState", false);
            orderList[0] = orderli;
            int count = 0;
            IList<ContractPayInfo> list = Core.Container.Instance.Resolve<IServiceContractPayInfo>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();


            IList<ContractPayInfo> listAll = Core.Container.Instance.Resolve<IServiceContractPayInfo>().GetAllByKeys(qryList, orderList);
            
            decimal donateTotal = 0M;
            foreach (ContractPayInfo eqpInfo in listAll)
            {
                donateTotal += eqpInfo.PayMoney;
            }
            //绑定合计数据
            JObject summary = new JObject();
            summary.Add("PayUser", "金额合计");
            summary.Add("PayMoney", donateTotal);

            Grid1.SummaryData = summary;
        }

        public string GetOrderState(string state)
        {
            string i = "";
            switch (state)
            {
                case "1":
                    i = "待审核";
                    break;
                case "2":
                    i = "通过";
                    break;
                case "3":
                    i = "未通过";
                    break;
            }
            return i;
        }

        public System.Drawing.Color GetColor(string state)
        {
            System.Drawing.Color i = new System.Drawing.Color();
            switch (state)
            {
                case "1":
                    i = System.Drawing.Color.Black;
                    break;
                case "2":
                    i = System.Drawing.Color.Blue;
                    break;
            }
            return i;
        }

        public System.Drawing.Color GetColor1(string state)
        {
            System.Drawing.Color i = new System.Drawing.Color();
            switch (state)
            {
                case "1":
                    i = System.Drawing.Color.Black;
                    break;
                case "2":
                    i = System.Drawing.Color.Green;
                    break;
                case "3":
                    i = System.Drawing.Color.Red;
                    break;
            }
            return i;
        }

        #endregion

        #region Events

        protected void btnEnableUsers_Click(object sender, EventArgs e)
        {
            SetSelectedUsersEnableStatus(true);
        }

        protected void btnDisableUsers_Click(object sender, EventArgs e)
        {
            SetSelectedUsersEnableStatus(false);
        }


        private void SetSelectedUsersEnableStatus(bool enabled)
        {
            // 在操作之前进行权限检查
            if (!CheckPower("CorePayApprove"))
            {
                CheckPowerFailWithAlert();
                return;
            }

            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid1);

            // 执行数据库操作 
            foreach (int id in ids)
            {
                ContractPayInfo payInfo = Core.Container.Instance.Resolve<IServiceContractPayInfo>().GetEntity(id);
                if (payInfo != null)
                {
                    payInfo.ApplyState = enabled ? 2 : 3;
                    payInfo.AproveDate = DateTime.Now.ToString("yyyy-MM-dd");
                    payInfo.AproveUser = User.Identity.Name;
                    Core.Container.Instance.Resolve<IServiceContractPayInfo>().Update(payInfo);
                }
            }
            // 重新绑定表格
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
