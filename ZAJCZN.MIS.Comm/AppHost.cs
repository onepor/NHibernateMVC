using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ZAJCZN.MIS.Comm
{
   public static class AppHost
   {
       public static string GetHost
       {
           get { return HttpContext.Current.Request.Url.AbsoluteUri.Replace("SysUser/Login", ""); }
       }

   }
}
