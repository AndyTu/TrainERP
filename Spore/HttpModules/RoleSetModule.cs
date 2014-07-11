using System;
using System.Web;

namespace Spore.HttpModules
{
    public class RoleSetModule : IHttpModule
    {
        /// <summary>
        /// 您将需要在您网站的 web.config 文件中配置此模块，
        /// 并向 IIS 注册此模块，然后才能使用。有关详细信息，
        /// 请参见下面的链接: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //此处放置清除代码。
        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);
        }

        #endregion

        //获取用户凭证,并设置用户roles
        void context_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext ctx = app.Context; //获取本次Http请求的HttpContext对象  
            if (ctx.User != null)
            {
                if (ctx.Request.IsAuthenticated == true) //验证过的一般用户才能进行角色验证  
                {
                    System.Web.Security.FormsIdentity fi = (System.Web.Security.FormsIdentity)ctx.User.Identity;
                    System.Web.Security.FormsAuthenticationTicket ticket = fi.Ticket; //取得身份验证票  
                    string userData = ticket.UserData;//从UserData中恢复role信息
                    string[] roles = userData.Split(','); //将角色数据转成字符串数组,得到相关的角色信息 
                    ctx.User = new System.Security.Principal.GenericPrincipal(fi, roles); //这样当前用户就拥有角色信息了
                }
            }

        }

    }
}
