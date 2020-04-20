using FineUIPro;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Helpers;
using ZAJCZN.MIS.Service;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;

namespace ZAJCZN.MIS.Web
{
    public partial class Settlement : PageBase
    {
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDefault();
            }
        }

        #endregion Page_Load

        #region 查询

        protected void BindDefault()
        {
            labDate.Text = DateTime.Now.ToShortDateString();
            labSettlementDate.Text = DateTime.Now.ToString();
            Databind();
        }

        protected void Databind()
        {
            //查询数据保存至缓存
            string start = DateTime.Now.ToString("yyyy-MM-dd");
            string end = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string sqlwhere = "SELECT sum(Population) AS personCount," +
                "COUNT(1) AS tabieCount," +
                "sum(usingInfo.Moneys) AS Moneys," +
                "sum(PrePrice) AS PrePrice," +
                "sum(DisCount) AS DisCount," +
                "sum(FactPrice) AS FactPrice," +
                "sum(Erasing) AS Erasing," +
                "(SELECT sum(FactPrice) FROM tm_tabieusinginfo WHERE OpenTime > '" + start + "' AND OpenTime < '" + end + "' AND OrderState = '3' ) AS freeMoney," +
                "(SELECT sum(FactPrice) FROM tm_tabieusinginfo WHERE OpenTime > '" + start + "' AND OpenTime < '" + end + "' AND OrderState = '4') AS ChargeMoney," +
                "sum(dishinfo.Moneys) AS BackMoneys," +
                "sum(usingInfo.GroupMoneys) AS GroupMoneys," +
                "sum(tablePay.CashMoneys) AS CashMoneys," +
                "sum(tablePay.CreditMoneys) AS CreditMoneys," +
                "sum(tablePay.VipcardMoneys) AS VipcardMoneys," +
                "sum(wx.OnlineMoneys) AS WxMoneys," +
                "sum(zfb.OnlineMoneys) AS ZfbMoneys " +
                "FROM tm_tabieusinginfo usingInfo " +
                "LEFT JOIN (SELECT Moneys,TabieUsingID FROM tm_tabiedishesinfo WHERE DishesType = '2') AS dishinfo ON dishinfo.TabieUsingID = usingInfo.ID " +
                "LEFT JOIN tm_tabiepayinfo tablePay ON tablePay.TabieUsingID = usingInfo.ID " +
                "LEFT JOIN (SELECT TabieUsingID,OnlineMoneys FROM tm_tabiepayinfo WHERE PayWayOnline = '1') AS wx ON wx.TabieUsingID = usingInfo.ID " +
                "LEFT JOIN (SELECT TabieUsingID,OnlineMoneys FROM tm_tabiepayinfo WHERE PayWayOnline = '2') AS zfb ON zfb.TabieUsingID = usingInfo.ID " +
                "WHERE OpenTime > '" + start + "' AND OpenTime < '" + end + "'";
            DataSet ds = DbHelperSQL.Query(sqlwhere);

            if (ds.Tables[0] != null)
            {
                DataRow row = ds.Tables[0].Rows[0];
                labCustomerCount.Text = row["personCount"].ToString() == "" ? "0" : row["personCount"].ToString();     //人数
                labOrderCount.Text = row["tabieCount"].ToString() == "" ? "0" : row["tabieCount"].ToString();        //订单数
                labAmountReceivable.Text = row["Moneys"].ToString() == "" ? "0.00" : row["Moneys"].ToString();      //应付金额
                labAmountCollected.Text = row["FactPrice"].ToString() == "" ? "0.00" : row["FactPrice"].ToString();    //实付金额
                labDonationAmount.Text = row["PrePrice"].ToString() == "" ? "0.00" : row["PrePrice"].ToString();      //赠送金额
                labDiscountAmount.Text = row["Erasing"].ToString() == "" ? "0.00" : row["Erasing"].ToString();       //抹零金额
                labBackAmount.Text = row["BackMoneys"].ToString() == "" ? "0.00" : row["BackMoneys"].ToString();        //退菜金额
                labSingleAmount.Text = row["freeMoney"].ToString() == "" ? "0.00" : row["freeMoney"].ToString();       //免单金额
                labCharge.Text = row["ChargeMoney"].ToString() == "" ? "0.00" : row["ChargeMoney"].ToString();           //挂账金额
                labCashAmount.Text = row["CashMoneys"].ToString() == "" ? "0.00" : row["CashMoneys"].ToString();        //现金金额
                tbxCashFact.Text = row["CashMoneys"].ToString() == "" ? "0.00" : row["CashMoneys"].ToString();            //现金实盘
                labWXAmount.Text = row["WxMoneys"].ToString() == "" ? "0.00" : row["WxMoneys"].ToString();            //微信金额
                tbxWXFact.Text = row["WxMoneys"].ToString() == "" ? "0.00" : row["WxMoneys"].ToString();                //微信实盘
                labZFBAmount.Text = row["ZfbMoneys"].ToString() == "" ? "0.00" : row["ZfbMoneys"].ToString();          //支付宝金额
                tbxZFBFact.Text = row["ZfbMoneys"].ToString() == "" ? "0.00" : row["ZfbMoneys"].ToString();              //支付宝实盘
                labCreditAmount.Text = row["CreditMoneys"].ToString() == "" ? "0.00" : row["CreditMoneys"].ToString();    //刷卡金额
                tbxCreditFact.Text = row["CreditMoneys"].ToString() == "" ? "0.00" : row["CreditMoneys"].ToString();        //刷卡实盘
                labVipAmount.Text = row["VipcardMoneys"].ToString() == "" ? "0.00" : row["VipcardMoneys"].ToString();      //会员卡金额
                tbxVipFact.Text = row["VipcardMoneys"].ToString() == "" ? "0.00" : row["VipcardMoneys"].ToString();          //会员卡实盘
                labGroupAmount.Text = row["GroupMoneys"].ToString() == "" ? "0.00" : row["GroupMoneys"].ToString();      //团购金额
                tbxGroupFact.Text = row["GroupMoneys"].ToString() == "" ? "0.00" : row["GroupMoneys"].ToString();          //团购实盘
                labDisCount.Text = row["DisCount"].ToString() == "" ? "0" : row["DisCount"].ToString();                 //折扣金额
            }
        }


        #endregion 查询

        #region 保存

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            tm_Settlement entity = new tm_Settlement();
            entity.SettlementDate = DateTime.Now.ToString("yyyy-MM-dd");
            entity.PrintTime = DateTime.Now;
            entity.CustomerCount = Int32.Parse(labCustomerCount.Text);
            entity.OrderCount = Int32.Parse(labOrderCount.Text);
            entity.AmountReceivable = decimal.Parse(labAmountReceivable.Text);
            entity.SingleAmount = decimal.Parse(labSingleAmount.Text);
            entity.ChargeAmount = decimal.Parse(labCharge.Text);
            entity.DonationAmount = decimal.Parse(labDonationAmount.Text);
            entity.DiscountAmount = decimal.Parse(labDiscountAmount.Text);
            entity.AmountCollected = decimal.Parse(labAmountCollected.Text);
            entity.CashAmount = decimal.Parse(labCashAmount.Text);
            entity.WXAmount = decimal.Parse(labWXAmount.Text);
            entity.ZFBAmount = decimal.Parse(labZFBAmount.Text);
            entity.CardAmount = decimal.Parse(labCreditAmount.Text);
            entity.MemberAmount = decimal.Parse(labVipAmount.Text);
            entity.GroupAmount = decimal.Parse(labGroupAmount.Text);
            entity.BackAmount = decimal.Parse(labBackAmount.Text);
            entity.ACCashAmount = decimal.Parse(tbxCashFact.Text.Trim());
            entity.ACWXAmount = decimal.Parse(tbxWXFact.Text.Trim());
            entity.ACZFBAmount = decimal.Parse(tbxZFBFact.Text.Trim());
            entity.ACCardAmount = decimal.Parse(tbxCreditFact.Text.Trim());
            entity.ACMemberAmount = decimal.Parse(tbxVipFact.Text.Trim());
            entity.ACGroupAmount = decimal.Parse(tbxGroupFact.Text.Trim());
            entity.DZAmount = decimal.Parse(labDisCount.Text.Trim());
            Core.Container.Instance.Resolve<IServiceSettlement>().Create(entity);

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            LocalPrint(entity);
        }

        #endregion 保存

        #region 前台打印

        /// <summary>
        /// 点菜单前台打印
        /// </summary>
        /// <param name="listDish"></param>
        /// <param name="entity"></param>
        protected void LocalPrint(tm_Settlement entity)
        {
            if (entity != null )
            {
                StringBuilder one = new StringBuilder();
                StringBuilder two = new StringBuilder();
                StringBuilder three = new StringBuilder();
                one.Append("农投良品生活馆\n");
                one.Append("交班单\n");
                one.Append("\n");
                two.Append("\n");
                two.Append("\n");
                two.Append("\n");
                three.Append("\n");
                three.Append("\n");
                three.Append("\n");
                one.Append("营业员\n");
                two.Append("\n");
                three.AppendFormat("{0}\n", User.Identity.Name);
                one.AppendFormat("营业日期：{0}\n",DateTime.Now.ToShortDateString());
                two.Append("\n");
                three.Append("\n");
                one.AppendFormat("打印时间：{0}\n", DateTime.Now.ToString());
                two.Append("\n");
                three.Append("\n");
                one.Append("---------------------------------------------------------------------------\n");
                two.Append("\n");
                three.Append("\n");

                one.Append("客数：\n");
                two.Append("\n");
                three.AppendFormat("{0}\n",entity.CustomerCount);
                one.Append("单数：\n");
                two.Append("\n");
                three.AppendFormat("{0}\n", entity.OrderCount);

                one.Append("-----------------------------------------------------------------------------\n");
                two.Append("\n");
                three.Append("\n");

                one.Append("应收金额\n");
                two.Append("\n");
                three.AppendFormat("{0}\n",entity.AmountReceivable);
                one.Append("赠送金额\n");
                two.Append("\n");
                three.AppendFormat("{0}\n", entity.DonationAmount);
                one.Append("折扣金额\n");
                two.Append("\n");
                three.AppendFormat("{0}\n", entity.DZAmount);
                one.Append("抹零金额\n");
                two.Append("\n");
                three.AppendFormat("{0}\n", entity.DiscountAmount);
                one.Append("退菜金额\n");
                two.Append("\n");
                three.AppendFormat("{0}\n", entity.BackAmount);
                one.Append("免单金额\n");
                two.Append("\n");
                three.AppendFormat("{0}\n", entity.SingleAmount);
                one.Append("挂账金额\n");
                two.Append("\n");
                three.AppendFormat("{0}\n", entity.ChargeAmount);
                one.Append("实付金额\n");
                two.Append("\n");
                three.AppendFormat("{0}\n", entity.AmountCollected);

                one.Append("-----------------------------------------------------------------------------\n");
                two.Append("\n");
                three.Append("\n");

                one.Append("票面金额\n");
                two.Append("实盘金额\n");
                three.Append("溢缺\n");
                one.AppendFormat("(现金){0}\n",entity.CashAmount);
                two.AppendFormat("{0}\n",entity.ACCashAmount);
                three.AppendFormat("{0}\n",entity.CashAmount-entity.ACCashAmount);
                one.AppendFormat("(美团){0}\n", entity.GroupAmount);
                two.AppendFormat("{0}\n",entity.ACGroupAmount);
                three.AppendFormat("{0}\n",entity.GroupAmount-entity.ACGroupAmount);
                one.AppendFormat("(微信){0}\n", entity.WXAmount);
                two.AppendFormat("{0}\n",entity.ACWXAmount);
                three.AppendFormat("{0}\n",entity.WXAmount-entity.ACWXAmount);
                one.AppendFormat("(支付宝){0}\n", entity.ZFBAmount);
                two.AppendFormat("{0}\n",entity.ACZFBAmount);
                three.AppendFormat("{0}\n",entity.ZFBAmount-entity.ACZFBAmount);
                one.AppendFormat("(刷卡){0}\n", entity.CardAmount);
                two.AppendFormat("{0}\n", entity.ACCardAmount);
                three.AppendFormat("{0}\n", entity.CardAmount - entity.ACCardAmount);
                one.AppendFormat("(会员卡){0}\n", entity.MemberAmount);
                two.AppendFormat("{0}\n", entity.ACMemberAmount);
                three.AppendFormat("{0}\n", entity.MemberAmount - entity.ACMemberAmount);

                one.Append("-----------------------------------------------------------------------------\n");
                two.Append("\n");
                three.Append("\n");
                one.Append("地址：渝北区冉家坝龙山路301号\n");
                two.Append(" \n");
                three.Append("\n");
                one.Append("电话：02367364577\n");
                two.Append(" \n");
                three.Append("\n");
                one.Append("欢迎光临\n");
                two.Append("\n");
                three.Append("\n");
                LocalPrint(one.ToString(), two.ToString(), three.ToString());
            }
        }
        #endregion 前台打印

        #region Events

        protected void tbxCashFact_TextChanged(object sender, EventArgs e)
        {
            if (tbxCashFact.Text.Trim().Equals(""))
                tbxCashFact.Text = "0";
            labCashLost.Text = (decimal.Parse(labCashAmount.Text) - decimal.Parse(tbxCashFact.Text.Trim())).ToString();
        }

        protected void tbxWXFact_TextChanged(object sender, EventArgs e)
        {
            if (tbxWXFact.Text.Trim().Equals(""))
                tbxWXFact.Text = "0";
            labWXLost.Text = (decimal.Parse(labWXAmount.Text) - decimal.Parse(tbxWXFact.Text.Trim())).ToString();
        }

        protected void tbxZFBFact_TextChanged(object sender, EventArgs e)
        {
            if (tbxZFBFact.Text.Trim().Equals(""))
                tbxZFBFact.Text = "0";
            labZFBLost.Text = (decimal.Parse(labZFBAmount.Text) - decimal.Parse(tbxZFBFact.Text.Trim())).ToString();
        }

        protected void tbxCreditFact_TextChanged(object sender, EventArgs e)
        {
            if (tbxCreditFact.Text.Trim().Equals(""))
                tbxCreditFact.Text = "0";
            labCreditLost.Text = (decimal.Parse(labCreditAmount.Text) - decimal.Parse(tbxCreditFact.Text.Trim())).ToString();
        }

        protected void tbxVipFact_TextChanged(object sender, EventArgs e)
        {
            if (tbxVipFact.Text.Trim().Equals(""))
                tbxVipFact.Text = "0";
            labVipLost.Text = (decimal.Parse(labVipAmount.Text) - decimal.Parse(tbxVipFact.Text.Trim())).ToString();
        }

        protected void tbxGroupFact_TextChanged(object sender, EventArgs e)
        {
            if (tbxGroupFact.Text.Trim().Equals(""))
                tbxGroupFact.Text = "0";
            labGroupLost.Text = (decimal.Parse(labGroupAmount.Text) - decimal.Parse(tbxGroupFact.Text.Trim())).ToString();
        }

        #endregion Events
    }
}