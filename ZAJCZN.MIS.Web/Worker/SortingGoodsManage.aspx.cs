using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class SortingGoodsManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreSortingGoodsView";
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
            CheckPowerWithButton("CoreSortingGoodsEdit", btnNew);
            btnNew.OnClientClick = Window1.GetShowReference("~/Contract/ContractEdit.aspx", "新增合同信息");
            //绑定数据
            BindGrid();
        }

        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
          
            if (!string.IsNullOrEmpty(dpStartDate.Text))
            {
                qryList.Add(Expression.Ge("ContractDate", DateTime.Parse(dpStartDate.Text)));
            }
            if (!string.IsNullOrEmpty(dpEndDate.Text))
            {
                qryList.Add(Expression.Le("ContractDate", DateTime.Parse(dpEndDate.Text)));
            }
         

            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;

            IList<ContractInfo> list = Core.Container.Instance.Resolve<IServiceContractInfo>().GetAllByKeys(qryList, orderList);

            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        public string GetIsUsed(string state)
        {
            string i = "";
            switch (state)
            {
                case "1":
                    i = "执行中";
                    break;
                case "2":
                    i = "收款中";
                    break;
                case "3":
                    i = "完成";
                    break;
            }
            return i;
        }

        //获取当前执行的报价体系
        public string GetPriceSet(string priceID)
        {
            PriceSetInfo priceSetInfo = Core.Container.Instance.Resolve<IServicePriceSetInfo>().GetEntity(int.Parse(priceID));
            return priceSetInfo != null ? priceSetInfo.SetDate : "";
        }

        //获取合同签订公司名称
        public string GetUnitName(string state)
        {
            return GetSystemEnumValue("JYGS", state);
        }

        #endregion

        #region Events

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreSortingGoodsEdit", Grid1, "editField");
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreSortingGoodsPrice", Grid1, "priceField");
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);

            if (e.CommandName == "Delete")
            {
                // 在操作之前进行权限检查
                if (!CheckPower("CoreSortingGoodsEdit"))
                {
                    CheckPowerFailWithAlert();
                    return;
                }
                //删除类型
                Core.Container.Instance.Resolve<IServiceFoodClass>().Delete(ID);
                //更新页面数据
                BindGrid();
            }
            if (e.CommandName == "Basket")
            {
                PageContext.Redirect(string.Format("~/Contract/ContractOrderManage.aspx?id={0}&type=1", ID));
            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void Grid1_RowDoubleClick(object sender, GridRowClickEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            PageContext.RegisterStartupScript(Window1.GetShowReference(string.Format("~/Contract/ContractEdit.aspx?id={0}", ID)
                , "编辑合同信息"));
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
        #endregion
    }
}
