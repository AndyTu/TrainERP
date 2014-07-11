using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Spore
{
    public partial class Tools
    {
        /// <summary>
        /// 获取认证cookie
        /// </summary>
        /// <param name="username"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static HttpCookie GetWebAuthCookie(string username, string role, DateTime expireTime)
        {
            //建立身份验证票对象,设置cookie,并设置role为admin
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, username, DateTime.Now, expireTime, true, role, "/");
            //加密序列化验证票为字符串
            string hashTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie userCookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashTicket);
            //设置cookie过期时间
            userCookie.Expires = expireTime;
            return userCookie;
        }

    }
}
