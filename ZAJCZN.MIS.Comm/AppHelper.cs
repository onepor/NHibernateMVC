using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZASOFT.IOA.Domain;
using Castle.ActiveRecord;
using System.Web;
using System.Configuration;
using System.IO;



namespace ZASOFT.IOA.Comm
{
    public static class AppHelper
    {
        /// <summary>
        /// 当前登陆用户
        /// </summary>
        public static SysUser LoginUser
        {
            get
            {
                if (HttpContext.Current.Session["LoginUser"] != null)
                {
                    return (SysUser)HttpContext.Current.Session["LoginUser"];
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session["LoginUser"] = value;
            }
        }


        /// <summary>
        /// 当前登陆用户的菜单权限
        /// </summary>
        public static IList<SysMenu> MenuAuthList
        {
            get
            {
                IList<SysMenu> lst = new List<SysMenu>();
                //1.获取用户角色列表
                if (LoginUser == null || LoginUser.SysRoleList == null) return lst;
                //LoginUser.RoleList
                //2.合并角色权限
                IList<SysRole> rolelist = LoginUser.SysRoleList;
                foreach (var role in rolelist)
                {
                    IList<SysMenu> menulist = role.SysMenuList;
                    foreach (var menu in menulist)
                    {
                        //if(lst.Where(m=>m.ID==menu.ID).Count()<1)
                        if (lst.Contains(menu) == false) lst.Add(menu);
                    }

                }

                return lst;
            }
        }


        public static int PageSize
        {
            get
            {

                return int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            }

        }


        /// <summary>
        /// 当前登陆用户的操作权限
        /// </summary>
        public static IList<SysFunc> SysFuncAuthList
        {
            get
            {
                IList<SysFunc> lst = new List<SysFunc>();
                //1.获取用户角色列表
                if (LoginUser == null || LoginUser.SysRoleList == null) return lst;
                //LoginUser.RoleList
                //2.合并角色权限
                IList<SysRole> rolelist = LoginUser.SysRoleList;
                foreach (var role in rolelist)
                {
                    IList<SysFunc> SysFunclist = role.SysFuncList;
                    foreach (var SysFunc in SysFunclist)
                    {
                        //if(lst.Where(m=>m.ID==SysFunc.ID).Count()<1)
                        if (lst.Contains(SysFunc) == false) lst.Add(SysFunc);
                    }

                }

                return lst;
            }
        }


        #region 字节转换为MB,GB,TB
        public static String GetFileSize(double size)
        {
            String[] units = new String[] { "B", "KB", "MB", "GB", "TB", "PB" };
            double mod = 1024.0;
            int i = 0;
            while (size >= mod)
            {
                size /= mod;
                i++;
            }
            return Math.Round(size) + units[i];
        }
        #endregion

        #region 删除指定文件
        public static void DeletDirFile(string filePath)
        {
            try
            {
                if (filePath != null)
                {
                    File.Delete(filePath);
                }
                else
                {
                    //todo
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion



    }
}
