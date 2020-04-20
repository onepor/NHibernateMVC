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
using ZAJCZN.MIS.Helpers;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;
using System.Configuration;
using System.Data;

namespace ZAJCZN.MIS.Web
{
    public partial class DinnerTabieManager : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreCateringView";
            }
        }

        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                //绑定餐桌信息
                BindTabieInfo();
            }
        }

        private void LoadData()
        {
            // 权限检查
            CheckPowerWithButton("CoreCateringView", btnAdd);
            CheckPowerWithButton("CoreCateringView", btnContent);
            CheckPowerWithButton("CoreCateringView", btnChange);
            CheckPowerWithButton("CoreCateringView", btnMontage);
            CheckPowerWithButton("CoreCateringView", btnDishes);
            CheckPowerWithButton("CoreCateringView", btnOut);
            CheckPowerWithButton("CoreCateringView", btnCalc);
            //绑定餐区信息
            BindTabieArea();
        }

        /// <summary>
        /// 绑定餐区信息
        /// </summary>
        private void BindTabieArea()
        {
            IList<tm_Diningarea> list = Core.Container.Instance.Resolve<IServiceDiningarea>().GetAll().ToList();
            list.Insert(0, new tm_Diningarea { AreaName = "全部餐区", ID = 0 });
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        /// <summary>
        /// 绑定餐桌信息
        /// </summary>
        private void BindTabieInfo()
        {
            int areaID = GetSelectedDataKeyID(Grid1);
            //根据选择的区域获取餐台信息
            IList<ICriterion> qryList = new List<ICriterion>();
            if (areaID > 0)
            {
                qryList.Add(Expression.Eq("Diningarea_Tabie.ID", areaID));
            }
            List<tm_Tabie> listTabie = Core.Container.Instance.Resolve<IServiceTabie>().Query(qryList).OrderBy(objinfo => objinfo.Sort).ToList();

            //加载餐台信息
            StringBuilder html = new StringBuilder();
            html.Append("<table>");
            int i = 1;
            foreach (tm_Tabie tabie in listTabie)
            {
                if ((i - 1) % 6 == 0)
                {
                    html.Append("<tr>");
                }
                //餐台空，没开台
                if (tabie.TabieState == 1)
                {
                    html.AppendFormat("<td  class=\"tabieFree\">");
                }
                //餐台已开台
                else
                {
                    if (tabie.TabieState == 2)          //开台未叫菜                   
                        html.AppendFormat("<td class=\"tabieOpen\">");
                    if (tabie.TabieState == 3)          //叫菜
                        html.AppendFormat("<td class=\"tabieBC\">");
                    if (tabie.TabieState == 4)          //走菜就餐
                        html.AppendFormat("<td class=\"tabieZC\">");
                }

                html.AppendFormat("<p>{0}</p>", tabie.TabieName);
                //判断是否已开台，显示开台就餐信息
                if (tabie.TabieState > 1)
                {
                    tm_TabieUsingInfo usingEntity = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(tabie.CurrentUsingID);
                    //显示就餐信息
                    if (usingEntity != null)
                    {
                        html.AppendFormat("<p>{0}人</p><p>￥{1}</p>", usingEntity.Population, usingEntity.Moneys);
                    }
                }
                else
                {
                    html.AppendFormat("<p>0人</p><p>￥0.00</p>");
                }

                html.AppendFormat("<input  style=\"zoom: 1.5\" name=\"Tabies\" type=\"checkbox\" value=\"{0}\" />", tabie.ID);
                html.Append("</td>");
                if (i != 1 && i % 6 == 0)
                {
                    html.Append("</tr>");
                }
                i++;
            }
            html.Append("</tr>");
            html.Append("</table>");
            TabieS.InnerHtml = html.ToString();
        }

        #endregion Page_Load

        #region Events

        /// <summary>
        /// 餐台开台处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string[] id = Hiddenid.Text.TrimEnd(',').Split(',');
            if (id.Length == 1 && !string.IsNullOrEmpty(id[0]))
            {
                //获取餐台信息
                tm_Tabie tabie = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(Int32.Parse(id[0]));
                //判断是否已经开台
                if (tabie.TabieState > 1)
                {
                    Alert.ShowInTop("此餐台已开台！", "错误操作", MessageBoxIcon.Error);
                }
                else
                {
                    //创建设置开台信息
                    tm_TabieUsingInfo newUsingInfo = new tm_TabieUsingInfo();
                    newUsingInfo.TabieID = Int32.Parse(id[0]);
                    newUsingInfo.OrderState = "1";                      //开台使用
                    newUsingInfo.Moneys = 0;
                    newUsingInfo.PrePrice = 0;
                    newUsingInfo.Discount = 0;
                    newUsingInfo.FactPrice = 0;
                    newUsingInfo.Population = 0;
                    newUsingInfo.ClearTime = null;
                    newUsingInfo.OpenTime = DateTime.Now;
                    newUsingInfo.OrderNO = string.Format("XF{0}", newUsingInfo.OpenTime.ToString("yyyyMMddhhmmss"));
                    Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Create(newUsingInfo);
                    //获取当前创建的开台信息，主要是获取ID
                    IList<ICriterion> qrylist = new List<ICriterion>();
                    qrylist.Add(Expression.Eq("OrderState", "1"));
                    qrylist.Add(Expression.Eq("TabieID", tabie.ID));
                    tm_TabieUsingInfo CurrentUsingInfo = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntityByFields(qrylist);
                    if (CurrentUsingInfo != null)
                    {
                        //开台成功，修改餐台的状态信息
                        tabie.TabieState = 2;                           //设置开台
                        tabie.CurrentUsingID = CurrentUsingInfo.ID;     //标识当前开台信息
                        Core.Container.Instance.Resolve<IServiceTabie>().Update(tabie);
                        PageContext.RegisterStartupScript(WindowSelectPeople.GetShowReference("~/Dinner/DSelectPeople.aspx?id=" + CurrentUsingInfo.ID + "&tabieid=" + tabie.ID, "就餐人数"));
                    }
                    else
                    {
                        Alert.ShowInTop("餐台开台失败！", "错误操作", MessageBoxIcon.Error);
                    }
                }
            }
            if (id.Length > 1)
            {
                Alert.ShowInTop("选择了" + id.Length + "个餐台，一次只能开一个！");
            }
        }

        /// <summary>
        /// 转台处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnChange_Click(object sender, EventArgs e)
        {
            string[] id = Hiddenid.Text.TrimEnd(',').Split(',');
            if (id.Length == 1 && !string.IsNullOrEmpty(id[0]))
            {
                PageContext.RegisterStartupScript(WindowTabieChange.GetShowReference("~/Dinner/DTabieChange.aspx?id=" + id[0], "就餐转台"));
            }
            if (id.Length > 1)
            {
                Alert.ShowInTop("选择了" + id.Length + "个餐台，转台只能操作一个餐台！");
            }
        }

        /// <summary>
        /// 清台处理(暂时不用)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string[] id = Hiddenid.Text.TrimEnd(',').Split(',');
            if (id.Length >= 1 && !string.IsNullOrEmpty(id[0]))
            {
                foreach (string i in id)
                {
                    tm_Tabie entity = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(Int32.Parse(i));
                    if (entity.TabieState == 1)
                        continue;
                    if (entity.TabieState == 3)
                    {
                        entity.TabieState = 1;
                        Core.Container.Instance.Resolve<IServiceTabie>().Update(entity);
                        continue;
                    }

                    IList<ICriterion> qrylist = new List<ICriterion>();
                    qrylist.Add(Expression.Eq("TabieID", Int32.Parse(i)));
                    qrylist.Add(Expression.Lt("OrderState", "3"));
                    tm_TabieUsingInfo sb = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntityByFields(qrylist);
                    if (sb == null)
                    {
                        Alert.ShowInTop("餐桌已上菜，不可清台", "错误操作", MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        entity.TabieState = 1;
                        sb.OrderState = "4";
                        sb.ClearTime = DateTime.Now;
                        Core.Container.Instance.Resolve<IServiceTabie>().Update(entity);
                        Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(sb);
                    }
                }
            }
            BindTabieInfo();
        }

        //合台合并支付
        protected void btnMontage_Click(object sender, EventArgs e)
        {
            string[] id = Hiddenid.Text.TrimEnd(',').Split(',');
            List<int> ids = new List<int>();
            foreach (string z in id)
            {
                tm_Tabie tm_Tabie = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(Int32.Parse(z));
                //判断餐台状态是否未就餐中
                if (tm_Tabie.TabieState < 4)
                {
                    Alert.ShowInTop("未就餐的餐台不能进行合台付款！", "错误操作", MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    ids.Add(tm_Tabie.ID);
                }
            }
            if (ids.Count > 1)
            {
                //创建合并付款编号
                string MergeNO = string.Format("XSHT{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                foreach (int u in ids)
                {
                    //获取餐台信息
                    tm_Tabie tabieInfo = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(u);
                    //获取餐台当前就餐信息
                    if (tabieInfo.CurrentUsingID > 0)
                    {
                        //更新就餐信息的合并付款编号信息
                        tm_TabieUsingInfo tabieUsingInfo = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(tabieInfo.CurrentUsingID);
                        tabieUsingInfo.MergeNO = MergeNO;
                        Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(tabieUsingInfo);
                    }
                }
                //跳转到合并付款页面
                PageContext.RegisterStartupScript(WindowPay.GetShowReference("~/Dinner/DMergePay.aspx?id=" + MergeNO, "合台付款"));
            }
            else
            {
                Alert.ShowInTop("至少选择两个未结算的餐台进行合台！", "错误操作", MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 查看餐台就餐详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnContent_Click(object sender, EventArgs e)
        {
            string[] id = Hiddenid.Text.TrimEnd(',').Split(',');
            if (id.Length == 1 && !string.IsNullOrEmpty(id[0]))
            {
                int tabieId = Int32.Parse(id[0]);
                tm_Tabie tabieInfo = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(tabieId);
                if (tabieInfo.TabieState > 1)
                {
                    PageContext.Redirect("~/Dinner/DinnerTabieUsingManager.aspx?id=" + tabieInfo.CurrentUsingID + "&TabieId=" + tabieId);
                }
                else
                {
                    Alert.ShowInTop("还没开台！", "错误操作", MessageBoxIcon.Error);
                }
            }
            if (id.Length > 1)
            {
                Alert.ShowInTop("选择了" + id.Length + "个餐台，一次只能查看一个餐台详情！");
            }
        }

        /// <summary>
        /// 点菜处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDishes_Click(object sender, EventArgs e)
        {
            string[] id = Hiddenid.Text.TrimEnd(',').Split(',');
            if (id.Length == 1 && !string.IsNullOrEmpty(id[0]))
            {
                int tabieId = Int32.Parse(id[0]);
                tm_Tabie tabieInfo = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(tabieId);
                if (tabieInfo.TabieState > 1)
                {
                    PageContext.Redirect("~/Dinner/DinnerOrderMealManager.aspx?id=" + tabieInfo.CurrentUsingID + "&TabieId=" + tabieId);
                }
                else
                {
                    Alert.ShowInTop("还没开台！不可点菜", "错误操作", MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 走菜处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOut_Click(object sender, EventArgs e)
        {
            string[] id = Hiddenid.Text.TrimEnd(',').Split(',');
            if (id.Length == 1 && !string.IsNullOrEmpty(id[0]))
            {
                tm_Tabie tabie = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(Int32.Parse(id[0]));

                //判断是否已经开台
                if (tabie.TabieState > 1)
                {
                    bool isPrint = bool.Parse(ConfigurationManager.AppSettings["IsPrint"]);
                    string sql = string.Empty;
                    //获取餐点信息和就餐信息
                    tm_TabieUsingInfo usingInfo = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(tabie.CurrentUsingID);
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
                            new NetPrintHelper().Printeg("就餐走菜!!!!!", listPrinter[0], (int)usingInfo.Population, tabieInfo.TabieName, 4);
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
                }
                else
                {
                    Alert.ShowInTop("未开台，不可走菜！", "错误操作", MessageBoxIcon.Error);
                }
            }
            if (id.Length > 1)
            {
                Alert.ShowInTop("选择了" + id.Length + "个餐台，一次只能执行一个！", MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 就餐人数选择返回处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WindowSelectPeople_Close(object sender, WindowCloseEventArgs e)
        {
            string[] id = Hiddenid.Text.TrimEnd(',').Split(',');
            //获取餐台信息
            tm_Tabie tabie = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(Int32.Parse(id[0]));
            if (tabie != null)
            {
                //跳转显示餐台就餐信息
                PageContext.Redirect("~/Dinner/DinnerTabieUsingManager.aspx?id=" + tabie.CurrentUsingID + "&TabieId=" + tabie.ID);
            }
        }

        /// <summary>
        /// 合台付款返回处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WindowPay_Close(object sender, WindowCloseEventArgs e)
        {
            //绑定餐桌信息
            BindTabieInfo();
        }

        /// <summary>
        /// 交班跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalc_Click(object sender, EventArgs e)
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Gt("TabieState", 1));
            IList<tm_Tabie> listTabie = Core.Container.Instance.Resolve<IServiceTabie>().Query(qryList);

            //检查是否有没有结清的台座
            if (listTabie != null && listTabie.Count > 0)
            {
                Alert.ShowInTop("存在已开餐台！");
            }
            else
            {
                //查询是否存在今日交班实体
                List<ICriterion> qrylist = new List<ICriterion>();
                qrylist.Add(Expression.Eq("SettlementDate", DateTime.Now.ToString("yyyy-MM-dd")));
                tm_Settlement entity = Core.Container.Instance.Resolve<IServiceSettlement>().GetEntityByFields(qrylist);

                if (entity == null)
                    //交班
                    PageContext.RegisterStartupScript(WindowJiaoBan.GetShowReference("~/Dinner/Settlement.aspx", "交班"));
                else
                    Alert.ShowInTop("今日已交班，如需更改数据，请前往交班表进行反结！！！", "提示窗口", MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 转台返回处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WindowTabieChange_Close(object sender, WindowCloseEventArgs e)
        {
            //绑定餐桌信息
            BindTabieInfo();
        }

        #region Grid1 Events

        protected void Grid1_RowClick(object sender, FineUIPro.GridRowClickEventArgs e)
        {
            BindTabieInfo();
        }

        #endregion

        #region Grid2 Events

        protected void Grid2_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithLinkButtonField("CoreCateringView", Grid1, "deleteField");
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            BindTabieInfo();
        }
        #endregion

        #endregion Events

        #region 前台打印

        /// <summary>
        /// 点菜单前台打印
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
                    count.Append("\n");
                    price.AppendFormat("{0}{1}\n", item.DishesCount, item.UnitName);
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

        #endregion 前台打印


    }
}
