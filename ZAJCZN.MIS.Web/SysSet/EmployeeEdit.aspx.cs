using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class EmployeeEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreEmployeeEdit";
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
                CheckPowerWithButton("CoreEmployeeEdit", btnSaveClose);

                if (action == "edit")
                {
                    Bind();
                    //txtVipName.Readonly = true;
                }

                btnClose.OnClientClick = ActiveWindow.GetHideReference();
            }
        }

        private void Bind()
        {
            EmployeeInfo entity = Core.Container.Instance.Resolve<IServiceEmployeeInfo>().GetEntity(_id);
            txtRemark.Text = entity.Remark;
            ddlType.SelectedValue = entity.UserType;
            txbVipPhone.Text = entity.ContractPhone;
            txtAddress.Text = entity.ContractAddress;
            rbtnSex.SelectedValue = entity.Sex;
            txtVipName.Text = entity.EmployeeName;
            ddlIsUsed.SelectedValue = entity.IsUsed;
        }

        #endregion

        #region Events
        private void SaveItem()
        {
            EmployeeInfo EmployeeInfo = new EmployeeInfo();
            if (action == "edit")
            {
                EmployeeInfo = Core.Container.Instance.Resolve<IServiceEmployeeInfo>().GetEntity(_id);
            }
            EmployeeInfo.EmployeeName = txtVipName.Text.Trim();
            EmployeeInfo.Remark = txtRemark.Text.Trim();
            EmployeeInfo.ContractPhone = txbVipPhone.Text.Trim();
            EmployeeInfo.ContractAddress = txtAddress.Text.Trim();
            EmployeeInfo.Sex = rbtnSex.SelectedValue;
            EmployeeInfo.IsUsed = ddlIsUsed.SelectedValue;
            EmployeeInfo.UserType = ddlType.SelectedValue;
            if (action == "edit")
            {
                Core.Container.Instance.Resolve<IServiceEmployeeInfo>().Update(EmployeeInfo);
            }
            else
            {
                Core.Container.Instance.Resolve<IServiceEmployeeInfo>().Create(EmployeeInfo);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            if (action == "add")
            {
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("EmployeeName", txtVipName.Text.Trim()));
                qryList.Add(Expression.Eq("UserType", ddlType.SelectedValue));
                Order[] orderList = new Order[1];
                Order orderli = new Order("ID", true);
                orderList[0] = orderli;
                EmployeeInfo EmployeeObj = Core.Container.Instance.Resolve<IServiceEmployeeInfo>().GetFirstEntityByFields(qryList, orderList);

                //判断重复
                if (EmployeeObj != null)
                {
                    Alert.Show("该分类下员工姓名已存在！");
                    return;
                }
            }
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion
    }
}