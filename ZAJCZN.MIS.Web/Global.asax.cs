using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.IO;
using System.Data.Entity;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using ZAJCZN.MIS.Core;
using ZAJCZN.MIS.Domain;

namespace ZAJCZN.MIS.Web
{
    public class Global : System.Web.HttpApplication
    {
        private Container container;
        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
                if (!ActiveRecordStarter.IsInitialized)
                {
                    IConfigurationSource source = System.Configuration.ConfigurationManager.GetSection("activerecord") as IConfigurationSource;
                    ActiveRecordStarter.Initialize(typeof(ContractInfo).Assembly, source);
                    container = Container.Instance;
                }
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            
        }
        
        protected virtual void Application_EndRequest()
        {
            //var context = HttpContext.Current.Items["__WebContext"] as WebContext;
            //if (context != null)
            //{
            //    context.Dispose();
            //}
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}