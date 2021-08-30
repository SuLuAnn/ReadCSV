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
        public const string STOCK_VOTE = "https://www.stockvote.com.tw/evote/login/index/meetingInfoMore.html";
        public const string STOCK_VOTE_PAGE = "https://www.stockvote.com.tw/evote/login/index/meetingInfoMore.html?stockName=&orderType=0&stockId=&searchType=0&meetingDate=&meetinfo=";
        public const string FUND_NO_BUSINESS_DAY = "https://www.sitca.org.tw/ROC/Industry/IN2107.aspx?pid=IN2213_03";
        public const string DAY_FUTURES = "https://www.taifex.com.tw/cht/3/dlFutDataDown";
        private readonly static HttpClient HttpGetter = new HttpClient(new HttpClientHandler() { UseCookies = true });


        public enum Fund : int
        {
            NO_BUSINESS_DAY,
            COMPANY_ID,
            TAX_ID,
            FUND_NAME
        }

        public enum Futures : int
        {
            TRANSACTION_DATE = 0,
            CONTRACT = 1,
            EXPIRY_MONTH = 2,
            OPENING_PRICE = 3,
            HIGHEST_PRICE = 4,
            LOWEST_PRICE = 5,
            CLOSING_PRICE = 6,
            TRADING_HOURS = 17
        }

        public static string GetWebPage(string url)
        {
            var responseMessage = HttpGetter.GetAsync(url).Result;
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
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
                return (result + 19110000).ToString();
            }
            return year;
        }
        public static  string HtmlPost(string requestUrl, Dictionary<string, string> postParams, string encoded)
        {
            string responseBody = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent postContent = ToContent(postParams);
                HttpResponseMessage response =httpClient.PostAsync(requestUrl, postContent).Result;
                response.EnsureSuccessStatusCode();
                response.Content.Headers.ContentType.CharSet = encoded;
                responseBody =response.Content.ReadAsStringAsync().Result;
            }
            return responseBody;
        }
        public static string ToJson(Dictionary<string, string> postParams)
        {
            var paramString= postParams.Select(param => $"\"{param.Key}\":\"{param.Value}\"");
            string result = string.Join(",", paramString);
            result = "{" + result + "}";
            return result;
        }

        public static MultipartFormDataContent ToContent(Dictionary<string, string> postParams)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            foreach (var param in postParams)
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
            foreach (var data in entity)
            {
                DataRow row = table.NewRow();
                foreach (var property in properties)
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
                var column = new DataColumn(property.Name, columnType);
                table.Columns.Add(column);
            }
            return table;
        }

        public static void SaveFile(string file, string fileName)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "BackupFile", fileName);
            using (FileStream fileHandle = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                StreamWriter writer = new StreamWriter(fileHandle, Encoding.UTF8);
                writer.Write(file);
                writer.Close();
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

        public static void SaveCsv<T>(List<T> items, string csvName)
        {
            int noNeedHeader = 2;//csv表頭不需要CTIME,MTIME
            PropertyInfo[] propertiesOfObject = typeof(T).GetProperties();
            IEnumerable<PropertyInfo> properties = propertiesOfObject.Take(propertiesOfObject.Count() - noNeedHeader);
            string headers = string.Join(",", properties.Select(property => property.Name));
            List<string> datas = items.Select(detail => string.Join(",", properties
                                                                                                                 .Select(property => property.GetValue(detail))))
                                           .ToList();
            datas.Insert(0, headers);
            Global.SaveFile(string.Join("\n", datas),csvName);
        }
    }
}
