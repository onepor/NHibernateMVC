using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Text;
using System.Web.Services;
using System.Configuration;
using ZAJCZN.MIS.Helpers;
using System.Data;

namespace ZAJCZN.MIS.Web
{
    public partial class DinnerOrderMealManager : PageBase
    {
        protected int TabieID
        {
            get { return GetQueryIntValue("TabieId"); }
        }
        protected int TabieUsingID
        {
            get { return GetQueryIntValue("id"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tm_Tabie tabieInfo = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(TabieID);
                if (tabieInfo != null)
                {
                    labArea.Text = tabieInfo.TabieName;
                    //判断当前餐点状态，已叫菜或者走菜就餐的就不用开启叫菜功能
                    if (tabieInfo.TabieState >= 3)
                    {
                        btnReady.Hidden = true;
                    }
                    Hiddenid.Text = TabieUsingID.ToString();
                    //绑定菜品类别
                    BindFoodClass();
                    //绑定菜品信息
                    BindDishInfo();
                    //绑定显示当前点菜信息
                    BindSelectDishInfo();
                }
                else
                {
                    Alert.ShowInTop("餐台信息获取失败", MessageBoxIcon.Error);
                    PageContext.Redirect("~/Dinner/DinnerTabieManager.aspx");
                }
            }
        }

        #region 页面数据绑定

        /// <summary>
        /// 绑定菜品类别
        /// </summary>
        private void BindFoodClass()
        {
            IList<tm_FoodClass> list = Core.Container.Instance.Resolve<IServiceFoodClass>().GetAll();
            Grid1.DataSource = list.ToList().OrderBy(o => o.SortIndex);
            Grid1.DataBind();
        }

        /// <summary>
        /// 绑定显示当前点菜信息
        /// </summary>
        private void BindSelectDishInfo()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("TabieUsingID", TabieUsingID));
            qryList.Add(Expression.Eq("IsPrint", 0));
            IList<tm_TabieDishesInfo> list = Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().Query(qryList).OrderByDescending(obj => obj.ID).ToList();
            Grid2.DataSource = list;
            Grid2.DataBind();
        }

