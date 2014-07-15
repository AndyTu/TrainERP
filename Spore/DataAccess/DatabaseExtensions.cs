using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace Spore.DataAccess
{
    public static class DatabaseExtesnsions
    {
        public static DbCommand GetSelectCommand(this Database database, string selectcommandtext, Dictionary<string, string> paramdic)
        {
            DbCommand selectcommand = database.GetSqlStringCommand(selectcommandtext);
            //添加参数
            return null;
        }

        public static DataSet CommonPagerQuery(this Database database, int page, int pagesize, DbCommand selectcommand, out int totalrow)
        {
            totalrow = 1;
            return null;
        }

        public static TDataTable CommonRowQuery<TDataTable>(this Database database, string idfiled, DbType type, object value) where TDataTable : DataTable, new()
        {
            TDataTable dt = new TDataTable();
            SelectCommandBuilder scb = new SelectCommandBuilder(database);

            DbCommand selectcommand
                = scb.GetSelectCommand(dt.TableName, idfiled, DbTypeConvert.ToDbType(dt.Columns[idfiled].DataType), value);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            database.LoadDataSet(selectcommand, ds, dt.TableName);

            ds.Tables.Remove(dt);
            ds.Dispose();

            return dt;

        }

        public static void UpdateDataSet(this Database database, DataSet dataset)
        {
            DataSetCommandBuilder dsc = new DataSetCommandBuilder(database);

            foreach (DataTable dt in dataset.Tables)
            {
                database.UpdateDataSet(dataset, dt.TableName, dsc.GetInsertCommand(dataset, dt.TableName),
                    dsc.GetUpdateCommand(dataset, dt.TableName),
                    dsc.GetDeleteCommand(dataset, dt.TableName),
                      UpdateBehavior.Standard);
            }
        }

        public static void UpdateDataSet(this Database database, DataSet dataset, string[] tables)
        {
            DataSetCommandBuilder dsc = new DataSetCommandBuilder(database);

            foreach (string tablename in tables)
            {
                database.UpdateDataSet(dataset, tablename, dsc.GetInsertCommand(dataset, tablename),
                dsc.GetUpdateCommand(dataset, tablename),
                dsc.GetDeleteCommand(dataset, tablename),
                    UpdateBehavior.Standard);
            }
        }

        public static void UpdateDataTable(this Database database, DataTable datatable)
        {
            DataSetCommandBuilder dsc = new DataSetCommandBuilder(database);

            DataSet tempDataSet;
            if (datatable.DataSet == null)
            {
                tempDataSet = new DataSet();
                tempDataSet.Tables.Add(datatable);
            }
            else
            {
                tempDataSet = datatable.DataSet;
            }

            database.UpdateDataSet(tempDataSet, datatable.TableName, dsc.GetInsertCommand(tempDataSet, datatable.TableName),
                    dsc.GetUpdateCommand(tempDataSet, datatable.TableName),
                    dsc.GetDeleteCommand(tempDataSet, datatable.TableName),
                        UpdateBehavior.Standard);
            //从数据集中移除
            tempDataSet.Tables.Remove(datatable);
        }

        public static DataRow ExecuteDataRow(this Database database, DbCommand command)
        {
            var ds = database.ExecuteDataSet(command);
            if (ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
            {
                return ds.Tables[0].Rows[0];
            }
            else
            {
                return null;
            }
        }

    }
}
