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
    public partial class PriceSetEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreFlavorEdit";
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
                 //加载数据
                Bind();
            }
        }
         
        public void Bind()
        {
            if (InfoID > 0)
            {
                Tm_FlavorInfo FlavorInfo = Core.Container.Instance.Resolve<IServiceFlavor>().GetEntity(InfoID);
                txtCostName.Text = FlavorInfo.FlavorName;                
                ddlIsUsed.SelectedValue = FlavorInfo.IsUsed.ToString(); 
            }
        }
        #endregion

        #region Events
        private void SaveItem()
        { 
            Tm_FlavorInfo FlavorInfo = new Tm_FlavorInfo();
            if (InfoID > 0)
            {
                FlavorInfo = Core.Container.Instance.Resolve<IServiceFlavor>().GetEntity(InfoID);
            }
            FlavorInfo.FlavorName = txtCostName.Text.Trim(); 
            FlavorInfo.IsUsed = int.Parse(ddlIsUsed.SelectedValue);
            FlavorInfo.ParentID = 0; 

            if (InfoID > 0)
            {
                Core.Container.Instance.Resolve<IServiceFlavor>().Update(FlavorInfo);
            }
            else
            {
                Core.Container.Instance.Resolve<IServiceFlavor>().Create(FlavorInfo);
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