        /// <summary>
        /// 绑定菜品信息
        /// </summary>
        private void BindDishInfo()
        {
            int foodclass = GetSelectedDataKeyID(Grid1);
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("IsUsed", 1));
            if (foodclass == -1)
            {
                tm_FoodClass entity = Core.Container.Instance.Resolve<IServiceFoodClass>().GetAll().FirstOrDefault();
                qryList.Add(Expression.Eq("ClassID", entity.ID));
            }
            else
            {
                qryList.Add(Expression.Eq("ClassID", foodclass));
            }
            IList<tm_Dishes> list = Core.Container.Instance.Resolve<IServiceDishes>().Query(qryList)/*.OrderBy(objinfo => objinfo.ID).ToList()*/;
            StringBuilder html = new StringBuilder();
            html.Append("<table>");
            int i = 1;
            foreach (tm_Dishes entity in list)
            {
                if ((i - 1) % 4 == 0)
                {
                    html.Append("<tr>");
                }
                html.AppendFormat("<td class=\"dish\">", entity.ID);
                html.AppendFormat("<p>{0}</p>", entity.DishesName);
                html.AppendFormat("</p><img width =\"170\" height =\"105\" src=\"{0}\" /></p>", entity.DishesPicture);
                html.AppendFormat("<p style =\"color:red ;\"><input  style=\"zoom: 1.5\" name=\"Tabies\" type=\"checkbox\" value=\"{0}\" />[￥{1}]</p>", entity.ID, entity.SellPrice);
                html.Append("</td>");
                if (i != 1 && i % 4 == 0)
                {
                    html.Append("</tr>");
                }
                i++;
            }
            html.Append("</tr>");
            html.Append("</table>");
            TabieS.InnerHtml = html.ToString();
        }

        #endregion 页面数据绑定

        #region 菜品操作处理

        /// <summary>
        /// 叫菜单处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReady_Click(object sender, EventArgs e)
        {
            bool isPrint = bool.Parse(ConfigurationManager.AppSettings["IsPrint"]);
            string sql = string.Empty;
            //获取餐点信息和就餐信息
            tm_TabieUsingInfo usingInfo = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(TabieUsingID);
            tm_Tabie tabieInfo = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(usingInfo.TabieID);
            List<ICriterion> qrylist = new List<ICriterion>();

            //获取没有打印的菜品信息
            qrylist = new List<ICriterion>();
            qrylist.Add(Expression.Eq("TabieUsingID", usingInfo.ID));
            qrylist.Add(Expression.Eq("IsPrint", 0));
            IList<tm_TabieDishesInfo> listDish = Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().Query(qrylist);

            //获取打印机列表信息
            IList<ICriterion> qrylistPrint = new List<ICriterion>();
            qrylistPrint = new List<ICriterion>();
            qrylistPrint.Add(Expression.Eq("PrinterType", "2"));
            IList<tm_Printer> listPrinter = Core.Container.Instance.Resolve<IServicePrinter>().GetAllByKeys(qrylistPrint);

            foreach (tm_Printer item in listPrinter)
            {
                //获取对应打印机下的菜品
                sql = string.Format("SELECT DishesName,sum(DishesCount) as DishesCount,UnitName FROM tm_tabiedishesinfo WHERE TabieUsingID={0} and IsPrint=0 and PrintID={1} GROUP BY DishesName,UnitName "
                                        , usingInfo.ID, item.ID);
                DataSet ds = DbHelperSQL.Query(sql);

                if (isPrint)
                {
                    if (ds.Tables[0] != null)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            //后厨打印菜品（叫菜）
                            new NetPrintHelper().Printeg(row["DishesName"].ToString(), decimal.Parse(row["DishesCount"].ToString())
                                                        , row["UnitName"].ToString(), (int)usingInfo.Population, item, 2
                                                        , tabieInfo.TabieName);
                            new NetPrintHelper().Printeg(row["DishesName"].ToString(), decimal.Parse(row["DishesCount"].ToString())
                                                      , row["UnitName"].ToString(), (int)usingInfo.Population, item, 8
                                                      , tabieInfo.TabieName);
                        }
                    }
                }
            }
            //打印美团单(开台状态下第一次打印,如果已经叫菜或走菜不再打印)
            if (tabieInfo.TabieState < 3 && !string.IsNullOrEmpty(usingInfo.GroupName))
            {
                new NetPrintHelper().Printeg(usingInfo.GroupName, listPrinter[0], (int)usingInfo.Population, tabieInfo.TabieName, 1);
                new NetPrintHelper().Printeg(usingInfo.GroupName, listPrinter[0], (int)usingInfo.Population, tabieInfo.TabieName, 1);
            }
            if (isPrint)
            {
                LocalPrint(listDish, usingInfo);//默认打印机             
            }

            //更新就餐信息菜品信息打印状态
            sql = string.Format("UPDATE tm_tabiedishesinfo SET IsPrint=1 WHERE TabieUsingID={0} and IsPrint=0 ", usingInfo.ID);
            DbHelperSQL.ExecuteSql(sql);
            //更新餐台状态为叫菜状态
            tabieInfo.TabieState = 3;
            Core.Container.Instance.Resolve<IServiceTabie>().Update(tabieInfo);
            PageContext.Redirect("~/Dinner/DinnerTabieManager.aspx");
        }

        /// <summary>
        /// 走菜处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGo_Click(object sender, EventArgs e)
        {
            bool isPrint = bool.Parse(ConfigurationManager.AppSettings["IsPrint"]);
            string sql = string.Empty;
            //获取餐点信息和就餐信息
            tm_TabieUsingInfo usingInfo = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(TabieUsingID);
            tm_Tabie tabieInfo = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(usingInfo.TabieID);
            List<ICriterion> qrylist = new List<ICriterion>();

            //获取没有打印的菜品信息
            qrylist = new List<ICriterion>();
            qrylist.Add(Expression.Eq("TabieUsingID", usingInfo.ID));
            qrylist.Add(Expression.Eq("IsPrint", 0));
            IList<tm_TabieDishesInfo> listDish = Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().Query(qrylist);

            //获取打印机列表信息
            IList<ICriterion> qrylistPrint = new List<ICriterion>();
            qrylistPrint = new List<ICriterion>();
            qrylistPrint.Add(Expression.Eq("PrinterType", "2"));
            IList<tm_Printer> listPrinter = Core.Container.Instance.Resolve<IServicePrinter>().GetAllByKeys(qrylistPrint);

            //判断餐台是否是叫菜状态
            if (tabieInfo.TabieState == 3)
            {
                if (isPrint)
                {
                    //后厨打印菜品（叫菜） 
                    new NetPrintHelper().Printeg("餐台走菜!!!!!", listPrinter[0], (int)usingInfo.Population, tabieInfo.TabieName, 4);
                }
            }
          
            //前台打印点菜单
            if (isPrint)
            {
                LocalPrint(listDish, usingInfo);//默认打印机             
            }

            //打印美团单(开台状态下第一次打印,如果已经叫菜或走菜不再打印)
            if (tabieInfo.TabieState < 3 && !string.IsNullOrEmpty(usingInfo.GroupName))
            {
                new NetPrintHelper().Printeg(usingInfo.GroupName, listPrinter[0], (int)usingInfo.Population, tabieInfo.TabieName, 1);
                new NetPrintHelper().Printeg(usingInfo.GroupName, listPrinter[0], (int)usingInfo.Population, tabieInfo.TabieName, 1);
            }
             
            //后厨打印
            foreach (tm_Printer item in listPrinter)
            {
                //获取对应打印机下的菜品
                sql = string.Format("SELECT DishesName,sum(DishesCount) as DishesCount,UnitName FROM tm_tabiedishesinfo WHERE TabieUsingID={0} and IsPrint=0 and PrintID={1} GROUP BY DishesName,UnitName "
                                        , usingInfo.ID, item.ID);
                DataSet ds = DbHelperSQL.Query(sql);

                if (isPrint)
                {
                    if (ds.Tables[0] != null)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            //后厨打印菜品（叫菜）
                            new NetPrintHelper().Printeg(row["DishesName"].ToString(), decimal.Parse(row["DishesCount"].ToString())
                                                        , row["UnitName"].ToString(), (int)usingInfo.Population, item, 1
                                                        , tabieInfo.TabieName);
                            new NetPrintHelper().Printeg(row["DishesName"].ToString(), decimal.Parse(row["DishesCount"].ToString())
                                                      , row["UnitName"].ToString(), (int)usingInfo.Population, item, 9
                                                      , tabieInfo.TabieName);
                        }
                    }
                }
            }
          
            //更新就餐信息菜品信息打印状态
            sql = string.Format("UPDATE tm_tabiedishesinfo SET IsPrint=1 WHERE TabieUsingID={0} and IsPrint=0 ", usingInfo.ID);
            DbHelperSQL.ExecuteSql(sql);

            //更新餐台状态为走菜就餐状态
            tabieInfo.TabieState = 4;
            Core.Container.Instance.Resolve<IServiceTabie>().Update(tabieInfo);
            PageContext.Redirect("~/Dinner/DinnerTabieManager.aspx");
        }

        /// <summary>
        /// 点菜单本地前台打印
        /// </summary>
        /// <param name="listDish"></param>
        /// <param name="entity"></param>
        protected void LocalPrint(IList<tm_TabieDishesInfo> listDish, tm_TabieUsingInfo entity)
        {
            if (listDish != null && listDish.Count > 0)
            {
                tm_Tabie tabieEntity = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(entity.TabieID);
                StringBuilder sb = new StringBuilder();
                StringBuilder count = new StringBuilder();
                StringBuilder price = new StringBuilder();
                sb.AppendFormat("{0}\n", tabieEntity.TabieName);
                sb.Append("点菜单\n");
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
                foreach (tm_TabieDishesInfo item in listDish)
                {
                    //判断赠送
                    if (item.IsFree.Equals("0"))
                    {
                        //判断退菜
                        if (item.DishesType.Equals("1"))
                        {
                            sb.AppendFormat("{0}\n", item.DishesName);
                        }
                        //判断退菜
                        if (item.DishesType.Equals("2"))
                        {
                            sb.AppendFormat("{0}[退菜]\n", item.DishesName);
                        }
                        //团购菜品
                        if (item.DishesType.Equals("3"))
                        {
                            sb.AppendFormat("{0}[套餐]\n", item.DishesName);
                        }
                    }
                    else
                    {
                        sb.AppendFormat("{0}[赠送]\n", item.DishesName);
                    }
                    count.AppendFormat("{0}{1}\n", item.DishesCount, item.UnitName);
                    price.AppendFormat("{0}\n", item.Moneys);                     
                }
                sb.Append("-----------------------------------------------------------------------------\n");
                count.Append("\n");
                price.Append("\n");
                sb.Append("服务员：002\n");
                count.Append("打印机：吧台\n");
                price.Append("\n");
                LocalPrint(sb.ToString(), count.ToString(), price.ToString());
            }
        }

        #endregion 菜品操作处理

        #region Events

        //提交确认菜单
        protected void btnUp_Click(object sender, EventArgs e)
        {

            PageContext.Redirect("~/Dinner/DinnerTabieUsingManager.aspx?id=" + TabieUsingID + "&TabieId=" + TabieID);
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            PageContext.Redirect("~/Dinner/DinnerTabieManager.aspx");
        }

        protected void WindowSelectCount_Close(object sender, WindowCloseEventArgs e)
        {
            //绑定显示当前点菜信息
            BindSelectDishInfo();
        }

        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            //绑定菜品信息
            BindDishInfo();
        }

        protected void Grid2_RowDoubleClick(object sender, GridRowClickEventArgs e)
        {
            //删除选择菜品
            int ID = GetSelectedDataKeyID(Grid2);
            Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().Delete(ID);
            BindSelectDishInfo();
        }
        #endregion Events
        
    }
}
