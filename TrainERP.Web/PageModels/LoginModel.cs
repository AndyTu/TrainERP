using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainERP.Web.PageModels
{
    public class LoginModel:LayoutModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Message { get; set; }

        public bool IsRemember { get; set; }
    }
}