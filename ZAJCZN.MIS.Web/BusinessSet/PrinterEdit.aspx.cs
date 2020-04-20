using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web
{
    public partial class PrinterEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CorePrinterEdit";
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
                CheckPowerWithButton("CorePrinterEdit", btnSaveClose);
                //绑定打印机配置
                BindDateTime();

                if (action == "edit")
                {
                    Bind();
                }
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
            }
        }

        private void BindDateTime()
        {
            List<Tm_Enum> list = GetSystemEnumByTypeKey("DYJPZ", false);
            ckeDeploy.DataSource = list;
            ckeDeploy.DataBind();
        }

        private void Bind()
        {
            tm_Printer entity = Core.Container.Instance.Resolve<IServicePrinter>().GetEntity(_id);
            labPrinterName.Text = entity.PrinterName;
            ddlWidth.SelectedValue = entity.Width;
            radioPrinterType.SelectedValue = entity.PrinterType;
            radioIsSinging.SelectedValue = entity.IsSinging;
            txbIP.Text = entity.IP;
            numPort.Text = entity.Port.ToString();
            radioAddress.SelectedValue = entity.Address;
            radioIsOpenCashbox.SelectedValue = entity.IsOpenCashbox;
            radioIsPrintslabels.SelectedValue = entity.IsPrintslabels;
            txbSerialNumber.Text = entity.SerialNumber;
            string[] list = entity.Deploy.Split(',');
            ckeDeploy.SelectedValueArray = list;
        }

        #endregion

        #region Events
        private void SaveItem()
        {
            tm_Printer entity = new tm_Printer();
            if (action == "edit")
            {
                entity = Core.Container.Instance.Resolve<IServicePrinter>().GetEntity(_id); ;
            }
            entity.PrinterName = labPrinterName.Text.Trim();
            entity.Width = ddlWidth.SelectedValue;
            entity.PrinterType = radioPrinterType.SelectedValue;
            entity.IsSinging = radioIsSinging.SelectedValue;
            entity.IP = txbIP.Text.Trim();
            entity.Port = Int32.Parse(numPort.Text);
            entity.Address = radioAddress.SelectedValue;
            entity.IsOpenCashbox = radioIsOpenCashbox.SelectedValue;
            entity.IsPrintslabels = radioIsPrintslabels.SelectedValue;
            entity.SerialNumber = txbSerialNumber.Text.Trim();
            entity.Deploy = String.Join(",", ckeDeploy.SelectedValueArray);
            if (action == "edit")
            {
                Core.Container.Instance.Resolve<IServicePrinter>().Update(entity);
            }
            else
            {
                Core.Container.Instance.Resolve<IServicePrinter>().Create(entity);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        { 
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        #endregion
    }
}