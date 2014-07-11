using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spore.Interaction.Client
{
    public class JsonResultPackage<TData> where TData : class
    {
        public JsonResultPackage()
        {

        }

        public bool success { get; set; }

        public string message { get; set; }

        public int count { get; set; }

        public TData data { get; set; }
    }
}