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
                return "CoreContractPrice";
            }
        }

        #endregion

        #region Page_Load

        private int ContractID
        {
            get { return GetQueryIntValue("id"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dpContractDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        #endregion

        #region Events

        #region 创建报价体系

        private void SaveItem()
        {
            //获取当前合同
            ContractInfo curretnInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(ContractID);
            //创建报价体系
            new ContractOrderBase().CreateContractPriceSetInfo(curretnInfo);
        }

        #endregion 创建报价体系

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        #endregion
    }
}