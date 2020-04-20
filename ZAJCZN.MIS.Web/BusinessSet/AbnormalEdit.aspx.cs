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
    public partial class AbnormalEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreAbnormalEdit";
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
                CheckPowerWithButton("CoreAbnormalEdit", btnSaveClose);
                //绑定异常原因类型
                BindCostType();
                //加载数据
                Bind();
            }
        }

        #region 绑定异常原因类型
        private void BindCostType()
        {
            List<Tm_Enum> list = GetSystemEnumByTypeKey("YCYYLX", false);
            ddlCostType.DataSource = list;
            ddlCostType.DataBind();
        }
        #endregion

        public void Bind()
        {
            if (InfoID > 0)
            {
                Tm_Abnormal AbnormalInfo = Core.Container.Instance.Resolve<IServiceAbnormal>().GetEntity(InfoID);
                txtCostName.Text = AbnormalInfo.AbnormalName;
                txtCode.Text = AbnormalInfo.AbnormalCode;
                lblPY.Text = AbnormalInfo.AbnormalPY;
                ddlIsUsed.SelectedValue = AbnormalInfo.IsUsed.ToString();
                ddlCostType.SelectedValue = AbnormalInfo.AbnormalType;
            }
        }
        #endregion

        #region Events
        private void SaveItem()
        {
            Tm_Abnormal AbnormalInfo = new Tm_Abnormal();
            if (InfoID > 0)
            {
                AbnormalInfo = Core.Container.Instance.Resolve<IServiceAbnormal>().GetEntity(InfoID);
            }
            AbnormalInfo.AbnormalName = txtCostName.Text.Trim();
            AbnormalInfo.AbnormalType = ddlCostType.SelectedValue;
            AbnormalInfo.IsUsed = int.Parse(ddlIsUsed.SelectedValue);
            AbnormalInfo.AbnormalCode = txtCode.Text.Trim();
            AbnormalInfo.AbnormalPY = GetChinesePY(txtCostName.Text.Trim()); ;

            if (InfoID > 0)
            {
                Core.Container.Instance.Resolve<IServiceAbnormal>().Update(AbnormalInfo);
            }
            else
            {
                Core.Container.Instance.Resolve<IServiceAbnormal>().Create(AbnormalInfo);
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