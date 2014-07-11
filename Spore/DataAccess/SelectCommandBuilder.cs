using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace Spore.DataAccess
{
    //构建selectCommand  各种类型的
    public class SelectCommandBuilder
    {
        private Database m_database;
        private string m_operatorchar = "";

        public SelectCommandBuilder(Database database)
        {
            this.m_database = database;

            if (database.DbProviderFactory.ToString().Contains("OleDb"))
            {
                this.m_operatorchar = "@";
            }
            else if (database.DbProviderFactory.ToString().Contains("SqlClient"))
            {
                this.m_operatorchar = "@";
            }
            else if (database.DbProviderFactory.ToString().Contains("Oracle"))
            {
                this.m_operatorchar = ":";
            }
        }

        public DbCommand GetSelectCommand(string tablename, string field, DbType type, object value)
        {
            string selectcommandtext = string.Format("select * from {0} where {1}={2}", tablename, field, this.m_operatorchar + field);
            DbCommand selectcomand = this.m_database.GetSqlStringCommand(selectcommandtext);

            this.m_database.AddParameter(selectcomand, field, type, ParameterDirection.Input, field, DataRowVersion.Current, value);

            return selectcomand;
        }

        //
    }
}
