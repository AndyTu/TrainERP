using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainERP.Web.PageModels
{
    public class MenuLayoutModel : LayoutModel
    {
        //当前页面,供页面设置菜单选中项目
        public string SelectedMenu { get; set; }

        public string SelectedSubMenu { get; set; }
    }
}