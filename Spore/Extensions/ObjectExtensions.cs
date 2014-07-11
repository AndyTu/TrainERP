using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Spore.Extensions
{
    public static class ObjectExtensions
    {
        public static T ConvertTo<T>(this object value)
        {
            if (value == null) return default(T);

            return (T)ConvertTo(value, typeof(T));
        }

        public static object ConvertTo(this object value, Type t)
        {
            object val1;
            if (value != null)
            {
                if (t == value.GetType()) return value;

                if (t.IsGenericType)    // 仅用于处理int?之类的
                {
                    Type[] t1 = t.GetGenericArguments();

                    // TODO 这是临时的判断，用于解决当value是空字符串时，转换会失败
                    // 应该将转换做为一个函数。
                    if (value.ToString().Length > 0)
                    {
                        val1 = Convert.ChangeType(value, t1[0]);
                    }
                    else
                    {
                        val1 = null;
                    }
                }
                else
                {
                    if (t.BaseType == typeof(Enum))  // 仅用于转换int类型到enum
                    {
                        var st = value.ToString();
                        if (string.IsNullOrEmpty(st))
                        {
                            val1 = null;
                        }
                        else
                        {
                            if (Regex.IsMatch(st, @"^\d*$"))
                            {
                                val1 = Enum.ToObject(t, Int32.Parse(st));
                            }
                            else
                            {
                                val1 = Enum.Parse(t, st, true);
                            }
                        }
                    }
                    else if (!t.IsInstanceOfType(value))
                    {
                        // todo 只能处理系统默认的转换，要控制一下
                        // 增加string => Guid的转换
                        if (t == typeof(Guid) && value.GetType() == typeof(string))
                        {
                            val1 = Tools.ConvertToGuid(value as string);
                        }
                        else
                        {
                            // 如果是String类型，则需要处理空字字符串
                            val1 = value.Equals("") ? null : Convert.ChangeType(value, t);
                        }
                    }
                    else
                    {
                        val1 = value;
                    }
                }
            }
            else // value is null
            {
                val1 = null;
            }

            return val1;
        }
    }
}
