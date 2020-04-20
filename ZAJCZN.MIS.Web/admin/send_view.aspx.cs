using FineUIPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Web.admin
{
    public partial class send_view : System.Web.UI.Page
    {

        private string RetId
        {
            get;
            set;
        }

        private string RetName
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //获取传参
            RetId = Request.QueryString["ID"].ToString();
            RetName = Request.QueryString["Name"].ToString();
            labName.Text = RetName;

            //关闭弹窗
            btnClose.OnClientClick = ActiveWindow.GetHideReference();
        }


        /// <summary>
        /// 发送并关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            string inputUserName = labName.Text.Trim();
            if (string.IsNullOrEmpty(RetId) || Convert.ToInt32(RetId)<=0)
            {
                Alert.Show("发送对象已失效，请重新选择！");
                return;
            }

            if (!string.IsNullOrEmpty(tbxContet.Text)&&!string.IsNullOrEmpty(TextTitle.Text))
            {
                SaveSend();
            }
            else
            {
                Alert.Show("请输入发送标题及内容！");
            }

        }

        /// <summary>
        /// 执行发送
        /// </summary>
        private void SaveSend()
        {
            try
            {
                //生成发送model
                MemberSiteMessage entity = new MemberSiteMessage()
                {
                    SendType = 1,
                    FirstID = 0,
                    IsRead = 0,
                    MesDate = DateTime.Now,
                    Message = tbxContet.Text,
                    MesTitle = TextTitle.Text,
                    UserID = Convert.ToInt32(RetId)
                };
                //添加发送记录
                Core.Container.Instance.Resolve<IServiceMemberSiteMessage>().Create(entity);
            }
            catch (Exception ex)
            {
                //throw ex;
            }

            //关闭弹窗
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

    }
}