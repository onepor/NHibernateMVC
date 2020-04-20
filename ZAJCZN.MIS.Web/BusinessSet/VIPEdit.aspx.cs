using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class VIPEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreVIPEdit";
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
                CheckPowerWithButton("CoreVIPEdit", btnSaveClose);

                lblDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (action == "edit")
                {
                    Bind();
                }

                btnClose.OnClientClick = ActiveWindow.GetHideReference();
            }
        }

        private void Bind()
        {
            tm_vipinfo entity = Core.Container.Instance.Resolve<IServiceVipInfo>().GetEntity(_id);
            txtRemark.Text = entity.Remark;
            lblDate.Text = entity.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss");
            txbVipPhone.Text = entity.VIPPhone;
            txbVipPhone.Readonly = true;
            txtVipName.Text = entity.VIPName;
        }

        #endregion

        #region Events
        private void SaveItem()
        {
            tm_vipinfo vipInfo = new tm_vipinfo();
            if (action == "edit")
            {
                vipInfo = Core.Container.Instance.Resolve<IServiceVipInfo>().GetEntity(_id); ;
            }
            if (action == "add")
            {             
                vipInfo.VIPPhone = txbVipPhone.Text.Trim();
                vipInfo.RegisterDate = DateTime.Now;
            }
            vipInfo.VIPName = txtVipName.Text.Trim();
            vipInfo.Remark = txtRemark.Text.Trim();
            if (action == "edit")
            {
                Core.Container.Instance.Resolve<IServiceVipInfo>().Update(vipInfo);
            }
            else
            {
                Core.Container.Instance.Resolve<IServiceVipInfo>().Create(vipInfo);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            if (action == "add")
            {
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("VIPPhone", txbVipPhone.Text.Trim()));
                tm_vipinfo vipObj = Core.Container.Instance.Resolve<IServiceVipInfo>().GetEntityByFields(qryList);

                //判断重复
                if (vipObj != null)
                {
                    Alert.Show("注册电话号码已存在！");
                    return;
                }
            }
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion
    }
}