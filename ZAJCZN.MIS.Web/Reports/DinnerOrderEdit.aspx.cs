using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Data;
using System.Linq;

namespace ZAJCZN.MIS.Web
{
    public partial class DinnerOrderEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreDinnerOrderView";
            }
        }

        #endregion

        #region 加载


        protected int _id
        {
            get { return GetQueryIntValue("id"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            //权限检查
            tm_TabieUsingInfo tm_tabieUsingInfo = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(_id);
            tm_Tabie entity = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(tm_tabieUsingInfo.TabieID);
            txbTitle.Text = entity.Diningarea_Tabie.AreaName + "区" + entity.TabieName;

            //绑定就餐信息
            BindTabieUingInfo();
            //绑定就餐点菜信息
            BindTabieDishesInfo();
        }
        #endregion

        #region 绑定数据

        /// <summary>
        /// 绑定就餐信息
        /// </summary>
        private void BindTabieUingInfo()
        {
            tm_TabieUsingInfo tabieUsing = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(_id);
            txbPopulation.Text = tabieUsing.Population.ToString();
            txbMoneys.Text = string.Format("{0}元", tabieUsing.Moneys.ToString());
            txbClearTime.Text = tabieUsing.ClearTime.ToString();
            txbOpenTime.Text = tabieUsing.OpenTime.ToString();
            txbPrePrice.Text = string.Format("{0}元", tabieUsing.PrePrice.ToString());
            txbFactPrice.Text = string.Format("{0}元", tabieUsing.FactPrice.ToString());
            switch (tabieUsing.OrderState)
            {
                case "1":
                    txtPayType.Text = "就餐中";
                    break;
                case "2":
                    txtPayType.Text = "已结账";
                    break;
                case "3":
                    txtPayType.Text = string.Format("【免单】{0}", tabieUsing.FreeReason);
                    break;
                case "4":
                    txtPayType.Text = string.Format("【挂账】{0}", tabieUsing.Charge);
                    break;
                default:
                    txtPayType.Text = "就餐中";
                    break;
            }

            lblML.Text = string.Format("{0}元", tabieUsing.Erasing.ToString());
            lblVipCard.Text = tabieUsing.VipID;
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("TabieUsingID", _id));
            //获取支付信息
            tm_TabiePayInfo payInfo = Core.Container.Instance.Resolve<IServiceTabiePayInfo>().GetEntityByFields(qryList);
            if (payInfo != null)
            {
                lblCash.Text = string.Format("{0}元", payInfo.CashMoneys.ToString());
                lblCard.Text = string.Format("{0}元", payInfo.CreditMoneys.ToString());
                lblMember.Text = string.Format("{0}元", payInfo.VipcardMoneys.ToString());
                lblWX.Text = string.Format("{0}元", payInfo.OnlineMoneys.ToString());
                lblZFB.Text = string.Format("{0}元", payInfo.ZFBMoneys.ToString());
                lblGroupNO.Text = payInfo.GroupCardNO;
                lblGroup.Text = string.IsNullOrEmpty(payInfo.PayWayGroup) || !payInfo.PayWayGroup.Equals("1") ? "" : string.Format("【{0}】{1}元", tabieUsing.GroupName, payInfo.GroupMoneys);
            }
        }

        /// <summary>
        /// 绑定就餐点菜信息
        /// </summary>
        private void BindTabieDishesInfo()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("TabieUsingID", _id));
            Order[] orderList = new Order[1];
            Order orderli = new Order(Grid1.SortField, Grid1.SortDirection == "ASC" ? true : false);
            orderList[0] = orderli;

            IList<tm_TabieDishesInfo> list = Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().GetAllByKeys(qryList, orderList);
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        /// <summary>
        /// 获取菜品名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetDishesNmae(string id)
        {
            tm_Dishes entity = Core.Container.Instance.Resolve<IServiceDishes>().GetEntity(Int32.Parse(id));
            return entity == null ? "" : entity.DishesName;
        }

        #endregion

        #region Events


        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindTabieDishesInfo();
        }



        #endregion

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            PageContext.Redirect("~/Reports/DinnerOrderManage.aspx");
        }
    }
}