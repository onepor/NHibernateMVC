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
    public partial class WareHouseEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreWareHouseEdit";
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
                CheckPowerWithButton("CoreWareHouseEdit", btnSaveClose);

                //加载数据
                Bind();
            }
        }

        public void Bind()
        {
            if (InfoID > 0)
            {
                WareHouse objInfo = Core.Container.Instance.Resolve<IServiceWareHouse>().GetEntity(InfoID);

                if (objInfo == null)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                    return;
                }

                tbxName.Text = objInfo.WHName;
                txtCode.Text = objInfo.WHCode;
                ddlIsUsed.SelectedValue = objInfo.IsUsed.ToString();
                rbtnDefault.SelectedValue = objInfo.IsDefault.ToString();               
            }
        }

        #endregion

        #region Events
        private void SaveItem()
        {
            WareHouse objInfo = new WareHouse();
            if (InfoID > 0)
            {
                objInfo = Core.Container.Instance.Resolve<IServiceWareHouse>().GetEntity(InfoID);
            }
            objInfo.WHName = tbxName.Text.Trim();
            objInfo.IsUsed = int.Parse(ddlIsUsed.SelectedValue);
            objInfo.WHCode = txtCode.Text.Trim();
            objInfo.IsDefault = int.Parse(rbtnDefault.SelectedValue);
             

            //先更新原来默认仓设为0
            if (objInfo.IsDefault > 0)
            {
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("IsDefault", 1));
                WareHouse obj = Core.Container.Instance.Resolve<IServiceWareHouse>().GetEntityByFields(qryList);
                if (obj != null)
                {
                    obj.IsDefault = 0;
                    Core.Container.Instance.Resolve<IServiceWareHouse>().Update(obj);
                }
            }  

            if (InfoID > 0)
            {               
                Core.Container.Instance.Resolve<IServiceWareHouse>().Update(objInfo);
            }
            else
            {
                Core.Container.Instance.Resolve<IServiceWareHouse>().Create(objInfo);
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