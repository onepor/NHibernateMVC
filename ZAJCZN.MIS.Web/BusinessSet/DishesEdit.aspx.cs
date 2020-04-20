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

namespace ZAJCZN.MIS.Web
{
    public partial class DishesEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreDishesEdit";
            }
        }

        #endregion

        #region Page_Load

        private int InfoID
        {
            get { return GetQueryIntValue("id"); }
        }
        private int TypeID
        {
            get { return GetQueryIntValue("typeid"); }
        }
        private string PicFilePath
        {
            get { return ViewState["pic"] != null ? ViewState["pic"].ToString() : ""; }
            set { ViewState["pic"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
                CheckPowerWithButton("CoreDishesEdit", btnSaveClose);

                //绑定单位
                BindDDLUnit();
                //绑定打印机
                BindPrint();
                //绑定菜品大类
                BindDDL();
                //加载数据
                Bind();
            }
        }

        public void Bind()
        {
            if (InfoID > 0)
            {
                tm_Dishes objInfo = Core.Container.Instance.Resolve<IServiceDishes>().GetEntity(InfoID);

                if (objInfo == null)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                    return;
                }
                ddlPType.SelectedValue = objInfo.ClassID.ToString();
                tbxName.Text = objInfo.DishesName;
                lblCode.Text = objInfo.DishesCode.ToString();
                lblPY.Text = objInfo.DishesPY;
                ddlIsUsed.SelectedValue = objInfo.IsUsed.ToString();
                ddlUnit.SelectedValue = objInfo.DishesUnit.ToString();
                txtIndex.Text = objInfo.ShowIndex.ToString();
                txtPrice.Text = objInfo.SellPrice.ToString();
                txtMemberPrice.Text = objInfo.MemberPrice.ToString();
                rbtnWeigh.SelectedValue = objInfo.IsWeigh.ToString();
                rbtnRanking.SelectedValue = objInfo.IsRanking.ToString();
                rbtnDiscount.SelectedValue = objInfo.IsDiscount.ToString();
                rbntScore.SelectedValue = objInfo.IsScore.ToString();
                imgPhoto.ImageUrl = objInfo.DishesPicture;
                ddlPrint.SelectedValue = objInfo.PrinterID.ToString();
            }
            else
            {
                //获取当前最大编号
                IList<tm_Dishes> list = Core.Container.Instance.Resolve<IServiceDishes>().GetAll();
                if (list.Count > 0)
                {
                    tm_Dishes maxGoods = list.ToList().OrderByDescending(objs => objs.DishesCode).First();
                    lblCode.Text = (maxGoods.DishesCode + 1).ToString();
                }
                else
                {
                    lblCode.Text = "100";
                }
            }
        }

        private void BindDDLUnit()
        {
            List<Tm_Enum> list = GetSystemEnumByTypeKey("CPDW", true);
            ddlUnit.DataSource = list;
            ddlUnit.DataBind();
        }

        private void BindDDL()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("IsUsed", 2));

            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<tm_FoodClass> list = Core.Container.Instance.Resolve<IServiceFoodClass>().GetAllByKeys(qryList, orderList);

            ddlPType.DataSource = list;
            ddlPType.DataBind();
            ddlPType.SelectedIndex = 0;

            if (TypeID > 0)
            {
                ddlPType.SelectedValue = TypeID.ToString();
                tm_FoodClass objClass = Core.Container.Instance.Resolve<IServiceFoodClass>().GetEntity(TypeID);
                ddlUnit.SelectedValue = objClass != null ? objClass.Unit : "0";
                ddlPrint.SelectedValue = objClass != null ? objClass.PrintID.ToString() : "0";
            }
        }

        private void BindPrint()
        {
            IList<tm_Printer> list = Core.Container.Instance.Resolve<IServicePrinter>().GetAll();
            ddlPrint.DataSource = list;
            ddlPrint.DataBind();
            ddlPrint.Items.Insert(0, new FineUIPro.ListItem("", "0"));
        }

        #endregion

        #region Events
        private void SaveItem()
        {
            tm_Dishes objInfo = new tm_Dishes();
            if (InfoID > 0)
            {
                objInfo = Core.Container.Instance.Resolve<IServiceDishes>().GetEntity(InfoID);
            }
            objInfo.ClassID = int.Parse(ddlPType.SelectedValue);
            objInfo.DishesName = tbxName.Text;
            objInfo.DishesCode = int.Parse(lblCode.Text);
            objInfo.DishesPY = GetChinesePY(tbxName.Text.Trim());
            objInfo.IsUsed = int.Parse(ddlIsUsed.SelectedValue);
            objInfo.DishesUnit = int.Parse(ddlUnit.SelectedValue);
            objInfo.ShowIndex = int.Parse(txtIndex.Text);
            objInfo.SellPrice = !string.IsNullOrEmpty(txtPrice.Text) ? decimal.Parse(txtPrice.Text) : 0;
            objInfo.MemberPrice = !string.IsNullOrEmpty(txtMemberPrice.Text) ? decimal.Parse(txtMemberPrice.Text) : 0;
            objInfo.IsWeigh = int.Parse(rbtnWeigh.SelectedValue);
            objInfo.IsRanking = int.Parse(rbtnRanking.SelectedValue);
            objInfo.IsDiscount = int.Parse(rbtnDiscount.SelectedValue);
            objInfo.IsScore = int.Parse(rbntScore.SelectedValue);
            if (!string.IsNullOrEmpty(PicFilePath))
            {
                objInfo.DishesPicture = PicFilePath;
            }
            objInfo.PrinterID = int.Parse(ddlPrint.SelectedValue);

            if (InfoID > 0)
            {
                Core.Container.Instance.Resolve<IServiceDishes>().Update(objInfo);
            }
            else
            {
                if (string.IsNullOrEmpty(PicFilePath))
                {
                    objInfo.DishesPicture = Helpers.PicHelper.GetEmptyPicPath();
                }
                Core.Container.Instance.Resolve<IServiceDishes>().Create(objInfo);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name = "sender" ></ param >
        /// < param name="e"></param>
        protected void filePhoto_FileSelected(object sender, EventArgs e)
        {
            string fileShowPicPath = "";
            string saveShowPicPath = "";

            if (filePhoto.HasFile)
            {
                #region - 增加上传图片的字段 -

                string fileShowrNameName = filePhoto.ShortFileName;

                if (!Helpers.PicHelper.CheckFileIsCorrect(fileShowrNameName))
                {
                    Alert.Show("图片为无效的文件类型！");
                    return;
                }

                DateTime curTime = DateTime.Now;
                fileShowPicPath = Helpers.PicHelper.GetShowPicPath(fileShowrNameName.Substring(fileShowrNameName.LastIndexOf(".") + 1), curTime);
                saveShowPicPath = Helpers.PicHelper.GetRealSavePath(fileShowrNameName.Substring(fileShowrNameName.LastIndexOf(".") + 1), curTime);
                filePhoto.SaveAs(saveShowPicPath);
                PicFilePath = fileShowPicPath;
                #endregion
                imgPhoto.ImageUrl = fileShowPicPath;
                //清空文件上传组件（上传后要记着清空，否则点击提交表单时会再次上传！！）
                filePhoto.Reset();
            }
        }
        #endregion
    }
}