﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataTraning
{
    public class Global
    {
        public const string STOCK_VOTE = "https://www.stockvote.com.tw/evote/login/index/meetingInfoMore.html";
        public const string STOCK_VOTE_PAGE = "https://www.stockvote.com.tw/evote/login/index/meetingInfoMore.html?stockName=&orderType=0&stockId=&searchType=0&meetingDate=&meetinfo=";
        public const string FUND_NO_BUSINESS_DAY = "https://www.sitca.org.tw/ROC/Industry/IN2107.aspx?pid=IN2213_03";
        private readonly static HttpClient HttpGetter = new HttpClient(new HttpClientHandler() { UseCookies = true });

        public enum Fund : int
        {
            NO_BUSINESS_DAY,
            COMPANY_ID,
            TAX_ID,
            FUND_NAME
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
        public static  string HtmlPost(string requestUrl, Dictionary<string, string> postParams)
        {
            string responseBody = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent postContent = ToContent(postParams);
                HttpResponseMessage response =httpClient.PostAsync(requestUrl, postContent).Result;
                response.EnsureSuccessStatusCode();
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
    }
}
