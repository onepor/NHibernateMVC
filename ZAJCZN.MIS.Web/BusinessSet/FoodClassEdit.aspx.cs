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
    public partial class FoodClassEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreFoodClassEdit";
            }
        }

        #endregion

        #region Page_Load

        private int InfoID
        {
            get { return GetQueryIntValue("id"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
                CheckPowerWithButton("CoreFlavorEdit", btnSaveClose);
                //绑定菜品单位 
                BindDDL();
                //绑定打印机
                BindPrint();
                //绑定出库库房
                BindWH();
                //加载数据
                Bind();
            }
        }

        public void Bind()
        {
            if (InfoID > 0)
            {
                tm_FoodClass objInfo = Core.Container.Instance.Resolve<IServiceFoodClass>().GetEntity(InfoID);

                if (objInfo == null)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                    return;
                }

                tbxName.Text = objInfo.ClassName;
                tbxSortIndex.Text = objInfo.SortIndex.ToString();
                ddlUnit.SelectedValue = objInfo.Unit;
                tbxRemark.Text = objInfo.Remark;
                ddlIsUsed.SelectedValue = objInfo.IsUsed.ToString();
                ddlPrint.SelectedValue = objInfo.PrintID.ToString();
                ddlWH.SelectedValue = objInfo.WareHouseID.ToString();
            }
        }

        private void BindDDL()
        {
            List<Tm_Enum> list = GetSystemEnumByTypeKey("CPDW", true);
            ddlUnit.DataSource = list;
            ddlUnit.DataBind();
        }

        private void BindPrint()
        {
            IList<tm_Printer> list = Core.Container.Instance.Resolve<IServicePrinter>().GetAll();
            ddlPrint.DataSource = list;
            ddlPrint.DataBind();
            ddlPrint.Items.Insert(0, new FineUIPro.ListItem("", "0"));
        }

        private void BindWH()
        {
            IList<WareHouse> list = Core.Container.Instance.Resolve<IServiceWareHouse>().GetAll();
            ddlWH.DataSource = list;
            ddlWH.DataBind();
            ddlWH.Items.Insert(0, new FineUIPro.ListItem("", "0"));
        }

        #endregion

        #region Events
        private void SaveItem()
        {
            tm_FoodClass objInfo = new tm_FoodClass();
            if (InfoID > 0)
            {
                objInfo = Core.Container.Instance.Resolve<IServiceFoodClass>().GetEntity(InfoID);
            }
            objInfo.ClassName = tbxName.Text.Trim();
            objInfo.IsUsed = int.Parse(ddlIsUsed.SelectedValue);
            objInfo.ParentID = 0;
            objInfo.Unit = ddlUnit.SelectedValue;
            objInfo.Remark = tbxRemark.Text;
            objInfo.SortIndex = int.Parse(tbxSortIndex.Text);
            objInfo.PrintID = int.Parse(ddlPrint.SelectedValue);
            objInfo.WareHouseID = int.Parse(ddlWH.SelectedValue);

            if (InfoID > 0)
            {
                Core.Container.Instance.Resolve<IServiceFoodClass>().Update(objInfo);
            }
            else
            {
                Core.Container.Instance.Resolve<IServiceFoodClass>().Create(objInfo);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        #endregion
    }
}