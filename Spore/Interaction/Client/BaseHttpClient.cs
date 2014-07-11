using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Spore.Interaction.Client;
using System.Web.Script.Serialization;
using Spore;

namespace Spore.Interaction.Client
{
    //客户端请求类
    public class BaseHttpClient
    {
        public BaseHttpClient(string hostaddress)
        {
            this.HostAddress = new Uri(hostaddress);
        }

        //主机地址
        public Uri HostAddress { get; private set; }
        //cookie对象
        private CookieContainer cookieContainer;

        //获取http请求对象
        protected HttpWebRequest getRequestObject(string relativePath, string method = "GET")
        {
            //检查this.alcedoHost是否以斜杠结尾或者relativePath是否以斜杠开头   todo
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.getFullAddress(relativePath));
            request.UserAgent = "SporeBaseHttpClient";
            request.Method = method;
            //设置cookie
            if (this.cookieContainer == null)
            {
                this.cookieContainer = new CookieContainer();
            }
            request.CookieContainer = this.cookieContainer;
            return request;
        }

        //保存cookie
        private void saveCookies(HttpWebResponse response)
        {
            this.cookieContainer.Add(response.Cookies);
        }

        //获取完整路径
        private string getFullAddress(string relativePath)
        {
            return string.Format("{0}/{1}", this.HostAddress.ToString(), relativePath);
        }

        //将对象转换为字典
        private Dictionary<string, string> objectToDictionary(object o)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var pi in o.GetType().GetProperties())
            {
                var value = pi.GetValue(o, null);
                if (value != null)
                {
                    dict.Add(pi.Name, pi.GetValue(o, null).ToString());
                }
            }
            return dict;
        }


        /// <summary>
        /// 通用的Post请求方法
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="data"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public Stream PostRequest(string relativePath, Dictionary<string, string> data, Dictionary<string, string> files)
        {
            if (data == null) data = new Dictionary<string, string>();
            if (files == null) files = new Dictionary<string, string>();

            //获取提交数组
            PostBytesCreator bytesCreator = new PostBytesCreator();
            List<byte[]> bytess = new List<byte[]>();
            foreach (var kvp in data)
            {
                bytess.Add(bytesCreator.CreateFieldData(kvp.Key, kvp.Value));
            }

            foreach (var kvp in files)
            {
                FileStream fs = new FileStream(kvp.Value, FileMode.Open, FileAccess.Read, FileShare.Read);
                //获取filestream
                string contentType = "application/octet-stream";
                byte[] fileBytes = new byte[fs.Length];
                fs.Read(fileBytes, 0, Convert.ToInt32(fs.Length));

                bytess.Add(bytesCreator.CreateFieldData(kvp.Key, kvp.Value, contentType, fileBytes));
            }

            byte[] finalBytes = bytesCreator.JoinBytes(bytess);

            //
            var request = this.getRequestObject(relativePath, "POST");

            request.ContentType = "multipart/form-data; boundary=" + bytesCreator.Boundary;
            request.ContentLength = finalBytes.Length;

            var requestStream = request.GetRequestStream();
            requestStream.Write(finalBytes, 0, finalBytes.Length);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            this.saveCookies(response);

            return response.GetResponseStream();

        }

        /// <summary>
        /// 通用的Get请求方法
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Stream GetRequest(string relativePath, Dictionary<string, string> data)
        {
            if (data == null) data = new Dictionary<string, string>();
            //生成querystring
            string querystring = "?";
            foreach (var kvp in data)
            {
                querystring += string.Format("&{0}={1}", kvp.Key, kvp.Value);
            }

            relativePath += querystring;

            var request = this.getRequestObject(relativePath, "GET");
            request.ContentLength = 0;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //保存cookie
            this.saveCookies(response);

            return response.GetResponseStream();
        }

        /// <summary>
        /// Alcedo请求方法
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="data"></param>
        /// <param name="files"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public string Request(string relativePath, Dictionary<string, string> data, Dictionary<string, string> files = null, string method = "GET")
        {
            //判断http方法
            method = method.Trim();
            if (method.ToLower() == "get")
            {
                return Tools.ConvertToString(GetRequest(relativePath, data));
            }
            else
            {
                return Tools.ConvertToString(PostRequest(relativePath, data, files));
            }
        }

        /// <summary>
        /// 泛型Alcedo请求方法
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="relativePath"></param>
        /// <param name="data"></param>
        /// <param name="files"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public JsonResultPackage<TData> Request<TData>(string relativePath, Dictionary<string, string> data, string method = "GET", Dictionary<string, string> files = null) where TData : class
        {
            string json = Request(relativePath, data, files, method);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

            return javaScriptSerializer.Deserialize<JsonResultPackage<TData>>(json);
        }

        /// <summary>
        /// 泛型Alcedo请求方法
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="relativePath"></param>
        /// <param name="data"></param>
        /// <param name="method"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public JsonResultPackage<TData> Request<TData>(string relativePath, Dictionary<string, object> data, string method = "GET", Dictionary<string, string> files = null) where TData : class
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            foreach (var kvp in data)
            {
                args.Add(kvp.Key, kvp.Value.ToString());
            }

            return Request<TData>(relativePath, args, method, files);
        }

        /// <summary>
        /// 泛型Alcedo请求方法  提交数据为对象
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="relativePath"></param>
        /// <param name="data"></param>
        /// <param name="files"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public JsonResultPackage<TData> Request<TData>(string relativePath, object data, string method = "GET", Dictionary<string, string> files = null) where TData : class
        {
            var dictdata = this.objectToDictionary(data);

            return Request<TData>(relativePath, dictdata, method, files);
        }

        public TData CommonRequest<TData>(string relativePath, Dictionary<string, string> data, string method = "GET", Dictionary<string, string> files = null) where TData : class
        {
            string json = Request(relativePath, data, files, method);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

            return javaScriptSerializer.Deserialize<TData>(json);
        }

        /// <summary>
        /// 将请求数据保存为文件  只支持Get
        /// </summary>
        /// <param name="savefilename"></param>
        /// <param name="relativePath"></param>
        /// <param name="data"></param>
        public void RequestFile(string savefilename, string relativePath, Dictionary<string, string> data)
        {
            var fileStream = GetRequest(relativePath, data);

            var bytes = Tools.ConvertToBytes(fileStream);

            Tools.SaveBytes(bytes, savefilename);

        }

    }
}
