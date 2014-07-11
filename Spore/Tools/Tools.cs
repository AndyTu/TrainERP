using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Drawing;

namespace Spore
{
    public partial class Tools
    {
        internal Tools()
        {

        }

        /// <summary>
        /// 转换枚举
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TEnum ConvertToEnum<TEnum>(object value) where TEnum : struct
        {
            TEnum result;
            if (Enum.TryParse<TEnum>(value.ToString(), true, out result))
            {
                return result;
            }
            else
            {
                throw new Exception("转换失败");
            }
        }


        /// <summary>
        /// 获取枚举的description
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription<TEnum>(object value) where TEnum : struct
        {
            Type enumType = typeof(TEnum);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumItem requires a Enum ");
            }
            var name = Enum.GetName(enumType, Convert.ToInt32(value));
            if (name == null)
                return string.Empty;
            object[] objs = enumType.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs == null || objs.Length == 0)
            {
                //返回枚举成员名称
                return value.ToString();
            }
            else
            {
                DescriptionAttribute attr = objs[0] as DescriptionAttribute;
                return attr.Description;
            }
        }

        /// <summary>
        /// 获取枚举的description
        /// </summary>
        /// <param name="eum"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum eum)
        {
            Type enumType = eum.GetType();
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumItem requires a Enum ");
            }
            var field = enumType.GetField(eum.ToString());
            if (field == null)
            {
                //to do 可选择报出异常
                throw new Exception();
                return string.Empty;
            }
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs == null || objs.Length == 0)
            {
                //返回枚举成员名称
                return eum.ToString();
            }
            else
            {
                DescriptionAttribute attr = objs[0] as DescriptionAttribute;
                return attr.Description;
            }
        }


        #region 字符串相关

        public static bool IsNumerical(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            int result = 0;
            if (!int.TryParse(value, out result))
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// 判断对象是否为字符串，如果是字符串，则Trim
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object TrimIfString(object obj)
        {
            if (obj == null) return null;

            var o1 = obj as string;
            if (o1 == null) return obj;

            return o1.Trim();
        }


        #endregion


        /// <summary>
        /// Stream转换为Sring
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ConvertToString(Stream stream)
        {
            string result = "";
            using (stream)
            {
                StreamReader streamReader = new StreamReader(stream, System.Text.Encoding.UTF8);

                result = streamReader.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// string转Steam UTF-8
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Stream ConvertToStream(string str)
        {
            return new MemoryStream(Encoding.Default.GetBytes(str));
        }

        public static Stream ConvertToStream(byte[] bytes)
        {
            return new MemoryStream(bytes);
        }

        /// <summary>
        /// 流转换为字节数组
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ConvertToBytes(Stream stream)
        {
            if (stream.CanSeek)
            {
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);

                // 设置当前流的位置为流的开始
                stream.Seek(0, SeekOrigin.Begin);
                return bytes;
            }
            else
            {
                MemoryStream memStream = new MemoryStream();
                byte[] respBuffer = new byte[1024];
                try
                {
                    int bytesRead = stream.Read(respBuffer, 0, respBuffer.Length);
                    while (bytesRead > 0)
                    {
                        memStream.Write(respBuffer, 0, bytesRead);//写入memo
                        bytesRead = stream.Read(respBuffer, 0, respBuffer.Length);
                    }
                    return memStream.ToArray();
                }
                finally
                {
                    memStream.Close();
                }
            }
        }

        public static Guid ConvertToGuid(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Guid.Empty;
            }

            Guid rc;
            try
            {
                rc = new Guid(value);
            }
            catch
            {
                return Guid.Empty;
            }

            return rc;
        }

        /// <summary>
        /// 保存字节数组到文件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filename"></param>
        public static void SaveBytes(byte[] data, string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                BinaryWriter brnew = new BinaryWriter(fs);
                brnew.Write(data, 0, data.Length);
            }
        }

        /// <summary>
        /// 保存流到文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filename"></param>
        public static void SaveImageStream(MemoryStream stream, string filename)
        {
            Image image = Image.FromStream(stream);
            image.Save(filename);
        }

        /// <summary>
        /// 将文件读取到字节数组
        /// </summary>
        /// <param name="filename"></param>
        public static byte[] ReadFileToBytes(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, data.Length);
                    return data;
                }
            }
            else
            {
                return null;
            }
        }

    }
}
