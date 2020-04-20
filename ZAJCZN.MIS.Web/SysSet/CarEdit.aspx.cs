using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class CarEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreCarEdit";
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
                CheckPowerWithButton("CoreCarEdit", btnSaveClose);

                if (action == "edit")
                {
                    Bind();
                }

                btnClose.OnClientClick = ActiveWindow.GetHideReference();
            }
        }

        private void Bind()
        {
            CarInfo entity = Core.Container.Instance.Resolve<IServiceCarInfo>().GetEntity(_id);
            txtRemark.Text = entity.Remark;
            txbVipPhone.Text = entity.ContractPhone;
            txtAddress.Text = entity.ContractAddress;
            txtLinkMan.Text = entity.DriverName;
            txtVipName.Text = entity.CarNO;
            ddlIsUsed.SelectedValue = entity.IsUsed;
            txtCarLoad.Text = entity.CarLoad;
            rbtnChargingType.SelectedValue = entity.ChargingType;
            txtDailyRents.Text = entity.PayPrice.ToString();
            rbtnPayType.SelectedValue = entity.IsCalcPeice.ToString();
        }

        #endregion

        #region Events
        private void SaveItem()
        {
            CarInfo carInfo = new CarInfo();
            if (action == "edit")
            {
                carInfo = Core.Container.Instance.Resolve<IServiceCarInfo>().GetEntity(_id);
            }
            carInfo.CarNO = txtVipName.Text.Trim();
            carInfo.CarLoad = txtCarLoad.Text.Trim();
            carInfo.Remark = txtRemark.Text.Trim();
            carInfo.ContractPhone = txbVipPhone.Text.Trim();
            carInfo.ContractAddress = txtAddress.Text.Trim();
            carInfo.DriverName = txtLinkMan.Text.Trim();
            carInfo.IsUsed = ddlIsUsed.SelectedValue;
            carInfo.ChargingType = rbtnChargingType.SelectedValue;
            carInfo.IsCalcPeice = int.Parse(rbtnPayType.SelectedValue);
            carInfo.PayPrice = !string.IsNullOrEmpty(txtDailyRents.Text) ? Math.Round(decimal.Parse(txtDailyRents.Text), 2) : 1;
            if (action == "edit")
            {
                Core.Container.Instance.Resolve<IServiceCarInfo>().Update(carInfo);
            }
            else
            {
                Core.Container.Instance.Resolve<IServiceCarInfo>().Create(carInfo);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            if (action == "add")
            {
                IList<ICriterion> qryList = new List<ICriterion>();
                qryList.Add(Expression.Eq("CarNO", txtVipName.Text.Trim()));
                CarInfo CarObj = Core.Container.Instance.Resolve<IServiceCarInfo>().GetEntityByFields(qryList);

                //判断重复
                if (CarObj != null)
                {
                    Alert.Show("车牌号已存在！");
                    return;
                }
            }
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion
    }
}