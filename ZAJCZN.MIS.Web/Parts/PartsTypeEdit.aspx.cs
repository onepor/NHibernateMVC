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
    public partial class PartsTypeEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CorePartsTypeEdit";
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
                CheckPowerWithButton("CorePartsTypeEdit", btnSaveClose); 
                //加载数据
                Bind();
            }
        }         

        public void Bind()
        {
            if (InfoID > 0)
            {
                PartsTypeInfo objInfo = Core.Container.Instance.Resolve<IServicePartsTypeInfo>().GetEntity(InfoID);

                if (objInfo == null)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                    return;
                }

                tbxName.Text = objInfo.TypeName;
                tbxRemark.Text = objInfo.Remark;
                ddlIsUsed.SelectedValue = objInfo.IsUsed.ToString();
                rbtnDoor.SelectedValue = objInfo.IsDoorDefault.ToString();    
            }
        }
        #endregion

        #region Events
        private void SaveItem()
        {
            PartsTypeInfo objInfo = new PartsTypeInfo();
            if (InfoID > 0)
            {
                objInfo = Core.Container.Instance.Resolve<IServicePartsTypeInfo>().GetEntity(InfoID);
            }
            objInfo.TypeName = tbxName.Text.Trim();
            objInfo.IsUsed = ddlIsUsed.SelectedValue;
            objInfo.IsDoorDefault = int.Parse(rbtnDoor.SelectedValue);
            //起租天数 
            objInfo.Remark = tbxRemark.Text; 

            if (InfoID > 0)
            {
                Core.Container.Instance.Resolve<IServicePartsTypeInfo>().Update(objInfo);
            }
            else
            {
                Core.Container.Instance.Resolve<IServicePartsTypeInfo>().Create(objInfo);
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