using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace Spore.DataAccess
{
    //根据Database为Dataset生成 insert update delete命令
    public class DataSetCommandBuilder
    {
        private Database m_database;
        private string m_operatorchar = "";

        public DataSetCommandBuilder(Database database)
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

        //添加参数
        private void addParameter(DbCommand dbcommand, DataSet dataset, string tablename, bool isupdate)
        {

            foreach (DataColumn dc in dataset.Tables[tablename].Columns)
            {
                if (isupdate)
                {
                    if (!dc.AutoIncrement)
                    {
                        this.m_database.AddInParameter(dbcommand, dc.ColumnName,
                            DbTypeConvert.ToDbType(dc.DataType), dc.ColumnName, DataRowVersion.Current);
                    }
                }
                else
                {
                    this.m_database.AddInParameter(dbcommand, dc.ColumnName,
                           DbTypeConvert.ToDbType(dc.DataType), dc.ColumnName, DataRowVersion.Current);
                }
            }
        }

        //insert
        public DbCommand GetInsertCommand(DataSet dataset, string tablename)
        {
            DbCommand insertcommand = this.m_database.GetSqlStringCommand(this.getInsertSqlString(dataset, tablename));
            //添加参数
            this.addParameter(insertcommand, dataset, tablename, true);
            //
            return insertcommand;
        }

        ////update
        public DbCommand GetUpdateCommand(DataSet dataset, string tablename)
        {
            DbCommand updatecommand = this.m_database.GetSqlStringCommand(this.getUpdateSqlString(dataset, tablename));
            //添加参数
            this.addParameter(updatecommand, dataset, tablename, false);
            //
            return updatecommand;
        }

        ////delete
        public DbCommand GetDeleteCommand(DataSet dataset, string tablename)
        {
            DbCommand deletecommand = this.m_database.GetSqlStringCommand(this.getDeleteSqlString(dataset, tablename));
            //添加参数
            this.addParameter(deletecommand, dataset, tablename, false);
            //
            return deletecommand;
        }

        private string getInsertSqlString(DataSet dataset, string tablename)
        {
            StringBuilder field = new StringBuilder();
            StringBuilder value = new StringBuilder();
            field.Append("INSERT INTO " + tablename + "(");
            value.Append(" VALUES (");
            for (int i = 0; i < dataset.Tables[tablename].Columns.Count; i++)
            {
                if (!dataset.Tables[tablename].Columns[i].AutoIncrement)
                {
                    field.Append(dataset.Tables[tablename].Columns[i].ColumnName);
                    if (i < dataset.Tables[tablename].Columns.Count - 1)
                    {
                        field.Append(",");
                    }
                    else
                    {
                        field.Append(")");
                    }

                    value.Append(this.m_operatorchar + dataset.Tables[tablename].Columns[i].ColumnName);
                    if (i < dataset.Tables[tablename].Columns.Count - 1)
                    {
                        value.Append(",");
                    }
                    else
                    {
                        value.Append(")");
                    }
                }

                //if (AppendParameter != null)
                //{
                //    AppendParameter(DataSource.Tables[this.TableName].Columns[i], DataRowVersion.Current);
                //}
            }

            return field.ToString() + value.ToString();
        }

        private string getUpdateSqlString(DataSet dataset, string tablename)
        {
            //构建Update语句
            StringBuilder updateCommand = new StringBuilder();
            updateCommand.Append("UPDATE " + tablename + " SET ");
            for (int i = 0; i < dataset.Tables[tablename].Columns.Count; i++)
            {
                if (!dataset.Tables[tablename].Columns[i].AutoIncrement)
                {
                    updateCommand.Append(dataset.Tables[tablename].Columns[i].ColumnName + "=" + this.m_operatorchar + dataset.Tables[tablename].Columns[i].ColumnName);
                    if (i < dataset.Tables[tablename].Columns.Count - 1)
                    {
                        updateCommand.Append(",");
                    }
                }
            }
            //if (AppendParameter != null)
            //{
            //    AppendParameter(DataSource.Tables[TableName].Columns[i], DataRowVersion.Current);
            //}
            //}
            if (dataset.Tables[tablename].PrimaryKey.Length == 0)
            {
                throw new Exception("数据表结构中不包含主键");
            }

            updateCommand.Append(" WHERE ");
            for (int i = 0; i < dataset.Tables[tablename].PrimaryKey.Length; i++)
            {
                updateCommand.Append(dataset.Tables[tablename].PrimaryKey[i].ColumnName + "=" + this.m_operatorchar + dataset.Tables[tablename].PrimaryKey[i].ColumnName);
                if (i < dataset.Tables[tablename].PrimaryKey.Length - 1)
                {
                    updateCommand.Append(" AND ");
                }

                //if (AppendParameter != null)
                //{
                //    AppendParameter(DataSource.Tables[TableName].PrimaryKey[i], DataRowVersion.Original);
                //}
            }

            return updateCommand.ToString();
        }

        private string getDeleteSqlString(DataSet dataset, string tablename)
        {
            //构建Delete语句
            StringBuilder deleteCommand = new StringBuilder();
            deleteCommand.Append("DELETE FROM " + tablename + " WHERE ");
            for (int i = 0; i < dataset.Tables[tablename].PrimaryKey.Length; i++)
            {
                deleteCommand.Append(dataset.Tables[tablename].PrimaryKey[i].ColumnName + "=" + this.m_operatorchar + dataset.Tables[tablename].PrimaryKey[i].ColumnName);
                if (i < dataset.Tables[tablename].PrimaryKey.Length - 1)
                {
                    deleteCommand.Append(" AND ");
                }

                //if (AppendParameter != null)
                //{
                //    AppendParameter(DataSource.Tables[TableName].Columns[i], DataRowVersion.Current);
                //}
            }

            return deleteCommand.ToString();
        }
    }
}
