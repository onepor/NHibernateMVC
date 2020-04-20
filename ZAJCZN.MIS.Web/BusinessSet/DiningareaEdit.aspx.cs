using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class DiningareaEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreDiningareaView";
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
                CheckPowerWithButton("CoreDiningareaEdit", btnSaveClose);

                if (action == "edit")
                {
                    Bind();
                }
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
            }
        }


        private void Bind()
        {
            tm_Diningarea entity = Core.Container.Instance.Resolve<IServiceDiningarea>().GetEntity(_id);
            txbAreaName.Text = entity.AreaName;
            numFee.Text = entity.Fee.ToString();
            numSort.Text = entity.Sort.ToString();
        }

        #endregion

        #region Events
        private void SaveItem()
        {
            tm_Diningarea entity = new tm_Diningarea();
            if (action == "edit")
            {
                entity = Core.Container.Instance.Resolve<IServiceDiningarea>().GetEntity(_id); ;
            }
            entity.AreaName = txbAreaName.Text.Trim();
            entity.Fee = decimal.Parse(numFee.Text);
            entity.Sort = Int32.Parse(numSort.Text);
            if (action == "edit")
            {
                Core.Container.Instance.Resolve<IServiceDiningarea>().Update(entity);
            }
            else
            {
                Core.Container.Instance.Resolve<IServiceDiningarea>().Create(entity);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            if (action == "add")
            {
                string areaName = txbAreaName.Text.Trim();
                int sort =Int32.Parse(numSort.Text);
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Disjunction()
                    .Add(Expression.Eq("AreaName", areaName))
                    .Add(Expression.Eq("Sort", sort))
                    );
                tm_Diningarea entity = Core.Container.Instance.Resolve<IServiceDiningarea>().GetEntityByFields(qryList);
                if (entity != null)
                {
                    Alert.ShowInTop("已存在餐区名[ " + entity.AreaName + " ]排序为[ " + entity.Sort + " ]的餐区！保存失败", MessageBoxIcon.Warning);
                    return;
                }
            }
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion
    }
}