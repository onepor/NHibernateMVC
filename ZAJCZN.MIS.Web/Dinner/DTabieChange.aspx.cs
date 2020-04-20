using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUIPro;
using ZAJCZN.MIS.Helpers;

namespace ZAJCZN.MIS.Web
{
    public partial class DTabieChange : PageBase
    {
        protected int TabieID
        {
            get { return GetQueryIntValue("id"); }
        }
        protected int TabieUsingID
        {
            get { return ViewState["usingId"] != null ? int.Parse(ViewState["usingId"].ToString()) : 0; }
            set { ViewState["usingId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //获取当前餐台信息
                tm_Tabie tabieInfo = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(TabieID);
                lblTabie.Text = tabieInfo.TabieName;
                TabieUsingID = tabieInfo.CurrentUsingID;
                //绑定其他空闲餐台信息
                BindFreeTabie();
            }
        }

        /// <summary>
        /// 绑定其他空闲餐台信息
        /// </summary>
        protected void BindFreeTabie()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("TabieState", 1));
            IList<tm_Tabie> list = Core.Container.Instance.Resolve<IServiceTabie>().Query(qryList);
            ddlTabie.DataSource = list;
            ddlTabie.DataBind();
            ddlTabie.SelectedIndex = 0;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //获取当前餐台信息
            tm_Tabie tabieInfo = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(TabieID);
            //获取就餐信息
            tm_TabieUsingInfo tabieUsingInfo = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(TabieUsingID);
            //获取转台餐台
            int tabieID = int.Parse(ddlTabie.SelectedValue);
            tm_Tabie tabieChangeInfo = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(tabieID);

            //更新就餐信息
            tabieUsingInfo.TabieID = tabieID;
            Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(tabieUsingInfo);
            //更新转台餐台信息
            tabieChangeInfo.CurrentUsingID = TabieUsingID;
            tabieChangeInfo.TabieState = tabieInfo.TabieState;
            Core.Container.Instance.Resolve<IServiceTabie>().Update(tabieChangeInfo);
            //更新原餐台信息
            tabieInfo.CurrentUsingID = 0;
            tabieInfo.TabieState = 1;
            Core.Container.Instance.Resolve<IServiceTabie>().Update(tabieInfo);

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}