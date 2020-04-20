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

namespace ZAJCZN.MIS.Web
{
    public partial class DishesBatchingManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreDishesView";
            }
        }

        #endregion

        private int DishesID
        {
            get { return GetQueryIntValue("id"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnNew.OnClientClick = Window1.GetShowReference(string.Format("~/PublicWebForm/GoodSelectDialog.aspx?rowid={0}", DishesID), "添加配料");
                // 删除选中单元格的客户端脚本 
                btnDeleteSelected.OnClientClick = Grid1.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
                btnDeleteSelected.ConfirmText = String.Format("确定要删除选中的<span class=\"highlight\"><script>{0}</script></span>项配料记录吗？", Grid1.GetSelectedCountReference());
                btnDeleteSelected.ConfirmTarget = FineUIPro.Target.Top;

                // 绑定表格
                BindGrid();
                //绑定菜品信息
                BindDishesInfo();
            }
        }

        #region 绑定菜品数据
        private void BindDishesInfo()
        {
            tm_Dishes DishesInfo = Core.Container.Instance.Resolve<IServiceDishes>().GetEntity(DishesID);
            Grid1.Title = string.Format("菜品名称：{0}（点击消耗数量直接修改数量）", DishesInfo != null ? DishesInfo.DishesName : "");
        }

        #endregion 绑定菜品数据

        #region BindGrid

        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("DishesInfo.ID", DishesID));

            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            int count = 0;
            IList<tm_DishesBatching> list = Core.Container.Instance.Resolve<IServiceDishesBatching>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);

            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        #endregion

        #region 页面数据转化

        //获取单位
        public string GetUnitName(string state)
        {
            return GetSystemEnumValue("WPDW", state);
        }

        public string GetIsUsed(string state)
        {
            string i = "";
            switch (state)
            {
                case "1":
                    i = "是";
                    break;
                case "0":
                    i = "否";
                    break;
            }
            return i;
        }

        #endregion 页面数据转化

        #region Events

        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            Dictionary<int, Dictionary<string, object>> modifiedDict = Grid1.GetModifiedDict();

            foreach (int rowIndex in modifiedDict.Keys)
            {
                int rowID = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
                tm_DishesBatching objInfo = Core.Container.Instance.Resolve<IServiceDishesBatching>().GetEntity(rowID);

                objInfo.UsingCount = Convert.ToDecimal(modifiedDict[rowIndex]["UsingCount"]);
                objInfo.CostPrice = objInfo.UsingUnitPrice * objInfo.UsingCount;

                Core.Container.Instance.Resolve<IServiceDishesBatching>().Update(objInfo);
            }

            BindGrid();
        }

        public void btnReturn_Click(object sender, EventArgs e)
        {
            PageContext.Redirect("~/BusinessSet/DishesManage.aspx");
        }

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            CheckPowerWithLinkButtonField("CoreGoodsEdit", Grid1, "deleteField");
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            foreach (int id in ids)
            {
                Core.Container.Instance.Resolve<IServiceDishesBatching>().Delete(id);
            }
            BindGrid();
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            if (e.CommandName == "Delete")
            {
                Core.Container.Instance.Resolve<IServiceDishesBatching>().Delete(ID);
                BindGrid();
            }
        }

        #endregion

    }
}
