using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net;
using System.IO;
using System.Web;

namespace Spore.Interaction.Client
{
    /// <summary>
    /// 创建WebClient.UploadData方法所需二进制数组
    /// </summary>
    public class PostBytesCreator
    {
        Encoding encoding = Encoding.UTF8;

        /// <summary>
        /// 拼接所有的二进制数组为一个数组
        /// </summary>
        /// <param name="byteArrays">数组</param>
        /// <returns></returns>
        /// <remarks>加上结束边界</remarks>
        public byte[] JoinBytes(List<byte[]> byteArrays)
        {
            int length = 0;
            int readLength = 0;

            // 加上结束边界
            byte[] endBoundaryBytes = encoding.GetBytes(this.BoundaryEnd);

            //设置长度
            foreach (byte[] b in byteArrays)
            {
                length += b.Length;
            }

            byte[] bytes = new byte[length + endBoundaryBytes.Length];

            // 遍历复制
            foreach (byte[] b in byteArrays)
            {
                b.CopyTo(bytes, readLength);
                readLength += b.Length;
            }

            //复制结尾
            endBoundaryBytes.CopyTo(bytes, readLength);

            return bytes;
        }


        /// <summary>
        /// 获取普通表单区域二进制数组
        /// </summary>
        /// <param name="fieldName">表单名</param>
        /// <param name="fieldValue">表单值</param>
        /// <returns></returns>
        /// <remarks>
        /// -----------------------------7d52ee27210a3c\r\nContent-Disposition: form-data; name=\"表单名\"\r\n\r\n表单值\r\n
        /// </remarks>
        public byte[] CreateFieldData(string fieldName, string fieldValue)
        {
            string textTemplate = "\r\n{0}Content-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}";
            string text = String.Format(textTemplate, BoundaryBegin, fieldName, fieldValue);
            byte[] bytes = encoding.GetBytes(text);
            return bytes;
        }


        /// <summary>
        /// 获取文件上传表单区域二进制数组
        /// </summary>
        /// <param name="fieldName">表单名</param>
        /// <param name="filename">文件名</param>
        /// <param name="contentType">文件类型</param>
        /// <param name="contentLength">文件长度</param>
        /// <param name="stream">文件流</param>
        /// <returns>二进制数组</returns>
        public byte[] CreateFieldData(string fieldName, string filename, string contentType, byte[] fileBytes)
        {
            string textTemplate = "\r\n{0}Content-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n";

            // 头数据
            string data = String.Format(textTemplate, BoundaryBegin, fieldName, filename, contentType);
            byte[] bytes = encoding.GetBytes(data);


            // 合成后的数组
            byte[] fieldData = new byte[bytes.Length + fileBytes.Length];

            bytes.CopyTo(fieldData, 0); // 头数据
            fileBytes.CopyTo(fieldData, bytes.Length); // 文件的二进制数据

            return fieldData;
        }

        public string Boundary
        {
            get
            {
                return "---------------------------7d5b915500cee";
            }
        }

        public string BoundaryBegin
        {
            get
            {
                return string.Format("--{0}\r\n", Boundary);
            }
        }

        public string BoundaryEnd
        {
            get
            {
                return string.Format("\r\n--{0}--\r\n", Boundary);
            }
        }

        public string ContentType
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return string.Format("multipart/form-data; boundary={0}", Boundary);
                }
                return HttpContext.Current.Request.ContentType;
            }
        }
    }
}

