using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataTraning
{
    public class Global
    {
        private readonly static HttpClient HttpGetter = new HttpClient();

        public static string GetWebPage(string url)
        {
            HttpResponseMessage responseMessage = HttpGetter.GetAsync(url).Result;

            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                string responseResult = responseMessage.Content.ReadAsStringAsync().Result;
                return responseResult;
            }
            return null;
        }

        public static string ChangeYear(string year)
        {
            if (int.TryParse(year, out int result))
            {
                year = (result + 19110000).ToString();
            }
            return year;
        }
        public static string HtmlPost(string requestUrl, Dictionary<string, string> postParams, string encoded)
        {
            HttpContent postContent = ToContent(postParams);
            HttpResponseMessage response = HttpGetter.PostAsync(requestUrl, postContent).Result;
            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.CharSet = encoded;
            return response.Content.ReadAsStringAsync().Result;
        }

        public static MultipartFormDataContent ToContent(Dictionary<string, string> postParams)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            foreach (KeyValuePair<string, string> param in postParams)
            {
                content.Add(new StringContent(param.Value), param.Key);
            }
            return content;
        }

        public static DataTable ConvertEntity<T>(IEnumerable<T> entity)
        {
            DataTable table = CreatDataTable(typeof(T));
            if (entity.Count() == 0)
            {
                return table;
            }
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (T data in entity)
            {
                DataRow row = table.NewRow();
                foreach (PropertyInfo property in properties)
                {
                    row[property.Name] = property.GetValue(data, null);
                }
                table.Rows.Add(row);
            }
            return table;
        }

        private static DataTable CreatDataTable(Type type)
        {
            DataTable table = new DataTable();
            //取得此類的所有屬性
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                //取得此屬性類型
                Type columnType = property.PropertyType;
                //假如這個屬性為泛型且代表此泛型類型的類型可為null
                if (columnType.IsGenericType && columnType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    columnType = Nullable.GetUnderlyingType(columnType);
                }
                DataColumn column = new DataColumn(property.Name, columnType);
                table.Columns.Add(column);
            }
            return table;
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
