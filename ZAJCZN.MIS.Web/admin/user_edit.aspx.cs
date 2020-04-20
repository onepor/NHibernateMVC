using System;
using System.Data.Entity;
using System.Linq;
using FineUIPro;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Collections.Generic;
using NHibernate.Criterion;

namespace ZAJCZN.MIS.Web.admin
{
    public partial class user_edit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreUserEdit";
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
            //User current = DB.Users
            //    .Include(u => u.Dept)
            //    .Include(u => u.Roles)
            //    .Include(u => u.Titles)
            //    .Where(u => u.ID == id).FirstOrDefault();
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

            labName.Text = current.Name;
            tbxRealName.Text = current.ChineseName;
            tbxCompanyEmail.Text = current.CompanyEmail;
            tbxEmail.Text = current.Email;
            tbxCellPhone.Text = current.CellPhone;
            tbxOfficePhone.Text = current.OfficePhone;
            tbxOfficePhoneExt.Text = current.OfficePhoneExt;
            tbxHomePhone.Text = current.HomePhone;
            tbxRemark.Text = current.Remark;
            cbxEnabled.Checked = current.Enabled;
            ddlGender.SelectedValue = current.Gender;

            // 初始化用户所属角色
            InitUserRole(current);

            // 初始化用户所属部门
            InitUserDept(current);

        }

        #region InitUserRole

        private void InitUserDept(users current)
        {
            if (current.DeptID != 0)
            {
                depts Dept = Core.Container.Instance.Resolve<IServiceDepts>().GetEntity(current.DeptID);
                if (Dept != null)
                {
                    tbSelectedDept.Text = Dept.Name;
                    hfSelectedDept.Text = Dept.ID.ToString();
                }
            }

            //// 打开编辑窗口
            //string selectDeptURL = String.Format("./user_select_dept.aspx?ids=<script>{0}</script>", hfSelectedDept.GetValueReference());
            //tbSelectedDept.OnClientTriggerClick = Window1.GetSaveStateReference(hfSelectedDept.ClientID, tbSelectedDept.ClientID)
            //        + Window1.GetShowReference(selectDeptURL, "选择用户所属的部门");

        }

        #endregion

        #region InitUserRole

        private void InitUserRole(users current)
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("UserID", current.ID));
            IList<roleusers> entityList = Core.Container.Instance.Resolve<IServiceRoleUsers>().GetAllByKeys(qryList);

            List<roles> roleList = new List<roles>();
            foreach (roleusers obj in entityList)
            {
                roles entity = Core.Container.Instance.Resolve<IServiceRoles>().GetEntity(obj.RoleID);
                roleList.Add(entity);
            }
            tbSelectedRole.Text = String.Join(",", roleList.Select(u => u.Name).ToArray());
            hfSelectedRole.Text = String.Join(",", roleList.Select(u => u.ID).ToArray());

            //// 打开编辑角色的窗口
            //string selectRoleURL = String.Format("./user_select_role.aspx?ids=<script>{0}</script>", hfSelectedRole.GetValueReference());
            //tbSelectedRole.OnClientTrigger2Click = Window1.GetSaveStateReference(hfSelectedRole.ClientID, tbSelectedRole.ClientID)
            //        + Window1.GetShowReference(selectRoleURL, "选择用户所属的角色");

        }
        #endregion

        #endregion

        #region Events

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            int id = GetQueryIntValue("id");
            users item = Core.Container.Instance.Resolve<IServiceUsers>().GetEntity(id);
            //User item = DB.Users
            //    .Include(u => u.Dept)
            //    .Include(u => u.Roles)
            //    .Include(u => u.Titles)
            //    .Where(u => u.ID == id).FirstOrDefault();
            //item.Name = tbxName.Text.Trim();
            item.ChineseName = tbxRealName.Text.Trim();
            item.Gender = ddlGender.SelectedValue;
            item.CompanyEmail = tbxCompanyEmail.Text.Trim();
            item.Email = tbxEmail.Text.Trim();
            item.CellPhone = tbxCellPhone.Text.Trim();
            item.OfficePhone = tbxOfficePhone.Text.Trim();
            item.OfficePhoneExt = tbxOfficePhoneExt.Text.Trim();
            item.HomePhone = tbxHomePhone.Text.Trim();
            item.Remark = tbxRemark.Text.Trim();
            item.Enabled = cbxEnabled.Checked;

            if (String.IsNullOrEmpty(hfSelectedDept.Text))
            {
                item.DeptID = 0;
            }
            else
            {
                item.DeptID = Convert.ToInt32(hfSelectedDept.Text);
            }
                      
            // 添加所有角色
            if (!String.IsNullOrEmpty(hfSelectedRole.Text))
            {
                //删除原来的用户角色关系
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("UserID", item.ID));
                IList<roleusers> entityList = Core.Container.Instance.Resolve<IServiceRoleUsers>().GetAllByKeys(qryList);
                foreach (roleusers obj in entityList)
                {
                    Core.Container.Instance.Resolve<IServiceRoleUsers>().Delete(obj);                   
                } 
                //创建用户角色关系
                int[] roleIDs = StringUtil.GetIntArrayFromString(hfSelectedRole.Text);
                foreach (int rolsid in roleIDs)
                {
                    roleusers obj = new roleusers { RoleID = rolsid, UserID = item.ID };
                    Core.Container.Instance.Resolve<IServiceRoleUsers>().Create(obj);
                }
            } 

            Core.Container.Instance.Resolve<IServiceUsers>().Update(item);


            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion

    }
}
