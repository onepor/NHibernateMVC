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

namespace ZAJCZN.MIS.Web
{
    public partial class GoodsEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreGoodsEdit";
            }
        }

        #endregion

        #region Page_Load

        private int InfoID
        {
            get { return GetQueryIntValue("id"); }
        }
        private int IsView
        {
            get { return GetQueryIntValue("isview"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
                CheckPowerWithButton("CoreGoodsEdit", btnSaveClose);
                //绑定物品类型信息
                BindCostType();
                //绑定物品单位
                BindUnit();
                //绑定盘点类型
                BindInventoryType();
                //绑定税率
                BindTaxPoint();
                //加载数据
                Bind();
                txtGoodsBarCode.Focus();
            }
        }

        #region 绑定物品类型
        private void BindCostType()
        {
            List<JQueryFeature> myList = new List<JQueryFeature>();

            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ParentID", 0));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<tm_GoodsType> list = Core.Container.Instance.Resolve<IServiceGoodsType>().GetAllByKeys(qryList, orderList);

            foreach (tm_GoodsType good in list)
            {
                myList.Add(new JQueryFeature(good.ID.ToString(), good.TypeName, 1, true));

                IList<ICriterion> qryListChild = new List<ICriterion>();
                qryListChild.Add(Expression.Eq("ParentID", good.ID));
                Order[] orderListChild = new Order[1];
                Order orderliChild = new Order("ID", true);
                orderListChild[0] = orderliChild;
                IList<tm_GoodsType> listChild = Core.Container.Instance.Resolve<IServiceGoodsType>().GetAllByKeys(qryListChild, orderListChild);
                foreach (tm_GoodsType goodChild in listChild)
                {
                    myList.Add(new JQueryFeature(goodChild.ID.ToString(), goodChild.TypeName, 2, true));
                }
            }
            myList.Insert(0, new JQueryFeature("", "", 1, true));
            ddlCostType.DataSource = myList;
            ddlCostType.DataBind();
            ddlCostType.SelectedIndex = 0;

            if (Session["GoodSType"] != null && !Session["GoodSType"].ToString().Equals("0"))
            {
                ddlCostType.SelectedValue = Session["GoodSType"].ToString();
            }
        }
        #endregion

        #region 绑定物品单位
        private void BindUnit()
        {
            List<Tm_Enum> list = GetSystemEnumByTypeKey("WPDW", false);
            list.Insert(0, new Tm_Enum { EnumKey = "", EnumValue = "" });
            ddlUnit.DataSource = list;
            ddlUnit.DataBind();

            ddlConsumeUnit.DataSource = list;
            ddlConsumeUnit.DataBind();

            ddlPurchaseUnit.DataSource = list;
            ddlPurchaseUnit.DataBind();

            ddlOrderUnit.DataSource = list;
            ddlOrderUnit.DataBind();
        }
        #endregion

        #region 绑定盘点类型
        private void BindInventoryType()
        {
            List<Tm_Enum> list = GetSystemEnumByTypeKey("PDLX", false);
            list.Insert(0, new Tm_Enum { EnumKey = "", EnumValue = "" });
            ddlPD.DataSource = list;
            ddlPD.DataBind();
        }
        #endregion

        #region 绑定税率
        private void BindTaxPoint()
        {
            List<Sys_Paras> list = GetSystemParamsmByGroupKey("TaxPoint", false);
            list.Insert(0, new Sys_Paras { ID = 0, ParaName = "" });
            ddlTaxPoint.DataSource = list;
            ddlTaxPoint.DataBind();
        }
        #endregion

        public void Bind()
        {
            if (InfoID > 0)
            {
                tm_Goods GoodsInfo = Core.Container.Instance.Resolve<IServiceGoods>().GetEntity(InfoID);
                txtCostName.Text = GoodsInfo.GoodsName;                                 //商品名称
                ddlCostType.SelectedValue = GoodsInfo.GoodsTypeID.ToString();           //商品类别
                lblCode.Text = GoodsInfo.GoodsCode.ToString();                          //商品编码
                lblPY.Text = GoodsInfo.GoodsPY;                                         //商品拼音
                ddlIsUsed.SelectedValue = GoodsInfo.IsUsed.ToString();                  //是否启用
                ddlUnit.SelectedValue = GoodsInfo.GoodsUnit;                            //标准单位
                ddlTaxPoint.SelectedValue = GoodsInfo.TaxPoint.ToString();              //税率
                txtPrice.Text = GoodsInfo.GoodsPrice.ToString();                        //商品价格
                txtFormat.Text = GoodsInfo.GoodsFormat;                                 //商品规格描述
                ddlPD.SelectedValue = GoodsInfo.InventoryType;                          //盘点类型
                txtConsumNum.Text = GoodsInfo.ConsumeNum.ToString();                    //消耗换算值
                ddlConsumeUnit.SelectedValue = GoodsInfo.ConsumeUnit;                   //消耗换算单位
                txtPurchaseNum.Text = GoodsInfo.PurchaseNum.ToString();                 //采购换算值
                ddlPurchaseUnit.SelectedValue = GoodsInfo.PurchaseUnit;                 //采购换算单位
                txtOrderNum.Text = GoodsInfo.OrderNum.ToString();                       //订货换算值
                ddlOrderUnit.SelectedValue = GoodsInfo.OrderUnit;                       //订货换算单位
                txtGoodsBarCode.Text = GoodsInfo.GoodsBarCode;
            }
            else
            {
                //获取当前最大编号
                IList<tm_Goods> list = Core.Container.Instance.Resolve<IServiceGoods>().GetAll();
                if (list.Count > 0)
                {
                    tm_Goods maxGoods = list.ToList().OrderByDescending(objs => objs.GoodsCode).First();
                    lblCode.Text = (maxGoods.GoodsCode + 1).ToString();
                }
                else
                {
                    lblCode.Text = "100001";
                }

            }
            if (IsView > 0)
            {
                btnSaveClose.Visible = false;
            }
        }
        #endregion

        #region Events
        private void SaveItem()
        {
            tm_Goods GoodsInfo = new tm_Goods();
            if (InfoID > 0)
            {
                GoodsInfo = Core.Container.Instance.Resolve<IServiceGoods>().GetEntity(InfoID);
            }

            GoodsInfo.GoodsName = txtCostName.Text;                                 //商品名称
            GoodsInfo.GoodsTypeID = int.Parse(ddlCostType.SelectedValue);           //商品类别
            GoodsInfo.GoodsCode = int.Parse(lblCode.Text);                          //商品编码
            GoodsInfo.GoodsPY = lblPY.Text;                                        //商品拼音
            GoodsInfo.IsUsed = int.Parse(ddlIsUsed.SelectedValue);                 //是否启用
            GoodsInfo.GoodsUnit = ddlUnit.SelectedValue;                           //标准单位
            GoodsInfo.TaxPoint = int.Parse(ddlTaxPoint.SelectedValue);              //税率
            //商品价格
            GoodsInfo.GoodsPrice = !string.IsNullOrEmpty(txtPrice.Text) ? decimal.Parse(txtPrice.Text) : 0;
            GoodsInfo.GoodsFormat = txtFormat.Text;                                //商品规格描述
            GoodsInfo.InventoryType = ddlPD.SelectedValue;                          //盘点类型
            GoodsInfo.ConsumeNum = int.Parse(txtConsumNum.Text);                   //消耗换算值
            GoodsInfo.ConsumeUnit = ddlConsumeUnit.SelectedValue;                   //消耗换算单位
            GoodsInfo.PurchaseNum = int.Parse(txtPurchaseNum.Text);                //采购换算值
            GoodsInfo.PurchaseUnit = ddlPurchaseUnit.SelectedValue;                 //采购换算单位
            GoodsInfo.OrderNum = int.Parse(txtOrderNum.Text);                       //订货换算值
            GoodsInfo.OrderUnit = ddlOrderUnit.SelectedValue;                       //订货换算单位
            GoodsInfo.GoodsPY = GetChinesePY(txtCostName.Text.Trim());
            GoodsInfo.GoodsBarCode = txtGoodsBarCode.Text;

            if (InfoID > 0)
            {
                Core.Container.Instance.Resolve<IServiceGoods>().Update(GoodsInfo);
            }
            else
            {
                Core.Container.Instance.Resolve<IServiceGoods>().Create(GoodsInfo);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected void ddlUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblUnit.Text = string.Format("1{0}", ddlUnit.SelectedText);

            ddlConsumeUnit.SelectedValue = ddlUnit.SelectedValue;                   //消耗换算单位 
            ddlPurchaseUnit.SelectedValue = ddlUnit.SelectedValue;                 //采购换算单位 
            ddlOrderUnit.SelectedValue = ddlUnit.SelectedValue;                       //订货换算单位
        }

        #endregion
    }
}