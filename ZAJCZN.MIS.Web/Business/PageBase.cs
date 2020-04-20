using System;
using System.Web.Security;
using System.Web.UI;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using System.Web;
using System.Data.Entity;
using FineUIPro;
using ZAJCZN.MIS.Domain;
using NHibernate.Criterion;
using ZAJCZN.MIS.Service;
using System.Text;
using NPinyin;
using System.IO;
using System.Drawing.Printing;
using System.Drawing;

namespace ZAJCZN.MIS.Web
{
    public class PageBase : System.Web.UI.Page
    {
        #region 只读静态变量

        // Session key
        private static readonly string SK_ONLINE_UPDATE_TIME = "OnlineUpdateTime";
        //private static readonly string SK_USER_ROLE_ID = "UserRoleId";

        private static readonly string CHECK_POWER_FAIL_PAGE_MESSAGE = "您无权访问此页面！";
        private static readonly string CHECK_POWER_FAIL_ACTION_MESSAGE = "您无权进行此操作！";



        #endregion

        #region 浏览权限

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public virtual string ViewPower
        {
            get
            {
                return String.Empty;
            }
        }

        #endregion

        #region 页面初始化

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // 此用户是否有访问此页面的权限
            if (!CheckPowerView())
            {
                CheckPowerFailWithPage();
                return;
            }

            // 设置主题
            if (PageManager.Instance != null)
            {
                var pm = PageManager.Instance;
                var themeValue = ConfigHelper.Theme;
                // 是否为内置主题
                if (IsSystemTheme(themeValue))
                {
                    pm.CustomTheme = String.Empty;
                    pm.Theme = (Theme)Enum.Parse(typeof(Theme), themeValue, true);
                }
                else
                {
                    pm.CustomTheme = themeValue;
                }
            }

            UpdateOnlineUser(User.Identity.Name);

            // 设置页面标题
            Page.Title = ConfigHelper.Title;
        }

        private bool IsSystemTheme(string themeName)
        {
            themeName = themeName.ToLower();
            string[] themes = Enum.GetNames(typeof(Theme));
            foreach (string theme in themes)
            {
                if (theme.ToLower() == themeName)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 请求参数

        /// <summary>
        /// 获取查询字符串中的参数值
        /// </summary>
        protected string GetQueryValue(string queryKey)
        {
            return Request.QueryString[queryKey];
        }


        /// <summary>
        /// 获取查询字符串中的参数值
        /// </summary>
        protected int GetQueryIntValue(string queryKey)
        {
            int queryIntValue = -1;
            try
            {
                queryIntValue = Convert.ToInt32(Request.QueryString[queryKey]);
            }
            catch (Exception)
            {
                // TODO
            }

            return queryIntValue;
        }

        #endregion

        #region 表格相关

        protected int GetSelectedDataKeyID(Grid grid)
        {
            int id = -1;
            int rowIndex = grid.SelectedRowIndex;
            if (rowIndex >= 0)
            {
                id = Convert.ToInt32(grid.DataKeys[rowIndex][0]);
            }
            return id;
        }

        protected string GetSelectedDataKey(Grid grid, int dataIndex)
        {
            string data = String.Empty;
            int rowIndex = grid.SelectedRowIndex;
            if (rowIndex >= 0)
            {
                data = grid.DataKeys[rowIndex][dataIndex].ToString();
            }
            return data;
        }

        /// <summary>
        /// 获取表格选中项DataKeys的第一个值，并转化为整型列表
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        protected List<int> GetSelectedDataKeyIDs(Grid grid)
        {
            List<int> ids = new List<int>();
            foreach (int rowIndex in grid.SelectedRowIndexArray)
            {
                ids.Add(Convert.ToInt32(grid.DataKeys[rowIndex][0]));
            }

            return ids;
        }

        #endregion

        #region 在线用户相关

        protected void UpdateOnlineUser(string username)
        {
            DateTime now = DateTime.Now;
            object lastUpdateTime = Session[SK_ONLINE_UPDATE_TIME];
            if (lastUpdateTime == null || (Convert.ToDateTime(lastUpdateTime).Subtract(now).TotalMinutes > 5))
            {
                // 记录本次更新时间
                Session[SK_ONLINE_UPDATE_TIME] = now;

                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("UserName", username));
                onlines online = Core.Container.Instance.Resolve<IServiceOnlines>().GetEntityByFields(qryList);
                //Online online = DB.Onlines.Where(o => o.User.Name == username).FirstOrDefault();
                if (online != null)
                {
                    online.UpdateTime = now;
                    //DB.SaveChanges();
                    Core.Container.Instance.Resolve<IServiceOnlines>().Update(online);
                }
            }
        }

        protected void RegisterOnlineUser(users user)
        {
            int actionFlag = 0;
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("UserID", user.ID));
            onlines online = Core.Container.Instance.Resolve<IServiceOnlines>().GetEntityByFields(qryList);

            //Online online = DB.Onlines.Where(o => o.User.ID == user.ID).FirstOrDefault();

            // 如果不存在，就创建一条新的记录
            if (online == null)
            {
                online = new onlines();
                actionFlag = 1;
            }
            DateTime now = DateTime.Now;
            online.UserID = user.ID;
            online.UserName = user.Name;
            online.IPAdddress = Request.UserHostAddress;
            online.LoginTime = now;
            online.UpdateTime = now;
            if (actionFlag == 1)
            {
                Core.Container.Instance.Resolve<IServiceOnlines>().Create(online);
            }
            else
            {
                Core.Container.Instance.Resolve<IServiceOnlines>().Update(online);
            }
            //DB.SaveChanges();
            // 记录本次更新时间
            Session[SK_ONLINE_UPDATE_TIME] = now;

        }

