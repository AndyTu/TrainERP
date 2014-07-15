using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainERP.Web
{
    public class PageModelBuilder
    {

        public static T Build<T>() where T : PageModels.LayoutModel, new()
        {
            //设置 layoutmodel值, 登录信息
            var model = new T();
            return model;
        }


        public static T Build4Menu<T>() where T : PageModels.MenuLayoutModel, new()
        {
            var model = new T();
            return model;
        }

    }
}