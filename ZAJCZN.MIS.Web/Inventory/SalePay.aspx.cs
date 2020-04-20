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

namespace ZAJCZN.MIS.Web
{
    public partial class SalePay : PageBase
    {
        #region 加载

        protected string OrderNO
        {
            get { return GetQueryValue("id"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("OrderNO", OrderNO));
                ContractOrderInfo orderInfo = Core.Container.Instance.Resolve<IServiceSaleOrder>().GetEntityByFields(qryList);
                labMoneys.Text = orderInfo.OrderAmount.ToString();
                labFactPrice.Text = orderInfo.OrderAmount.ToString();
            }
        }
        #endregion

        #region 保存
        protected void Accept_Click(object sender, EventArgs e)
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("OrderNO", OrderNO));
            ContractOrderInfo orderInfo = Core.Container.Instance.Resolve<IServiceSaleOrder>().GetEntityByFields(qryList);

            decimal Amount = 0;
            tm_SalePayInfo PayEntity = new tm_SalePayInfo();
            if (!string.IsNullOrEmpty(nbxVip.Text) && decimal.Parse(nbxVip.Text) > 0)
            {
                PayEntity.VipcardMoneys = decimal.Parse(nbxVip.Text);
                PayEntity.PayWayVipcard = "1";
                Amount += decimal.Parse(nbxVip.Text);
            }
            if (!string.IsNullOrEmpty(nbCash.Text) && decimal.Parse(nbCash.Text) > 0)
            {
                PayEntity.CashMoneys = decimal.Parse(nbCash.Text);
                PayEntity.PayWayCash = "1";
                Amount += decimal.Parse(nbCash.Text);
            }
            if (!string.IsNullOrEmpty(nbCard.Text) && decimal.Parse(nbCard.Text) > 0)
            {
                PayEntity.CreditMoneys = decimal.Parse(nbCard.Text);
                PayEntity.PayWayCredit = "1";
                Amount += decimal.Parse(nbCard.Text);
            }
            if (!string.IsNullOrEmpty(nbWX.Text) && decimal.Parse(nbWX.Text) > 0)
            {
                PayEntity.OnlineMoneys = decimal.Parse(nbWX.Text);
                PayEntity.PayWayOnline = "1";
                Amount += decimal.Parse(nbWX.Text);
            }
            if (!string.IsNullOrEmpty(nbZFB.Text) && decimal.Parse(nbZFB.Text) > 0)
            {
                PayEntity.OnlineMoneys = decimal.Parse(nbZFB.Text);
                PayEntity.PayWayOnline = "2";
                Amount += decimal.Parse(nbZFB.Text);
            }
          
            PayEntity.PayTime = DateTime.Now;
            PayEntity.SaleOrderID = orderInfo.ID;         

            Core.Container.Instance.Resolve<IServiceSalePayInfo>().Create(PayEntity);
            //更新订单状态为正式订单       
            orderInfo.IsTemp = 0;  
            Core.Container.Instance.Resolve<IServiceSaleOrder>().Update(orderInfo);
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        #endregion
    }
}