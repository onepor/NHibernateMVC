using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class CustomerEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreCustomerEdit";
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
                CheckPowerWithButton("CoreCustomerEdit", btnSaveClose);
                 
                if (action == "edit")
                {
                    Bind(); 
                }

                btnClose.OnClientClick = ActiveWindow.GetHideReference();
            }
        }

        private void Bind()
        {
            CustomerInfo entity = Core.Container.Instance.Resolve<IServiceCustomerInfo>().GetEntity(_id);
            txtRemark.Text = entity.Remark;
            txbVipPhone.Text = entity.ContractPhone;
            txtAddress.Text = entity.ContractAddress;
            txtLinkMan.Text = entity.LinkMan;
            txtVipName.Text = entity.CustomerName;
            ddlIsUsed.SelectedValue = entity.IsUsed;
            nbxNumber.Text = entity.CustomerNumber.ToString();
        }

        #endregion

        #region Events
        private void SaveItem()
        {
            CustomerInfo customerInfo = new CustomerInfo();
            if (action == "edit")
            {
                customerInfo = Core.Container.Instance.Resolve<IServiceCustomerInfo>().GetEntity(_id);
            }
            customerInfo.CustomerNumber = int.Parse(nbxNumber.Text);
            customerInfo.CustomerName = txtVipName.Text.Trim();
            customerInfo.Remark = txtRemark.Text.Trim();
            customerInfo.ContractPhone = txbVipPhone.Text.Trim();
            customerInfo.ContractAddress = txtAddress.Text.Trim();
            customerInfo.LinkMan = txtLinkMan.Text.Trim();
            customerInfo.IsUsed = ddlIsUsed.SelectedValue;
            if (action == "edit")
            {
                Core.Container.Instance.Resolve<IServiceCustomerInfo>().Update(customerInfo);
            }
            else
            {
                Core.Container.Instance.Resolve<IServiceCustomerInfo>().Create(customerInfo);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            if (action == "add")
            {
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("CustomerName", txtVipName.Text.Trim()));
                CustomerInfo CustomerObj = Core.Container.Instance.Resolve<IServiceCustomerInfo>().GetEntityByFields(qryList);

                //判断重复
                if (CustomerObj != null)
                {
                    Alert.Show("客户名称已存在！");
                    return;
                }
            }
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion
    }
}