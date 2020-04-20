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
    public partial class ContractCabinetEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreContractCabinet";
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
                CheckPowerWithButton("CoreContractCabinet", btnNew);
                CheckPowerWithButton("CoreContractCabinet", btnDeleteSelected);
                btnNew.OnClientClick = Window1.GetShowReference(string.Format("~/Contract/ContractCabinetAdd.aspx?id={0}", OrderID), "新增商品");
                btnNewHW.OnClientClick = Window2.GetShowReference(string.Format("~/PublicWebForm/OrderGoodsSelectDialog.aspx?id={0}&tid=2", OrderID), "添加五金配件");

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

        #region 页面信息绑定显示

        /// <summary>
        /// 获取订单发货材料各项明细
        /// </summary>
        private void BindOrderDetail()
        {
            //根据订单号获取发货主材信息
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractInfo.ID", OrderID));
            Order[] orderList = new Order[1];
            Order orderli = new Order("GoodsType", true);
            orderList[0] = orderli;
            IList<ContractCabinetInfo> list = Core.Container.Instance.Resolve<IServiceContractCabinetInfo>().GetAllByKeys(qryList, orderList);

            gdCostInfo.DataSource = list;
            gdCostInfo.DataBind();

            decimal donateTotal = 0M;
            foreach (ContractCabinetInfo eqpInfo in list)
            {
                donateTotal += eqpInfo.OrderAmount;
            }
            //绑定合计数据
            JObject summary = new JObject();
            summary.Add("GPrice", "金额合计");
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
            qryList.Add(Expression.Eq("OrderType", 2));
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
            summary.Add("GoodsUnitPrice", "收费合计");
            summary.Add("GoodAmount", donateTotal);

            gdHandWare.SummaryData = summary;
        }

        /// <summary>
        /// 获取五金信息
        /// </summary>
        private void BindFiles()
        {
            //根据订单号获取发货主材信息
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractInfo.ID", OrderID));
            qryList.Add(Expression.Eq("FileType", 2));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<ContractFiles> list = Core.Container.Instance.Resolve<IServiceContractFiles>().GetAllByKeys(qryList, orderList);

            gdFiles.DataSource = list;
            gdFiles.DataBind();
        }

        #endregion 页面信息绑定显示

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
                ContractCabinetInfo objInfo = Core.Container.Instance.Resolve<IServiceContractCabinetInfo>().GetEntity(rowID);
                //修改高度
                if (modifiedDict[rowIndex].Keys.Contains("GHeight"))
                {
                    objInfo.GHeight = Convert.ToDecimal(modifiedDict[rowIndex]["GHeight"]);
                }
                //修改宽度
                if (modifiedDict[rowIndex].Keys.Contains("GWide"))
                {
                    objInfo.GWide = Convert.ToDecimal(modifiedDict[rowIndex]["GWide"]);
                }
                //修改数量
                if (modifiedDict[rowIndex].Keys.Contains("OrderNumber"))
                {
                    objInfo.OrderNumber = Convert.ToDecimal(modifiedDict[rowIndex]["OrderNumber"]);
                }
                //修改单价
                if (modifiedDict[rowIndex].Keys.Contains("GPrice"))
                {
                    objInfo.GPrice = Convert.ToDecimal(modifiedDict[rowIndex]["GPrice"]);
                }
                //修改备注
                if (modifiedDict[rowIndex].Keys.Contains("Remark"))
                {
                    objInfo.Remark = modifiedDict[rowIndex]["Remark"].ToString();
                }
                //修改颜色1
                if (modifiedDict[rowIndex].Keys.Contains("GColorOne"))
                {
                    objInfo.GColorOne = modifiedDict[rowIndex]["GColorOne"].ToString();
                }
                //修改颜色2
                if (modifiedDict[rowIndex].Keys.Contains("GColorTwo"))
                {
                    objInfo.GColorTwo = modifiedDict[rowIndex]["GColorTwo"].ToString();
                }

                //更新面积和金额
                objInfo.GArea = (objInfo.GHeight / 1000) * (objInfo.GWide / 1000) * objInfo.OrderNumber;
                objInfo.OrderAmount = objInfo.GArea * objInfo.GPrice;

                //更新订单明细
                Core.Container.Instance.Resolve<IServiceContractCabinetInfo>().Update(objInfo);
            }
            //更新订单总金额
            UpdateOrderAmount();
            //重新加载订单发货信息
            BindOrderDetail();
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
            //获取订单柜子总金额
            decimal payNumber = 0;
            string sql = string.Format(@"select isnull(sum(OrderAmount),0) as OrderAmount from ContractCabinetInfo where ContractID ={0}", OrderID);
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0] != null)
            {
                payNumber = decimal.Parse(ds.Tables[0].Rows[0]["OrderAmount"].ToString());
            }
            //获取柜子收费五金总金额
            decimal hwNumber = 0;
            string sqlHW = string.Format(@"select isnull(sum(GoodAmount),0) as GoodAmount from ContractHandWareDetail where ContractID ={0} and IsFree=1 and OrderType=2 ", OrderID);
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
            //更新合同相关费用金额
            ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(OrderID);
            contractInfo.CabinetAmount = payNumber + hwNumber;
            contractInfo.HandWareCost = costNumber;
            contractInfo.TotalAmount = contractInfo.CabinetAmount + contractInfo.DoorAmount;
            ////如果门报价金额大于0，则视为订单为生产中状态
            //if (contractInfo.CabinetAmount > 0)
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
                Core.Container.Instance.Resolve<IServiceContractCabinetInfo>().Delete(id);
            }
            //更新订单总金额
            UpdateOrderAmount();
            //绑定柜子明细
            BindOrderDetail();
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
        /// 添加柜子返回处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window1_Close(object sender, EventArgs e)
        {
            BindOrderDetail();
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
                fileInfo.FileType = 2;
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
            //获取合同信息
            ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(OrderID);
            //获取订单明细
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractInfo.ID", OrderID));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<ContractCabinetInfo> list = Core.Container.Instance.Resolve<IServiceContractCabinetInfo>().GetAllByKeys(qryList, orderList);

            //获取位置信息 
            string sql = string.Format(@"select GoodsType from ContractCabinetInfo where ContractID ={0} group by GoodsType ", OrderID);
            DataSet ds = DbHelperSQL.Query(sql);

            StringBuilder sb = new StringBuilder();
            sb.Append("<meta http-equiv=\"content-type\" content=\"application/excel; charset=UTF-8\"/>");

            #region - 拼凑主订单导出结果 -
            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");

            #region - 拼凑导出的列名 -
            //单据头
            sb.Append("<tr>");
            sb.AppendFormat("<td colspan=\"10\" style=\"font-size :x-large; text-align:center;\">{0}</td>", "重庆喜莱克家具有限公司（家喜林门）定制家具收费明细表");
            sb.Append("</tr>");
            //订单信息
            sb.Append("<tr>");
            sb.AppendFormat("<td colspan=\"2\">{0}</td>", "订单号");
            sb.AppendFormat("<td colspan=\"2\">{0}</td>", contractInfo.ContractNO);
            sb.AppendFormat("<td>{0}</td>", "供货单位");
            sb.AppendFormat("<td colspan=\"2\">{0}</td>", ConfigHelper.StoreName);
            sb.AppendFormat("<td>{0}</td>", "联系电话");
            sb.AppendFormat("<td colspan=\"2\">{0}</td>", ConfigHelper.ContractPhone);
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.AppendFormat("<td colspan=\"2\">{0}</td>", "客户地址");
            sb.AppendFormat("<td colspan=\"5\">{0}</td>", contractInfo.ProjectName);
            sb.AppendFormat("<td>{0}</td>", "联系电话");
            sb.AppendFormat("<td colspan=\"2\">{0}</td>", contractInfo.ContactPhone);
            sb.Append("</tr>");
            //明细表头
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", "序号");
            sb.AppendFormat("<td>{0}</td>", "房间位置及柜体名称");
            sb.AppendFormat("<td>{0}</td>", "产品名称");
            sb.AppendFormat("<td>{0}</td>", "高度(m)");
            sb.AppendFormat("<td>{0}</td>", "宽度(m)");
            sb.AppendFormat("<td>{0}</td>", "数量(块/米)");
            sb.AppendFormat("<td>{0}</td>", "面积(㎡/米)");
            sb.AppendFormat("<td>{0}</td>", "单价（元）");
            sb.AppendFormat("<td>{0}</td>", "金额（元）");
            sb.AppendFormat("<td>{0}</td>", "备注");
            sb.Append("</tr>");

            #endregion

            #region - 拼凑导出的数据行 -

            int recordIndex1 = 1;
            int index = 1;
            decimal singleAmount = 0;
            decimal totalAmount = 0;
            //导出商品明细
            if (ds.Tables[0] != null)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    //获取位置
                    string goodLocation = row["GoodsType"].ToString();
                    //获取位置下面的产品信息
                    List<ContractCabinetInfo> listInfo = list.Where(obj => obj.GoodsType == goodLocation).OrderBy(obj => obj.ID).ToList();
                    int rowspan = listInfo.Count;
                    index = 1;
                    singleAmount = 0;
                    foreach (ContractCabinetInfo obj in listInfo)
                    {
                        sb.Append("<tr>");
                        sb.AppendFormat("<td>{0}</td>", recordIndex1);
                        if (index == 1)
                        {
                            sb.AppendFormat("<td rowspan=\"{1}\">{0}</td>", goodLocation, rowspan);
                        }
                        sb.AppendFormat("<td>{0}</td>", obj.GoodsName);
                        sb.AppendFormat("<td>{0}</td>", obj.GWide / 1000);
                        sb.AppendFormat("<td>{0}</td>", obj.GHeight / 1000);
                        sb.AppendFormat("<td>{0}</td>", obj.OrderNumber);
                        sb.AppendFormat("<td>{0}</td>", obj.GArea);
                        sb.AppendFormat("<td>{0}</td>", obj.GPrice);
                        sb.AppendFormat("<td>{0}</td>", obj.OrderAmount);
                        sb.AppendFormat("<td>{0}</td>", obj.Remark);
                        sb.Append("</tr>");
                        singleAmount += obj.OrderAmount;
                        totalAmount += obj.OrderAmount;
                        recordIndex1++;
                        index++;
                    }
                    //小计
                    //sb.Append("<tr>");
                    //sb.Append("<th colspan=\"8\" style=\"text-align: right;\">小计：</th>");
                    //sb.AppendFormat("<td>{0}</td>", singleAmount);
                    //sb.Append("<td></td>");
                    //sb.Append("</tr>");
                }
            }

            //导出五金明细 
            qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractID", OrderID));
            qryList.Add(Expression.Eq("IsFree", 1));
            qryList.Add(Expression.Eq("OrderType", 2));
            orderList = new Order[1];
            orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<ContractHandWareDetail> listHWDetail = Core.Container.Instance.Resolve<IServiceContractHandWareDetail>().GetAllByKeys(qryList, orderList);
            index = 1;
            singleAmount = 0;
            foreach (ContractHandWareDetail obj in listHWDetail)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td>{0}</td>", recordIndex1);
                if (index == 1)
                {
                    sb.AppendFormat("<td rowspan=\"{0}\">五金及配件</td>", listHWDetail.Count);
                }
                sb.AppendFormat("<td>{0}</td>", obj.GoodsName);
                sb.AppendFormat("<td>{0}</td>", 0);
                sb.AppendFormat("<td>{0}</td>", 0);
                sb.AppendFormat("<td>{0}</td>", obj.GoodsNumber);
                sb.AppendFormat("<td>{0}</td>", 0);
                sb.AppendFormat("<td>{0}</td>", obj.GoodsUnitPrice);
                sb.AppendFormat("<td>{0}</td>", obj.GoodAmount);
                sb.AppendFormat("<td>{0}</td>", "");
                sb.Append("</tr>");
                recordIndex1++;
                index++;
                singleAmount += obj.GoodAmount;
                totalAmount += obj.GoodAmount;
            }
            //五金明细小计
            //sb.Append("<tr>");
            //sb.Append("<th colspan=\"8\" style=\"text-align: right;\">小计：</th>");
            //sb.AppendFormat("<td>{0}</td>", singleAmount);
            //sb.Append("<td></td>");
            //sb.Append("</tr>");

            //合计行
            sb.Append("<tr>");
            sb.Append("<th colspan=\"8\" style=\"text-align: right;font-size:16;\">合计：</th>");
            sb.AppendFormat("<td>{0}</td>", totalAmount);
            sb.Append("<td></td>");
            sb.Append("</tr>");

            #endregion

            //单据尾
            sb.Append("<tr>");
            sb.AppendFormat("<td colspan=\"10\">安装售后：{0} 量尺设计：{1} 投诉电话：{2} 地址：{3}</td>"
                            , ConfigHelper.InstallPhone
                            , ConfigHelper.DesignPhone
                            , ConfigHelper.ComplaintPhone
                            , ConfigHelper.Address);
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td colspan=\"10\"></td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td style=\"text-align: center;\" colspan=\"5\">客户确认签字</td>");
            sb.Append("<td style=\"text-align: center;\" colspan=\"5\">货方签字（盖章）</td>");
            sb.Append("</tr>");

            sb.Append("</table>");

            #endregion

            return sb.ToString();
        }

        #endregion 导出客户报价单


        #endregion
    }
}
