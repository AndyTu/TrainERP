using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainERP.Web.Controllers
{
    public class HomeController : Controller
    {

        //登录
        public ActionResult Login()
        {
            return View();
        }


        //我的页面
        public ActionResult My()
        {
            return View();
        }

        //公告
        public ActionResult Notice()
        {
            return View();
        }



    }
}
