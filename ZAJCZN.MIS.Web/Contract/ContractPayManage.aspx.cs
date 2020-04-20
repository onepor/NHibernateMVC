using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Helpers;
using ZAJCZN.MIS.Service;
using Newtonsoft.Json.Linq;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractPayManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CorePayment";
            }
        }

        #endregion

        #region request param

        //订单ID（传入参数）
        private int OrderID
        {
            get { return GetQueryIntValue("id"); }
        }

        #endregion request param

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 权限检查
                CheckPowerWithButton("CorePayment", btnNew);
                btnNew.OnClientClick = Window1.GetShowReference(string.Format("~/Contract/ContractPayAdd.aspx?id={0}", OrderID), "新增收/付款单据");
                Grid1.AutoScroll = true;
                LoadData();
            }
        }

        private void LoadData()
        {
            //绑定数据
            BindGrid();
        }

        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractID", OrderID));
            qryList.Add(!Expression.Eq("PayType", 3));
            if (!string.IsNullOrEmpty(ddlPay.SelectedValue))
            {
                qryList.Add(Expression.Eq("PayType", int.Parse(ddlPay.SelectedValue)));
            }
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<ContractPayInfo> list = Core.Container.Instance.Resolve<IServiceContractPayInfo>().GetAllByKeys(qryList, orderList);

            Grid1.DataSource = list;
            Grid1.DataBind();

            decimal donateTotal = 0M;
            foreach (ContractPayInfo eqpInfo in list)
            {
                donateTotal += eqpInfo.PayMoney;
            }
            //绑定合计数据
            JObject summary = new JObject();
            summary.Add("PayUser", "金额合计");
            summary.Add("PayMoney", donateTotal);

            Grid1.SummaryData = summary;
        }

        public string GetOrderState(string state)
        {
            string i = "";
            switch (state)
            {
                case "1":
                    i = "待审核";
                    break;
                case "2":
                    i = "通过";
                    break;
                case "3":
                    i = "未通过";
                    break;
            }
            return i;
        }

        public System.Drawing.Color GetColor(string state)
        {
            System.Drawing.Color i = new System.Drawing.Color();
            switch (state)
            {
                case "1":
                    i = System.Drawing.Color.Black;
                    break;
                case "2":
                    i = System.Drawing.Color.Blue;
                    break;
            }
            return i;
        }

        public System.Drawing.Color GetColor1(string state)
        {
            System.Drawing.Color i = new System.Drawing.Color();
            switch (state)
            {
                case "1":
                    i = System.Drawing.Color.Black;
                    break;
                case "2":
                    i = System.Drawing.Color.Green;
                    break;
                case "3":
                    i = System.Drawing.Color.Red;
                    break;
            }
            return i;
        }

        #endregion

        #region Events

        protected void btnDeleteHW_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            ContractPayInfo payInfo = new ContractPayInfo();
            foreach (int id in ids)
            {
                payInfo = Core.Container.Instance.Resolve<IServiceContractPayInfo>().GetEntity(id);
                if (payInfo.ApplyState == 1)
                {
                    Core.Container.Instance.Resolve<IServiceContractPayInfo>().Delete(id);
                }
                else
                {
                    Alert.Show("已审核通过的订单不能删除！");
                }
            }
            BindGrid();
        }

        protected void ddlPay_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// 返回订单列表页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnReturn_Click(object sender, EventArgs e)
        {
            //返回订单列表页面
            PageContext.Redirect("~/Contract/ContractDesignManage.aspx");
        }


        /// <summary>
        /// 添加柜子返回处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        #region 导出客户报价单

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.xls", OrderNO));
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
            sb.AppendFormat("<td>{0}</td>", "数量(块)");
            sb.AppendFormat("<td>{0}</td>", "面积(㎡)");
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
                    sb.Append("<tr>");
                    sb.Append("<th colspan=\"8\" style=\"text-align: right;\">小计：</th>");
                    sb.AppendFormat("<td>{0}</td>", singleAmount);
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
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
            sb.Append("<tr>");
            sb.Append("<th colspan=\"8\" style=\"text-align: right;\">小计：</th>");
            sb.AppendFormat("<td>{0}</td>", singleAmount);
            sb.Append("<td></td>");
            sb.Append("</tr>");

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

            sb.Append("</table>");

            #endregion

            return sb.ToString();
        }

        #endregion 导出客户报价单

        #endregion

        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            int rowIndex = Grid1.SelectedRowIndex;
            hdNO.Text = Grid1.DataKeys[rowIndex][0].ToString();
            ContractPayInfo payInfo = Core.Container.Instance.Resolve<IServiceContractPayInfo>().GetEntity(int.Parse(hdNO.Text));
            hdType.Text = payInfo.PayType.ToString();
        }
    }
}
