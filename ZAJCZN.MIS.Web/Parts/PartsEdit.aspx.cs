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
    public partial class PartsEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CorePartsEdit";
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
                CheckPowerWithButton("CorePartsEdit", btnSaveClose);
                //绑定物品单位
                BindUnit();
                //绑定配件类型信息
                BindCostType();
                //加载数据
                Bind();
            }
        }

        #region 绑定配件类型
        private void BindCostType()
        {
            List<PartsTypeInfo> myList = new List<PartsTypeInfo>();

            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("IsUsed", "1"));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<PartsTypeInfo> list = Core.Container.Instance.Resolve<IServicePartsTypeInfo>().GetAllByKeys(qryList, orderList);

            myList.AddRange(list);
            myList.Insert(0, new PartsTypeInfo { ID = 0, TypeName = "选择分类" });

            ddlCostType.DataSource = myList;
            ddlCostType.DataBind();
            ddlCostType.SelectedIndex = 0;

            if (Session["PartsInfoType"] != null && !Session["PartsInfoType"].ToString().Equals("-1"))
            {
                ddlCostType.SelectedValue = Session["PartsInfoType"].ToString();
                PartsTypeInfo ETypeInfo = new PartsTypeInfo();
                ETypeInfo = Core.Container.Instance.Resolve<IServicePartsTypeInfo>().GetEntity(int.Parse(ddlCostType.SelectedValue));
                if (ETypeInfo != null)
                {
                    txtRemark.Text = ETypeInfo.Remark;
                }
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

            ddlUnit.DataSource = list;
            ddlUnit.DataBind();
        }
        #endregion

        public void Bind()
        {
            if (InfoID > 0)
            {
                PartsInfo PartsInfo = Core.Container.Instance.Resolve<IServicePartsInfo>().GetEntity(InfoID);
                txtCostName.Text = PartsInfo.PartsName;                                 //配件名称
                ddlCostType.SelectedValue = PartsInfo.PartsTypeID.ToString();           //配件类别
                ddlIsUsed.SelectedValue = PartsInfo.IsUsed.ToString();                      //是否启用
                ddlUnit.SelectedValue = PartsInfo.PartsUnit;                            //标准单位  
                txtRemark.Text = PartsInfo.Remark;                                          //备注
                nbPrice.Text = PartsInfo.UnitPrice.ToString();
                nbCost.Text = PartsInfo.CostPrice.ToString(); ;
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
            PartsInfo PartsInfoInfo = new PartsInfo();
            if (InfoID > 0)
            {
                PartsInfoInfo = Core.Container.Instance.Resolve<IServicePartsInfo>().GetEntity(InfoID);
            }

            PartsInfoInfo.PartsName = txtCostName.Text;                                 //配件名称
            PartsInfoInfo.PartsTypeID = int.Parse(ddlCostType.SelectedValue);           //配件类别           
            PartsInfoInfo.IsUsed = ddlIsUsed.SelectedValue;                                 //是否启用
            PartsInfoInfo.PartsUnit = ddlUnit.SelectedValue;                            //标准单位   
            PartsInfoInfo.Remark = txtRemark.Text;                                        //备注
            PartsInfoInfo.UnitPrice = decimal.Parse(nbPrice.Text);
            PartsInfoInfo.CostPrice = decimal.Parse(nbCost.Text);
            if (InfoID > 0)
            {
                Core.Container.Instance.Resolve<IServicePartsInfo>().Update(PartsInfoInfo);
            }
            else
            {
                Core.Container.Instance.Resolve<IServicePartsInfo>().Create(PartsInfoInfo);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected void ddlCostType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PartsTypeInfo ETypeInfo = Core.Container.Instance.Resolve<IServicePartsTypeInfo>().GetEntity(int.Parse(ddlCostType.SelectedValue));
            if (ETypeInfo != null)
            {
                txtRemark.Text = ETypeInfo.Remark;
            }
        }

        #endregion
    }
}