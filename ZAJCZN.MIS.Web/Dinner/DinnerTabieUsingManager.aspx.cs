using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using ZAJCZN.MIS.Helpers;
using System.Configuration;

namespace ZAJCZN.MIS.Web
{
    public partial class DinnerTabieUsingManager : PageBase
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
                    //判断当前餐点状态，已叫菜或者走菜就餐的就不用开启叫菜功能
                    if (tabieInfo.TabieState >= 3)
                    {
                        btnReady.Hidden = true;
                    }

                    //绑定套餐信息
                    BindGroup();
                    //加载就餐相关信息
                    LoadData();
                }
                else
                {
                    Alert.ShowInTop("餐台信息获取失败", MessageBoxIcon.Error);
                    PageContext.Redirect("~/Dinner/DinnerTabieManager.aspx");
                }
            }
        }

        #endregion Page_Load

        #region 加载绑定

        /// <summary>
        /// 绑定套餐信息
        /// </summary>
        private void BindGroup()
        {
            IList<ICriterion> qrylist = new List<ICriterion>();
            qrylist.Add(Expression.Eq("IsEnabled", "1"));
            IList<tm_SetMealInfo> list = Core.Container.Instance.Resolve<IServiceSetMealInfo>().GetAllByKeys(qrylist);
            ddlGroup.DataSource = list;
            ddlGroup.DataBind();
            ddlGroup.Items.Insert(0, new ListItem { Text = "", Value = "0" });
        }

        /// <summary>
        /// 加载就餐相关信息
        /// </summary>
        private void LoadData()
        {
            //权限检查
            CheckPowerWithButton("CoreCateringView", btnNew);

            tm_TabieUsingInfo Usingentity = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(TabieUsingID);
            //绑定开台时间
            dltStart.Text = Usingentity.OpenTime.ToString();
            //绑定团购信息
            ddlGroup.SelectedValue = Usingentity.GroupMoneys.ToString();
            tbGroup.Text = Usingentity.GroupCardNO;

            tbxPeople.Text = Usingentity.Population.ToString();
            tbxVipID.Text = Usingentity.VipID;
            tm_Tabie entity = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(TabieID);
            groupTabie.Title = string.Format("{0}区{1}【消费单号：{2}】", entity.Diningarea_Tabie.AreaName, entity.TabieName, Usingentity.OrderNO);
            btnPay.OnClientClick = WindowPay.GetShowReference("~/Dinner/DPay.aspx?id=" + TabieUsingID + "&TabieId=" + TabieID, "付款");
            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
            //绑定就餐点餐信息
            BindTabieDishesInfo();
        }

        /// <summary>
        /// 绑定就餐点餐信息
        /// </summary>
        private void BindTabieDishesInfo()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("TabieUsingID", TabieUsingID));
            Order[] orderList = new Order[1];
            Order orderli = new Order(Grid1.SortField, Grid1.SortDirection == "ASC" ? true : false);
            orderList[0] = orderli;
            IList<tm_TabieDishesInfo> list = Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().GetAllByKeys(qryList, orderList);
            Grid1.DataSource = list;
            Grid1.DataBind();

            tm_TabieUsingInfo entity = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(TabieUsingID);
            labMoneys.Text = "￥" + (entity.Moneys - entity.PrePrice);
        }

        #endregion

        #region 菜品赠送处理

        protected void btnEnableIsFree_Click(object sender, EventArgs e)
        {
            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            tm_TabieUsingInfo Usingentity = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(TabieUsingID);

            // 执行数据库操作
            foreach (int ID in ids)
            {
                tm_TabieDishesInfo entity = Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().GetEntity(ID);
                if (entity.IsFree == "0")
                    continue;
                entity.IsFree = "0";
                Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().Update(entity);
                Usingentity.PrePrice -= entity.Moneys;
            }
            Usingentity.FactPrice = Usingentity.Moneys - Usingentity.PrePrice;
            Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(Usingentity);
            // 重新绑定表格
            BindTabieDishesInfo();
        }

        protected void btnDisableIsFree_Click(object sender, EventArgs e)
        {
            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            // 执行数据库操作
            tm_TabieUsingInfo Usingentity = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(TabieUsingID);

            foreach (int ID in ids)
            {
                tm_TabieDishesInfo entity = Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().GetEntity(ID);
                if (entity.DishesType == "2" || entity.DishesType == "3")
                {
                    Alert.ShowInTop("已退菜或团购套餐菜品不可赠送！");
                    continue;
                }
                if (entity.IsFree == "1")
                    continue;
                entity.IsFree = "1";
                Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().Update(entity);
                Usingentity.PrePrice += entity.Moneys;

            }
            Usingentity.FactPrice = Usingentity.Moneys - Usingentity.PrePrice;
            Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(Usingentity);
            // 重新绑定表格
            BindTabieDishesInfo();
        }

        #endregion 菜品数量、金额修改

        #region 菜品团购状态

        //改为非团购
        protected void btnDisableGroup_Click(object sender, EventArgs e)
        {
            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            tm_TabieUsingInfo Usingentity = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(TabieUsingID);

            // 执行数据库操作
            foreach (int ID in ids)
            {
                tm_TabieDishesInfo entity = Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().GetEntity(ID);
                if (entity.DishesType == "1" || entity.DishesType == "2")
                    continue;
                entity.DishesType = "1";
                Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().Update(entity);
                Usingentity.Moneys += entity.Moneys;
                Usingentity.FactPrice += entity.Moneys;
            }
            Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(Usingentity);
            // 重新绑定表格
            BindTabieDishesInfo();

        }

        //改为团购
        protected void btnEnableGroup_Click(object sender, EventArgs e)
        {
            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            tm_TabieUsingInfo Usingentity = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(TabieUsingID);

            // 执行数据库操作
            foreach (int ID in ids)
            {
                tm_TabieDishesInfo entity = Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().GetEntity(ID);
                if (entity.IsFree == "1" || entity.DishesType == "2")
                {
                    Alert.ShowInTop("赠送菜品和退菜产品不可设置为团购套餐！");
                    continue;
                }
                if (entity.DishesType == "3" || entity.DishesType == "2")
                    continue;
                entity.DishesType = "3";
                Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().Update(entity);
                Usingentity.Moneys -= entity.Moneys;
                Usingentity.FactPrice -= entity.Moneys;
            }
            Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(Usingentity);
            // 重新绑定表格
            BindTabieDishesInfo();
        }

        #endregion 菜品团购状态

        #region Events

        /// <summary>
        /// 菜品数量和价格变动处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            Dictionary<int, Dictionary<string, object>> modifiedDict = Grid1.GetModifiedDict();
            foreach (int rowIndex in modifiedDict.Keys)
            {
                int rowID = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
                tm_TabieDishesInfo tabieDishesInfo = Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().GetEntity(rowID);

                if (tabieDishesInfo.DishesType == "2" || tabieDishesInfo.IsFree == "1" || tabieDishesInfo.DishesType == "3")
                    continue;
                //更新菜品总价
                decimal Amount = tabieDishesInfo.Moneys;
                tabieDishesInfo.DishesCount = decimal.Parse(modifiedDict[rowIndex]["DishesCount"].ToString());
                tabieDishesInfo.Moneys = tabieDishesInfo.DishesCount * tabieDishesInfo.Price;
                Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().Update(tabieDishesInfo);
                //根据菜品变动前总价和变动后总价差异更新就餐信息消费金额
                tm_TabieUsingInfo tabieUsingInfo = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(TabieUsingID);
                tabieUsingInfo.Moneys += (tabieDishesInfo.Moneys - Amount);
                tabieUsingInfo.FactPrice += (tabieDishesInfo.Moneys - Amount);
                Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(tabieUsingInfo);
            }
            //绑定餐台点菜信息
            BindTabieDishesInfo();
        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindTabieDishesInfo();
        }

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindTabieDishesInfo();
        }

        protected void WindowSelectRetireCount_Close(object sender, EventArgs e)
        {
            BindTabieDishesInfo();
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            PageContext.Redirect("~/Dinner/DinnerTabieManager.aspx");
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            PageContext.Redirect("~/Dinner/DinnerOrderMealManager.aspx?id=" + TabieUsingID + "&TabieId=" + TabieID);
        }
        #endregion

        #region 页面数据转换

        public string GetDishesNmae(string id)
        {
            tm_Dishes entity = Core.Container.Instance.Resolve<IServiceDishes>().GetEntity(Int32.Parse(id));
            return entity == null ? "" : entity.DishesName;
        }

        public string GetDishesType(string id)
        {
            return id == "1" ? "" : id == "2" ? "退菜" : "套餐";
        }

        public string GetDishesFree(string id)
        {
            return id == "0" ? "否" : "赠送";
        }

        #endregion 页面数据转换

        #region 餐台信息输入信息处理

        /// <summary>
        /// 就餐人数修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbxPeople_Blur(object sender, EventArgs e)
        {
            tm_TabieUsingInfo entity = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(TabieUsingID);
            if (string.IsNullOrEmpty(tbxPeople.Text))
                entity.Population = 0;
            else
                entity.Population = Int32.Parse(tbxPeople.Text);
            Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(entity);
        }

        /// <summary>
        /// 会员卡号修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbxVipID_Blur(object sender, EventArgs e)
        {
            string vip = tbxVipID.Text;
            tm_TabieUsingInfo entity = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(TabieUsingID);
            if (!string.IsNullOrEmpty(vip))
            {
                //此处需添加代码验证VIP账号
                //
                //
                entity.VipID = vip;
            }
            else
                entity.VipID = "";
            Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(entity);
        }

        /// <summary>
        /// 美团券号修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tbGroup_Blur(object sender, EventArgs e)
        {
            string txtGroup = tbGroup.Text;
            tm_TabieUsingInfo entity = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(TabieUsingID);
            if (!string.IsNullOrEmpty(txtGroup))
            {
                entity.GroupCardNO = txtGroup;
                Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(entity);
            }
        }

        /// <summary>
        /// 修改开台时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dltStart_TextChanged(object sender, EventArgs e)
        {
            tm_TabieUsingInfo entity = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(TabieUsingID);
            entity.OpenTime = DateTime.Parse(DateTime.Parse(dltStart.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss"));
            Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(entity);
        }

        /// <summary>
        /// 团购信息修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            tm_TabieUsingInfo entity = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(TabieUsingID);

            //加入团购菜品前删除原团购菜品信息
            List<ICriterion> qryDishesList = new List<ICriterion>();
            qryDishesList.Add(Expression.Eq("TabieUsingID", entity.ID) && Expression.Eq("DishesType", "3"));
            IList<tm_TabieDishesInfo> disheslist = Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().Query(qryDishesList);
            foreach (tm_TabieDishesInfo item in disheslist)
            {
                //删除原团购菜品信息
                Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().Delete(item.ID);
            }

            if (!ddlGroup.SelectedValue.Equals("0"))
            {
                entity.GroupName = ddlGroup.SelectedText;
                entity.GroupMoneys = decimal.Parse(ddlGroup.SelectedValue);

                #region 加入选择的团购菜品

                //根据套餐名称获取团餐信息
                List<ICriterion> qrysetMealList = new List<ICriterion>();
                qrysetMealList.Add(Expression.Eq("SetMealName", ddlGroup.SelectedText));
                tm_SetMealInfo setMealInfo = Core.Container.Instance.Resolve<IServiceSetMealInfo>().GetEntityByFields(qrysetMealList);
                if (setMealInfo != null)
                {
                    //查询出团购餐下的菜品信息
                    List<ICriterion> qrysetMealdetailList = new List<ICriterion>();
                    qrysetMealdetailList.Add(Expression.Eq("SetMealID", setMealInfo.ID));
                    IList<tm_SetMealDetail> setMealDetailList = Core.Container.Instance.Resolve<IServiceSetMealDetail>().Query(qrysetMealdetailList);

                    //遍历团购菜品信息添加餐台菜品信息
                    foreach (tm_SetMealDetail item in setMealDetailList)
                    {
                        tm_TabieDishesInfo Dishentity = new tm_TabieDishesInfo();
                        Dishentity.DishesID = item.DishID;
                        Dishentity.DishesCount = item.DishCount;
                        Dishentity.Price = item.Price ?? 0;
                        Dishentity.Moneys = item.TotalPrice ?? 0;
                        Dishentity.DishesType = "3";
                        Dishentity.IsFree = "0";
                        Dishentity.TabieUsingID = entity.ID;
                        //获取菜品信息
                        tm_Dishes _Dishes = Core.Container.Instance.Resolve<IServiceDishes>().GetEntity(Dishentity.DishesID);
                        Dishentity.UnitName = GetSystemEnumValue("CPDW", _Dishes.DishesUnit.ToString());
                        Dishentity.DishesName = _Dishes.DishesName;
                        Dishentity.PrintID = _Dishes.PrinterID;
                        Dishentity.IsPrint = 0;
                        Dishentity.IsDiscount = 0;
                        //创建点菜信息
                        Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().Create(Dishentity);
                    }
                }

                #endregion 加入选择的团购菜品
            }
            else
            {
                entity.GroupName = "";
                entity.GroupMoneys = 0;
                entity.GroupCardNO = "";
            }
            Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().Update(entity);
            //绑定菜品信息
            BindTabieDishesInfo();
        }

        #endregion 餐台信息输入信息处理

        #region 菜品操作

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
                                                        , row["UnitName"].ToString(), (int)usingInfo.Population, item, 2
                                                        , tabieInfo.TabieName);
                            new NetPrintHelper().Printeg(row["DishesName"].ToString(), decimal.Parse(row["DishesCount"].ToString())
                                                      , row["UnitName"].ToString(), (int)usingInfo.Population, item, 8
                                                      , tabieInfo.TabieName);
                        }
                    }
                }
            }

            //更新就餐信息菜品信息打印状态
            sql = string.Format("UPDATE tm_tabiedishesinfo SET IsPrint=1 WHERE TabieUsingID={0} and IsPrint=0 ", usingInfo.ID);
            DbHelperSQL.ExecuteSql(sql);
            //更新餐台状态为叫菜状态
            tabieInfo.TabieState = 3;
            Core.Container.Instance.Resolve<IServiceTabie>().Update(tabieInfo);
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

        /// <summary>
        /// 退菜处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            int id = 0;
            if (ids.Count > 0)
            {
                id = ids[0];
                tm_TabieDishesInfo entity = Core.Container.Instance.Resolve<IServiceTabieDishesInfo>().GetEntity(id);
                if (entity.IsFree == "1")
                {
                    Alert.ShowInTop("赠送菜品不可退菜！");
                    return;
                }
                if (entity.DishesType == "2" || entity.DishesType == "3")
                {
                    Alert.ShowInTop("已退菜品或团购菜品不可退菜！");
                    return;
                }
                PageContext.RegisterStartupScript(WindowSelectRetireCount.GetShowReference("~/Dinner/DSelectRetireCount.aspx?id=" + id, "退菜数量"));
            }
            // 重新绑定表格
            BindTabieDishesInfo();
        }

        #endregion 菜品操作

        #region 前台打印

        /// <summary>
        /// 客户预打单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            tm_TabieUsingInfo tabieUsingInfo = Core.Container.Instance.Resolve<IServiceTabieUsingInfo>().GetEntity(TabieUsingID);
            tm_Tabie tabieEntity = Core.Container.Instance.Resolve<IServiceTabie>().GetEntity(tabieUsingInfo.TabieID);
            StringBuilder sb = new StringBuilder();
            StringBuilder count = new StringBuilder();
            StringBuilder price = new StringBuilder();
            sb.Append("农投良品生活馆\n");
            sb.Append("顾客联\n");
            sb.AppendFormat("桌  位:{0}\n", tabieEntity.TabieName);
            sb.AppendFormat("账单编号：{0}\n", tabieUsingInfo.OrderNO);
            sb.AppendFormat("营业日期：{0}\n", DateTime.Now.ToShortDateString());
            sb.AppendFormat("开台时间：{0}\n", tabieUsingInfo.OpenTime.ToString("yyyy-MM-dd hh:mm:ss"));
            sb.AppendFormat("结账时间：{0}\n", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            sb.AppendFormat("客  数：{0}\n", tabieUsingInfo.Population);
            sb.AppendFormat("收款机号 \n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            count.Append("\n");
            count.AppendFormat("收银员：{0} \n", User.Identity.Name);
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

            //获取点菜信息
            string sql = string.Format("SELECT DishesName,sum(DishesCount) as DishesCount,SUM(Moneys) as Moneys,UnitName,IsFree,DishesType FROM tm_tabiedishesinfo WHERE TabieUsingID={0} GROUP BY DishesName,UnitName,IsFree,DishesType ORDER BY DishesName "
                                    , tabieUsingInfo.ID);
            DataSet ds = DbHelperSQL.Query(sql);

            if (ds.Tables[0] != null)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    //判断赠送
                    if (!row["IsFree"].ToString().Equals("1"))
                    {
                        //判断退菜
                        if (row["DishesType"].ToString().Equals("1"))
                        {
                            sb.AppendFormat("{0}\n", row["DishesName"].ToString());
                        }
                        if (row["DishesType"].ToString().Equals("2"))
                        {
                            sb.AppendFormat("{0}[退菜]\n", row["DishesName"].ToString());
                        }
                        //团购菜品
                        if (row["DishesType"].ToString().Equals("3"))
                        {
                            sb.AppendFormat("{0}[套餐]\n", row["DishesName"].ToString());
                        }
                    }
                    else
                    {
                        sb.AppendFormat("{0}[赠送]\n", row["DishesName"].ToString());
                    }
                    count.AppendFormat("{0}{1}\n", row["DishesCount"].ToString(), row["UnitName"].ToString());
                    price.AppendFormat("{0}\n", row["Moneys"].ToString());
                }
            }
            sb.Append("-----------------------------------------------------------------------------\n");
            count.Append("\n");
            price.Append("\n");
            sb.Append("消费合计：\n");
            count.Append("\n");
            price.AppendFormat("{0}\n", tabieUsingInfo.Moneys);
            sb.Append("赠送金额：\n");
            count.Append("\n");
            price.AppendFormat("{0}\n", tabieUsingInfo.PrePrice);
            sb.Append("应付金额：\n");
            count.Append("\n");
            price.AppendFormat("{0}\n", tabieUsingInfo.FactPrice);
            sb.Append("-----------------------------------------------------------------------------\n");
            count.Append("\n");
            price.Append("\n");
            sb.Append("地址：渝北区冉家坝龙山路301号\n");
            count.Append("\n");
            price.Append("\n");
            sb.Append("电话：02367364577\n");
            count.Append(" \n");
            price.Append("\n");
            sb.Append("欢迎光临\n");
            count.Append("\n");
            price.Append("\n");
            LocalPrint(sb.ToString(), count.ToString(), price.ToString());
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
                        //判断退菜
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

        #endregion

    }
}