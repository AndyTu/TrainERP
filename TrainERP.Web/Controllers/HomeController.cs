using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainERP.Web.PageModels;

namespace TrainERP.Web.Controllers
{
    public class HomeController : Controller
    {

        //登录
        public ActionResult Login()
        {
            return View();
        }

        [ActionName("Login")]
        public ActionResult Login_post()
        {
            var model = PageModelBuilder.Build<LoginModel>();
            //
            var username = Request.Form["UserName"].FirstOrDefault();
            var password = Request.Form["Password"].FirstOrDefault();

            if (username != null && password != null)
            {
                //验证密码
            }
            else
            {
                //请输入用户名或密码
            }

            //登入成功转到我的主页
            RedirectToAction("My", "Home");
            return View();
        }


        //我的页面
        [EPRAuthorize("")]
        public ActionResult My()
        {
            return View();
        }

        //公告
        [EPRAuthorize("")]
        public ActionResult Notice()
        {
            return View();
        }



    }
}
