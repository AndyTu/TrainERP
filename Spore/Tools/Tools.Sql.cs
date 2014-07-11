using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spore
{
    /* //生成in sql字符串

        //汉字到拼音

        //加解密

        //序列化

        //反序列化

        //生成分页SQL*/
    public partial class Tools
    {
        /// <summary>
        /// MS SQLSERVER 分页SQL语句生成器，同样适用于ACCESS数据库(edit:2008.3.29) GenteratePagerSQL4MSSQL
        /// </summary>
        /// <param name="strSQLInfo">原始SQL语句</param>
        /// <param name="strWhere">在分页前要替换的字符串，用于分页前的筛选</param>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNumber">页码</param>
        /// <param name="AllCount">记录总数</param>
        /// <returns>生成SQL分页语句</returns>
        public static string GeneratePagerSQL4MSSQL(string strSQLInfo, string strWhere, int PageSize, int PageNumber, int AllCount)
        {
            #region 分页位置分析
            string strSQLType = string.Empty;
            if (AllCount != 0)
            {
                if (PageNumber == 1) //首页
                {
                    strSQLType = "First";
                }
                else if (PageSize * PageNumber > AllCount) //最后的页 @@LeftSize
                {
                    PageSize = AllCount - PageSize * (PageNumber - 1);
                    strSQLType = "Last";
                }
                else //中间页
                {
                    strSQLType = "Mid";
                }
            }
            else if (AllCount < 0) //特殊处理
            {
                strSQLType = "First";
            }
            else
            {
                strSQLType = "Count";
            }

            #endregion

            #region SQL 复杂度分析
            //SQL 复杂度分析 开始
            bool SqlFlag = true;//简单SQL标记
            string TestSQL = strSQLInfo.ToUpper();
            int n = TestSQL.IndexOf("SELECT ", 0);
            n = TestSQL.IndexOf("SELECT ", n + 7);
            if (n == -1)
            {
                //可能是简单的查询，再次处理
                n = TestSQL.IndexOf(" JOIN ", n + 7);
                if (n != -1) SqlFlag = false;
                else
                {
                    //判断From 谓词情况
                    n = TestSQL.IndexOf("FROM ", 9);
                    if (n == -1) return "";
                    //计算 WHERE 谓词的位置
                    int m = TestSQL.IndexOf("WHERE ", n + 5);
                    // 如果没有WHERE 谓词
                    if (m == -1) m = TestSQL.IndexOf("ORDER BY ", n + 5);
                    //如果没有ORDER BY 谓词，那么无法排序，退出；
                    if (m == -1)
                        throw new Exception("查询语句分析：当前没有为分页查询指定排序字段！请适当修改SQL语句。 " + strSQLInfo);
                    string strTableName = TestSQL.Substring(n, m - n);
                    //表名中有 , 号表示是多表查询
                    if (strTableName.IndexOf(",") != -1)
                        SqlFlag = false;
                }
            }
            else
            {
                //有子查询；
                SqlFlag = false;
            }
            //SQL 复杂度分析 结束
            #endregion

            #region 排序语法分析
            //排序语法分析 开始
            int iOrderAt = strSQLInfo.ToLower().LastIndexOf("order by ");
            //如果没有ORDER BY 谓词，那么无法排序分页，退出；
            if (iOrderAt == -1)
                throw new Exception("查询语句分析：当前没有为分页查询指定排序字段！请适当修改SQL语句。 " + strSQLInfo);

            string strOrder = strSQLInfo.Substring(iOrderAt + 9);
            strSQLInfo = strSQLInfo.Substring(0, iOrderAt);
            string[] strArrOrder = strOrder.Split(new char[] { ',' });
            for (int i = 0; i < strArrOrder.Length; i++)
            {
                string[] strArrTemp = (strArrOrder[i].Trim() + " ").Split(new char[] { ' ' });
                //压缩多余空格
                for (int j = 1; j < strArrTemp.Length; j++)
                {
                    if (strArrTemp[j].Trim() == "")
                    {
                        continue;
                    }
                    else
                    {
                        strArrTemp[1] = strArrTemp[j];
                        if (j > 1) strArrTemp[j] = "";
                        break;
                    }
                }
                //判断字段的排序类型
                switch (strArrTemp[1].Trim().ToUpper())
                {
                    case "DESC":
                        strArrTemp[1] = "ASC";
                        break;
                    case "ASC":
                        strArrTemp[1] = "DESC";
                        break;
                    default:
                        //未指定排序类型，默认为降序
                        strArrTemp[1] = "DESC";
                        break;
                }
                //消除排序字段对象限定符
                if (strArrTemp[0].IndexOf(".") != -1)
                    strArrTemp[0] = strArrTemp[0].Substring(strArrTemp[0].IndexOf(".") + 1);
                strArrOrder[i] = string.Join(" ", strArrTemp);

            }
            //生成反向排序语句
            string strNewOrder = string.Join(",", strArrOrder).Trim();
            strOrder = strNewOrder.Replace("ASC", "ASC0").Replace("DESC", "ASC").Replace("ASC0", "DESC");
            //排序语法分析结束
            #endregion

            #region 构造分页查询
            string SQL = string.Empty;
            if (!SqlFlag)
            {
                //复杂查询处理
                switch (strSQLType.ToUpper())
                {
                    case "FIRST":
                        SQL = "Select Top @@PageSize * FROM ( " + strSQLInfo +
                            " ) P_T0 @@Where ORDER BY " + strOrder;
                        break;
                    case "MID":
                        SQL = @"SELECT Top @@PageSize * FROM
                         (SELECT Top @@PageSize * FROM
                           (
                             SELECT Top @@Page_Size_Number * FROM (";
                        SQL += " " + strSQLInfo + " ) P_T0 @@Where ORDER BY " + strOrder + " ";
                        SQL += @") P_T1
            ORDER BY " + strNewOrder + ") P_T2  " +
                            "ORDER BY " + strOrder;
                        break;
                    case "LAST":
                        SQL = @"SELECT * FROM (     
                          Select Top @@LeftSize * FROM (" + " " + strSQLInfo + " ";
                        SQL += " ) P_T0 @@Where ORDER BY " + strNewOrder + " " +
                            " ) P_T1 ORDER BY " + strOrder;
                        break;
                    case "COUNT":
                        SQL = "Select COUNT(*) FROM ( " + strSQLInfo + " ) P_Count @@Where";
                        break;
                    default:
                        SQL = strSQLInfo + strOrder;//还原
                        break;
                }

            }
            else
            {
                //简单查询处理
                switch (strSQLType.ToUpper())
                {
                    case "FIRST":
                        SQL = strSQLInfo.ToUpper().Replace("SELECT ", "SELECT TOP @@PageSize ");
                        SQL += "  @@Where ORDER BY " + strOrder;
                        break;
                    case "MID":
                        string strRep = @"SELECT Top @@PageSize * FROM
                         (SELECT Top @@PageSize * FROM
                           (
                             SELECT Top @@Page_Size_Number  ";
                        SQL = strSQLInfo.ToUpper().Replace("SELECT ", strRep);
                        SQL += "  @@Where ORDER BY " + strOrder;
                        SQL += "  ) P_T0 ORDER BY " + strNewOrder + " " +
                            " ) P_T1 ORDER BY " + strOrder;
                        break;
                    case "LAST":
                        string strRep2 = @"SELECT * FROM (     
                          Select Top @@LeftSize ";
                        SQL = strSQLInfo.ToUpper().Replace("SELECT ", strRep2);
                        SQL += " @@Where ORDER BY " + strNewOrder + " " +
                            " ) P_T1 ORDER BY " + strOrder;
                        break;
                    case "COUNT":
                        SQL = "Select COUNT(*) FROM ( " + strSQLInfo + " @@Where) P_Count ";//edit
                        break;
                    default:
                        SQL = strSQLInfo + strOrder;//还原
                        break;
                }
            }

            //执行分页参数替换
            SQL = SQL.Replace("@@PageSize", PageSize.ToString())
                .Replace("@@Page_Size_Number", Convert.ToString(PageSize * PageNumber))
                .Replace("@@LeftSize", PageSize.ToString());//
            //.Replace ("@@Where",strWhere);
            //针对用户的额外条件处理：
            if (strWhere != "" && strWhere.ToUpper().Trim().StartsWith("WHERE "))
            {
                throw new Exception("分页额外查询条件不能带Where谓词！");
            }
            if (!SqlFlag)
            {
                if (strWhere != "") strWhere = " Where " + strWhere;
                SQL = SQL.Replace("@@Where", strWhere);
            }
            else
            {
                if (strWhere != "") strWhere = " And (" + strWhere + ")";
                SQL = SQL.Replace("@@Where", strWhere);
            }
            return SQL;
            #endregion
        }
    }
}
