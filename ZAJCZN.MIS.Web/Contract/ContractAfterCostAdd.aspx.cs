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
using System.Data;
using ZAJCZN.MIS.Helpers;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractAfterCostAdd : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreContractAfterSale";
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
                btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
                CheckPowerWithButton("CoreContractAfterSale", btnSaveClose);
                dpStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                //绑定付款方式
                BindPayType();
            }
        }

        #region 绑定商品类型

        /// <summary>
        /// 绑定物品类别
        /// </summary>
        private void BindPayType()
        {
            List<Tm_Enum> list = GetSystemEnumByTypeKey("FKFS", false);

            ddlPayType.DataSource = list;
            ddlPayType.DataBind();
            ddlPayType.SelectedIndex = 0;
            ddlPayType.Enabled = false;
        }

        #endregion

        #endregion

        #region 录入信息保存

        private void SaveItem()
        {
            ContractPayInfo contractPayInfo = new ContractPayInfo();

            ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(InfoID);

            contractPayInfo.ApplyDate = dpStartDate.Text;
            contractPayInfo.Remark = taRemark.Text;
            contractPayInfo.ContractID = InfoID;
            contractPayInfo.Operator = User.Identity.Name;
            contractPayInfo.PayMoney = decimal.Parse(nbHeight.Text);
            contractPayInfo.PayType = 3;
            contractPayInfo.ApplyState = 2;
            contractPayInfo.PayUser = "";

            //商品类型及名称
            contractPayInfo.PayWay = ddlPayType.SelectedValue;
            //保存商品信息
            Core.Container.Instance.Resolve<IServiceContractPayInfo>().Create(contractPayInfo);
        }

        #endregion 录入信息保存

        #region Events

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion
    }
}