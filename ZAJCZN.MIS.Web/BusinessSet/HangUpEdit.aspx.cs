using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class HangUpEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CorePaymentView";
            }
        }

        #endregion

        #region Page_Load

        protected string action
        {
            get
            {
                return GetQueryValue("action");
            }
        }
        protected int _id
        {
            get
            {
                return GetQueryIntValue("id");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //权限检查
                CheckPowerWithButton("CorePaymentEdit", btnSaveClose);

                if (action == "edit")
                {
                    Bind();
                }
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
            }
        }


        private void Bind()
        {
            tm_Payment entity = Core.Container.Instance.Resolve<IServicePayment>().GetEntity(_id);
            txbPaymentName.Text = entity.PaymentName;
            numProportion.Text = entity.Proportion.ToString();
            numSort.Text = entity.Sort.ToString();
            radioIsVip.SelectedValue = entity.IsVip;
            radioIsIntegral.SelectedValue = entity.IsIntegral;
            radioIsUsed.SelectedValue = entity.IsUsed;
            rbtnRecord.SelectedValue = entity.Proportion.ToString();
        }

        #endregion

        #region Events
        private void SaveItem()
        {
            tm_Payment entity = new tm_Payment();
            if (action == "edit")
            {
                entity = Core.Container.Instance.Resolve<IServicePayment>().GetEntity(_id); ;
            }
            entity.PaymentName = txbPaymentName.Text.Trim();
            entity.IsUsed = radioIsUsed.SelectedValue;
            entity.IsVip = radioIsVip.SelectedValue;
            entity.IsIntegral = radioIsIntegral.SelectedValue;
            entity.Proportion = int.Parse(rbtnRecord.SelectedValue);
            entity.Sort = Int32.Parse(numSort.Text);
            if (action == "edit")
            {
                Core.Container.Instance.Resolve<IServicePayment>().Update(entity);
            }
            else
            {
                Core.Container.Instance.Resolve<IServicePayment>().Create(entity);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            if (action == "add")
            {
                string paymentName = txbPaymentName.Text.Trim();
                int sort = Int32.Parse(numSort.Text);
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Disjunction()
                    .Add(Expression.Eq("PaymentName", paymentName))
                    );
                tm_Payment entity = Core.Container.Instance.Resolve<IServicePayment>().GetEntityByFields(qryList);
                if (entity != null)
                {
                    Alert.ShowInTop("已存在支付方式名为[ " + entity.PaymentName + " ]的记录！保存失败", MessageBoxIcon.Warning);
                    return;
                }
            }
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion
    }
}