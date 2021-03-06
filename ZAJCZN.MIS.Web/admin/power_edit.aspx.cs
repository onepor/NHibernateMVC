﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUIPro;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web.admin
{
    public partial class power_edit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CorePowerEdit";
            }
        }

        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHideReference();

            int id = GetQueryIntValue("id");
            powers current = Core.Container.Instance.Resolve<IServicePowers>().GetEntity(id);
            if (current == null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }

            tbxName.Text = current.Name;
            tbxGroupName.Text = current.GroupName;
            tbxTitle.Text = current.Title;
            tbxRemark.Text = current.Remark;

        }


        #endregion

        #region Events

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            int id = GetQueryIntValue("id");
            powers item = Core.Container.Instance.Resolve<IServicePowers>().GetEntity(id);
            item.Name = tbxName.Text.Trim();
            item.GroupName = tbxGroupName.Text.Trim();
            item.Title = tbxTitle.Text.Trim();
            item.Remark = tbxRemark.Text.Trim();
            Core.Container.Instance.Resolve<IServicePowers>().Update(item);

            //FineUIPro.Alert.Show("保存成功！", String.Empty, FineUIPro.Alert.DefaultIcon, FineUIPro.ActiveWindow.GetHidePostBackReference());
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion

    }
}
