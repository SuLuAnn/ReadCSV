using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Global
    {
        private readonly static HttpClient HttpGetter = new HttpClient();

        

        public static string HtmlPost(string requestUrl, MultipartFormDataContent content, string encoded)
        {
            HttpResponseMessage response = HttpGetter.PostAsync(requestUrl, content).Result;
            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.CharSet = encoded;
            return response.Content.ReadAsStringAsync().Result;
        }

        public static void SaveFile(string file, string fileName)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "BackupFile", fileName);
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.Write(file);
            }
        }

        public static string CreatDirectory(string fileName)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "BackupFile", fileName);
            if (!Directory.Exists(fileName))
            {
                Directory.CreateDirectory(path);
            }
            return fileName;
        }

        public static void SaveCsv<T>(List<T> processedData, string csvName)
        {
            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties().Where(property => property.Name != "CTIME" && property.Name != "MTIME");
            string headers = string.Join(",", properties.Select(property => property.Name));
            List<string> datas = processedData.Select(detail => string.Join(",", properties.Select(property => property.GetValue(detail))))
                                                             .ToList();
            datas.Insert(0, headers);
            SaveFile(string.Join(Environment.NewLine, datas), csvName);
        }
    }
}
