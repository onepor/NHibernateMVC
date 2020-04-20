using FineUIPro;
using System;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Collections.Generic;
using NHibernate.Criterion;

namespace ZAJCZN.MIS.Web
{
    public partial class DSelectDishesCount : PageBase
    {
        protected int _id
        {
            get { return GetQueryIntValue("id"); }
        }

        protected int _usingid
        {
            get { return GetQueryIntValue("usingid"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
                numCount.Text = "1";
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int count = Int32.Parse(numCount.Text);
            numCount.Text = (++count).ToString();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int count = Int32.Parse(numCount.Text);
            if (count > 1)
            {
                numCount.Text = (--count).ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            tm_TabieUsingInfo usingenitty = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(_usingid);
            tm_Dishes dish = Core.Container.Instance.Resolve<IServiceDishes>().GetEntity(_id);
            tm_TabieDishesInfo entity = new tm_TabieDishesInfo();
            entity.DishesID = _id;
            entity.DishesCount = decimal.Parse(numCount.Text);
            entity.Price = dish.SellPrice;
            entity.Moneys = entity.DishesCount * entity.Price;
            entity.DishesType = "1";
            entity.IsFree = "0";
            entity.IsDiscount = dish.IsDiscount;
            entity.TabieUsingID = _usingid;
            entity.UnitName = GetSystemEnumValue("CPDW", dish.DishesUnit.ToString());
            entity.DishesName = dish.DishesName;
            entity.PrintID = dish.PrinterID;
            entity.IsPrint = 0;
            Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().Create(entity);
            usingenitty.Moneys += entity.Moneys;
            usingenitty.FactPrice += entity.Moneys;
            Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(usingenitty);
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}