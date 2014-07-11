using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Spore.CloudAPI.GrandCloud
{
    public class StorageObject
    {
        public StorageObject(string name, string filepath)
        {
            byte[] bytes = null;
            //读取文件并转换为字节数组
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                bytes = Tools.ConvertToBytes(fs);
            }
            this.initialize(name, bytes);
        }

        public StorageObject(string name, System.IO.Stream stream)
        {
            //将流转换为字节数组
            byte[] bytes = null;
            bytes = Tools.ConvertToBytes(stream);
            this.initialize(name, bytes);
        }
        
        public StorageObject(string name, byte[] bytes)
        {
            this.initialize(name, bytes);
        }

        private void initialize(string name, byte[] data)
        {
            this.MetaData = new Dictionary<string, string>();
            this.Data = data;
            this.Name = name;
        }

        //对象名
        public string Name { get; private set; }

        //内容
        public byte[] Data { get; private set; }

        //元数据
        public Dictionary<string, string> MetaData { get; private set; }

    }
}
