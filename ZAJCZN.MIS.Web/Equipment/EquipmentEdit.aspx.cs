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
    public partial class EquipmentEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreEquipmentEdit";
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
                CheckPowerWithButton("CoreEquipmentEdit", btnSaveClose);

                //绑定商品类型信息
                BindCostType();
                //加载数据
                Bind();
            }
        }

        #region 绑定商品类型
        private void BindCostType()
        {
            List<EquipmentTypeInfo> myList = new List<EquipmentTypeInfo>();

            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("IsUsed", "1"));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<EquipmentTypeInfo> list = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetAllByKeys(qryList, orderList);

            myList.AddRange(list);
            myList.Insert(0, new EquipmentTypeInfo { ID = 0, TypeName = "选择分类" });

            ddlCostType.DataSource = myList;
            ddlCostType.DataBind();
            ddlCostType.SelectedIndex = 0;

            if (Session["EquipmentInfoType"] != null && !Session["EquipmentInfoType"].ToString().Equals("-1"))
            {
                ddlCostType.SelectedValue = Session["EquipmentInfoType"].ToString();
                EquipmentTypeInfo ETypeInfo = new EquipmentTypeInfo();
                ETypeInfo = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(int.Parse(ddlCostType.SelectedValue));
                if (ETypeInfo != null)
                {
                    txtRemark.Text = ETypeInfo.Remark;
                }
            }
        }
        #endregion

        public void Bind()
        {
            if (InfoID > 0)
            {
                EquipmentInfo EquipmentInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(InfoID);
                txtCostName.Text = EquipmentInfo.EquipmentName;                                 //商品名称
                ddlCostType.SelectedValue = EquipmentInfo.EquipmentTypeID.ToString();           //商品类别
                ddlIsUsed.SelectedValue = EquipmentInfo.IsUsed.ToString();                      //是否启用
                ddlUnit.SelectedValue = EquipmentInfo.EquipmentUnit;                            //标准单位 
                ddlCalcUnit.SelectedValue = EquipmentInfo.PassCalcType.ToString();
                ddlCalcUnitType.SelectedValue = EquipmentInfo.CalcUnitType.ToString();
                txtRemark.Text = EquipmentInfo.Remark;   //备注
                txtLine.Text = EquipmentInfo.LineName;

                nbPrice.Text = EquipmentInfo.UnitPrice.ToString();
                nbHeght.Text = EquipmentInfo.EHeight.ToString();
                nbWide.Text = EquipmentInfo.EWide.ToString();
                nbThckness.Text = EquipmentInfo.EThickness.ToString();
                nbPassHeght.Text = EquipmentInfo.PassHeight.ToString();
                nbPassWide.Text = EquipmentInfo.PassWide.ToString();
                nbPassTK.Text = EquipmentInfo.PassThckness.ToString();
                nbInstall.Text = EquipmentInfo.InstallCost.ToString();
                nbPassArea.Text = EquipmentInfo.PassArea.ToString();
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
            EquipmentInfo EquipmentInfoInfo = new EquipmentInfo();
            if (InfoID > 0)
            {
                EquipmentInfoInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(InfoID);
            }

            EquipmentInfoInfo.EquipmentName = txtCostName.Text;                                 //商品名称
            EquipmentInfoInfo.EquipmentTypeID = int.Parse(ddlCostType.SelectedValue);           //商品类别           
            EquipmentInfoInfo.IsUsed = ddlIsUsed.SelectedValue;                                 //是否启用
            EquipmentInfoInfo.EquipmentUnit = ddlUnit.SelectedValue;                            //标准单位          
            EquipmentInfoInfo.PassCalcType = int.Parse(ddlCalcUnit.SelectedValue);
            EquipmentInfoInfo.Remark = txtRemark.Text;
            EquipmentInfoInfo.LineName = txtLine.Text;
            EquipmentInfoInfo.UnitPrice = decimal.Parse(nbPrice.Text);
            EquipmentInfoInfo.EHeight = int.Parse(nbHeght.Text);
            EquipmentInfoInfo.EWide = int.Parse(nbWide.Text);
            EquipmentInfoInfo.EThickness = int.Parse(nbThckness.Text);
            EquipmentInfoInfo.PassHeight = int.Parse(nbPassHeght.Text);
            EquipmentInfoInfo.PassWide = int.Parse(nbPassWide.Text);
            EquipmentInfoInfo.PassThckness = decimal.Parse(nbPassTK.Text);
            EquipmentInfoInfo.PassArea = decimal.Parse(nbPassArea.Text);
            EquipmentInfoInfo.InstallCost = decimal.Parse(nbInstall.Text);
            EquipmentInfoInfo.CalcUnitType = int.Parse(ddlCalcUnitType.SelectedValue);
            if (InfoID > 0)
            {
                Core.Container.Instance.Resolve<IServiceEquipmentInfo>().Update(EquipmentInfoInfo);
            }
            else
            {
                Core.Container.Instance.Resolve<IServiceEquipmentInfo>().Create(EquipmentInfoInfo);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected void ddlCostType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EquipmentTypeInfo ETypeInfo = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(int.Parse(ddlCostType.SelectedValue));
            if (ETypeInfo != null)
            {
                txtRemark.Text = ETypeInfo.Remark;
            }
        }

        #endregion
    }
}