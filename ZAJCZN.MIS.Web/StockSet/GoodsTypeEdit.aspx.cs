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
    public partial class GoodsTypeEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreGoodsTypeEdit";
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
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
                CheckPowerWithButton("CoreGoodsTypeEdit", btnSaveClose);
              
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
                tm_GoodsType objInfo = Core.Container.Instance.Resolve<IServiceGoodsType>().GetEntity(InfoID);

                if (objInfo == null)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                    return;
                }

                ddlPType.SelectedValue = objInfo.ParentID.ToString();
                tbxName.Text = objInfo.TypeName;
                txtCode.Text = objInfo.Typecode;
                tbxRemark.Text = objInfo.Remark;
                ddlIsUsed.SelectedValue = objInfo.IsUsed.ToString();
                rbtnIsCalc.SelectedValue = objInfo.IsCalc.ToString();
            }
        }

        private void BindDDL()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ParentID", 0));

            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<tm_GoodsType> list = Core.Container.Instance.Resolve<IServiceGoodsType>().GetAllByKeys(qryList, orderList);

            List<tm_GoodsType> goodsList = list.ToList();
            goodsList.Insert(0, new tm_GoodsType { ID = 0, TypeName = "一级大类" });
            ddlPType.DataSource = goodsList;
            ddlPType.DataBind();
            ddlPType.SelectedIndex = 0;
            if (TypeID > 0)
            {
                ddlPType.SelectedValue = TypeID.ToString();
            }
        }

        #endregion

        #region Events
        private void SaveItem()
        {
            tm_GoodsType objInfo = new tm_GoodsType();
            if (InfoID > 0)
            {
                objInfo = Core.Container.Instance.Resolve<IServiceGoodsType>().GetEntity(InfoID);
            }
            objInfo.TypeName = tbxName.Text.Trim();
            objInfo.IsUsed = int.Parse(ddlIsUsed.SelectedValue);
            objInfo.IsCalc = int.Parse(rbtnIsCalc.SelectedValue); 
            objInfo.ParentID = int.Parse(ddlPType.SelectedValue);
            objInfo.Remark = tbxRemark.Text;
            objInfo.Typecode = txtCode.Text.Trim();

            if (InfoID > 0)
            {
                Core.Container.Instance.Resolve<IServiceGoodsType>().Update(objInfo);
            }
            else
            {
                Core.Container.Instance.Resolve<IServiceGoodsType>().Create(objInfo);
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