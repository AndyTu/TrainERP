using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spore.CloudAPI.GrandCloud
{
    public class StorageException : Exception
    {
        public StorageException(byte[] responseBytes)
        {
            this.ResponseString = Encoding.UTF8.GetString(responseBytes);

            if (!string.IsNullOrWhiteSpace(this.ResponseString))
            {
                //获取xml对象
                var xmldoc = System.Xml.Linq.XDocument.Parse(this.ResponseString);

                var errorCode = xmldoc.Descendants("Code").SingleOrDefault();
                var errorMessage = xmldoc.Descendants("Message").SingleOrDefault();
                var requestId = xmldoc.Descendants("RequestId").SingleOrDefault();

                this.ErrorCode = errorCode.Value;
                this.ErrorMessage = errorMessage.Value;
                this.RequestId = requestId.Value;

            }
        }

        public string ResponseString { get; private set; }

        public string ErrorCode { get; private set; }

        public string ErrorMessage { get; private set; }

        public string RequestId { get; private set; }
    }
}
