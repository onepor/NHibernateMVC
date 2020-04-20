using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class VIPPrepaidManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreVIPPrepaid";
            }
        }

        #endregion

        #region Page_Load

        protected int _id
        {
            get { return GetQueryIntValue("id"); }
        }

        //生成充值单号
        protected string _OrderNO
        {
            get { return "CZ" + DateTime.Now.ToString("yyyyMMddHHmmss"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        #endregion Page_Load

        #region 保存

        protected void btnSure_Click(object sender, EventArgs e)
        {
            string Moneys = tbxMoneys.Text.Trim();      //充值金额

            if ((!string.IsNullOrEmpty(Moneys) && !Moneys.Equals("0")))
            {
                //判断是否为在线支付
                if (cblPayWay.SelectedValue.Equals("3") || cblPayWay.SelectedValue.Equals("4"))
                {
                    //判断是否已完成在线支付（前者）,如果支付失败再次支付（后者）
                    if (string.IsNullOrEmpty(hfdPayType.Text) || hfdPayType.Text.Equals("1"))
                    {
                        //判断在线支付方式
                        if (cblPayWay.SelectedValue.Equals("3"))//微信
                        {
                            //指定子页面传回的值的去处并挑战至子页面
                            PageContext.RegisterStartupScript(Window1.GetSaveStateReference(hfdPayTime.ClientID, hfdPayType.ClientID)
                                    + Window1.GetShowReference("~/Dinner/DOnlinePay.aspx?money=" + tbxMoneys.Text.Trim() + "&pay_type=010&terminal_trace=" + _OrderNO));
                            return;
                        }
                        else//支付宝
                        {
                            //指定子页面传回的值的去处并挑战至子页面
                            PageContext.RegisterStartupScript(Window1.GetSaveStateReference(hfdPayTime.ClientID, hfdPayType.ClientID)
                                    + Window1.GetShowReference("~/Dinner/DOnlinePay.aspx?money=" + tbxMoneys.Text.Trim() + "&pay_type=020&terminal_trace=" + _OrderNO));
                            return;
                        }
                    }
                }

            }
            string Presentation = tbxFree.Text.Trim();  //赠送金额

            if ((!string.IsNullOrEmpty(Moneys) && !Moneys.Equals("0")) || (!string.IsNullOrEmpty(Presentation) && !Presentation.Equals("0")))
            {
                //新增会员卡充值记录
                tm_VIPPrepaid entity = new tm_VIPPrepaid();
                entity.VipID = _id;
                entity.VIPPhone = Int32.Parse(Core.Container.Instance.Resolve<IServiceVipInfo>().GetEntity(_id).VIPPhone);
                entity.PrepaidDate = DateTime.Now;
                entity.PrepaidAmount = decimal.Parse(Moneys == "" ? "0" : Moneys);
                entity.PresentationAmount = decimal.Parse(Presentation == "" ? "0" : Presentation);
                entity.PrepaidWay = cblPayWay.SelectedValue;
                entity.Operator = User.Identity.Name;
                entity.OrderNO = _OrderNO;
                Core.Container.Instance.Resolve<IServiceVIPPrepaid>().Create(entity);

                //更新会员卡金额
                tm_vipinfo _Vipinfo = Core.Container.Instance.Resolve<IServiceVipInfo>().GetEntity(_id);
                _Vipinfo.VIPCount += (entity.PrepaidAmount + entity.PresentationAmount);
                if (_Vipinfo.VIPCount > decimal.Parse((99999999.99).ToString()) || _Vipinfo.VIPCount < 0)
                {
                    Alert.ShowInTop("充值金额不能为0！");
                    return;
                }
                Core.Container.Instance.Resolve<IServiceVipInfo>().Update(_Vipinfo);


                //清空金额输入框
                tbxMoneys.Text = String.Empty;
                tbxFree.Text = String.Empty;
                labPayState.Text = String.Empty;
                imgPayState.ImageUrl = String.Empty;
                Alert.ShowInTop("充值成功！");
                PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            }
        }

        #endregion 保存

        #region Events

        //查看充值记录
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(WindowPayRecord.GetShowReference("~/BusinessSet/VIPPrepaidEdit.aspx?id=" + _id, "充值记录"));
        }

        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            //支付成功对支付时间更新
            if (hfdPayType.Text.Equals("0"))
            {
                labPayState.Text = "在线支付成功";
                imgPayState.ImageUrl = "../res/icon/accept.png";
            }
            //支付失败
            else
            {
                //子页面传回的失败原因描述
                labPayState.Text = hfdPayTime.Text;
                imgPayState.ImageUrl = "../res/icon/cross.png";
            }
        }
        #endregion Events
    }
}