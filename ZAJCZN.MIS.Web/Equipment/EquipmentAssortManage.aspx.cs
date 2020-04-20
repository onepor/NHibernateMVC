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
    public partial class EquipmentAssortManage : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreEquipmentView";
            }
        }

        #endregion

        private int DishesID
        {
            get { return GetQueryIntValue("id"); }
        }

        private int TypeID
        {
            get { return GetQueryIntValue("type"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckPowerWithButton("CoreEquipmentEdit", btnNew);

                btnNew.OnClientClick = Window1.GetShowReference(string.Format("~/Equipment/EquipmentAssortSelectDialog.aspx?rowid={0}&type={1}", DishesID, TypeID), "添加配套物品");
                // 删除选中单元格的客户端脚本 
                btnDeleteSelected.OnClientClick = Grid1.GetNoSelectionAlertInParentReference("请至少应该选择一项记录！");
                btnDeleteSelected.ConfirmText = String.Format("确定要删除选中的<span class=\"highlight\"><script>{0}</script></span>项配料记录吗？", Grid1.GetSelectedCountReference());
                btnDeleteSelected.ConfirmTarget = FineUIPro.Target.Top;
                if (TypeID == 2)
                {
                    btnUpdate.Hidden = true;
                }
                // 绑定表格
                BindGrid();
                //绑定菜品信息
                BindDishesInfo();
            }
        }

        #region 绑定菜品数据
        private void BindDishesInfo()
        {
            if (TypeID == 1)
            {
                EquipmentTypeInfo DishesInfo = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(DishesID);
                Grid1.Title = string.Format("主材名称：{0}（点击配套数量直接修改数量）", DishesInfo != null ? DishesInfo.TypeName : "");
            }
            else
            {
                EquipmentInfo DishesInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(DishesID);
                Grid1.Title = string.Format("主材名称：{0}（点击配套数量直接修改数量）", DishesInfo != null ? string.Format("{0}[{1}]", DishesInfo.EquipmentName, "") : "");
            }
        }

        #endregion 绑定菜品数据

        #region BindGrid

        private void BindGrid()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ParentEquipmentID", DishesID));
            qryList.Add(Expression.Eq("EquipmentType", TypeID.ToString()));
            List<EquipmentInfo> eqpList = new List<EquipmentInfo>();

            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            int count = 0;
            IList<EquipmentAssortInfo> list = Core.Container.Instance.Resolve<IServiceEquipmentAssortInfo>().GetPaged(qryList, orderList, Grid1.PageIndex, Grid1.PageSize, out count);
            foreach (EquipmentAssortInfo assortInfo in list)
            {
                assortInfo.EquipmentInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(assortInfo.EquipmentID);
            }

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

        //获取分类名称
        public string GetType(string typeID)
        {
            EquipmentTypeInfo objType = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetEntity(int.Parse(typeID));
            return objType != null ? objType.TypeName : "";
        }
        #endregion 页面数据转化

        #region Events

        protected void Grid1_AfterEdit(object sender, GridAfterEditEventArgs e)
        {
            Dictionary<int, Dictionary<string, object>> modifiedDict = Grid1.GetModifiedDict();

            foreach (int rowIndex in modifiedDict.Keys)
            {
                int rowID = Convert.ToInt32(Grid1.DataKeys[rowIndex][0]);
                EquipmentAssortInfo objInfo = Core.Container.Instance.Resolve<IServiceEquipmentAssortInfo>().GetEntity(rowID);
                if (modifiedDict[rowIndex].Keys.Contains("AssortCount"))
                {
                    objInfo.AssortCount = Convert.ToDecimal(modifiedDict[rowIndex]["AssortCount"]);
                }
                if (modifiedDict[rowIndex].Keys.Contains("EquipmentCount"))
                {
                    objInfo.EquipmentCount = Convert.ToDecimal(modifiedDict[rowIndex]["EquipmentCount"]);
                }
                if (modifiedDict[rowIndex].Keys.Contains("IsOutCalcNumber"))
                {
                    objInfo.IsOutCalcNumber = Convert.ToInt32(modifiedDict[rowIndex]["IsOutCalcNumber"]);
                }
                if (modifiedDict[rowIndex].Keys.Contains("IsOutCalcPrice"))
                {
                    objInfo.IsOutCalcPrice = Convert.ToInt32(modifiedDict[rowIndex]["IsOutCalcPrice"]);
                }
                if (modifiedDict[rowIndex].Keys.Contains("IsInCalcNumber"))
                {
                    objInfo.IsInCalcNumber = Convert.ToInt32(modifiedDict[rowIndex]["IsInCalcNumber"]);
                }
                if (modifiedDict[rowIndex].Keys.Contains("IsInCalcPrice"))
                {
                    objInfo.IsInCalcPrice = Convert.ToInt32(modifiedDict[rowIndex]["IsInCalcPrice"]);
                }


                Core.Container.Instance.Resolve<IServiceEquipmentAssortInfo>().Update(objInfo);
            }

            BindGrid();
        }

        public void btnReturn_Click(object sender, EventArgs e)
        {
            if (TypeID == 1)
            {
                PageContext.Redirect("~/Equipment/EquipmentTypeManage.aspx");
            }
            else
            {
                PageContext.Redirect("~/Equipment/EquipmentManage.aspx");
            }
        }

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            CheckPowerWithLinkButtonField("CoreEquipmentEdit", Grid1, "deleteField");
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
                Core.Container.Instance.Resolve<IServiceEquipmentAssortInfo>().Delete(id);
            }
            BindGrid();
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            int ID = GetSelectedDataKeyID(Grid1);
            if (e.CommandName == "Delete")
            {
                Core.Container.Instance.Resolve<IServiceEquipmentAssortInfo>().Delete(ID);
                BindGrid();
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string sqlWhere = string.Empty;
            IList<ICriterion> qryList = new List<ICriterion>();

            //获取物品规格信息
            qryList.Add(Expression.Eq("EquipmentTypeID", DishesID));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<EquipmentInfo> eqpList = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetAllByKeys(qryList, orderList);
            //获取物品配套辅材信息
            qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ParentEquipmentID", DishesID));
            qryList.Add(Expression.Eq("EquipmentType", "1"));
            IList<EquipmentAssortInfo> list = Core.Container.Instance.Resolve<IServiceEquipmentAssortInfo>().GetAllByKeys(qryList, orderList);

            foreach (EquipmentInfo eqpObj in eqpList)
            {
                //删除物品规格已有配套辅材信息
                sqlWhere = string.Format(" ParentEquipmentID={0} and EquipmentType='2' ", eqpObj.ID);
                Core.Container.Instance.Resolve<IServiceEquipmentAssortInfo>().DelelteAll(sqlWhere);
                foreach (EquipmentAssortInfo obj in list)
                {
                    //添加新的配套材料信息
                    EquipmentAssortInfo newObj = new EquipmentAssortInfo();
                    newObj.EquipmentCount = obj.EquipmentCount;
                    newObj.AssortCount = obj.AssortCount;
                    newObj.EquipmentID = obj.EquipmentID;
                    newObj.EquipmentType = "2";
                    newObj.Remark = obj.Remark;
                    newObj.IsInCalcNumber = obj.IsInCalcNumber;
                    newObj.IsInCalcPrice = obj.IsInCalcPrice;
                    newObj.IsOutCalcNumber = obj.IsOutCalcNumber;
                    newObj.IsOutCalcPrice = obj.IsOutCalcPrice;
                    newObj.ParentEquipmentID = eqpObj.ID;
                    Core.Container.Instance.Resolve<IServiceEquipmentAssortInfo>().Create(newObj);
                }
            }
            Alert.ShowInTop("物品规格信息中配套材料信息同步更新完成！", MessageBoxIcon.Information);
        }

        #endregion
    }
}
