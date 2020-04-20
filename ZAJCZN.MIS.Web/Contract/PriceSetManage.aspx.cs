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
    public partial class PriceSetManage : PageBase
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
                //绑定价格套系
                BindPriceSet();
                //绑定物品类型信息
                BindCostType();
                //绑定价格套系商品信息
                BindGrid();
                btnNew.OnClientClick = Window1.GetShowReference(string.Format("~/Contract/PriceSetNew.aspx?id={0}", ContractID), "新增合同价格设置信息");
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

        #region 绑定价格套系

        private void BindPriceSet()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractID", ContractID));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", false);
            orderList[0] = orderli;
            IList<PriceSetInfo> list = Core.Container.Instance.Resolve<IServicePriceSetInfo>().GetAllByKeys(qryList, orderList);

            ddlWH.DataSource = list;
            ddlWH.DataBind();
            ddlWH.SelectedIndex = 0;
        }

        #endregion 绑定价格套系

        #region 绑定物品类型

        private void BindCostType()
        {
            List<JQueryFeature> myList = new List<JQueryFeature>();

            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("IsUsed", "1"));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<EquipmentTypeInfo> list = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetAllByKeys(qryList, orderList);

            foreach (EquipmentTypeInfo good in list)
            {
                myList.Add(new JQueryFeature(good.ID.ToString(), good.TypeName, 1, true));
            }
            myList.Insert(0, new JQueryFeature("0", "全部分类", 1, true));
            ddlCostType.DataSource = myList;
            ddlCostType.DataBind();
            ddlCostType.SelectedIndex = 0;
        }
        #endregion 绑定物品类型

        #region 绑定价格套系商品信息

        /// <summary>
        /// 绑定价格套系商品信息
        /// </summary>
        private void BindGrid()
        {
            //获取物品规格信息
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("SetID", int.Parse(ddlWH.SelectedValue)));
            if (ddlCostType.SelectedValue != "0")
            {
                qryList.Add(Expression.Eq("GoodsTypeID", int.Parse(ddlCostType.SelectedValue)));
            }

            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;

            //从默认的销售库房中获取当前库存大于0的物品信息
            IList<PriceSetGoodsInfo> list = Core.Container.Instance.Resolve<IServicePriceSetGoodsInfo>().GetAllByKeys(qryList, orderList);
            foreach (PriceSetGoodsInfo detail in list)
            {
                if (detail.IsGoodType > 1)
                {
                    detail.GoodsInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(detail.EquipmentID);
                    //detail.EquipmentCode = detail.GoodsInfo.EquipmentCode;
                    detail.EquipmentName = detail.GoodsInfo.EquipmentName;
                    //detail.Standard = detail.GoodsInfo.Standard;
                    detail.EquipmentUnit = detail.GoodsInfo.EquipmentUnit;
                }
                else
                {
                    EquipmentTypeInfo typeObj = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(detail.EquipmentID);
                    detail.EquipmentCode = "";
                    detail.EquipmentName = typeObj.TypeName;
                    detail.Standard = 1; 
                }
            }
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        #endregion 绑定价格套系商品信息

        #endregion

        #region 页面数据转换

        //获取分类名称
        public string GetType(string typeID)
        {
            EquipmentTypeInfo objType = new EquipmentTypeInfo();
            string[] ids = typeID.Split(',');
            if (int.Parse(ids[0]) > 0)
            {
                objType = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(int.Parse(ids[0]));
            }
            else
            {
                objType = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(int.Parse(ids[1]));
            }
            return objType != null ? objType.TypeName : "";
        }
        //获取单位
        public string GetUnitName(string state)
        {
            return GetSystemEnumValue("WPDW", state);
        }
        #endregion

        #region Events

        protected void Window1_Close(object sender, EventArgs e)
        {
            //绑定价格套系
            BindPriceSet();
            //绑定价格套系商品信息
            BindGrid();
        }

        protected void ddlCostType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void ddlWH_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

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
                PriceSetGoodsInfo objInfo = Core.Container.Instance.Resolve<IServicePriceSetGoodsInfo>().GetEntity(rowID);
                if (modifiedDict[rowIndex].Keys.Contains("DailyRents"))
                {
                    objInfo.DailyRents = Convert.ToDecimal(modifiedDict[rowIndex]["DailyRents"]);
                }
                if (modifiedDict[rowIndex].Keys.Contains("UnitPrice"))
                {
                    objInfo.UnitPrice = Convert.ToDecimal(modifiedDict[rowIndex]["UnitPrice"]);
                }
                if (modifiedDict[rowIndex].Keys.Contains("FixPrice"))
                {
                    objInfo.FixPrice = Convert.ToDecimal(modifiedDict[rowIndex]["FixPrice"]);
                }
                if (modifiedDict[rowIndex].Keys.Contains("MinRentingDays"))
                {
                    objInfo.MinRentingDays = Convert.ToInt32(modifiedDict[rowIndex]["MinRentingDays"]);
                }
                if (modifiedDict[rowIndex].Keys.Contains("CustomerUnit"))
                {
                    objInfo.CustomerUnit = Convert.ToDecimal(modifiedDict[rowIndex]["CustomerUnit"]);
                }
                if (modifiedDict[rowIndex].Keys.Contains("DriverUnit"))
                {
                    objInfo.DriverUnit = Convert.ToDecimal(modifiedDict[rowIndex]["DriverUnit"]);
                }
                if (modifiedDict[rowIndex].Keys.Contains("StaffUnit"))
                {
                    objInfo.StaffUnit = Convert.ToDecimal(modifiedDict[rowIndex]["StaffUnit"]);
                }

                Core.Container.Instance.Resolve<IServicePriceSetGoodsInfo>().Update(objInfo);
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