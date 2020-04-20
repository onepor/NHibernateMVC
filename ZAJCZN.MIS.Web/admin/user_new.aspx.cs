using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUIPro;
using ZAJCZN.MIS.Domain;
using NHibernate.Criterion;
using ZAJCZN.MIS.Service;


namespace ZAJCZN.MIS.Web.admin
{
    public partial class user_new : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreUserNew";
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

            // 初始化用户所属角色
            InitUserRole();

            // 初始化用户所属部门
            InitUserDept();
        }

        #region InitUserRole

        private void InitUserDept()
        {
            //// 打开编辑窗口
            //string selectDeptURL = String.Format("./user_select_dept.aspx?ids=<script>{0}</script>", hfSelectedDept.GetValueReference());
            //tbSelectedDept.OnClientTriggerClick = Window1.GetSaveStateReference(hfSelectedDept.ClientID, tbSelectedDept.ClientID)
            //        + Window1.GetShowReference(selectDeptURL, "选择用户所属的部门");

        }

        #endregion

        #region InitUserRole

        private void InitUserRole()
        {
            //// 打开编辑角色的窗口
            //string selectRoleURL = String.Format("./user_select_role.aspx?ids=<script>{0}</script>", hfSelectedRole.GetValueReference());
            //tbSelectedRole.OnClientTriggerClick = Window1.GetSaveStateReference(hfSelectedRole.ClientID, tbSelectedRole.ClientID)
            //        + Window1.GetShowReference(selectRoleURL, "选择用户所属的角色");

        }
        #endregion

        #endregion

        #region Events


        private void SaveItem()
        {
            users item = new users();
            item.Name = tbxName.Text.Trim();
            item.Password = PasswordUtil.CreateDbPassword(tbxPassword.Text.Trim());
            item.ChineseName = tbxRealName.Text.Trim();
            item.Gender = ddlGender.SelectedValue;
            item.CompanyEmail = tbxCompanyEmail.Text.Trim();
            item.Email = tbxEmail.Text.Trim();
            item.OfficePhone = tbxOfficePhone.Text.Trim();
            item.OfficePhoneExt = tbxOfficePhoneExt.Text.Trim();
            item.HomePhone = tbxHomePhone.Text.Trim();
            item.CellPhone = tbxCellPhone.Text.Trim();
            item.Remark = tbxRemark.Text.Trim();
            item.Enabled = cbxEnabled.Checked;
            item.CreateTime = DateTime.Now;

            // 添加所有部门
            if (!String.IsNullOrEmpty(hfSelectedDept.Text))
            {
                item.DeptID = Convert.ToInt32(hfSelectedDept.Text);
            }

            Core.Container.Instance.Resolve<IServiceUsers>().Create(item);

            item = new users();
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("Name", tbxName.Text.Trim()));
            item = Core.Container.Instance.Resolve<IServiceUsers>().GetEntityByFields(qryList);


            // 添加所有角色
            if (!String.IsNullOrEmpty(hfSelectedRole.Text))
            {
                int[] roleIDs = StringUtil.GetIntArrayFromString(hfSelectedRole.Text);
                foreach (int rolsid in roleIDs)
                {
                    roleusers obj = new roleusers { RoleID = rolsid, UserID = item.ID };
                    Core.Container.Instance.Resolve<IServiceRoleUsers>().Create(obj);
                }
            }

        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            string inputUserName = tbxName.Text.Trim();

            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("Name", inputUserName));
            users user = Core.Container.Instance.Resolve<IServiceUsers>().GetEntityByFields(qryList);

            if (user != null)
            {
                Alert.Show("用户 " + inputUserName + " 已经存在！");
                return;
            }

            SaveItem();

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        #endregion

    }
}
