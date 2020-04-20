using FineUIPro;
using System;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class DSelectPeople : PageBase
    {
        protected int _id
        {
            get { return GetQueryIntValue("id"); }
        }

        protected int _tabieid
        {
            get { return GetQueryIntValue("tabieid"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string people = (Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(_id).Population).ToString();
                if (people == "0")
                    tbxPeople.Text = "0";
                else
                    tbxPeople.Text = people;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int people = Int32.Parse(tbxPeople.Text);
            tbxPeople.Text = (++people).ToString();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int people = Int32.Parse(tbxPeople.Text);
            if (people > 1)
            {
                tbxPeople.Text = (--people).ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string people = tbxPeople.Text.Trim();
            if (!string.IsNullOrEmpty(people))
            {
                tm_TabieUsingInfo entity = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(_id);
                entity.Population = Int32.Parse(people);
                entity.VipID = tbxVipCard.Text.Trim();
                Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(entity);
                PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            }
            
            //PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(ddlSheng.SelectedValue) + ActiveWindow.GetHideReference());
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            if (_tabieid > 0)
            {
                Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Delete(_id);
                tm_Tabie entity = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(_tabieid);
                entity.TabieState = 1;
                Core.Container.Instance.Resolve<IServiceTabie>().Update(entity);
            }
            PageContext.RegisterStartupScript(ActiveWindow.GetHideRefreshReference());
        }
    }
}