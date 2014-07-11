using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;

namespace Spore.CloudAPI.GrandCloud
{
    /// <summary>
    /// 盛大云存储访问类
    /// </summary>
    public class StorageService
    {

        private string accessKey;

        private string secretAccessKey;

        private string hostUrl = "http://storage.grandcloud.cn";

        //传入AccessKey   SecretAccessKey  Bucket
        public StorageService(string akey, string sakey)
        {
            this.accessKey = akey;
            this.secretAccessKey = sakey;
        }


        #region 操作方法

        /// <summary>
        /// 获取对象信息
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="objkey"></param>
        /// <returns></returns>
        public StorageObject HeadObject(string bucket, string objname)
        {
            var resource = "";
            var datestr = "";
            HttpWebRequest hwr = this.getBasicHttpWebRequest(bucket, objname, out resource, out datestr);
            hwr.Method = "HEAD";

            return null;
        }

        /// <summary>
        /// 插入/更新对象
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="obj"></param>
        /// <param name="contenttype"></param>
        public void PutObject(string bucket, StorageObject obj, string contenttype = "")
        {
            var resource = "";
            var datestr = "";
            HttpWebRequest hwr = this.getBasicHttpWebRequest(bucket, obj.Name, out resource, out datestr);
            hwr.Method = "PUT";

            //设置头
            //Content-Type*
            if (!string.IsNullOrWhiteSpace(contenttype))
            {
                hwr.ContentType = contenttype;
            }
            //Authorization
            hwr.Headers.Add(HttpRequestHeader.Authorization, this.get_Authorization(hwr.Method, datestr, resource, "", hwr.ContentType));
            //Content-Length  写入数据时加入
            this.writeHttpWebRequest(hwr, obj.Data);
            try
            {
                using (var response = (HttpWebResponse)hwr.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.NoContent)//成功返回204
                    {
                        return;
                    }
                    else//错误
                    {
                        var bytes = Tools.ConvertToBytes(response.GetResponseStream());
                        throw new StorageException(bytes);
                    }
                }
            }
            catch (WebException wex)
            {
                throw new StorageException(Tools.ConvertToBytes(wex.Response.GetResponseStream()));
            }
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="objname"></param>
        public void DeleteObject(string bucket, string objname)
        {
            var resource = "";
            var datestr = "";
            HttpWebRequest hwr = this.getBasicHttpWebRequest(bucket, objname, out resource, out datestr);
            hwr.Method = "DELETE";


        }

        /// <summary>
        /// 获取对象,包括数据
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="objname"></param>
        /// <returns></returns>
        public StorageObject GetObject(string bucket, string objname)
        {
            var resource = "";
            var datestr = "";
            HttpWebRequest hwr = this.getBasicHttpWebRequest(bucket, objname, out resource, out datestr);
            hwr.Method = "GET";

            //设置header
            //Authorization
            hwr.Headers.Add(HttpRequestHeader.Authorization, this.get_Authorization(hwr.Method, datestr, resource));

            StorageObject so = null;
            try
            {
                using (var response = (HttpWebResponse)hwr.GetResponse())
                {
                    var bytes = Tools.ConvertToBytes(response.GetResponseStream());
                    if (response.StatusCode == HttpStatusCode.OK)//成功返回204
                    {
                        //获取内容,并生成 StorageObject 对象
                        so = new StorageObject(objname, bytes);
                    }
                    else//错误
                    {
                        //抛出云存储异常对象
                        throw new StorageException(bytes);
                    }
                }
            }
            catch (WebException wex)
            {
                throw new StorageException(Tools.ConvertToBytes(wex.Response.GetResponseStream()));
            }

            return so;
        }

        /// <summary>
        /// 获取基本的请求对象
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="objname"></param>
        /// <param name="resource"></param>
        /// <param name="datestring"></param>
        /// <returns></returns>
        private HttpWebRequest getBasicHttpWebRequest(string bucket, string objname, out string resource, out string datestring)
        {
            resource = string.Format("/{0}/{1}", bucket, objname);
            var date = DateTime.Now.ToUniversalTime();
            HttpWebRequest hwr
                = (HttpWebRequest)WebRequest.Create(this.hostUrl + resource);
            hwr.Date = date;

            datestring = date.ToString("r");

            return hwr;
        }

        #endregion


        #region 私有方法

        //获取ahthorization
        private string get_Authorization(string verb,
            string date, string resource, string sndaHeaders = "", string type = "", string md5 = "")
        {
            string stringToSign = string.Format("{0}\n{1}\n{2}\n{3}\n{4}{5}",
                verb, md5, type, date, sndaHeaders, resource);

            byte[] key = Encoding.UTF8.GetBytes(this.secretAccessKey);

            HMACSHA1 hmacsha1 = new HMACSHA1(key);

            var signaturebytes = hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(stringToSign));

            string signature = Convert.ToBase64String(signaturebytes);

            return string.Format("SNDA {0}:{1}", this.accessKey, signature);
        }


        //将字节写入httprequest stream
        private void writeHttpWebRequest(HttpWebRequest hwr, byte[] data)
        {
            //设置长度
            hwr.ContentLength = data.Length;
            using (Stream reqStream = hwr.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
            }
        }


        #endregion

    }
}
