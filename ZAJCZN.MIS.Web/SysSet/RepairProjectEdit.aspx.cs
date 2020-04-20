using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class RepairProjectEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreRepairProjectEdit";
            }
        }

        #endregion

        #region Page_Load

        protected string action
        {
            get
            {
                return GetQueryValue("action");
            }
        }
        protected int _id
        {
            get
            {
                return GetQueryIntValue("id");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //权限检查
                CheckPowerWithButton("CoreRepairProjectEdit", btnSaveClose);
                //绑定物品单位
                BindUnit();

                if (action == "edit")
                {
                    Bind();
                    txtVipName.Readonly = true;
                }

                btnClose.OnClientClick = ActiveWindow.GetHideReference();
            }
        }

        private void Bind()
        {
            RepairProjectInfo entity = Core.Container.Instance.Resolve<IServiceRepairProjectInfo>().GetEntity(_id);
            txtRemark.Text = entity.Remark;
            ddlUnit.SelectedValue = entity.PayUnit;
            txtDailyRents.Text = entity.PayPrice.ToString();
            txtVipName.Text = entity.ProjectName;
            ddlIsUsed.SelectedValue = entity.IsUsed;
            ddlType.SelectedValue = entity.ProjectType.ToString();
            ddlUsingType.SelectedValue = entity.UsingType.ToString();
            ddlContractPrice.SelectedValue = entity.PriceSourceType.ToString();
            ddlIsRegular.SelectedValue = entity.IsRegular.ToString();
            rbtnIsCreateJob.SelectedValue = entity.IsCreateJob.ToString();
            tbtnIsSorting.SelectedValue = entity.IsSorting.ToString();
            if (entity.IsSorting == 1)
            {
                //绑定物品类别
                BindGoodsTypes(1);
            }
            else
            {
                BindGoodsTypes(0);
            }

            if (!string.IsNullOrEmpty(entity.UsingGoods))
            {
                cblGoods.SelectedValueArray = entity.UsingGoods.Split(',');
            }
        }

        #region 绑定物品单位
        private void BindUnit()
        {
            List<Tm_Enum> list = GetSystemEnumByTypeKey("FYDW", false);
            list.Insert(0, new Tm_Enum { EnumKey = "", EnumValue = "" });
            ddlUnit.DataSource = list;
            ddlUnit.DataBind();
        }
        #endregion

        #region 绑定物品单位类别
        private void BindGoodsTypes(int isSorting)
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("IsUsed", "1"));
            qryList.Add(Expression.Eq("TypeClass", 1));
            if (isSorting == 1)
            {
                qryList.Add(Expression.Eq("IsStockByRepaired", 1));
            }
            IList<EquipmentTypeInfo> list = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetAllByKeys(qryList);

            cblGoods.DataTextField = "TypeName";
            cblGoods.DataValueField = "ID";
            cblGoods.DataSource = list;
            cblGoods.DataBind();
        }
        #endregion

        #endregion

        #region Events

        protected void ddlContractPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDailyRents.Readonly = false;
            if (!ddlContractPrice.SelectedValue.Equals("0"))
            {
                txtDailyRents.Text = "0";
                txtDailyRents.Readonly = true;
            }
        }

        private void SaveItem()
        {
            RepairProjectInfo RepairProjectInfo = new RepairProjectInfo();
            if (action == "edit")
            {
                RepairProjectInfo = Core.Container.Instance.Resolve<IServiceRepairProjectInfo>().GetEntity(_id);
            }
            RepairProjectInfo.ProjectName = txtVipName.Text.Trim();
            RepairProjectInfo.Remark = txtRemark.Text.Trim();
            RepairProjectInfo.PayPrice = !string.IsNullOrEmpty(txtDailyRents.Text) ? Math.Round(decimal.Parse(txtDailyRents.Text), 3) : 0;
            RepairProjectInfo.PayUnit = ddlUnit.SelectedValue;
            RepairProjectInfo.IsUsed = ddlIsUsed.SelectedValue;
            RepairProjectInfo.ProjectType = int.Parse(ddlType.SelectedValue);
            RepairProjectInfo.UsingType = int.Parse(ddlUsingType.SelectedValue);
            RepairProjectInfo.PriceSourceType = int.Parse(ddlContractPrice.SelectedValue);
            RepairProjectInfo.IsRegular = int.Parse(ddlIsRegular.SelectedValue);
            RepairProjectInfo.IsCreateJob = int.Parse(rbtnIsCreateJob.SelectedValue);
            RepairProjectInfo.IsSorting = int.Parse(tbtnIsSorting.SelectedValue);
            if (cblGoods.SelectedValueArray.Length > 0)
            {
                RepairProjectInfo.UsingGoods = String.Join(",", cblGoods.SelectedValueArray);
            }
            else
            {
                RepairProjectInfo.UsingGoods = "";
            }

            if (action == "edit")
            {
                Core.Container.Instance.Resolve<IServiceRepairProjectInfo>().Update(RepairProjectInfo);
            }
            else
            {
                Core.Container.Instance.Resolve<IServiceRepairProjectInfo>().Create(RepairProjectInfo);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            if (action == "add")
            {
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("ProjectName", txtVipName.Text.Trim()));
                RepairProjectInfo RepairProjectObj = Core.Container.Instance.Resolve<IServiceRepairProjectInfo>().GetEntityByFields(qryList);

                //判断重复
                if (RepairProjectObj != null)
                {
                    Alert.Show("维修项目名称已存在！");
                    return;
                }
            }
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected void tbtnIsSorting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbtnIsSorting.SelectedValue.Equals("1"))
            {
                //绑定物品类别
                BindGoodsTypes(1);
            }
            else
            {
                //绑定物品类别
                BindGoodsTypes(0);
            }
        }

        #endregion



    }
}