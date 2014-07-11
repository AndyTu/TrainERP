using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace Spore.DataAccess
{
    public class DbTypeConvert
    {
        /// <summary>
        /// 把基本数据类型映射为OleDbType数据类型
        /// </summary>
        /// <param name="baseType">基本类型参数</param>
        /// <returns>返回OleDbType类型</returns>
        public static OleDbType ToOleDbType(System.Type baseType)
        {
            System.Data.OleDb.OleDbType type = new OleDbType();
            switch (baseType.ToString())
            {
                case "System.Object":
                    type = OleDbType.Variant;
                    break;

                case "System.Boolean":
                    type = OleDbType.Boolean;
                    break;

                case "System.SByte":
                    type = OleDbType.TinyInt;
                    break;

                case "System.Int16":
                    type = OleDbType.SmallInt;
                    break;

                case "System.Int32":
                    type = OleDbType.Integer;
                    break;

                case "System.Int64":
                    type = OleDbType.BigInt;
                    break;

                case "System.UInt16":
                    type = OleDbType.UnsignedSmallInt;
                    break;

                case "System.UInt32":
                    type = OleDbType.UnsignedInt;
                    break;

                case "System.UInt64":
                    type = OleDbType.UnsignedBigInt;
                    break;

                case "System.Single":
                    type = OleDbType.Single;
                    break;

                case "System.Double":
                    type = OleDbType.Double;
                    break;

                case "System.Decimal":
                    type = OleDbType.Decimal;
                    break;

                case "System.String":
                    type = OleDbType.VarChar;
                    break;

                case "System.DateTime":
                    type = OleDbType.Date;
                    break;

                case "System.TimeSpan":
                    type = OleDbType.DBTime;
                    break;

                case "System.Exception":
                    type = OleDbType.Error;
                    break;

                case "System.Guid":
                    type = OleDbType.Guid;
                    break;

                case "System.Byte[]":
                    type = OleDbType.VarBinary;
                    break;

                default:
                    type = OleDbType.Variant;
                    break;
            }

            return type;
        }

        /// <summary>
        /// 把基本数据类型映射为OracleType数据类型
        /// </summary>
        /// <param name="baseType">基本类型参数</param>
        /// <returns>返回OracleType类型</returns>
        //public static OracleType ToOracleType(System.Type baseType)
        //{
        //    OracleType type = new OracleType();
        //    switch (baseType.ToString())
        //    {
        //        case "System.Object":
        //            type = OracleType.Clob;
        //            break;

        //        case "System.Boolean":
        //            type = OracleType.SByte;
        //            break;

        //        case "System.SByte":
        //            type = OracleType.SByte;
        //            break;

        //        case "System.Int16":
        //            type = OracleType.Int16;
        //            break;

        //        case "System.Int32":
        //            type = OracleType.Int32;
        //            break;

        //        case "System.Int64":
        //            type = OracleType.Number;
        //            break;

        //        case "System.UInt16":
        //            type = OracleType.UInt16;
        //            break;

        //        case "System.UInt32":
        //            type = OracleType.UInt32;
        //            break;

        //        case "System.UInt64":
        //            type = OracleType.Number;
        //            break;

        //        case "System.Single":
        //            type = OracleType.Float;
        //            break;

        //        case "System.Double":
        //            type = OracleType.Double;
        //            break;

        //        case "System.Decimal":
        //            type = OracleType.Number;
        //            break;

        //        case "System.String":
        //            type = OracleType.VarChar;
        //            break;

        //        case "System.DateTime":
        //            type = OracleType.DateTime;
        //            break;

        //        case "System.TimeSpan":
        //            type = OracleType.IntervalDayToSecond;
        //            break;

        //        case "System.Byte[]":
        //            type = OracleType.Blob;
        //            break;

        //        default:
        //            type = OracleType.Clob;
        //            break;
        //    }

        //    return type;
        //}

        //ToSqlDbType


        //ToODPType

        /// <summary>
        /// 把基本数据类型映射为DbType数据类型
        /// </summary>
        /// <param name="baseType">基本类型参数</param>
        /// <returns>返回DbType类型</returns>
        public static DbType ToDbType(System.Type baseType)
        {
            DbType type = new DbType();

            switch (baseType.ToString())
            {
                case "System.Object":
                    type = DbType.Object;
                    break;

                case "System.Boolean":
                    type = DbType.Boolean;
                    break;

                case "System.SByte":
                    type = DbType.SByte;
                    break;

                case "System.Int16":
                    type = DbType.Int16;
                    break;

                case "System.Int32":
                    type = DbType.Int32;
                    break;

                case "System.Int64":
                    type = DbType.Int64;
                    break;

                case "System.UInt16":
                    type = DbType.UInt16;
                    break;

                case "System.UInt32":
                    type = DbType.UInt32;
                    break;

                case "System.UInt64":
                    type = DbType.UInt64;
                    break;

                case "System.Single":
                    type = DbType.Single;
                    break;

                case "System.Double":
                    type = DbType.Double;
                    break;

                case "System.Decimal":
                    type = DbType.Decimal;
                    break;

                case "System.String":
                    type = DbType.String;
                    break;

                case "System.DateTime":
                    type = DbType.DateTime;
                    break;

                case "System.TimeSpan":
                    type = DbType.DateTimeOffset;
                    break;

                //case "System.Exception":
                //    type = DbType.String;
                //    break;

                case "System.Guid":
                    type = DbType.Guid;
                    break;

                case "System.Byte[]":
                    type = DbType.Binary;
                    break;

                default:
                    type = DbType.Object;
                    break;
            }

            return type;
        }
    }
}
