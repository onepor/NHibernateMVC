using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;
using FineUIPro;


namespace ZAJCZN.MIS.Web.admin
{
    public partial class StoreConfig : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreStoreConfigView";
            }
        }

        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Panel1.AutoScroll = true;
                LoadData();
            }
        }

        private void LoadData()
        {
            // 权限检查
            CheckPowerWithButton("CoreStoreConfigEdit", btnSave);


            tbxTitle.Text = ConfigHelper.StoreName;
            tbxAddress.Text = ConfigHelper.Address;
            tbxContractPhone.Text = ConfigHelper.ContractPhone;
            tbxInstallPhone.Text = ConfigHelper.InstallPhone;
            tbxDesignPhone.Text = ConfigHelper.DesignPhone;
            tbxComplaintPhone.Text = ConfigHelper.ComplaintPhone;

        }

        #endregion

        #region Events

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            // 在操作之前进行权限检查
            if (!CheckPower("CoreStoreConfigEdit"))
            {
                CheckPowerFailWithAlert();
                return;
            }

            ConfigHelper.StoreName = tbxTitle.Text;
            ConfigHelper.Address = tbxAddress.Text;
            ConfigHelper.ContractPhone = tbxContractPhone.Text;
            ConfigHelper.InstallPhone = tbxInstallPhone.Text;
            ConfigHelper.DesignPhone = tbxDesignPhone.Text;
            ConfigHelper.ComplaintPhone = tbxComplaintPhone.Text;



            //Alert.ShowInTop("修改系统配置成功（点击确定刷新页面）！", String.Empty, "top.window.location.reload(false);");

            PageContext.RegisterStartupScript("top.window.location.reload(false);");
        }

        #endregion
    }
}
