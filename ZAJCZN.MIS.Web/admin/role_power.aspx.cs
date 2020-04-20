using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUIPro;
using System.Data;
using Newtonsoft.Json.Linq;
using AspNet = System.Web.UI.WebControls;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using NHibernate.Criterion;

namespace ZAJCZN.MIS.Web.admin
{
    public partial class role_power : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreRolePowerView";
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
            // 权限检查
            CheckPowerWithButton("CoreRolePowerEdit", btnGroupUpdate);



            // 每页记录数
            Grid1.PageSize = ConfigHelper.PageSize;
            BindGrid();

            // 默认选中第一个角色
            Grid1.SelectedRowIndex = 0;

            // 每页记录数
            Grid2.PageSize = ConfigHelper.PageSize;
            BindGrid2();
        }

        private void BindGrid()
        {
            IList<roles> roleList = Core.Container.Instance.Resolve<IServiceRoles>().GetAll();
            //IQueryable<Role> q = DB.Roles;

            // 排列
            //q = Sort<Role>(q, Grid1);

            Grid1.DataSource = roleList;
            Grid1.DataBind();
        }

        private Dictionary<string, bool> _currentRolePowers = new Dictionary<string, bool>();

        private void BindGrid2()
        {
            int roleId = GetSelectedDataKeyID(Grid1);
            if (roleId == -1)
            {
                Grid2.DataSource = null;
                Grid2.DataBind();
            }
            else
            {
                // 当前选中角色拥有的权限列表
                _currentRolePowers.Clear();

                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("RoleID", roleId));
                IList<rolepowers> rpowersList = Core.Container.Instance.Resolve<IServiceRolePowers>().GetAllByKeys(qryList);
                foreach (rolepowers rpower in rpowersList)
                {
                    powers power = Core.Container.Instance.Resolve<IServicePowers>().GetEntity(rpower.PowerID);
                    if (power != null)
                    {
                        string powerName = power.Name;
                        if (!_currentRolePowers.ContainsKey(powerName))
                        {
                            _currentRolePowers.Add(powerName, true);
                        }
                    }
                }
                //Role role = DB.Roles.Include(r => r.Powers).Where(r => r.ID == roleId).FirstOrDefault();
                // foreach (var power in role.Powers)
                //{
                //    string powerName = power.Name;
                //    if (!_currentRolePowers.ContainsKey(powerName))
                //    {
                //        _currentRolePowers.Add(powerName, true);
                //    }
                //}

                IList<powers> powerList = Core.Container.Instance.Resolve<IServicePowers>().GetAll();
                var q = powerList.GroupBy(p => p.GroupName);

                if (Grid2.SortField == "GroupName")
                {
                    if (Grid2.SortDirection == "ASC")
                    {
                        q = q.OrderBy(g => g.Key);
                    }
                    else
                    {
                        q = q.OrderByDescending(g => g.Key);
                    }
                }

                var powers = q.Select(g => new
                {
                    GroupName = g.Key,
                    Powers = g
                });


                Grid2.DataSource = powers;
                Grid2.DataBind();
            }

        }

        #endregion

        #region Grid1 Events

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();

            // 默认选中第一个角色
            Grid1.SelectedRowIndex = 0;

            BindGrid2();
        }

        protected void Grid1_RowClick(object sender, FineUIPro.GridRowClickEventArgs e)
        {
            BindGrid2();
        }

        #endregion

        #region Grid2 Events

        protected void Grid2_RowDataBound(object sender, FineUIPro.GridRowEventArgs e)
        {
            AspNet.CheckBoxList ddlPowers = (AspNet.CheckBoxList)Grid2.Rows[e.RowIndex].FindControl("ddlPowers");

            IGrouping<string, powers> powers = e.DataItem.GetType().GetProperty("Powers").GetValue(e.DataItem, null) as IGrouping<string, powers>;

            foreach (powers power in powers.ToList())
            {
                AspNet.ListItem item = new AspNet.ListItem();
                item.Value = power.ID.ToString();
                item.Text = power.Title;
                item.Attributes["data-qtip"] = power.Name;

                if (_currentRolePowers.ContainsKey(power.Name))
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }

                ddlPowers.Items.Add(item);
            }
        }



        protected void Grid2_Sort(object sender, GridSortEventArgs e)
        {
            Grid2.SortDirection = e.SortDirection;
            Grid2.SortField = e.SortField;
            BindGrid2();
        }

        protected void btnGroupUpdate_Click(object sender, EventArgs e)
        {
            // 在操作之前进行权限检查
            if (!CheckPower("CoreRolePowerEdit"))
            {
                CheckPowerFailWithAlert();
                return;
            }

            int roleId = GetSelectedDataKeyID(Grid1);
            if (roleId == -1)
            {
                return;
            }

            // 当前角色新的权限列表
            List<int> newPowerIDs = new List<int>();
            for (int i = 0; i < Grid2.Rows.Count; i++)
            {
                AspNet.CheckBoxList ddlPowers = (AspNet.CheckBoxList)Grid2.Rows[i].FindControl("ddlPowers");
                foreach (AspNet.ListItem item in ddlPowers.Items)
                {
                    if (item.Selected)
                    {
                        newPowerIDs.Add(Convert.ToInt32(item.Value));
                    }
                }
            }

            //先删除原来权限信息

            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("RoleID", roleId));
            IList<rolepowers> rpowersList = Core.Container.Instance.Resolve<IServiceRolePowers>().GetAllByKeys(qryList);
            foreach (rolepowers rpower in rpowersList)
            {
                Core.Container.Instance.Resolve<IServiceRolePowers>().Delete(rpower);
            }
            //更新权限信息
            foreach (int powerid in newPowerIDs)
            {
                rolepowers entity = new rolepowers { PowerID = powerid, RoleID = roleId };
                Core.Container.Instance.Resolve<IServiceRolePowers>().Create(entity);
            }
           
            //Role role = DB.Roles.Include(r => r.Powers).Where(r => r.ID == roleId).FirstOrDefault();

            //ReplaceEntities<Power>(role.Powers, newPowerIDs.ToArray());

            //DB.SaveChanges();


            Alert.ShowInTop("当前角色的权限更新成功！");
        }


        #endregion

    }
}
