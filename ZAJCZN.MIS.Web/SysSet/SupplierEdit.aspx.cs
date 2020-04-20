using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class SupplierEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreSupplierEdit";
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
                CheckPowerWithButton("CoreSupplierEdit", btnSaveClose);
                 
                if (action == "edit")
                {
                    Bind(); 
                }

                btnClose.OnClientClick = ActiveWindow.GetHideReference();
            }
        }

        private void Bind()
        {
            SupplierInfo entity = Core.Container.Instance.Resolve<IServiceSupplierInfo>().GetEntity(_id);
            txtRemark.Text = entity.Remark;
            txbVipPhone.Text = entity.ContractPhone;
            txtAddress.Text = entity.ContractAddress;
            txtLinkMan.Text = entity.LinkMan;
            txtVipName.Text = entity.SupplierName;
            ddlIsUsed.SelectedValue = entity.IsUsed;
        }

        #endregion

        #region Events
        private void SaveItem()
        {
            SupplierInfo SupplierInfo = new SupplierInfo();
            if (action == "edit")
            {
                SupplierInfo = Core.Container.Instance.Resolve<IServiceSupplierInfo>().GetEntity(_id);
            }  
            SupplierInfo.SupplierName = txtVipName.Text.Trim();
            SupplierInfo.Remark = txtRemark.Text.Trim();
            SupplierInfo.ContractPhone = txbVipPhone.Text.Trim();
            SupplierInfo.ContractAddress = txtAddress.Text.Trim();
            SupplierInfo.LinkMan = txtLinkMan.Text.Trim();
            SupplierInfo.IsUsed = ddlIsUsed.SelectedValue;
            if (action == "edit")
            {
                Core.Container.Instance.Resolve<IServiceSupplierInfo>().Update(SupplierInfo);
            }
            else
            {
                Core.Container.Instance.Resolve<IServiceSupplierInfo>().Create(SupplierInfo);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            if (action == "add")
            {
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("SupplierName", txtVipName.Text.Trim()));
                SupplierInfo SupplierObj = Core.Container.Instance.Resolve<IServiceSupplierInfo>().GetEntityByFields(qryList);

                //判断重复
                if (SupplierObj != null)
                {
                    Alert.Show("供应商名称已存在！");
                    return;
                }
            }
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion
    }
}