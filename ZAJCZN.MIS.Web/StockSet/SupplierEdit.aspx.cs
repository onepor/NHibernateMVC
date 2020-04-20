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

        private int _ID
        {
            get { return GetQueryIntValue("id"); }
        }
        private string Action
        {
            get { return GetQueryValue("action"); }
        }

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckPowerWithButton("CoreSupplierEdit", btnSaveClose);
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
                //绑定供应商信息
                BindInfo();
            }
        }

        private void BindInfo()
        {
            if (Action == "show")
            {
                txtSupplierCode.Readonly = true;
                txtSupplierName.Readonly = true;
                txtFullName.Readonly = true;
                txtRemark.Readonly = true;
                txtLinkMan.Readonly = true;
                txtPhone.Readonly = true;
                txtAddress.Readonly = true;
                btnSaveClose.Hidden = true;
            }
            if (Action == "edit")
            {
                SupplierInfo entity = Core.Container.Instance.Resolve<IServiceSupplierInfo>().GetEntity(_ID);
                txtSupplierCode.Text = entity.SupplierCode;
                txtSupplierName.Text = entity.SupplierName;
                txtFullName.Text = entity.FullName;
                txtRemark.Text = entity.Remark;
                txtLinkMan.Text = entity.ContactPerson;
                txtPhone.Text = entity.ContactPhone;
                txtAddress.Text = entity.ContactAddress;
                rbtnIsUsed.SelectedValue = entity.IsUsed.ToString();
            }
        }

        #endregion

        #region Events
        private void SaveItem()
        {
            SupplierInfo entity = new SupplierInfo();
            if (Action == "edit")
            {
                entity = Core.Container.Instance.Resolve<IServiceSupplierInfo>().GetEntity(_ID); ;
            }
            entity.SupplierName = txtSupplierName.Text.Trim();
            entity.SupplierCode = txtSupplierCode.Text;
            entity.SupplierName = txtSupplierName.Text;
            entity.FullName = txtFullName.Text;
            entity.Remark = txtRemark.Text;
            entity.ContactPerson = txtLinkMan.Text;
            entity.ContactPhone = txtPhone.Text;
            entity.ContactAddress = txtAddress.Text;
            entity.IsUsed = int.Parse(rbtnIsUsed.SelectedValue);

            if (Action == "edit")
            {
                Core.Container.Instance.Resolve<IServiceSupplierInfo>().Update(entity);
                //Alert.ShowInTop("编辑成功！", MessageBoxIcon.Success);
            }
            else
            {
                Core.Container.Instance.Resolve<IServiceSupplierInfo>().Create(entity);
                //Alert.ShowInTop("添加成功！", MessageBoxIcon.Success);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            //可优化，减少判断
            if (Action == "add")
            {
                string organizationCode = txtSupplierName.Text.Trim();
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("SupplierName", organizationCode));
                SupplierInfo entity = Core.Container.Instance.Resolve<IServiceSupplierInfo>().GetEntityByFields(qryList);
                if (entity != null)
                {
                    Alert.ShowInTop("已存在名称为[ " + organizationCode + " ]的供应商，公司名称保存为[ " + entity.SupplierName + " ]！保存失败", MessageBoxIcon.Warning);
                    return;
                }
            }
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion
    }
}