using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUIPro;
using ZAJCZN.MIS.Service;
using ZAJCZN.MIS.Domain;
using NHibernate.Criterion;

namespace ZAJCZN.MIS.Web.admin
{
    public partial class menu_edit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreMenuEdit";
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
            //Menu current = DB.Menus
            //    .Include(m => m.Parent).Include(m => m.ViewPower)
            //    .Where(m => m.ID == id).FirstOrDefault();
            menus current = Core.Container.Instance.Resolve<IServiceMenus>().GetEntity(id);
            if (current == null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }

            tbxName.Text = current.Name;
            tbxUrl.Text = current.NavigateUrl;
            tbxSortIndex.Text = current.SortIndex.ToString();
            tbxIcon.Text = current.ImageUrl;
            tbxRemark.Text = current.Remark;
            if (current.ViewPowerID != 0)
            {
                powers power = Core.Container.Instance.Resolve<IServicePowers>().GetEntity(current.ViewPowerID);
                //获取权限信息
                if (power != null)
                {
                    tbxViewPower.Text = power.Name;
                }
            }


            // 绑定上级菜单下拉列表
            BindDDL(current);

            // 预置图标列表
            InitIconList(iconList);

            if (!String.IsNullOrEmpty(current.ImageUrl))
            {
                iconList.SelectedValue = current.ImageUrl;
            }

        }

        public void InitIconList(FineUIPro.RadioButtonList iconList)
        {
            string[] icons = new string[] { "tag_yellow", "tag_red", "tag_purple", "tag_pink", "tag_orange", "tag_green", "tag_blue" };
            foreach (string icon in icons)
            {
                string value = String.Format("~/res/icon/{0}.png", icon);
                string text = String.Format("<img style=\"vertical-align:bottom;\" src=\"{0}\" />&nbsp;{1}", ResolveUrl(value), icon);

                iconList.Items.Add(new RadioItem(text, value));
            }
        }

        private void BindDDL(menus current)
        {
            //List<Menu> mys = ResolveDDL<Menu>(MenuHelper.Menus, current.ID);

            List<MenuTree> listmenus = new List<MenuTree>();
            foreach (menus dep in MenuHelper.Menus)
            {
                listmenus.Add(new MenuTree
                {
                    ID = dep.ID,
                    Name = dep.Name,
                    SortIndex = dep.SortIndex,
                    TreeLevel = dep.TreeLevel,
                    ParentID = dep.ParentID,
                    Remark = dep.Remark,
                    Enabled = dep.Enabled,
                    ImageUrl = dep.ImageUrl,
                    ViewPowerID = dep.ViewPowerID
                }
                );
            }
            // 添加根节点
            listmenus.Insert(0, new MenuTree
            {
                ID = 0,
                Name = "--根节点--",
                SortIndex = 1,
                TreeLevel = 0,
                ParentID = 0,
                Enabled = true
            });
            //List<MenuTree> mys = ResolveDDL<MenuTree>(listmenus);

            // 绑定到下拉列表（启用模拟树功能和不可选择项功能）
            ddlParent.EnableSimulateTree = true;
            ddlParent.DataTextField = "Name";
            ddlParent.DataValueField = "ID";
            ddlParent.DataSimulateTreeLevelField = "TreeLevel";
            ddlParent.DataEnableSelectField = "Enabled";
            ddlParent.DataSource = listmenus;
            ddlParent.DataBind();

            if (current.ParentID != 0)
            {
                // 选中当前节点的父节点
                ddlParent.SelectedValue = current.ParentID.ToString();
            }
        }

        #endregion

        #region Events

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            int id = GetQueryIntValue("id");
            //Menu item = DB.Menus
            //    .Include(m => m.Parent).Include(m => m.ViewPower)
            //    .Where(m => m.ID == id).FirstOrDefault();
            menus item = Core.Container.Instance.Resolve<IServiceMenus>().GetEntity(id);
            if (item != null)
            {
                item.Name = tbxName.Text.Trim();
                item.NavigateUrl = tbxUrl.Text.Trim();
                item.SortIndex = Convert.ToInt32(tbxSortIndex.Text.Trim());
                item.ImageUrl = tbxIcon.Text;
                item.Remark = tbxRemark.Text.Trim();

                int parentID = Convert.ToInt32(ddlParent.SelectedValue);
                if (parentID == -1)
                {
                    item.ParentID = 0;
                }
                else
                {
                    item.ParentID = parentID;
                }

                string viewPowerName = tbxViewPower.Text.Trim();
                if (String.IsNullOrEmpty(viewPowerName))
                {
                    item.ViewPowerID = 0;
                }
                else
                {
                    //获取权限信息
                    IList<ICriterion> qryList = new List<ICriterion>();
                    qryList.Add(Expression.Eq("Name", viewPowerName));
                    powers entity = Core.Container.Instance.Resolve<IServicePowers>().GetEntityByFields(qryList);

                    item.ViewPowerID = entity != null ? entity.ID : 0;
                }
                Core.Container.Instance.Resolve<IServiceMenus>().Update(item);
            }
            //FineUIPro.Alert.Show("保存成功！", String.Empty, FineUIPro.Alert.DefaultIcon, FineUIPro.ActiveWindow.GetHidePostBackReference());
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }


        #endregion

    }
}
