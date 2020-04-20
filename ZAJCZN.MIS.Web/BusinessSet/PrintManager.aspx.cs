using KZKJ.IOA.Helpers;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;

namespace KZKJ.IOA.Web
{
    public partial class PrintManager : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            new NetPrintHelper().Printeg();
        }

        #region 打印
        //打印
        protected void Unnamed_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder count = new StringBuilder();
            StringBuilder price = new StringBuilder();
            sb.Append("农投良品\n");
            sb.Append("结账单(顾客)\n");
            sb.Append("桌  位：正大厅\n");
            sb.Append("账单编号：1234567890\n");
            sb.Append("营业日期：2018-12-30\n");
            sb.Append("开台时间：2018-12-30 23:59\n");
            sb.Append("结账时间：2018-12-30 23:59\n");
            sb.Append("客  数：10\n");
            sb.Append("收款机号 \n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            count.Append("收银员：002 \n");
            price.Append("\n");
            price.Append("\n");
            price.Append("\n");
            price.Append("\n");
            price.Append("\n");
            price.Append("\n");
            price.Append("\n");
            price.Append("\n");
            price.Append("\n");
            sb.Append("---------------------------------------------------------------------------\n");
            count.Append("\n");
            price.Append("\n");
            sb.Append("菜品名称\n");
            count.Append("数量 \n");
            price.Append("金额 \n");
            sb.Append("---------------------------------------------------------------------------\n");
            count.Append("\n");
            price.Append("\n");
            string[] i = new string[7] { "炸鸡,1,只,50", "啤酒,2,瓶,5", "女朋友,3,位,无价之宝", "广场,4,个,9999999", "癞皮狗,5,条,倒贴价", "中华田园犬,6,条,555555", "大帝国柴犬,7,条,1000" };
            foreach (var item in i)
            {
                string[] x = item.Split(',');
                sb.AppendFormat("{0}\n", x[0]);
                count.AppendFormat("{0}{1}\n", x[1], x[2]);
                price.AppendFormat("{0}元\n", x[3]);
            }
            sb.Append("-----------------------------------------------------------------------------\n");
            count.Append("\n");
            price.Append("\n");

            sb.Append("菜目小计：\n");
            count.Append("\n");
            price.Append("100000.00\n");
            sb.Append("消费合计：\n");
            count.Append("\n");
            price.Append("1000.00\n");
            sb.Append("赠送金额：\n");
            count.Append("\n");
            price.Append("200.00\n");
            sb.Append("应付金额：\n");
            count.Append("\n");
            price.Append("800.00\n");

            sb.Append("-----------------------------------------------------------------------------\n");
            count.Append("\n");
            price.Append("\n");
            sb.Append("挂账：\n");
            count.Append("\n");
            price.Append("800.00\n");
            sb.Append("-----------------------------------------------------------------------------\n");
            count.Append("\n");
            price.Append("\n");

            sb.Append("地址：渝北区冉家坝龙山路301号\n");
            count.Append(" \n");
            price.Append("\n");
            sb.Append("电话：02367364577\n");
            count.Append(" \n");
            price.Append("\n");
            sb.Append("\n");
            count.Append("欢迎光临\n");
            price.Append("\n");
            Print(sb.ToString(), count.ToString(), price.ToString());
        }
        private StringReader sr;
        private StringReader _count;
        private StringReader _price;

        public bool Print(string sb, string count, string price)
        {
            bool result = true;
            try
            {
                sr = new StringReader(sb);
                _count = new StringReader(count);
                _price = new StringReader(price);
                PrintDocument pd = new PrintDocument();
                pd.PrintController = new System.Drawing.Printing.StandardPrintController();
                pd.DefaultPageSettings.Margins.Top = 0;
                pd.DefaultPageSettings.Margins.Left = 0;
                pd.PrinterSettings.PrinterName = pd.DefaultPageSettings.PrinterSettings.PrinterName;//默认打印机
                pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                pd.Print();
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                if (sr != null && _count != null & _price != null)
                {
                    sr.Close();
                    _count.Close();
                    _price.Close();
                }
            }
            return result;
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {

            Font printFont = new Font("Arial", 8);//打印字体
            Font printFont2 = new Font("微软雅黑", 14);//标题
            float linesPerPage = 0;
            float yPos = 0;
            float yPos2 = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            String line = "";
            String line2 = "";
            String line3 = "";
            linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics);
            while (count < linesPerPage && ((line = sr.ReadLine()) != null) && ((line2 = _count.ReadLine()) != null) && ((line3 = _price.ReadLine()) != null))
            {
                if (count < 2)
                {
                    yPos2 = topMargin + (count * printFont2.GetHeight(ev.Graphics));
                    ev.Graphics.DrawString(line, printFont2, Brushes.Black,
                   (ev.PageBounds.Width - ev.PageBounds.Width / 2 - ev.Graphics.MeasureString(line, printFont2).Width / 2), yPos2, new StringFormat());
                }                                                               //获取字符串所占像素
                else
                {
                    yPos = yPos2 + ((count + 1) * printFont.GetHeight(ev.Graphics)) + 10;
                    if (line == "欢迎光临")
                    {
                        ev.Graphics.DrawString(line, printFont2, Brushes.Black,
                   (ev.PageBounds.Width - ev.PageBounds.Width / 2 - ev.Graphics.MeasureString(line, printFont2).Width / 2), yPos, new StringFormat());
                    }
                    else
                    {
                        ev.Graphics.DrawString(line, printFont, Brushes.Black,
                           leftMargin, yPos - 10, new StringFormat());
                        ev.Graphics.DrawString(line2, printFont, Brushes.Black,
                           (ev.PageBounds.Width - ev.PageBounds.Width / 2), yPos - 10, new StringFormat());
                        ev.Graphics.DrawString(line3, printFont, Brushes.Black,
                           (ev.PageBounds.Width - ev.PageBounds.Width / 4 - 10), yPos - 10, new StringFormat());
                    }

                }
                count++;
            }
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }


        #endregion
    }
}