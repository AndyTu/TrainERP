using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrainERP
{
    public class ERPUser 
    {
        public string UserName { get; set; }

        public List<string> Roles { get; set; }

        public string Organize { get; set; }

        public List<string> OperationRights { get; set; }

        public List<string> ModuleRights { get; set; }

        public string LatestUpdateBy { get; set; }

        public DateTime LatestUpdateTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }

        public bool IsDeleted { get; set; }

        public int Status { get; set; }




        public bool HasOperationRight(string operationCode)
        {
            return this.OperationRights.Contains(operationCode);
        }

        public bool HasModuleRight(string moduleCode)
        {
            return this.ModuleRights.Contains(moduleCode);
        }


    }
}
