using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Data;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractSHManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreSHOrderView";
            }
        }

        #endregion

        #region 加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //页面数据初始加载
                LoadData();
            }
        }

        /// <summary>
        /// 加载页面记录
        /// </summary>
        private void LoadData()
        {
            // 权限检查 
            CheckPowerWithButton("CoreSHOrderEdit", btnNew);
            //设置记录分页记录数
            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
            //加载收货单信息
            BindGrid();
        }
        #endregion 加载

        #region 获取订单信息

        /// <summary>
        /// 获取订单信息
        /// </summary>
        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            string qryName = txtOrderNo.Text.Trim();
            qryList.Add(Expression.Eq("OrderType", 2));
            if (!string.IsNullOrEmpty(qryName))
            {
                qryList.Add(Expression.Like("ManualNO", qryName, MatchMode.Anywhere)
                || Expression.Like("CustomerName", qryName, MatchMode.Anywhere));
            }
            if (!string.IsNullOrEmpty(dpStartDate.Text))
            {
                qryList.Add(Expression.Ge("OrderDate", DateTime.Parse(dpStartDate.Text)));
            }
            if (!string.IsNullOrEmpty(dpEndDate.Text))
            {
                qryList.Add(Expression.Le("OrderDate", DateTime.Parse(dpEndDate.Text + " 23:59:59")));
            }
            if (!string.IsNullOrEmpty(ddlState.SelectedValue))
            {
                qryList.Add(Expression.Eq("IsTemp", int.Parse(ddlState.SelectedValue)));
            }
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", false);
            orderList[0] = orderli;
            int count = 0;
            IList<ContractOrderInfo> list = Core.Container.Instance.Resolve<IServiceContractOrderInfo>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        #endregion 获取订单信息

        #region Events

        /// <summary>
        /// 绑定记录前按钮权限判断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            // 数据绑定之前，进行权限检查
            CheckPowerWithWindowField("CoreSHOrderEdit", Grid1, "editField");
            CheckPowerWithLinkButtonField("CoreSHOrderDelete", Grid1, "deleteField");
        }

        /// <summary>
        /// 绑定记录前按钮权限判断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PreRowDataBound(object sender, GridPreRowEventArgs e)
        {
            ContractOrderInfo row = e.DataItem as ContractOrderInfo;
            int isTemp = Convert.ToInt32(row.IsTemp);
            LinkButtonField lbtnEditField = Grid1.FindColumn("lbtnEditField") as LinkButtonField;
            LinkButtonField deleteField = Grid1.FindColumn("deleteField") as LinkButtonField;
            lbtnEditField.Enabled = isTemp > 0;
            deleteField.Enabled = isTemp > 0;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        
        /// <summary>
        /// 分页每页记录数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindGrid();
        }

        /// <summary>
        /// 记录列表行记录操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            //获取当前选中记录信息
            ContractOrderInfo orderInfo = Core.Container.Instance.Resolve<IServiceContractOrderInfo>().GetEntity(ID);

            if (orderInfo != null)
            {
                if (orderInfo.IsTemp == 1)
                {
                    //删除收货单
                    if (e.CommandName == "Delete")
                    {
                        //删除订单信息及附属信息
                        DeleteOrderByID(ID, orderInfo.OrderNO);
                        //加载收货单信息
                        BindGrid();
                    }
                    //编辑收货单
                    if (e.CommandName == "editField")
                    {
                        PageContext.Redirect(string.Format("~/Contract/SH/ContractSHEdit.aspx?id={0}", ID));
                    }
                }
                else
                {
                    Alert.Show("正式订单不能删除或者修改！");
                }
            }
        }

        /// <summary>
        /// 批量删除订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            foreach (int id in ids)
            {
                //获取当前选中记录信息
                ContractOrderInfo orderInfo = Core.Container.Instance.Resolve<IServiceContractOrderInfo>().GetEntity(id);
                if (orderInfo != null)
                {
                    if (orderInfo.IsTemp == 1)
                    {
                        //删除订单信息及附属信息
                        DeleteOrderByID(id, orderInfo.OrderNO);
                    }
                }
            }
            //加载收货单信息
            BindGrid();
        }

        /// <summary>
        /// 删除订单信息及附属信息
        /// </summary>
        /// <param name="orderID">订单ID</param>
        /// <param name="orderNO">订单编号</param>
        private void DeleteOrderByID(int orderID, string orderNO)
        {
            //删除订单附属相关信息
            string sqlWhere = string.Format(" OrderNO='{0}' ", orderNO);
            Core.Container.Instance.Resolve<IServiceContractOrderDetail>().DelelteAll(sqlWhere);
            Core.Container.Instance.Resolve<IServiceContractOrderSecondaryDetail>().DelelteAll(sqlWhere);
            Core.Container.Instance.Resolve<IServiceContractOrderCostInfo>().DelelteAll(sqlWhere);
            //删除临时订单信息  
            Core.Container.Instance.Resolve<IServiceContractOrderInfo>().Delete(orderID);
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //加载收货单信息
            BindGrid();
        }

        /// <summary>
        /// 新增记录按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNew_Click(object sender, EventArgs e)
        {
            List<ContractShowObj> listShow = new List<ContractShowObj>();
            //获取执行中的合同信息
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractState", "1"));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<ContractInfo> list = Core.Container.Instance.Resolve<IServiceContractInfo>().GetAllByKeys(qryList, orderList);
            //判读是否有执行中的合同，如没有，不能收货
            if (list.Count > 0)
            {
                PageContext.Redirect("~/Contract/SH/ContractSHEdit.aspx");
            }
            else
            {
                Alert.Show("当前没有执行中的合同，不能创建收货单！", MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 订单信息查看弹出页面关闭处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window1_Close(object sender, EventArgs e)
        {
            //加载收货单信息
            BindGrid();
        }
        #endregion
    }
}