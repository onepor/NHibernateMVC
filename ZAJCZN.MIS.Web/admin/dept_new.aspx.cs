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


namespace ZAJCZN.MIS.Web.admin
{
    public partial class dept_new : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreDeptNew";
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

            BindDDL();
        }

        private void BindDDL()
        {
            List<DeptTree> listdepts = new List<DeptTree>();
            foreach (depts dep in DeptHelper.Depts)
            {
                listdepts.Add(new DeptTree
                                {
                                    ID = dep.ID,
                                    Name = dep.Name,
                                    SortIndex = dep.SortIndex,
                                    ParentID = dep.ParentID,
                                    Remark = dep.Remark,
                                    Enabled = dep.Enabled
                                }
                );
            }
            List<DeptTree> depts = ResolveDDL<DeptTree>(listdepts);

            // 绑定到下拉列表（启用模拟树功能）
            ddlParent.EnableSimulateTree = true;
            ddlParent.DataTextField = "Name";
            ddlParent.DataValueField = "ID";
            ddlParent.DataSimulateTreeLevelField = "TreeLevel";
            ddlParent.DataSource = depts;
            ddlParent.DataBind();

            // 选中根节点
            ddlParent.SelectedValue = "0";
        }

        #endregion

        #region Events

        private void SaveItem()
        {
            depts item = new depts();
            item.Name = tbxName.Text.Trim();
            item.SortIndex = Convert.ToInt32(tbxSortIndex.Text.Trim());
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
            Core.Container.Instance.Resolve<IServiceDepts>().Create(item);
            //DB.Depts.Add(item);
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
