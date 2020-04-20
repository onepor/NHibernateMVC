using System;
using System.Web;
using System.Web.Security;

using FineUIPro;
using System.Text;
using System.Linq;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using NHibernate.Criterion;
using System.Collections.Generic;
using ZAJCZN.MIS.Helpers.DEncrypt;

namespace ZAJCZN.MIS.Web
{
    public partial class _default : PageBase
    {
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();

                //TimeSpan ts = DateTime.Parse("2018-07-27") - DateTime.Now;
                //lblTime.Text = string.Format("系统试用到期时间：2018-07-27,剩余使用时间：{0}天", ts.Days);
                //if (ts.Days <= 0)
                //{ 
                //    lblTime.Text = "系统试用到期!"; 
                //}
            }
        }

        private void LoadData()
        {
            // 如果用户已经登录，则重定向到管理首页
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect(FormsAuthentication.DefaultUrl);
            }
            Window1.Title = "系统登录（家喜林门管理系统）";
            // Window1.Title = String.Format("系统登录（ZAJCZN.MIS.Web v{0}）", GetProductVersion());

        }

        #endregion

        #region Events

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string userName = tbxUserName.Text.Trim();
            string password = tbxPassword.Text.Trim();

            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("Name", userName));
            users user = Core.Container.Instance.Resolve<IServiceUsers>().GetEntityByFields(qryList);

            //users user = DB.Users.Where(u => u.Name == userName).FirstOrDefault();
            //string tes = PasswordUtil.CreateDbPassword("123456");
            //Alert.Show(tes);
            //return;

            if (user != null)
            {
                if (PasswordUtil.ComparePasswords(user.Password, password))
                {
                    if (!user.Enabled)
                    {
                        Alert.Show("用户未启用，请联系管理员！");
                    }
                    else
                    {
                        // 登录成功
                        //logger.Info(String.Format("登录成功：用户“{0}”", user.Name));

                        LoginSuccess(user);

                        return;
                    }
                }
                else
                {
                    //logger.Warn(String.Format("登录失败：用户“{0}”密码错误", userName));
                    Alert.Show("用户名或密码错误！");
                    return;
                }

            }
            else
            {
                //logger.Warn(String.Format("登录失败：用户“{0}”不存在", userName));
                Alert.Show("用户名或密码错误！");
                return;
            }

        }

        private void LoginSuccess(users user)
        {
            RegisterOnlineUser(user);

            // 用户所属的角色字符串，以逗号分隔
            string roleIDs = String.Empty;
            List<roleusers> rolesList = new List<roleusers>();

            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("UserID", user.ID));
            rolesList = Core.Container.Instance.Resolve<IServiceRoleUsers>().GetAllByKeys(qryList).ToList();
            if (rolesList != null && rolesList.Count > 0)
            {
                roleIDs = String.Join(",", rolesList.Select(r => r.RoleID).ToArray());
            }

            bool isPersistent = false;
            DateTime expiration = DateTime.Now.AddMinutes(120);
            CreateFormsAuthenticationTicket(user.Name, roleIDs, isPersistent, expiration);

            // 重定向到登陆后首页
            Response.Redirect(FormsAuthentication.DefaultUrl);
        }


        #endregion
    }
}
