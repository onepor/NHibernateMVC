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
using Newtonsoft.Json.Linq;
using System.Data;
using System.Configuration;
using ZAJCZN.MIS.Helpers;
using System.IO;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractDoorDetail : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreContractDoor";
            }
        }

        #endregion

        #region request param

        /// <summary>
        /// 订单编号
        /// </summary>
        private string OrderNO
        {
            get { return ViewState["OrderNO"].ToString(); }
            set
            {
                ViewState["OrderNO"] = value;

            }
        }
        //订单ID（传入参数）
        private int OrderID
        {
            get { return GetQueryIntValue("id"); }
        }

        #endregion request param

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gdCostInfo.AutoScroll = true;
                // 权限检查
                CheckPowerWithButton("CoreContractDoor", btnNew);
                CheckPowerWithButton("CoreContractDoor", btnDeleteSelected);
                btnNew.OnClientClick = Window1.GetShowReference(string.Format("~/Contract/ContractDoorAdd.aspx?id={0}", OrderID), "新增商品");
                btnNewHW.OnClientClick = Window2.GetShowReference(string.Format("~/PublicWebForm/OrderGoodsSelectDialog.aspx?id={0}&tid=1", OrderID), "添加五金配件");

                // 删除选中单元格的客户端脚本 
                btnDeleteFiles.OnClientClick = gdFiles.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
                btnDeleteFiles.ConfirmText = String.Format("确定要删除选中的<span class=\"highlight\"><script>{0}</script></span>项附件信息吗？", gdFiles.GetSelectedCountReference());
                btnDeleteFiles.ConfirmTarget = FineUIPro.Target.Top;
                // 删除选中单元格的客户端脚本 
                btnDeleteSelected.OnClientClick = gdCostInfo.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
                btnDeleteSelected.ConfirmText = String.Format("确定要删除选中的<span class=\"highlight\"><script>{0}</script></span>项记录吗？", gdCostInfo.GetSelectedCountReference());
                btnDeleteSelected.ConfirmTarget = FineUIPro.Target.Top;
                // 删除选中单元格的客户端脚本 
                btnDeleteHW.OnClientClick = gdHandWare.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
                btnDeleteHW.ConfirmText = String.Format("确定要删除选中的<span class=\"highlight\"><script>{0}</script></span>项配件记录吗？", gdHandWare.GetSelectedCountReference());
                btnDeleteHW.ConfirmTarget = FineUIPro.Target.Top;

                if (OrderID <= 0)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误，订单号不存在！", String.Empty, ActiveWindow.GetHideReference());
                }
                else
                {
                    GetOrderInfo();
                    hdNO.Text = OrderID.ToString();
                }
                //获取订单发货材料各项明细
                BindOrderDetail();
                //获取锁具列表
                BindLockInfo();
                //获取五金信息
                BindHandWareDetail();
                //获取相关附件信息
                BindFiles();
            }
        }

        #region 页面初始数据绑定

        /// <summary>
        /// 根据订单号，获取订单信息
        /// </summary>
        private void GetOrderInfo()
        {
            //获取订单信息
            ContractInfo order = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(OrderID);
            OrderNO = order.ContractNO;
            //初始化页面数据 
            lblOrderNo.Text = OrderNO;
            lblProject.Text = order.ProjectName;
            lblCustName.Text = order.CustomerName;
            if (order.IsUrgent == 1)
            {
                lblOrderNo.Text = string.Format("{0}【加急】", order.ContractNO);
                lblOrderNo.CssStyle = "color:red;";
            }
        }

        #endregion 页面初始数据绑定

        #region 发货信息绑定显示

        /// <summary>
        /// 获取订单发货材料各项明细
        /// </summary>
        private void BindOrderDetail()
        {
            //根据订单号获取发货主材信息
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractInfo.ID", OrderID));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<ContractDoorInfo> list = Core.Container.Instance.Resolve<IServiceContractDoorInfo>().GetAllByKeys(qryList, orderList);

            gdCostInfo.DataSource = list;
            gdCostInfo.DataBind();

            decimal donateTotal = 0M;
            foreach (ContractDoorInfo eqpInfo in list)
            {
                donateTotal += eqpInfo.OrderAmount;
            }
            //绑定合计数据
            JObject summary = new JObject();
            summary.Add("OtherAmount", "金额合计");
            summary.Add("OrderAmount", donateTotal);

            gdCostInfo.SummaryData = summary;
        }

        /// <summary>
        /// 获取五金信息
        /// </summary>
        private void BindHandWareDetail()
        {
            //根据订单号获取发货主材信息
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractID", OrderID));
            qryList.Add(Expression.Eq("OrderType", 1));
            Order[] orderList = new Order[1];
            Order orderli = new Order("GoodTypeID", true);
            orderList[0] = orderli;
            IList<ContractHandWareDetail> list = Core.Container.Instance.Resolve<IServiceContractHandWareDetail>().GetAllByKeys(qryList, orderList);

            gdHandWare.DataSource = list;
            gdHandWare.DataBind();

            decimal donateTotal = 0M;
            foreach (ContractHandWareDetail eqpInfo in list)
            {
                if (eqpInfo.IsFree == 1)
                {
                    donateTotal += eqpInfo.GoodAmount;
                }
            }
            //绑定合计数据
            JObject summary = new JObject();
            summary.Add("GoodsUnitPrice", "金额合计");
            summary.Add("GoodAmount", donateTotal);

            gdHandWare.SummaryData = summary;
        }

        /// <summary>
        /// 获取上传附件信息
        /// </summary>
        private void BindFiles()
        {
            //根据订单号获取上传附件信息
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractInfo.ID", OrderID));
            qryList.Add(Expression.Eq("FileType", 1));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<ContractFiles> list = Core.Container.Instance.Resolve<IServiceContractFiles>().GetAllByKeys(qryList, orderList);

            gdFiles.DataSource = list;
            gdFiles.DataBind();
        }

        /// <summary>
        /// 获取锁具列表
        /// </summary>
        private void BindLockInfo()
        {
            //获取订单门总金额 
            string sql = string.Format(@"select count(*) as GoodsNumber,parts.PartsName as GoodsName from ContractDoorInfo door left join PartsInfo parts on door.LockID = parts.ID where door.ContractID ={0} AND door.LockID>0 group by door.LockID,parts.PartsName  ", OrderID);
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0] != null)
            {

                gdLock.DataSource = ds.Tables[0];
                gdLock.DataBind();
            }
        }

        #endregion 发货信息绑定显示

        #region 页面数据转化

        public System.Drawing.Color GetColor(string state)
        {
            System.Drawing.Color i = new System.Drawing.Color();
            switch (state)
            {
                case "0":
                    i = System.Drawing.Color.Red;
                    break;
                case "1":
                    i = System.Drawing.Color.Black;
                    break;
            }
            return i;
        }

        public string GetSuplierName(string id)
        {
            SupplierInfo supplier = Core.Container.Instance.Resolve<IServiceSupplierInfo>().GetEntity(int.Parse(id));
            return supplier != null ? supplier.SupplierName : "";
        }

        public string GetLock(string lockID)
        {
            PartsInfo partsInfo = Core.Container.Instance.Resolve<IServicePartsInfo>().GetEntity(int.Parse(lockID));
            return partsInfo != null ? partsInfo.PartsName : "";
        }

        #endregion 页面数据转化

        #region 发货明细清单调整编辑

        /// <summary>
        /// 主材单价及发货数量编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gdCostInfo_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            Dictionary<int, Dictionary<string, object>> modifiedDict = gdCostInfo.GetModifiedDict();
            foreach (int rowIndex in modifiedDict.Keys)
            {
                //根据绑定列的记录编号，获取发货物品信息和物品基本信息
                int rowID = Convert.ToInt32(gdCostInfo.DataKeys[rowIndex][0]);
                ContractDoorInfo contractDoorInfo = Core.Container.Instance.Resolve<IServiceContractDoorInfo>().GetEntity(rowID);
                //修改高度
                if (modifiedDict[rowIndex].Keys.Contains("GHeight"))
                {
                    contractDoorInfo.GHeight = Convert.ToInt32(modifiedDict[rowIndex]["GHeight"]);
                    //计算超标
                    CalcDoorPrice(ref contractDoorInfo);
                }
                //修改宽度
                if (modifiedDict[rowIndex].Keys.Contains("GWide"))
                {
                    contractDoorInfo.GWide = Convert.ToInt32(modifiedDict[rowIndex]["GWide"]);
                    //计算超标
                    CalcDoorPrice(ref contractDoorInfo);
                }
                //修改厚度
                if (modifiedDict[rowIndex].Keys.Contains("GThickness"))
                {
                    contractDoorInfo.GThickness = Convert.ToInt32(modifiedDict[rowIndex]["GThickness"]);
                    //计算超标
                    CalcDoorPrice(ref contractDoorInfo);
                }
                //修改五金费用
                if (modifiedDict[rowIndex].Keys.Contains("HardWareAmount"))
                {
                    contractDoorInfo.HardWareAmount = Convert.ToDecimal(modifiedDict[rowIndex]["HardWareAmount"]);
                }
                //修改超标加价
                if (modifiedDict[rowIndex].Keys.Contains("PassAmount"))
                {
                    contractDoorInfo.PassAmount = Convert.ToDecimal(modifiedDict[rowIndex]["PassAmount"]);
                }
                //修改标准单价
                if (modifiedDict[rowIndex].Keys.Contains("GStandardPrice"))
                {
                    contractDoorInfo.GStandardPrice = Convert.ToDecimal(modifiedDict[rowIndex]["GStandardPrice"]);
                    //计算最终单价和金额
                    contractDoorInfo.GPrice = contractDoorInfo.GStandardPrice + contractDoorInfo.PassPriceAmount;
                    contractDoorInfo.GoodsAmount = contractDoorInfo.GPrice * contractDoorInfo.OrderNumber;
                }
                //修改最终单价
                if (modifiedDict[rowIndex].Keys.Contains("GPrice"))
                {
                    contractDoorInfo.GPrice = Convert.ToDecimal(modifiedDict[rowIndex]["GPrice"]);
                    //计算标准单价和金额
                    contractDoorInfo.GStandardPrice = contractDoorInfo.GPrice - contractDoorInfo.PassPriceAmount;
                    contractDoorInfo.GoodsAmount = contractDoorInfo.GPrice * contractDoorInfo.OrderNumber;
                }
                //修改备注
                if (modifiedDict[rowIndex].Keys.Contains("Remark"))
                {
                    contractDoorInfo.Remark = modifiedDict[rowIndex]["Remark"].ToString();
                }
                //修改型号
                if (modifiedDict[rowIndex].Keys.Contains("ModelName"))
                {
                    contractDoorInfo.ModelName = modifiedDict[rowIndex]["ModelName"].ToString();
                }
                //修改颜色
                if (modifiedDict[rowIndex].Keys.Contains("DoorColor"))
                {
                    contractDoorInfo.DoorColor = modifiedDict[rowIndex]["DoorColor"].ToString();
                }
                //修改商品金额
                if (modifiedDict[rowIndex].Keys.Contains("GoodsAmount"))
                {
                    contractDoorInfo.GoodsAmount = Convert.ToDecimal(modifiedDict[rowIndex]["GoodsAmount"].ToString());
                }
                //修改运输安装
                if (modifiedDict[rowIndex].Keys.Contains("InstallCost"))
                {
                    contractDoorInfo.InstallCost = Convert.ToDecimal(modifiedDict[rowIndex]["InstallCost"].ToString());
                }
                //修改五金费用
                if (modifiedDict[rowIndex].Keys.Contains("HardWareAmount"))
                {
                    contractDoorInfo.HardWareAmount = Convert.ToDecimal(modifiedDict[rowIndex]["HardWareAmount"].ToString());
                }
                //修改其他金额
                if (modifiedDict[rowIndex].Keys.Contains("OtherAmount"))
                {
                    contractDoorInfo.OtherAmount = Convert.ToDecimal(modifiedDict[rowIndex]["OtherAmount"].ToString());
                }
                //修改线条
                if (modifiedDict[rowIndex].Keys.Contains("LineName"))
                {
                    contractDoorInfo.LineName = modifiedDict[rowIndex]["LineName"].ToString();
                }
                //修改玻璃款式
                if (modifiedDict[rowIndex].Keys.Contains("GlassRemark"))
                {
                    contractDoorInfo.GlassRemark = modifiedDict[rowIndex]["GlassRemark"].ToString();
                }

                //计算商品总价
                //contractDoorInfo.PassAmount = contractDoorInfo.GPassHeightAmount
                //                          + contractDoorInfo.GPassWideAmount
                //                          + contractDoorInfo.GPassThicknessAmount
                //                          + contractDoorInfo.GPassAreaAmount;
                contractDoorInfo.OrderAmount = contractDoorInfo.GoodsAmount
                                                + contractDoorInfo.PassAmount
                                                + contractDoorInfo.OtherAmount
                                                + contractDoorInfo.InstallCost
                                                + contractDoorInfo.HardWareAmount;
                //更新订单明细
                Core.Container.Instance.Resolve<IServiceContractDoorInfo>().Update(contractDoorInfo);
            }
            //更新订单总金额
            UpdateOrderAmount();
            //重新加载订单发货信息
            BindOrderDetail();
        }

        /// <summary>
        /// 根据基本录入尺寸信息计算超标和费用
        /// </summary>
        /// <param name="contractDoorInfo">门信息</param>
        private void CalcDoorPrice(ref ContractDoorInfo contractDoorInfo)
        {
            /*-------------------根据基本录入尺寸信息计算超标和费用---------------------*/
            //计算面积  
            contractDoorInfo.GArea = ((decimal)contractDoorInfo.GHeight / 1000) * ((decimal)contractDoorInfo.GWide / 1000);
            //计算尺寸超标和金额          
            EquipmentInfo equipmentInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(contractDoorInfo.GoodsID);
            if (equipmentInfo != null)
            {
                // 商品单位 1:樘  2：米  3：平方米
                contractDoorInfo.GoodUnit = equipmentInfo.EquipmentUnit == "1" ? "樘" : equipmentInfo.EquipmentUnit == "2" ? "米" : "平方米";
                //计算尺寸面积超标
                contractDoorInfo.GPassHeight = contractDoorInfo.GHeight > equipmentInfo.EHeight ? contractDoorInfo.GHeight - equipmentInfo.EHeight : 0;
                contractDoorInfo.GPassWide = contractDoorInfo.GWide > equipmentInfo.EWide ? contractDoorInfo.GWide - equipmentInfo.EWide : 0;
                contractDoorInfo.GPassThickness = contractDoorInfo.GThickness > equipmentInfo.EThickness ? contractDoorInfo.GThickness - equipmentInfo.EThickness : 0;
                decimal standerArea = ((decimal)equipmentInfo.EHeight / 1000) * ((decimal)equipmentInfo.EWide / 1000);
                contractDoorInfo.GPassArea = contractDoorInfo.GArea > standerArea ? contractDoorInfo.GArea - standerArea : 0;
            }
            //计算各项超标金额 【超标计算 1：按公分计算 2:单价加价  3：面积加价】
            switch (equipmentInfo.PassCalcType)
            {
                case 1:     //按公分计算
                    contractDoorInfo.GPassHeightAmount = contractDoorInfo.GPassHeight / 10 * equipmentInfo.PassHeight;
                    contractDoorInfo.GPassWideAmount = contractDoorInfo.GPassWide / 10 * equipmentInfo.PassWide;
                    contractDoorInfo.GPassThicknessAmount = contractDoorInfo.GPassThickness / 10 * equipmentInfo.PassThckness;
                    break;
                case 2:     //单价加价
                    contractDoorInfo.GPassHeightAmount = contractDoorInfo.GPassHeight > 0 ? equipmentInfo.PassHeight : 0;
                    contractDoorInfo.GPassWideAmount = contractDoorInfo.GPassWide > 0 ? equipmentInfo.PassWide : 0;
                    contractDoorInfo.GPassThicknessAmount = contractDoorInfo.GPassThickness > 0 ? equipmentInfo.PassThckness : 0;
                    //计算单价加价
                    contractDoorInfo.PassPriceAmount = contractDoorInfo.GPassHeightAmount + contractDoorInfo.GPassWideAmount + contractDoorInfo.GPassThicknessAmount;
                    contractDoorInfo.GPrice = contractDoorInfo.GStandardPrice + contractDoorInfo.PassPriceAmount;
                    break;
                case 3:     //面积加价
                    contractDoorInfo.GPassAreaAmount = contractDoorInfo.GPassArea * equipmentInfo.PassArea;
                    break;
            }
            //计算商品数量 1：单位数量 2:三方周长  3：四方周长 4：面积 
            switch (equipmentInfo.CalcUnitType)
            {
                case 1:     //单位数量
                    contractDoorInfo.OrderNumber = 1;
                    break;
                case 2:     //三方周长
                    contractDoorInfo.OrderNumber = ((decimal)contractDoorInfo.GHeight * 2 + (decimal)contractDoorInfo.GWide) / 1000;
                    break;
                case 3:     //四方周长
                    contractDoorInfo.OrderNumber = ((decimal)contractDoorInfo.GHeight * 2 + (decimal)contractDoorInfo.GWide * 2) / 1000;
                    break;
                case 4:     //面积
                    contractDoorInfo.OrderNumber = contractDoorInfo.GArea;
                    break;
            }
        }

        /// <summary>
        /// 五金明细编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gdHandWare_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            Dictionary<int, Dictionary<string, object>> modifiedDict = gdHandWare.GetModifiedDict();
            foreach (int rowIndex in modifiedDict.Keys)
            {
                //根据绑定列的记录编号，获取发货物品信息和物品基本信息
                int rowID = Convert.ToInt32(gdHandWare.DataKeys[rowIndex][0]);
                ContractHandWareDetail objInfo = Core.Container.Instance.Resolve<IServiceContractHandWareDetail>().GetEntity(rowID);
                //修改数量
                if (modifiedDict[rowIndex].Keys.Contains("GoodsNumber"))
                {
                    objInfo.GoodsNumber = Convert.ToInt32(modifiedDict[rowIndex]["GoodsNumber"]);
                }
                //修改单价
                if (modifiedDict[rowIndex].Keys.Contains("GoodsUnitPrice"))
                {
                    objInfo.GoodsUnitPrice = Convert.ToDecimal(modifiedDict[rowIndex]["GoodsUnitPrice"]);
                }
                //更新总价和成本金额
                objInfo.GoodAmount = objInfo.GoodsNumber * objInfo.GoodsUnitPrice;
                objInfo.CostAmount = objInfo.GoodsNumber * objInfo.CostPrice;
                //更新订单明细
                Core.Container.Instance.Resolve<IServiceContractHandWareDetail>().Update(objInfo);
            }
            //重新加载
            BindHandWareDetail();
        }

        #endregion 发货明细清单调整编辑

        #region 更新订单金额

        private void UpdateOrderAmount()
        {
            //获取订单门总金额
            decimal payNumber = 0;
            string sql = string.Format(@"select isnull(sum(OrderAmount),0) as OrderAmount from ContractDoorInfo where ContractID ={0}  ", OrderID);
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0] != null)
            {
                payNumber = decimal.Parse(ds.Tables[0].Rows[0]["OrderAmount"].ToString());
            }
            //获取门收费五金总金额
            decimal hwNumber = 0;
            string sqlHW = string.Format(@"select isnull(sum(GoodAmount),0) as GoodAmount from ContractHandWareDetail where ContractID ={0} and IsFree=1 and OrderType=1 ", OrderID);
            DataSet dsHW = DbHelperSQL.Query(sqlHW);
            if (dsHW.Tables[0] != null)
            {
                hwNumber = decimal.Parse(dsHW.Tables[0].Rows[0]["GoodAmount"].ToString());
            }
            //获取合同五金成本总金额
            decimal costNumber = 0;
            string sqlCost = string.Format(@"select isnull(sum(CostAmount),0) as CostAmount from ContractHandWareDetail where ContractID ={0}", OrderID);
            DataSet dsCost = DbHelperSQL.Query(sqlCost);
            if (dsCost.Tables[0] != null)
            {
                costNumber = decimal.Parse(dsCost.Tables[0].Rows[0]["CostAmount"].ToString());
            }
            //获取运费总金额
            decimal sendNumber = 0;
            string sqlSend = string.Format(@"select isnull(sum(InstallCost),0) as InstallAmount from ContractDoorInfo where ContractID ={0}  ", OrderID);
            DataSet dsSend = DbHelperSQL.Query(sqlSend);
            if (dsSend.Tables[0] != null)
            {
                sendNumber = decimal.Parse(dsSend.Tables[0].Rows[0]["InstallAmount"].ToString());
            }
            //获取门锁具成本总金额 
            string sqlLock = string.Format(@"select ISNULL(SUM(parts.CostPrice),0) as CostPrice from ContractDoorInfo door left join PartsInfo parts on door.LockID = parts.ID  where ContractID ={0} and door.LockID>0 ", OrderID);
            DataSet dsLock = DbHelperSQL.Query(sqlLock);
            if (dsLock.Tables[0] != null)
            {
                costNumber += decimal.Parse(dsLock.Tables[0].Rows[0]["CostPrice"].ToString());
            }
            //更新合同相关费用金额
            ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(OrderID);
            contractInfo.DoorAmount = payNumber + hwNumber;
            contractInfo.TotalAmount = contractInfo.CabinetAmount + contractInfo.DoorAmount;
            contractInfo.HandWareCost = costNumber;
            contractInfo.SendCost = sendNumber;
            ////如果门报价金额大于0，则视为订单为生产中状态
            //if (contractInfo.DoorAmount > 0)
            //{
            //    contractInfo.ContractState = 4;
            //}
            Core.Container.Instance.Resolve<IServiceContractInfo>().Update(contractInfo);
        }

        #endregion 更新订单金额

        #region Events

        /// <summary>
        /// 返回订单列表页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnReturn_Click(object sender, EventArgs e)
        {
            //更新订单总金额
            UpdateOrderAmount();
            //返回订单列表页面
            PageContext.Redirect("~/Contract/ContractDesignManage.aspx");
        }

        #region 删除处理

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedDataKeyIDs(gdCostInfo);
            foreach (int id in ids)
            {
                Core.Container.Instance.Resolve<IServiceContractDoorInfo>().Delete(id);
            }
            //更新订单总金额
            UpdateOrderAmount();
            //绑定柜子明细
            BindOrderDetail();
            //获取锁具列表
            BindLockInfo();
        }

        protected void btnDeleteHW_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedDataKeyIDs(gdHandWare);
            foreach (int id in ids)
            {
                Core.Container.Instance.Resolve<IServiceContractHandWareDetail>().Delete(id);
            }
            BindHandWareDetail();
        }

        protected void btnDeleteFiles_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedDataKeyIDs(gdFiles);
            foreach (int id in ids)
            {
                ContractFiles objInfo = Core.Container.Instance.Resolve<IServiceContractFiles>().GetEntity(id);
                Core.Container.Instance.Resolve<IServiceContractFiles>().Delete(id);
                //删除服务器上文件
                Helpers.FileHelper.DeleteFile(objInfo.FileSavePath);
            }
            BindFiles();
        }

        #endregion 删除处理

        /// <summary>
        /// 设置五金收费
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnIsFree_Click(object sender, EventArgs e)
        {
            ContractHandWareDetail objInfo = new ContractHandWareDetail();
            List<int> ids = GetSelectedDataKeyIDs(gdHandWare);
            foreach (int id in ids)
            {
                objInfo = new ContractHandWareDetail();
                objInfo = Core.Container.Instance.Resolve<IServiceContractHandWareDetail>().GetEntity(id);
                objInfo.IsFree = objInfo.IsFree == 1 ? 0 : 1;
                Core.Container.Instance.Resolve<IServiceContractHandWareDetail>().Update(objInfo);
            }
            BindHandWareDetail();
        }

        /// <summary>
        /// 添加门返回处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window1_Close(object sender, EventArgs e)
        {
            BindOrderDetail();
            //获取锁具列表
            BindLockInfo();
        }

        /// <summary>
        /// 添加五金返回处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window2_Close(object sender, EventArgs e)
        {
            BindHandWareDetail();
        }

        protected void gdCostInfo_RowDoubleClick(object sender, GridRowClickEventArgs e)
        {
            int ID = GetSelectedDataKeyID(gdCostInfo);
            PageContext.RegisterStartupScript(Window1.GetShowReference(string.Format("~/Contract/ContractDoorEdit.aspx?id={0}", ID)
                , "编辑门明细信息"));

        }

        #endregion

        #region 附件文件上传/下载处理

        /// <summary>
        /// 附件上传处理
        /// </summary>
        /// <param name = "sender" ></ param >
        /// < param name="e"></param>
        protected void filePhoto_FileSelected(object sender, EventArgs e)
        {
            string fileShowPicPath = "";
            string saveShowPicPath = "";

            if (filePhoto.HasFile)
            {
                string fileShowrNameName = filePhoto.ShortFileName;
                //if (!Helpers.PicHelper.CheckFileIsCorrect(fileShowrNameName))
                //{
                //    Alert.Show("图片为无效的文件类型！");
                //    return;
                //}
                //上传附件文件
                DateTime curTime = DateTime.Now;
                fileShowPicPath = Helpers.PicHelper.GetShowPicPath(fileShowrNameName.Substring(fileShowrNameName.LastIndexOf(".") + 1), curTime);
                saveShowPicPath = Helpers.PicHelper.GetRealSavePath(fileShowrNameName.Substring(fileShowrNameName.LastIndexOf(".") + 1), curTime);
                filePhoto.SaveAs(saveShowPicPath);
                //保存附件表信息
                ContractFiles fileInfo = new ContractFiles();
                fileInfo.ContractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(OrderID);
                fileInfo.FileName = fileShowrNameName;
                fileInfo.FileSavePath = saveShowPicPath;
                fileInfo.FileShowPath = fileShowPicPath;
                fileInfo.FileType = 1;
                Core.Container.Instance.Resolve<IServiceContractFiles>().Create(fileInfo);
                //清空文件上传组件（上传后要记着清空，否则点击提交表单时会再次上传！！）
                filePhoto.Reset();
                //更新附件列表
                BindFiles();
            }
        }

        protected void gdFiles_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(gdFiles);
            if (e.CommandName == "DownLoad")
            {
                ContractFiles fileInfo = Core.Container.Instance.Resolve<IServiceContractFiles>().GetEntity(ID);
                if (fileInfo != null)
                {
                    DownloadFile(fileInfo.FileName, fileInfo.FileSavePath);
                }
            }
        }

        /// <summary>
        ///字符流下载方法
        /// </summary>
        /// <param name="fileName">下载文件生成的名称</param>
        /// <param name="fPath">下载文件路径</param>
        /// <returns></returns>
        public void DownloadFile(string fileName, string fPath)
        {
            string filePath = fPath;//路径
            //以字符流的形式下载文件
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.ContentType = "application/octet-stream";
            //通知浏览器下载文件而不是打开
            Response.AddHeader("Content-Disposition", "attachment;   filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        #endregion 附件文件上传/下载处理

        #region 导出

        #region 导出客户报价单

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.xls", OrderNO));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(GetAllTableHtml());
            Response.End();
        }

        /// <summary>
        /// 导出获取客户报价单
        /// </summary>
        /// <returns></returns>
        private string GetAllTableHtml()
        {
            decimal totalAmount = 0;
            //获取合同信息
            ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(OrderID);
            //获取订单明细
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractInfo.ID", OrderID));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<ContractDoorInfo> list = Core.Container.Instance.Resolve<IServiceContractDoorInfo>().GetAllByKeys(qryList, orderList);

            StringBuilder sb = new StringBuilder();
            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

            #region - 拼凑主订单导出结果 -
            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

            #region - 拼凑导出的列名 -
            //单据头
            sb.Append("<tr>");
            sb.AppendFormat("<td colspan=\"20\" style=\"font-size :x-large; text-align:center;\">{0}</td>", "重庆喜莱克家具有限公司（家喜林门）销售订单");
            sb.Append("</tr>");
            //订单信息
            sb.Append("<tr>");
            sb.AppendFormat("<td colspan=\"2\">{0}</td>", "订单号");
            sb.AppendFormat("<td colspan=\"3\">{0}</td>", contractInfo.ContractNO);
            sb.AppendFormat("<td colspan=\"1\">{0}</td>", "供货单位");
            sb.AppendFormat("<td colspan=\"4\">{0}</td>", ConfigHelper.StoreName);
            sb.AppendFormat("<td colspan=\"2\">{0}</td>", "联系电话");
            sb.AppendFormat("<td colspan=\"8\">{0}</td>", ConfigHelper.ContractPhone);
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.AppendFormat("<td colspan=\"2\">{0}</td>", "客户地址");
            sb.AppendFormat("<td colspan=\"8\">{0}</td>", contractInfo.ProjectName);
            sb.AppendFormat("<td colspan=\"2\">{0}</td>", "联系电话");
            sb.AppendFormat("<td colspan=\"8\">{0}</td>", contractInfo.ContactPhone);
            sb.Append("</tr>");
            //明细表头
            //序号 位置 高 宽 厚  线条 材质 颜色  款式 商品名称 开启方向 玻璃款式  单位 数量 商品价格 运输安装及五金 总价 备注
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", "序号");
            sb.AppendFormat("<td>{0}</td>", "位置");
            sb.AppendFormat("<td>{0}</td>", "高(mm)");
            sb.AppendFormat("<td>{0}</td>", "宽(mm)");
            sb.AppendFormat("<td>{0}</td>", "厚(mm)");
            sb.AppendFormat("<td>{0}</td>", "线条");
            sb.AppendFormat("<td>{0}</td>", "材质");
            sb.AppendFormat("<td>{0}</td>", "颜色");
            sb.AppendFormat("<td>{0}</td>", "款式");
            sb.AppendFormat("<td>{0}</td>", "商品名称");
            sb.AppendFormat("<td>{0}</td>", "开启方向");
            sb.AppendFormat("<td>{0}</td>", "玻璃款式");
            sb.AppendFormat("<td>{0}</td>", "五金(锁具)");
            sb.AppendFormat("<td>{0}</td>", "单位");
            sb.AppendFormat("<td>{0}</td>", "单价");
            sb.AppendFormat("<td>{0}</td>", "数量");
            sb.AppendFormat("<td>{0}</td>", "商品价格");
            sb.AppendFormat("<td>{0}</td>", "运输安装及五金");
            sb.AppendFormat("<td>{0}</td>", "总价");
            sb.AppendFormat("<td>{0}</td>", "备注");
            sb.Append("</tr>");

            #endregion

            #region - 拼凑导出的数据行 -
            int recordIndex1 = 1;
            foreach (ContractDoorInfo row in list)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td>{0}</td>", recordIndex1);
                sb.AppendFormat("<td>{0}</td>", row.GoodsLocation);
                sb.AppendFormat("<td>{0}</td>", row.GHeight);
                sb.AppendFormat("<td>{0}</td>", row.GWide);
                sb.AppendFormat("<td>{0}</td>", row.GThickness);
                sb.AppendFormat("<td>{0}</td>", row.LineName);
                sb.AppendFormat("<td>{0}</td>", row.TypeName);
                sb.AppendFormat("<td>{0}</td>", row.DoorColor);
                sb.AppendFormat("<td>{0}</td>", row.ModelName);
                sb.AppendFormat("<td>{0}</td>", row.GoodsName);
                sb.AppendFormat("<td>{0}</td>", row.DoorDirection);
                sb.AppendFormat("<td>{0}</td>", row.GlassRemark);
                sb.AppendFormat("<td>{0}</td>", GetLock(row.LockID.ToString()));
                sb.AppendFormat("<td>{0}</td>", row.GoodUnit);
                sb.AppendFormat("<td>{0}</td>", row.GPrice);
                sb.AppendFormat("<td>{0}</td>", row.OrderNumber);
                sb.AppendFormat("<td>{0}</td>", row.GoodsAmount + row.PassAmount + row.OtherAmount);
                sb.AppendFormat("<td>{0}</td>", row.InstallCost + row.HardWareAmount);
                sb.AppendFormat("<td>{0}</td>", row.OrderAmount);
                sb.AppendFormat("<td>{0}</td>", row.Remark);
                sb.Append("</tr>");
                recordIndex1++;
                totalAmount += row.OrderAmount;
            }

            //合计行
            sb.Append("<tr>");
            sb.Append("<th colspan=\"18\" style=\"text-align: right;font-size:x-large;\">合计</th>");
            sb.AppendFormat("<td>{0}</td>", totalAmount);
            sb.Append("<td></td>");
            sb.Append("</tr>");

            #endregion

            //单据尾
            sb.Append("<tr>");
            sb.AppendFormat("<td colspan=\"20\">安装售后：{0} 量尺设计：{1} 投诉电话：{2} 地址：{3}</td>"
                            , ConfigHelper.InstallPhone
                            , ConfigHelper.DesignPhone
                            , ConfigHelper.ComplaintPhone
                            , ConfigHelper.Address);
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td colspan=\"20\"></td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td style=\"text-align: center;\" colspan=\"10\">客户确认签字</td>");
            sb.Append("<td style=\"text-align: center;\" colspan=\"10\">货方签字（盖章）</td>");
            sb.Append("</tr>");

            sb.Append("</table>");

            #endregion

            return sb.ToString();
        }

        #endregion 导出客户报价单

        #region 导出厂商生产单

        protected void btnExportCS_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment; filename=CS_{0}.xls", OrderNO));
            Response.ContentType = "application/excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(GetAllTableHtmlForSupplier());
            Response.End();
        }

        /// <summary>
        /// 导出厂商下定单
        /// </summary> 
        /// <param name="suppilerID">厂商编号</param>
        /// <returns></returns>
        private string GetAllTableHtmlForSupplier()
        {
            //获取合同信息
            ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(OrderID);
            //获取厂商信息
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractID", OrderID));
            qryList.Add(Expression.Eq("CostType", 1));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<ContractCostInfo> costInfoList = Core.Container.Instance.Resolve<IServiceContractCostInfo>().GetAllByKeys(qryList, orderList);

            StringBuilder sb = new StringBuilder();
            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

            #region - 拼凑主订单导出结果 -
            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");


            foreach (ContractCostInfo cost in costInfoList)
            {
                //获取厂商信息
                SupplierInfo supplierInfo = Core.Container.Instance.Resolve<IServiceSupplierInfo>().GetEntity(cost.SuppilerID);
                //获取订单明细
                qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("ContractInfo.ID", OrderID));
                qryList.Add(Expression.Eq("SupplyID", cost.SuppilerID));
                orderList = new Order[1];
                orderli = new Order("ID", true);
                orderList[0] = orderli;
                IList<ContractDoorInfo> list = Core.Container.Instance.Resolve<IServiceContractDoorInfo>().GetAllByKeys(qryList, orderList);


                #region - 拼凑导出的列名 -
                //单据头
                sb.Append("<tr>");
                sb.AppendFormat("<td colspan=\"14\" style=\"font-size :x-large; text-align:center;\">{0}</td>", "重庆喜莱克家具有限公司（家喜林门）生产订单");
                sb.Append("</tr>");
                //订单信息
                sb.Append("<tr>");
                sb.AppendFormat("<td colspan=\"2\">{0}</td>", "订单号");
                sb.AppendFormat("<td colspan=\"3\">{0}</td>", contractInfo.ContractNO);
                sb.AppendFormat("<td colspan=\"1\">{0}</td>", "供货单位");
                sb.AppendFormat("<td colspan=\"3\">{0}</td>", ConfigHelper.StoreName);
                sb.AppendFormat("<td>{0}</td>", "联系电话");
                sb.AppendFormat("<td colspan=\"4\">{0}</td>", ConfigHelper.ContractPhone);
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.AppendFormat("<td colspan=\"2\">{0}</td>", "客户地址");
                sb.AppendFormat("<td colspan=\"3\">{0}</td>", contractInfo.ProjectName);
                sb.AppendFormat("<td>{0}</td>", "联系电话");
                sb.AppendFormat("<td colspan=\"4\">{0}</td>", contractInfo.ContactPhone);
                sb.AppendFormat("<td>{0}</td>", "生产厂商");
                sb.AppendFormat("<td colspan=\"3\">{0}</td>", supplierInfo.SupplierName);
                sb.Append("</tr>");
                //明细表头
                sb.Append("<tr>");
                sb.AppendFormat("<td>{0}</td>", "序号");
                sb.AppendFormat("<td>{0}</td>", "位置");
                sb.AppendFormat("<td>{0}</td>", "高(mm)");
                sb.AppendFormat("<td>{0}</td>", "宽(mm)");
                sb.AppendFormat("<td>{0}</td>", "厚(mm)");
                sb.AppendFormat("<td>{0}</td>", "线条");
                sb.AppendFormat("<td>{0}</td>", "材质");
                sb.AppendFormat("<td>{0}</td>", "颜色");
                sb.AppendFormat("<td>{0}</td>", "款式");
                sb.AppendFormat("<td>{0}</td>", "商品名称");
                sb.AppendFormat("<td>{0}</td>", "玻璃款式");
                sb.AppendFormat("<td>{0}</td>", "开启方向");
                sb.AppendFormat("<td>{0}</td>", "数量");
                sb.AppendFormat("<td>{0}</td>", "备注");
                sb.Append("</tr>");

                #endregion

                #region - 拼凑导出的数据行 -
                int recordIndex1 = 1;
                foreach (ContractDoorInfo row in list)
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td>{0}</td>", recordIndex1);
                    sb.AppendFormat("<td>{0}</td>", row.GoodsLocation);
                    sb.AppendFormat("<td>{0}</td>", row.GHeight);
                    sb.AppendFormat("<td>{0}</td>", row.GWide);
                    sb.AppendFormat("<td>{0}</td>", row.GThickness);
                    sb.AppendFormat("<td>{0}</td>", row.LineName);
                    sb.AppendFormat("<td>{0}</td>", row.TypeName);
                    sb.AppendFormat("<td>{0}</td>", row.DoorColor);
                    sb.AppendFormat("<td>{0}</td>", row.ModelName);
                    sb.AppendFormat("<td>{0}</td>", row.GoodsName);
                    sb.AppendFormat("<td>{0}</td>", row.GlassRemark);
                    sb.AppendFormat("<td>{0}</td>", row.DoorDirection);
                    sb.AppendFormat("<td>{0}</td>", row.OrderNumber);
                    sb.AppendFormat("<td>{0}</td>", row.Remark);
                    sb.Append("</tr>");
                    recordIndex1++;
                }
                #endregion

                //单据尾
                sb.Append("<tr>");
                sb.AppendFormat("<td colspan=\"14\">店铺名称：{0} 联系电话：{1} 地址：{2}</td>"
                                , ConfigHelper.StoreName
                                , ConfigHelper.ContractPhone
                                , ConfigHelper.Address);
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan=\"14\"></td>");
                sb.Append("</tr>");
            }

            sb.Append("</table>");

            #endregion

            return sb.ToString();
        }

        #endregion 导出厂商生产单

        #endregion 导出
    }
}
