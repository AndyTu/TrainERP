using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Spore
{
    public partial class Tools
    {

        //常用正则表达式
        public static readonly string Regex_Email = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        public static readonly string Regex_InternetURL = @"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$";
        public static readonly string Regex_Chinese = "^[\u4e00-\u9fa5]{0,}$";


        //是否邮箱
        public static bool IsValidEmail(string value)
        {
             return Regex.IsMatch(value, Regex_Email);
        }

        //是否网址
        public static bool IsValidInternetURL(string value)
        {
            return Regex.IsMatch(value, Regex_InternetURL);
        }

        //是否数字
        public static bool IsValidNumber()
        {
            return true;
        }

        //是否汉字
        public static bool IsValidChinese(string value)
        {
            return Regex.IsMatch(value, Regex_Chinese);
        }

        //是否英文字母
        public static bool IsValidEnglishLetter()
         {
             return true;
         }

        //是否包含空白字符
        public static bool IsValidHasNullCharacter()
        {
            return true;
        }

    }
}
