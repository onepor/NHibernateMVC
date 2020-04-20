using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUIPro;
using System.Text;
using System.Linq;
using System.Data.Entity;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using NHibernate.Criterion;

namespace ZAJCZN.MIS.Web.admin
{
    public partial class user_view : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreUserView";
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
            //User item = DB.Users
            //    .Include(u => u.Dept)
            //    .Include(u => u.Roles)
            //    .Include(u => u.Titles)
            //    .Where(u => u.ID == id).FirstOrDefault();
            //item.Name = tbxName.Text.Trim();
            if (current == null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }

            labName.Text = current.Name;
            labRealName.Text = current.ChineseName;
            labCompanyEmail.Text = current.CompanyEmail;
            labEmail.Text = current.Email;
            labCellPhone.Text = current.CellPhone;
            labOfficePhone.Text = current.OfficePhone;
            labOfficePhoneExt.Text = current.OfficePhoneExt;
            labHomePhone.Text = current.HomePhone;
            labRemark.Text = current.Remark;
            labEnabled.Text = current.Enabled ? "启用" : "禁用";
            labGender.Text = current.Gender;

            // 用户所属角色
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("UserID", current.ID));
            IList<roleusers> entityList = Core.Container.Instance.Resolve<IServiceRoleUsers>().GetAllByKeys(qryList);

            List<roles> roleList = new List<roles>();
            foreach (roleusers obj in entityList)
            {
                roles entity = Core.Container.Instance.Resolve<IServiceRoles>().GetEntity(obj.RoleID);
                roleList.Add(entity);
            }           
            labRole.Text = String.Join(",", roleList.Select(r => r.Name).ToArray());
             

            // 用户所属的部门
            if (current.DeptID != 0)
            {
                depts Dept = Core.Container.Instance.Resolve<IServiceDepts>().GetEntity(current.DeptID);
                labDept.Text = Dept.Name;
            }

        }

        #endregion

    }
}
