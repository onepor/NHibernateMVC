using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Data;

namespace ZAJCZN.MIS.Web
{
    public partial class EquipmentAssortSelectDialog : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreEquipmentEdit";
            }
        }

        #endregion

        private int DishesID
        {
            get { return GetQueryIntValue("rowid"); }
        }
        private int TypeID
        {
            get { return GetQueryIntValue("type"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Grid1.PageSize = ConfigHelper.PageSize;
                ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString();
                //绑定菜品信息
                BindDishesInfo();
                //绑定物品类型信息
                BindCostType();
                //绑定物品列表
                BindGrid();

            }
        }

        #region 绑定数据
        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            string qryName = ttbSearchMessage.Text.Trim();
            if (!string.IsNullOrEmpty(qryName))
            {
                qryList.Add(Expression.Like("EquipmentName", qryName, MatchMode.Anywhere)
                            || Expression.Like("EquipmentCode", qryName.ToUpper(), MatchMode.Anywhere));
            }
            qryList.Add(Expression.Eq("IsUsed", "1"));
            if (TypeID == 2)
            {
                qryList.Add(Expression.Gt("ID", DishesID) || Expression.Lt("ID", DishesID));
            }

            if (ddlCostType.SelectedValue != "0")
            {
                qryList.Add(Expression.Eq("EquipmentTypeID", int.Parse(ddlCostType.SelectedValue)));
            }

            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            //int count = 0;
            IList<EquipmentInfo> list = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetAllByKeys(qryList, orderList);
            // Grid1.RecordCount = count;
            Grid1.DataSource = list;
            Grid1.DataBind();
        }

        #endregion

        #region 绑定物品类型
        private void BindCostType()
        {
            List<EquipmentTypeInfo> myList = new List<EquipmentTypeInfo>();

            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("IsUsed", "1"));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<EquipmentTypeInfo> list = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetAllByKeys(qryList, orderList);

            myList.AddRange(list);
            myList.Insert(0, new EquipmentTypeInfo { ID = 0, TypeName = "全部分类" });

            ddlCostType.DataSource = myList;
            ddlCostType.DataBind();
            ddlCostType.SelectedIndex = 0;
        }
        #endregion

        #region 绑定菜品数据
        private void BindDishesInfo()
        {
            if (TypeID == 1)
            {
                EquipmentTypeInfo DishesInfo = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(DishesID);
                lblTitle.Text = string.Format("主材名称：{0}（点击配套数量直接修改数量）", DishesInfo != null ? DishesInfo.TypeName : "");
            }
            else
            {
                EquipmentInfo DishesInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(DishesID);
                //lblTitle.Text = string.Format("主材名称：{0}（点击配套数量直接修改数量）", DishesInfo != null ? string.Format("{0}[{1}]", DishesInfo.EquipmentName, DishesInfo.Standard) : "");
            }
        }

        #endregion 绑定菜品数据

        #region 页面数据转换

        //获取分类名称
        public string GetType(string typeID)
        {
            EquipmentTypeInfo objType = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(int.Parse(typeID));
            return objType != null ? objType.TypeName : "";
        }
        //获取单位
        public string GetUnitName(string state)
        {
            return GetSystemEnumValue("WPDW", state);
        }

        #endregion

        #region Events

        protected void ttbSearchMessage_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearchMessage.Text = String.Empty;
            ttbSearchMessage.ShowTrigger1 = false;
            BindGrid();
        }

        protected void ttbSearchMessage_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearchMessage.ShowTrigger1 = true;
            BindGrid();
        }

        protected void ddlCostType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindGrid();
        }

        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        private bool IsExists(int goodsID)
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ParentEquipmentID", DishesID));
            qryList.Add(Expression.Eq("EquipmentID", goodsID));
            qryList.Add(Expression.Eq("EquipmentType", TypeID.ToString()));

            EquipmentAssortInfo objInfo = Core.Container.Instance.Resolve<IServiceEquipmentAssortInfo>().GetEntityByFields(qryList);
            return objInfo != null ? true : false;
        }

        private void SaveItem()
        {
            // 从每个选中的行中获取ID（在Grid1中定义的DataKeyNames）
            List<int> ids = GetSelectedDataKeyIDs(Grid1);
            EquipmentAssortInfo dbEntity = new EquipmentAssortInfo();
            EquipmentInfo dishesInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(DishesID);

            // 执行数据库操作
            foreach (int ID in ids)
            {
                //判断是否已经添加改配料物品
                if (!IsExists(ID))
                {
                    dbEntity = new EquipmentAssortInfo();
                    dbEntity.EquipmentID = ID;
                    dbEntity.EquipmentType = TypeID.ToString();
                    dbEntity.ParentEquipmentID = DishesID;
                    dbEntity.AssortCount = 1;
                    dbEntity.EquipmentCount = 1;
                    Core.Container.Instance.Resolve<IServiceEquipmentAssortInfo>().Create(dbEntity);
                }
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();
            Alert.Show("配套物品添加成功!");
            BindGrid();
            //PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion Events

    }
}