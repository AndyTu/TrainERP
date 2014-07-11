using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spore.Interaction.Server
{
    public class JsonResultPackage
    {
        public JsonResultPackage(bool isSuccess = true, string msg = "", object data = null)
        {
            this.success = isSuccess;
            this.message = msg;
            this.data = data;
        }

        public bool success { get; private set; }

        public string message { get; private set; }

        /// <summary>
        /// 查询数据总数(仅当返回列表数据时有用
        /// </summary>
        public int count { get; set; }

        public object data { get; set; }

        public JsonResultPackage Error(string msg)
        {
            this.success = false;
            this.message = msg;
            return this;
        }

        public JsonResultPackage Success(string msg)
        {
            this.success = true;
            this.message = msg;
            return this;
        }
    }
}