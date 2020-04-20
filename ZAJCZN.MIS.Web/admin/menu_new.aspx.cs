using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUIPro;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using NHibernate.Criterion;


namespace ZAJCZN.MIS.Web.admin
{
    public partial class menu_new : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreMenuNew";
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

            //// 模块名称列表
            //ddlModules.DataSource = ModuleTypeHelper.GetAppModules();
            //ddlModules.DataBind();

            //ddlModules.SelectedValue = ModuleTypeHelper.Module2String(ModuleType.None);

            BindDDL();

            InitIconList(iconList);
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

        private void BindDDL()
        {
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

            //List<MenuTree> menus = ResolveDDL<MenuTree>(listmenus);

            // 绑定到下拉列表（启用模拟树功能）
            ddlParent.EnableSimulateTree = true;
            ddlParent.DataTextField = "Name";
            ddlParent.DataValueField = "ID";
            ddlParent.DataSimulateTreeLevelField = "TreeLevel";
            ddlParent.DataSource = listmenus;
            ddlParent.DataBind();

            // 选中根节点
            ddlParent.SelectedValue = "0";
        }

        #endregion

        #region Events

        private void SaveItem()
        {
            menus item = new menus();
            item.Name = tbxName.Text.Trim();
            item.NavigateUrl = tbxUrl.Text.Trim();
            item.SortIndex = Convert.ToInt32(tbxSortIndex.Text.Trim());
            item.Remark = tbxRemark.Text.Trim();
            item.ImageUrl = tbxIcon.Text;

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
            Core.Container.Instance.Resolve<IServiceMenus>().Create(item);
            //DB.SaveChanges();

        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();

            //Alert.Show("添加成功！", String.Empty, ActiveWindow.GetHidePostBackReference());
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion

    }
}