        /// <summary>
        /// 在线人数
        /// </summary>
        /// <returns></returns>
        protected int GetOnlineCount()
        {
            DateTime lastM = DateTime.Now.AddMinutes(-15);

            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Gt("UpdateTime", lastM));
            int onlineCount = Core.Container.Instance.Resolve<IServiceOnlines>().GetRecordCountByFields(qryList);
            return onlineCount;

            //return DB.Onlines.Where(o => o.UpdateTime > lastM).Count();
        }

        #endregion

        #region 当前登录用户信息

        // http://blog.163.com/zjlovety@126/blog/static/224186242010070024282/
        // http://www.cnblogs.com/gaoshuai/articles/1863231.html
        /// <summary>
        /// 当前登录用户的角色列表
        /// </summary>
        /// <returns></returns>
        protected List<int> GetIdentityRoleIDs()
        {
            List<int> roleIDs = new List<int>();

            if (User.Identity.IsAuthenticated)
            {
                FormsAuthenticationTicket ticket = ((FormsIdentity)User.Identity).Ticket;
                string userData = ticket.UserData;

                foreach (string roleID in userData.Split(','))
                {
                    if (!String.IsNullOrEmpty(roleID))
                    {
                        roleIDs.Add(Convert.ToInt32(roleID));
                    }
                }
            }

            return roleIDs;
        }

        /// <summary>
        /// 当前登录用户名
        /// </summary>
        /// <returns></returns>
        protected string GetIdentityName()
        {
            if (User.Identity.IsAuthenticated)
            {
                return User.Identity.Name;
            }
            return String.Empty;
        }


        /// <summary>
        /// 创建表单验证的票证并存储在客户端Cookie中
        /// </summary>
        /// <param name="userName">当前登录用户名</param>
        /// <param name="roleIDs">当前登录用户的角色ID列表</param>
        /// <param name="isPersistent">是否跨浏览器会话保存票证</param>
        /// <param name="expiration">过期时间</param>
        protected void CreateFormsAuthenticationTicket(string userName, string roleIDs, bool isPersistent, DateTime expiration)
        {
            // 创建Forms身份验证票据
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                userName,                       // 与票证关联的用户名
                DateTime.Now,                   // 票证发出时间
                expiration,                     // 票证过期时间
                isPersistent,                   // 如果票证将存储在持久性 Cookie 中（跨浏览器会话保存），则为 true；否则为 false。
                roleIDs                         // 存储在票证中的用户特定的数据
             );

            // 对Forms身份验证票据进行加密，然后保存到客户端Cookie中
            string hashTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashTicket);
            cookie.HttpOnly = true;
            // 1. 关闭浏览器即删除（Session Cookie）：DateTime.MinValue
            // 2. 指定时间后删除：大于 DateTime.Now 的某个值
            // 3. 删除Cookie：小于 DateTime.Now 的某个值
            if (isPersistent)
            {
                cookie.Expires = expiration;
            }
            else
            {
                cookie.Expires = DateTime.MinValue;
            }
            Response.Cookies.Add(cookie);
        }

        #endregion

        #region 权限检查

        /// <summary>
        /// 检查当前用户是否拥有当前页面的浏览权限
        /// 页面需要先定义ViewPower属性，以确定页面与某个浏览权限的对应关系
        /// </summary>
        /// <returns></returns>
        protected bool CheckPowerView()
        {
            return CheckPower(ViewPower);
        }

        /// <summary>
        /// 检查当前用户是否拥有某个权限
        /// </summary>
        /// <param name="powerType"></param>
        /// <returns></returns>
        protected bool CheckPower(string powerName)
        {
            // 如果权限名为空，则放行
            if (String.IsNullOrEmpty(powerName))
            {
                return true;
            }

            // 当前登陆用户的权限列表
            List<string> rolePowerNames = GetRolePowerNames();
            if (rolePowerNames.Contains(powerName))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取当前登录用户拥有的全部权限列表
        /// </summary>
        /// <param name="roleIDs"></param>
        /// <returns></returns>
        protected List<string> GetRolePowerNames()
        {
            // 将用户拥有的权限列表保存在Session中，这样就避免每个请求多次查询数据库
            if (Session["UserPowerList"] == null)
            {
                List<string> rolePowerNames = new List<string>();

                // 超级管理员拥有所有权限
                if (GetIdentityName() == "administrator")
                {
                    rolePowerNames = Core.Container.Instance.Resolve<IServicePowers>().GetAll().Select(p => p.Name).ToList();
                }
                else
                {
                    List<int> roleIDs = GetIdentityRoleIDs();

                    //foreach (var role in DB.Roles.Include(r => r.Powers).Where(r => roleIDs.Contains(r.ID)))
                    //{
                    foreach (var roleid in roleIDs)
                    {
                        //获取角色权限列表
                        IList<ICriterion> qryList = new List<ICriterion>();
                        qryList.Add(Expression.Eq("RoleID", roleid));
                        IList<rolepowers> powerList = Core.Container.Instance.Resolve<IServiceRolePowers>().GetAllByKeys(qryList);

                        foreach (rolepowers rolepower in powerList)
                        {
                            powers power = Core.Container.Instance.Resolve<IServicePowers>().GetEntity(rolepower.PowerID);
                            if (!rolePowerNames.Contains(power.Name))
                            {
                                rolePowerNames.Add(power.Name);
                            }
                        }
                    }
                }

                Session["UserPowerList"] = rolePowerNames;
            }
            return (List<string>)Session["UserPowerList"];
        }

        #endregion

        #region 权限相关

        protected void CheckPowerFailWithPage()
        {
            Response.Write(CHECK_POWER_FAIL_PAGE_MESSAGE);
            Response.End();
        }

        protected void CheckPowerFailWithButton(FineUIPro.Button btn)
        {
            btn.Enabled = false;
            btn.Hidden = true;
            btn.ToolTip = CHECK_POWER_FAIL_ACTION_MESSAGE;
        }

        protected void CheckPowerFailWithLinkButtonField(FineUIPro.Grid grid, string columnID)
        {
            FineUIPro.LinkButtonField btn = grid.FindColumn(columnID) as FineUIPro.LinkButtonField;
            btn.Enabled = false;
            btn.Hidden = true;
            btn.ToolTip = CHECK_POWER_FAIL_ACTION_MESSAGE;
        }

        protected void CheckPowerFailWithWindowField(FineUIPro.Grid grid, string columnID)
        {
            FineUIPro.WindowField btn = grid.FindColumn(columnID) as FineUIPro.WindowField;
            btn.Hidden = true;
            btn.Enabled = false;
            btn.ToolTip = CHECK_POWER_FAIL_ACTION_MESSAGE;
        }

        protected void CheckPowerFailWithBoundField(FineUIPro.Grid grid, string columnID)
        {
            FineUIPro.BoundField btn = grid.FindColumn(columnID) as FineUIPro.BoundField;
            btn.Hidden = true;
            btn.ToolTip = CHECK_POWER_FAIL_ACTION_MESSAGE;
        }

        protected void CheckPowerFailWithAlert()
        {
            PageContext.RegisterStartupScript(Alert.GetShowInTopReference(CHECK_POWER_FAIL_ACTION_MESSAGE));
        }

        protected void CheckPowerWithButton(string powerName, FineUIPro.Button btn)
        {
            if (!CheckPower(powerName))
            {
                CheckPowerFailWithButton(btn);
            }
        }

        protected void CheckPowerWithLinkButtonField(string powerName, FineUIPro.Grid grid, string columnID)
        {
            if (!CheckPower(powerName))
            {
                CheckPowerFailWithLinkButtonField(grid, columnID);
            }
        }

        protected void CheckPowerWithWindowField(string powerName, FineUIPro.Grid grid, string columnID)
        {
            if (!CheckPower(powerName))
            {
                CheckPowerFailWithWindowField(grid, columnID);
            }
        }

        protected void CheckPowerWithBoundField(string powerName, FineUIPro.Grid grid, string columnID)
        {
            if (!CheckPower(powerName))
            {
                CheckPowerFailWithBoundField(grid, columnID);
            }
        }

        /// <summary>
        /// 为删除Grid中选中项的按钮添加提示信息
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="grid"></param>
        protected void ResolveDeleteButtonForGrid(FineUIPro.Button btn, Grid grid)
        {
            ResolveDeleteButtonForGrid(btn, grid, "确定要删除选中的{0}项记录吗？");
        }

        protected void ResolveDeleteButtonForGrid(FineUIPro.Button btn, Grid grid, string confirmTemplate)
        {
            ResolveDeleteButtonForGrid(btn, grid, "请至少应该选择一项记录！", confirmTemplate);
        }

        protected void ResolveDeleteButtonForGrid(FineUIPro.Button btn, Grid grid, string noSelectionMessage, string confirmTemplate)
        {
            // 点击删除按钮时，至少选中一项
            btn.OnClientClick = grid.GetNoSelectionAlertInParentReference(noSelectionMessage);
            btn.ConfirmText = String.Format(confirmTemplate, "&nbsp;<span class=\"highlight\"><script>" + grid.GetSelectedCountReference() + "</script></span>&nbsp;");
            btn.ConfirmTarget = Target.Top;
        }

        #endregion

        #region 产品版本

        public string GetProductVersion()
        {
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            return String.Format("{0}.{1}", v.Major, v.Minor);
        }

        #endregion

        #region 隐藏字段相关

        /// <summary>
        /// 从隐藏字段中获取选择的全部ID列表
        /// </summary>
        /// <param name="hfSelectedIDS"></param>
        /// <returns></returns>
        public List<int> GetSelectedIDsFromHiddenField(FineUIPro.HiddenField hfSelectedIDS)
        {
            JArray idsArray = new JArray();

            string currentIDS = hfSelectedIDS.Text.Trim();
            if (!String.IsNullOrEmpty(currentIDS))
            {
                idsArray = JArray.Parse(currentIDS);
            }
            else
            {
                idsArray = new JArray();
            }
            return new List<int>(idsArray.ToObject<int[]>());
        }

        /// <summary>
        /// 跨页保持选中项 - 将表格当前页面选中行对应的数据同步到隐藏字段中
        /// </summary>
        /// <param name="hfSelectedIDS"></param>
        /// <param name="grid"></param>
        public void SyncSelectedRowIndexArrayToHiddenField(FineUIPro.HiddenField hfSelectedIDS, Grid grid)
        {
            List<int> ids = GetSelectedIDsFromHiddenField(hfSelectedIDS);

            List<int> selectedRows = new List<int>();
            if (grid.SelectedRowIndexArray != null && grid.SelectedRowIndexArray.Length > 0)
            {
                selectedRows = new List<int>(grid.SelectedRowIndexArray);
            }

            if (grid.IsDatabasePaging)
            {
                for (int i = 0, count = Math.Min(grid.PageSize, (grid.RecordCount - grid.PageIndex * grid.PageSize)); i < count; i++)
                {
                    int id = Convert.ToInt32(grid.DataKeys[i][0]);
                    if (selectedRows.Contains(i))
                    {
                        if (!ids.Contains(id))
                        {
                            ids.Add(id);
                        }
                    }
                    else
                    {
                        if (ids.Contains(id))
                        {
                            ids.Remove(id);
                        }
                    }
                }
            }
            else
            {
                int startPageIndex = grid.PageIndex * grid.PageSize;
                for (int i = startPageIndex, count = Math.Min(startPageIndex + grid.PageSize, grid.RecordCount); i < count; i++)
                {
                    int id = Convert.ToInt32(grid.DataKeys[i][0]);
                    if (selectedRows.Contains(i - startPageIndex))
                    {
                        if (!ids.Contains(id))
                        {
                            ids.Add(id);
                        }
                    }
                    else
                    {
                        if (ids.Contains(id))
                        {
                            ids.Remove(id);
                        }
                    }
                }
            }

            hfSelectedIDS.Text = new JArray(ids).ToString(Formatting.None);
        }

        /// <summary>
        /// 跨页保持选中项 - 根据隐藏字段的数据更新表格当前页面的选中行
        /// </summary>
        /// <param name="hfSelectedIDS"></param>
        /// <param name="grid"></param>
        public void UpdateSelectedRowIndexArray(FineUIPro.HiddenField hfSelectedIDS, Grid grid)
        {
            List<int> ids = GetSelectedIDsFromHiddenField(hfSelectedIDS);

            List<int> nextSelectedRowIndexArray = new List<int>();
            if (grid.IsDatabasePaging)
            {
                for (int i = 0, count = Math.Min(grid.PageSize, (grid.RecordCount - grid.PageIndex * grid.PageSize)); i < count; i++)
                {
                    int id = Convert.ToInt32(grid.DataKeys[i][0]);
                    if (ids.Contains(id))
                    {
                        nextSelectedRowIndexArray.Add(i);
                    }
                }
            }
            else
            {
                int nextStartPageIndex = grid.PageIndex * grid.PageSize;
                for (int i = nextStartPageIndex, count = Math.Min(nextStartPageIndex + grid.PageSize, grid.RecordCount); i < count; i++)
                {
                    int id = Convert.ToInt32(grid.DataKeys[i][0]);
                    if (ids.Contains(id))
                    {
                        nextSelectedRowIndexArray.Add(i - nextStartPageIndex);
                    }
                }
            }
            grid.SelectedRowIndexArray = nextSelectedRowIndexArray.ToArray();
        }

        #endregion

        #region 模拟树的下拉列表

        protected List<T> ResolveDDL<T>(List<T> mys) where T : ICustomTree, ICloneable, IKeyID, new()
        {
            return ResolveDDL<T>(mys, -1, true);
        }

        protected List<T> ResolveDDL<T>(List<T> mys, int currentId) where T : ICustomTree, ICloneable, IKeyID, new()
        {
            return ResolveDDL<T>(mys, currentId, true);
        }


        // 将一个树型结构放在一个下列列表中可供选择
        protected List<T> ResolveDDL<T>(List<T> source, int currentID, bool addRootNode) where T : ICustomTree, ICloneable, IKeyID, new()
        {
            List<T> result = new List<T>();

            if (addRootNode)
            {
                // 添加根节点
                T root = new T();
                root.Name = "--根节点--";
                root.ID = -1;
                root.TreeLevel = 0;
                root.Enabled = true;
                result.Add(root);
            }

            foreach (T item in source)
            {
                T newT = (T)item.Clone();
                result.Add(newT);

                // 所有节点的TreeLevel加一
                if (addRootNode)
                {
                    newT.TreeLevel++;
                }
            }

            // currentId==-1表示当前节点不存在
            if (currentID != -1)
            {
                // 本节点不可点击（也就是说当前节点不可能是当前节点的父节点）
                // 并且本节点的所有子节点也不可点击，你想如果当前节点跑到子节点的子节点，那么这些子节点就从树上消失了
                bool startChileNode = false;
                int startTreeLevel = 0;
                foreach (T my in result)
                {
                    if (my.ID == currentID)
                    {
                        startTreeLevel = my.TreeLevel;
                        my.Enabled = false;
                        startChileNode = true;
                    }
                    else
                    {
                        if (startChileNode)
                        {
                            if (my.TreeLevel > startTreeLevel)
                            {
                                my.Enabled = false;
                            }
                            else
                            {
                                startChileNode = false;
                            }
                        }
                    }
                }
            }

            return result;
        }

        #endregion

        #region 图片相关
        protected readonly static List<string> VALID_FILE_TYPES = new List<string> { "jpg", "bmp", "gif", "jpeg", "png" };

        protected static bool ValidateFileType(string fileName)
        {
            string fileType = String.Empty;
            int lastDotIndex = fileName.LastIndexOf(".");
            if (lastDotIndex >= 0)
            {
                fileType = fileName.Substring(lastDotIndex + 1).ToLower();
            }

            if (VALID_FILE_TYPES.Contains(fileType))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 附件相关
        protected readonly static List<string> VALID_FILES_TYPES = new List<string> { "xls", "doc", "xlsx", "docx" };

        protected static bool ValidateFileSType(string fileName)
        {
            string fileType = String.Empty;
            int lastDotIndex = fileName.LastIndexOf(".");
            if (lastDotIndex >= 0)
            {
                fileType = fileName.Substring(lastDotIndex + 1).ToLower();
            }

            if (VALID_FILES_TYPES.Contains(fileType))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //除去多余附件名
        protected static string ValidateFileName(string fileName)
        {
            int lastDotIndex = fileName.LastIndexOf("_");
            if (lastDotIndex >= 0)
            {
                return fileName.Remove(0, lastDotIndex + 1);
            }
            return "";
        }
 
        #endregion

        #region 系统枚举及系统参数获取

        #region 系统枚举方法
        /// <summary>
        /// 根据枚举类型获取枚举值
        /// </summary>
        /// <param name="typeKey"></param>
        protected List<Tm_Enum> GetSystemEnumByTypeKey(string typeKey, bool isDefultVale)
        {
            List<Tm_Enum> enumList = new List<Tm_Enum>();
            IList<ICriterion> qryList = new List<ICriterion>();
            if (!string.IsNullOrEmpty(typeKey))
            {
                qryList.Add(Expression.Eq("TypeCode", typeKey));
            }

            Tm_EnumType objType = Core.Container.Instance.Resolve<IServiceEnumTypes>().GetEntityByFields(qryList);
            if (objType != null)
            {
                enumList = objType.EnumList.OrderBy(obj => obj.ShowIndex).ToList();
                if (isDefultVale)
                {
                    enumList.Insert(0, new Tm_Enum { EnumKey = "--全部--", EnumValue = "0" });
                }
            }
            return enumList;
        }

        /// <summary>
        /// 根据枚举类型和枚举值获取枚举描述
        /// </summary>
        /// <param name="typeKey"></param>
        protected string GetSystemEnumValue(string typeKey, string enumValue)
        {
            string enumKey = "";
            IList<ICriterion> qryList = new List<ICriterion>();
            if (!string.IsNullOrEmpty(typeKey))
            {
                qryList.Add(Expression.Eq("EnumTypeCode", typeKey));
            }
            if (!string.IsNullOrEmpty(typeKey))
            {
                qryList.Add(Expression.Eq("EnumValue", enumValue));
            }

            Tm_Enum objType = Core.Container.Instance.Resolve<IServiceEnums>().GetEntityByFields(qryList);
            if (objType != null)
            {
                enumKey = objType.EnumKey;
            }
            return enumKey;
        }

        #endregion 系统枚举方法

        #region 系统参数方法

        /// <summary>
        /// 根据参数分组标识获取参数列表
        /// </summary>
        /// <param name="typeKey"></param>
        protected List<Sys_Paras> GetSystemParamsmByGroupKey(string typeKey, bool isDefultVale)
        {
            List<Sys_Paras> paramList = new List<Sys_Paras>();
            IList<ICriterion> qryList = new List<ICriterion>();
            if (!string.IsNullOrEmpty(typeKey))
            {
                qryList.Add(Expression.Eq("ParaGroup", typeKey));
            }

            IList<Sys_Paras> objList = Core.Container.Instance.Resolve<IServiceSysParams>().Query(qryList);
            if (objList != null)
            {
                paramList = objList.ToList();
                if (isDefultVale)
                {
                    paramList.Insert(0, new Sys_Paras { ParaName = "--请选择--", ParaKey = "0" });
                }
            }
            return paramList;
        }

        /// <summary>
        /// 根据参数分组标识和参数标识获取参数值
        /// </summary>
        /// <param name="groupeKey">参数分组标识</param>
        /// <param name="paramkey"参数标识>参数标识</param>
        /// <returns>参数值</returns>
        protected string GetSystemParamValueByKey(string groupeKey, string paramkey)
        {
            string paramValue = "";
            IList<ICriterion> qryList = new List<ICriterion>();
            if (!string.IsNullOrEmpty(groupeKey))
            {
                qryList.Add(Expression.Eq("ParaGroup", groupeKey));
            }
            if (!string.IsNullOrEmpty(paramkey))
            {
                qryList.Add(Expression.Eq("ParaKey", paramkey));
            }

            Sys_Paras objType = Core.Container.Instance.Resolve<IServiceSysParams>().GetEntityByFields(qryList);
            if (objType != null)
            {
                paramValue = objType.ParaValue;
            }
            return paramValue;
        }

        #endregion 系统参数方法

        #endregion 系统枚举获取

        #region 中文转拼音首字母

        protected string GetChinesePY(string chineseString)
        {
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            string strPY = Pinyin.GetInitials(chineseString, gb2312);
            return strPY;
        }

        #endregion 中文转拼音首字母

        #region 本地打印

        private StringReader strPrint;
        private StringReader strCount;
        private StringReader strPrice;

        public bool LocalPrint(string sb, string count, string price)
        {
            bool result = true;
            try
            {
                strPrint = new StringReader(sb);
                strCount = new StringReader(count);
                strPrice = new StringReader(price);
                PrintDocument pd = new PrintDocument();
                pd.PrintController = new System.Drawing.Printing.StandardPrintController();
                pd.DefaultPageSettings.Margins.Top = 0;
                pd.DefaultPageSettings.Margins.Left = 0;
                pd.PrinterSettings.PrinterName = "5890X"; //pd.DefaultPageSettings.PrinterSettings.PrinterName;//默认打印机
                pd.PrintPage += new PrintPageEventHandler(this.LocalPrintPaging);
                pd.Print();
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                if (strPrint != null && strCount != null & strPrice != null)
                {
                    strPrint.Close();
                    strCount.Close();
                    strPrice.Close();
                }
            }
            return result;
        }

        private void LocalPrintPaging(object sender, PrintPageEventArgs ev)
        {
            Font printFont = new Font("Arial", 8);//打印字体
            Font printFont2 = new Font("微软雅黑", 14);//标题
            float linesPerPage = 0;
            float yPos = 0;
            float yPos2 = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            String line = "";
            String line2 = "";
            String line3 = "";
            linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics);
            while (count < linesPerPage && ((line = strPrint.ReadLine()) != null) && ((line2 = strCount.ReadLine()) != null) && ((line3 = strPrice.ReadLine()) != null))
            {
                if (count < 2)
                {
                    yPos2 = topMargin + (count * printFont2.GetHeight(ev.Graphics));
                    ev.Graphics.DrawString(line, printFont2, Brushes.Black,
                   (ev.PageBounds.Width - ev.PageBounds.Width / 2 - ev.Graphics.MeasureString(line, printFont2).Width / 2), yPos2, new StringFormat());
                }                                                               //获取字符串所占像素
                else
                {
                    yPos = yPos2 + ((count + 1) * printFont.GetHeight(ev.Graphics)) + 10;
                    if (line == "欢迎光临")
                    {
                        ev.Graphics.DrawString(line, printFont2, Brushes.Black,
                   (ev.PageBounds.Width - ev.PageBounds.Width / 2 - ev.Graphics.MeasureString(line, printFont2).Width / 2), yPos, new StringFormat());
                    }
                    else
                    {
                        ev.Graphics.DrawString(line, printFont, Brushes.Black,
                           leftMargin, yPos - 10, new StringFormat());
                        ev.Graphics.DrawString(line2, printFont, Brushes.Black,
                           (ev.PageBounds.Width - ev.PageBounds.Width / 2), yPos - 10, new StringFormat());
                        ev.Graphics.DrawString(line3, printFont, Brushes.Black,
                           (ev.PageBounds.Width - ev.PageBounds.Width / 4 - 10), yPos - 10, new StringFormat());
                    }
                }
                count++;
            }
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }

        #endregion 本地打印

    }
}
