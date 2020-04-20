using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class TabieEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreTabieView";
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
                CheckPowerWithButton("CoreTabieEdit", btnSaveClose);
                //绑定销售模式
                BindDateTime();

                if (action == "edit")
                {
                    Bind();
                }
                else
                {
                    tm_Diningarea area = Core.Container.Instance.Resolve<IServiceDiningarea>().GetEntity(_id);
                    labDiningArea.Text = area.AreaName;
                }
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
            }
        }

        private void BindDateTime()
        {
            List<Tm_Enum> list = GetSystemEnumByTypeKey("XSMS", false);
            lstSalesModel.DataSource = list;
            lstSalesModel.DataBind();
        }

        private void Bind()
        {
            tm_Tabie entity = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(_id);
            labDiningArea.Text = entity.Diningarea_Tabie.AreaName;
            txbTabieName.Text = entity.TabieName;
            lstSalesModel.SelectedValue = entity.SalesModel;
            numTabieNumber.Text = entity.TabieNumber;
            numSort.Text = entity.Sort.ToString();
        }

        #endregion

        #region Events
        private void SaveItem()
        {
            tm_Tabie entity = new tm_Tabie();
            if (action == "edit")
            {
                entity = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(_id); ;
            }
            if (action == "add")
            {
                tm_Diningarea area= Core.Container.Instance.Resolve<IServiceDiningarea>().GetEntity(_id);
                entity.Diningarea_Tabie = area;
            }
            entity.TabieName = txbTabieName.Text.Trim();
            entity.TabieNumber = numTabieNumber.Text;
            entity.SalesModel = lstSalesModel.SelectedValue;
            entity.Sort = Int32.Parse(numSort.Text);
            if (action == "edit")
            {
                Core.Container.Instance.Resolve<IServiceTabie>().Update(entity);
            }
            else
            {
                Core.Container.Instance.Resolve<IServiceTabie>().Create(entity);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            if (action == "add")
            {
                string tabieName = txbTabieName.Text.Trim();
                int sort = Int32.Parse(numSort.Text);
                string tabieNumber= numTabieNumber.Text;
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("Diningarea_Tabie.ID", _id));
                qryList.Add(Expression.Disjunction()
                    .Add(Expression.Eq("TabieName", tabieName))
                    .Add(Expression.Eq("TabieNumber", tabieNumber))
                    .Add(Expression.Eq("Sort", sort))
                    );
                tm_Tabie entity = Core.Container.Instance.Resolve<IServiceTabie>().GetEntityByFields(qryList);
                if (entity != null)
                {
                    Alert.ShowInTop("餐区[ " + labDiningArea.Text + " ]已存在餐台名为[ " + entity.TabieName + " ]编号为[ "+entity.TabieNumber+" ]排序为[ " + entity.Sort + " ]的餐台！保存失败", MessageBoxIcon.Warning);
                    return;
                }
            }
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion
    }
}