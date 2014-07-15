using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Spore.DataAccess;
using System.Data;

namespace TrainERP.DataServices
{
    public class BaseDataService : IDisposable
    {

        //数据访问对象
        public BaseDataService()
        {
            //获取链接字符串
            var dbcon = System.Configuration.ConfigurationManager.ConnectionStrings["System.Data.SqlClient"];
            this.TrainDBConnection = new System.Data.SqlClient.SqlConnection(dbcon.ConnectionString);

        }

        //数据库链接对象
        public SqlConnection TrainDBConnection { get; private set; }


        public DataSet QueryDataSet(SqlCommand selectcmd)
        {
            DataSet ds = new DataSet();


            return ds;
        }

        public DataSet QueryDataSet(string selectcmd)
        {
            SqlCommand sqlcmd = new SqlCommand(selectcmd);
            return this.QueryDataSet(sqlcmd);
        }

        public void QueryDataSet(DataSet ds)
        {

        }

        public void QueryDataSet(DataSet ds, string selectcmd)
        {

        }

        //执行cmd



        //销毁数据库链接与对象
        public void Dispose()
        {
            try
            {
                this.TrainDBConnection.Close();
            }
            finally
            {
                this.TrainDBConnection.Dispose();
                this.Dispose();
            }
        }
    }
}
