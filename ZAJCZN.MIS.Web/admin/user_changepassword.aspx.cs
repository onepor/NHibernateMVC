﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;
using System.Linq;
using FineUIPro;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web.admin
{
    public partial class user_changepassword : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreUserChangePassword";
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
            users current = Core.Container.Instance.Resolve<IServiceUsers>().GetEntity(id);
            if (current == null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }

            if (current.Name == "administrator" && GetIdentityName() != "administrator")
            {
                Alert.Show("你无权编辑超级管理员！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }

            labUserName.Text = current.Name;
            labUserRealName.Text = current.ChineseName;
        }

        #endregion

        #region Events

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            int id = GetQueryIntValue("id");
            users item = Core.Container.Instance.Resolve<IServiceUsers>().GetEntity(id);
            item.Password = PasswordUtil.CreateDbPassword(tbxPassword.Text.Trim());
            Core.Container.Instance.Resolve<IServiceUsers>().Update (item);

            //Alert.Show("保存成功！", String.Empty, Alert.DefaultIcon, ActiveWindow.GetHidePostBackReference());
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        #endregion

    }
}
