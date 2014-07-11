using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spore.Extensions
{
    public static class DateTimeExtensions
    {

        /// <summary>
        /// 返回指定的时间距 GMT 时间 1970/01/01的毫秒数。
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static double UTC(this DateTime datetime)
        {
            DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan span = datetime.ToUniversalTime() - startDate;
            return span.TotalMilliseconds;
        }


        /// <summary>
        /// 是否月末天
        /// </summary>
        /// <param name="datetime">某个日期</param>
        /// <returns></returns>
        public static bool IsMonthLastDay(this DateTime datetime)
        {
            DateTime tmp2 = Convert.ToDateTime(GetMonthLastDay(datetime));
            if (tmp2 == datetime)
            { return true; }
            else
            { return false; }
        }
        /// <summary>
        /// 是否同一天,不考虑时间,只到日期
        /// </summary>
        /// <param name="dt1">第一天</param>
        /// <param name="dt2">第二天</param>
        /// <returns></returns>
        public static bool IsSameDate(this DateTime dt1, DateTime dt2)
        {
            if (dt1.Year == dt2.Year && dt1.Month == dt2.Month && dt1.Day == dt2.Day)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 是否年末天
        /// </summary>
        /// <param name="datetime">某个日期</param>
        /// <returns></returns>
        public static bool IsYearLastDay(this DateTime datetime)
        {
            DateTime tmp2 = Convert.ToDateTime(GetYearLastDay(datetime));
            if (tmp2 == datetime)
            { return true; }
            else
            { return false; }
        }
        /// <summary>
        /// 某月第一天
        /// </summary>
        /// <param name="datetime">某个日期</param>
        /// <returns></returns>
        public static string GetMonthFirstDay(this DateTime datetime)
        {
            return datetime.Year.ToString() + "-" + datetime.Month.ToString() + "-1";
        }
        /// <summary>
        /// 某月最后一天
        /// </summary>
        /// <param name="datetime">某个日期</param>
        /// <returns></returns>
        public static string GetMonthLastDay(this DateTime datetime)
        {
            int month;
            int year;

            DateTime tmp;
            tmp = datetime;
            month = tmp.Month;
            year = tmp.Year;

            DateTime tmp2;

            if (month < 12)
            {
                tmp2 = Convert.ToDateTime(year.ToString() + "-" + Convert.ToString(month + 1) + "-1").AddDays(-1);
            }
            else
            {
                tmp2 = Convert.ToDateTime(year.ToString() + "-" + month.ToString() + "-31");
            }

            return tmp2.Year.ToString() + "-" + tmp2.Month.ToString() + "-" + tmp2.Day.ToString();
        }
        /// <summary>
        /// 上月最后一天
        /// </summary>
        /// <param name="datetime">某个日期</param>
        /// <returns></returns>
        public static string GetLastMonthLastDay(this DateTime datetime)
        {
            DateTime tmp2 = Convert.ToDateTime(GetMonthFirstDay(datetime)).AddDays(-1);
            return tmp2.Year.ToString() + "-" + tmp2.Month.ToString() + "-" + tmp2.Day.ToString();
        }
        /// <summary>
        /// 该年第一天
        /// </summary>
        /// <param name="datetime">某个日期</param>
        /// <returns></returns>
        public static string GetYearFirstDay(this DateTime datetime)
        {
            return Convert.ToString((datetime.Year)) + "-" + "1" + "-" + "1";
        }
        /// <summary>
        /// 该年最后一天
        /// </summary>
        /// <param name="datetime">某个日期</param>
        /// <returns></returns>
        public static string GetYearLastDay(this DateTime datetime)
        {
            return Convert.ToString((datetime.Year)) + "-" + "12" + "-" + "31";
        }
        /// <summary>
        /// 上一年最后一天
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string GetLastYearLastDay(this DateTime datetime)
        {
            return Convert.ToString((datetime.Year - 1)) + "-" + "12" + "-" + "31";
        }
    }
}
