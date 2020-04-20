using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Configuration;

namespace ZAJCZN.MIS.Web
{
    public partial class CarPriceSetManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreContractPrice";
            }
        }

        #endregion

        #region retquest param
        private int ContractID
        {
            get { return GetQueryIntValue("id"); }
        }

        #endregion retquest param

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Grid1.PageSize = ConfigHelper.PageSize;
                ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
                //绑定合同信息
                BindContractInfo();
                //绑定车辆费用信息
                BindGrid();
            }
        }

        #region 绑定数据

        #region 绑定合同信息
        private void BindContractInfo()
        {
            ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(ContractID);
            if (contractInfo != null)
            {
                lblTitle.Text = string.Format("【{0}】{1} ", contractInfo.CustomerName, contractInfo.ContractNO);
            }
        }

        #endregion 绑定合同信息

        #region 绑定车辆费用信息

        /// <summary>
        /// 绑定车辆费用信息
        /// </summary>
        private void BindGrid()
        {
            //获取合同车辆费用信息
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractID", ContractID));
            //从默认的销售库房中获取当前库存大于0的物品信息
            IList<ContractCarPriceSetInfo> list = Core.Container.Instance.Resolve<IServiceContractCarPriceSetInfo>().GetAllByKeys(qryList);
            if (list.Count > 0)
            {
                foreach (ContractCarPriceSetInfo detail in list)
                {
                    detail.CarInfo = Core.Container.Instance.Resolve<IServiceCarInfo>().GetEntity(detail.CarID);
                }
            }
            else
            {
                #region 创建车辆报价
                //获取车辆信息 
                qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("IsUsed", "1"));
                IList<CarInfo> carList = Core.Container.Instance.Resolve<IServiceCarInfo>().GetAllByKeys(qryList);

                ContractCarPriceSetInfo carSetInfo = new ContractCarPriceSetInfo();
                //创建车辆运费
                foreach (CarInfo obj in carList)
                {
                    carSetInfo = new ContractCarPriceSetInfo();
                    carSetInfo.ContractID = ContractID;
                    carSetInfo.CarID = obj.ID;
                    carSetInfo.CarPayPrice = 0;
                    carSetInfo.TonPayPrice = 0;
                    carSetInfo.MinTon = 20;
                    Core.Container.Instance.Resolve<IServiceContractCarPriceSetInfo>().Create(carSetInfo);
                }
                #endregion 创建车辆报价

                list = Core.Container.Instance.Resolve<IServiceContractCarPriceSetInfo>().GetAllByKeys(qryList);
            }
            Grid1.DataSource = list;
            Grid1.DataBind();

        }

        #endregion 绑定价格套系商品信息

        #endregion

        #region Events

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindGrid();
        }

        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();
        }

        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            Dictionary<int, Dictionary<string, object>> modifiedDict = Grid1.GetModifiedDict();
            foreach (int rowIndex in modifiedDict.Keys)
            {
                int rowID = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
                ContractCarPriceSetInfo objInfo = Core.Container.Instance.Resolve<IServiceContractCarPriceSetInfo>().GetEntity(rowID);
                if (modifiedDict[rowIndex].Keys.Contains("TonPayPrice"))
                {
                    objInfo.TonPayPrice = Convert.ToDecimal(modifiedDict[rowIndex]["TonPayPrice"]);
                }
                if (modifiedDict[rowIndex].Keys.Contains("CarPayPrice"))
                {
                    objInfo.CarPayPrice = Convert.ToDecimal(modifiedDict[rowIndex]["CarPayPrice"]);
                }
                if (modifiedDict[rowIndex].Keys.Contains("MinTon"))
                {
                    objInfo.MinTon = Convert.ToDecimal(modifiedDict[rowIndex]["MinTon"]);
                }

                Core.Container.Instance.Resolve<IServiceContractCarPriceSetInfo>().Update(objInfo);
            }

            BindGrid();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion Events

    }
}