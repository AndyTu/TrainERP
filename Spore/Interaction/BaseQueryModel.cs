using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spore.Interaction
{
    //用于查询的基础model
    public class BaseQueryModel
    {

        //每页行数
        public int PageSize { get; set; }

        //页码
        public int Page { get; set; }

        //关键字
        public string Keyword { get; set; }

    }
}
