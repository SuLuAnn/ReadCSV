using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Common
{
    public abstract class DataBaseTable : IDataSheet
    {
        public HttpClient HttpGetter { get; set; }
        public string DataTableName { get; set; }
        
        public DataBaseTable(string dataTableName)
        {
            DataTableName = dataTableName;
            HttpGetter = new HttpClient();
            CreatDirectory(string.Empty);
        }
        public string GetDataTableName()
        {
            return DataTableName;
        }

        public abstract void GetWebs();
        public string GetWebPage(string url)
        {
            HttpResponseMessage responseMessage = HttpGetter.GetAsync(url).Result;
            return responseMessage.Content.ReadAsStringAsync().Result;
        }

        public string CreatDirectory(string fileName)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "BackupFile", fileName);
            if (!Directory.Exists(fileName))
            {
                Directory.CreateDirectory(path);
            }
            return fileName;
        }

        public string HtmlPost(string requestUrl, MultipartFormDataContent content, string encoded)
        {
            HttpResponseMessage response = HttpGetter.PostAsync(requestUrl, content).Result;
            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.CharSet = encoded;
            return response.Content.ReadAsStringAsync().Result;
        }

        public void SaveFile(string file, string fileName)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "BackupFile", fileName);
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.Write(file);
            }
        }

        public void SaveXml(XDocument document, string xmlName)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "BackupFile", xmlName);
            using (XmlWriter writer = XmlWriter.Create(path))
            {
                document.Save(writer);
            }
        }

        public abstract void GetXML();
    }
}
