using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainERP.Web
{
    public class EPRAuthorizeAttribute : AuthorizeAttribute
    {
        public EPRAuthorizeAttribute(string NeededRightCode)
        {

        }


        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return base.AuthorizeCore(httpContext);
            //获取ERPUser对象
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

    }
}