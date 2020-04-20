using FineUIPro;
using System;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Collections.Generic;
using NHibernate.Criterion;
using System.Configuration;
using ZAJCZN.MIS.Helpers;
using System.Text;
using System.IO;
using System.Drawing.Printing;
using System.Drawing;

namespace ZAJCZN.MIS.Web
{
    public partial class DSelectRetireCount : PageBase
    {
        protected int DishesID
        {
            get { return GetQueryIntValue("id"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();
                numCount.Text = "1";

                tm_TabieDishesInfo entity = Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().GetEntity(DishesID);
                if (entity != null)
                {
                    //获取菜品信息
                    tm_Dishes dish = Core.Container.Instance.Resolve<IServiceDishes>().GetEntity((int)entity.DishesID);
                    lblName.Text = dish.DishesName;
                }
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
            //获取退菜菜品信息
            tm_TabieDishesInfo tabieDishesInfo = Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().GetEntity(DishesID);
            if (tabieDishesInfo != null)
            {
                //获取开台信息
                tm_TabieUsingInfo usingEnitty = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(tabieDishesInfo.TabieUsingID);
                tm_Tabie objTabie = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(usingEnitty.TabieID);
                //获取菜品信息
                tm_Dishes dish = Core.Container.Instance.Resolve<IServiceDishes>().GetEntity((int)tabieDishesInfo.DishesID);
                //创建退菜菜品信息
                tm_TabieDishesInfo backEntity = new tm_TabieDishesInfo();
                backEntity.DishesID = tabieDishesInfo.DishesID;
                backEntity.DishesCount = -decimal.Parse(numCount.Text);
                backEntity.Price = dish.SellPrice;
                backEntity.Moneys = backEntity.DishesCount * tabieDishesInfo.Price;
                backEntity.DishesType = "2";
                backEntity.IsFree = "0";
                backEntity.IsDiscount = dish.IsDiscount;
                backEntity.TabieUsingID = tabieDishesInfo.TabieUsingID;
                backEntity.UnitName = GetSystemEnumValue("CPDW", dish.DishesUnit.ToString());
                backEntity.DishesName = dish.DishesName;
                backEntity.PrintID = dish.PrinterID;
                backEntity.IsPrint = 1;
                Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().Create(backEntity);
                //更新开台总价  
                usingEnitty.Moneys += backEntity.Moneys;
                usingEnitty.FactPrice += backEntity.Moneys;
                Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(usingEnitty);
                //打印退菜单据              
                tm_Printer objPrinter = Core.Container.Instance.Resolve<IServicePrinter>().GetEntity(tabieDishesInfo.PrintID);
                bool isPrint = bool.Parse(ConfigurationManager.AppSettings["IsPrint"]);

                if (isPrint)
                {
                    //判断是否是后厨打印单据
                    if (objPrinter.PrinterType.Equals("2"))
                    {
                        //后厨打印单据
                        new NetPrintHelper().Printeg(backEntity.DishesName, backEntity.DishesCount, backEntity.UnitName, (int)usingEnitty.Population, objPrinter, 3, objTabie.TabieName);
                    }
                    //前台打印单据
                    LocalPrint(backEntity, usingEnitty);//默认打印机             
                }
            }
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #region 打印
        /// <summary>
        /// 退菜单打印
        /// </summary>
        /// <param name="listDish">退菜菜品信息</param>
        /// <param name="entity">开台信息</param>
        protected void LocalPrint(tm_TabieDishesInfo listDish, tm_TabieUsingInfo entity)
        {
            tm_Tabie tabieEntity = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(entity.TabieID);
            StringBuilder sb = new StringBuilder();
            StringBuilder count = new StringBuilder();
            StringBuilder price = new StringBuilder();
            sb.AppendFormat("{0}\n", tabieEntity.TabieName);
            sb.Append("##退菜单##\n");
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            price.Append("\n");
            price.Append("\n");
            price.Append("\n");
            sb.Append("---------------------------------------------------------------------------\n");
            count.Append("\n");
            price.Append("\n");
            sb.AppendFormat("{0}\n", listDish.DishesName);
            count.Append("\n");
            price.AppendFormat("{0}{1}[退菜]\n", Math.Abs(listDish.DishesCount), listDish.UnitName);
            sb.Append("-----------------------------------------------------------------------------\n");
            count.Append("\n");
            price.Append("\n");
            sb.Append("服务员：002\n");
            count.Append("打印机：吧台\n");
            price.Append("\n");
            LocalPrint(sb.ToString(), count.ToString(), price.ToString());
        }

        #endregion
    }
}