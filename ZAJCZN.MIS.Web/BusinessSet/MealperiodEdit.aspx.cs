using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class MealperiodEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreMealperiodView";
            }
        }

        #endregion

        #region Page_Load

        protected string action
        {
            get
            {
                return GetQueryValue("action");
            }
        }
        protected int _id
        {
            get
            {
                return GetQueryIntValue("id");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //权限检查
                CheckPowerWithButton("CoreMealperiodEdit", btnSaveClose); 
                //绑定餐饮时间
                BindDateTime();

                if (action == "edit")
                {
                    Bind();
                }
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
            }
        }

        private void BindDateTime()
        {
            List<Tm_Enum> list = GetSystemEnumByTypeKey("YYSJ", false);
            lstStarttime.DataSource = list;
            lstStarttime.DataBind();
            lstEndtime.DataSource = list;
            lstEndtime.DataBind();
        }

        private void Bind()
        {
            tm_Mealtime entity = Core.Container.Instance.Resolve<IServiceMealtime>().GetEntity(_id);
            txbMealsName.Text = entity.MealsName;
            lstStarttime.SelectedValue = entity.StartTime.ToString();
            lstEndtime.SelectedValue = entity.EndTime.ToString();
            radioIsTomorrow.SelectedValue = entity.IsTomorrow;
            tbxRemark.Text = entity.Remark;
        }

        #endregion

        #region Events
        private void SaveItem()
        {
            tm_Mealtime entity = new tm_Mealtime();
            if (action == "edit")
            {
                entity = Core.Container.Instance.Resolve<IServiceMealtime>().GetEntity(_id); ;
            }
            entity.MealsName = txbMealsName.Text.Trim();
            entity.StartTime = lstStarttime.SelectedValue;
            entity.EndTime = lstEndtime.SelectedValue;
            entity.IsTomorrow = radioIsTomorrow.SelectedValue;
            entity.Remark = tbxRemark.Text.Trim();
            if (action == "edit")
            {
                Core.Container.Instance.Resolve<IServiceMealtime>().Update(entity);
            }
            else
            {
                Core.Container.Instance.Resolve<IServiceMealtime>().Create(entity);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            if (action == "add")
            {
                string mealsName = txbMealsName.Text.Trim();
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("MealsName", mealsName));
                tm_Mealtime entity = Core.Container.Instance.Resolve<IServiceMealtime>().GetEntityByFields(qryList);
                if (entity != null)
                {
                    Alert.ShowInTop("已存在餐段[ " + mealsName + " ]！保存失败", MessageBoxIcon.Warning);
                    return;
                }
            }
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion
    }
}