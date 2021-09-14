using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// <summary>
    /// 所有產表物件的抽象類，有所有物件的共通方法
    /// </summary>
    public abstract class DataBaseTable : IDataSheet
    {
        /// <summary>
        /// 做網站爬蟲的物件
        /// </summary>
        public HttpClient HttpGetter { get; set; }

        /// <summary>
        /// 存物件所對應的資料表名稱
        /// </summary>
        public string DataTableName { get; set; }

        /// <summary>
        /// 連線字串
        /// </summary>
        public SqlConnection SQLConnection { get; set; }



        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="dataTableName">此物件對應的資料表名稱</param>
        public DataBaseTable(string dataTableName)
        {
            DataTableName = dataTableName;
            HttpGetter = new HttpClient();
            SQLConnection = new SqlConnection();
            SQLConnection.ConnectionString = @"Data Source=192.168.10.180;Initial Catalog=StockDB;User ID=test;Password=test; Connection Timeout=180";
            SQLConnection.FireInfoMessageEventOnUserErrors = false;
            //創建BackupFile資料夾
            CreatDirectory(string.Empty);
        }

        /// <summary>
        /// 取得資料表名稱，因介面沒有屬性，所以要給方法來讓主程式取得物件的資料表名稱
        /// </summary>
        /// <returns>資料表名稱</returns>
        public string GetDataTableName()
        {
            return DataTableName;
        }

        /// <summary>
        /// 取得網站資料表原始資料的抽象方法
        /// </summary>
        public abstract void GetWebs();

        /// <summary>
        /// 爬蟲的get方法
        /// </summary>
        /// <param name="url">要爬的網址</param>
        /// <returns>get取得的網站html文字內容</returns>
        public string GetWebPage(string url)
        {
            HttpResponseMessage responseMessage = HttpGetter.GetAsync(url).Result;
            return responseMessage.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// 創建資料夾
        /// </summary>
        /// <param name="fileName">資料夾名稱</param>
        /// <returns>回傳資料夾名稱</returns>
        public string CreatDirectory(string fileName)
        {
            //組資料夾路徑，所有資料夾都會在BackupFile這個資料夾下面
            string path = Path.Combine(Environment.CurrentDirectory, GlobalConst.FOLDER_NAME, fileName);
            if (!Directory.Exists(fileName))
            {
                Directory.CreateDirectory(path);
            }
            return fileName;
        }

        /// <summary>
        /// 爬蟲的post方法
        /// </summary>
        /// <param name="requestUrl">要爬的網站網址</param>
        /// <param name="content">post所需的form data內容</param>
        /// <param name="encoded">編碼方式</param>
        /// <returns></returns>
        public string HtmlPost(string requestUrl, MultipartFormDataContent content, string encoded)
        {
            HttpResponseMessage response = HttpGetter.PostAsync(requestUrl, content).Result;
            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.CharSet = encoded;
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// 寫檔
        /// </summary>
        /// <param name="file">檔案內容</param>
        /// <param name="fileName">檔名</param>
        public void SaveFile(string file, string fileName)
        {
            //組檔案路徑
            string path = Path.Combine(Environment.CurrentDirectory, GlobalConst.FOLDER_NAME, fileName);
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.Write(file);
            }
        }

        public string ReadFile(string fileName)
        {
            //組檔案路徑
            string path = Path.Combine(Environment.CurrentDirectory, GlobalConst.FOLDER_NAME, fileName);
            using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 將xml檔存到本機
        /// </summary>
        /// <param name="document">xml檔</param>
        /// <param name="xmlName">檔名</param>
        public void SaveXml(XDocument document, string xmlName)
        {
            //組檔案路徑
            string path = Path.Combine(Environment.CurrentDirectory, GlobalConst.FOLDER_NAME, xmlName);
            using (XmlWriter writer = XmlWriter.Create(path))
            {
                document.Save(writer);
            }
        }

        /// <summary>
        /// 取得中介資料(Html產出的XML檔)
        /// </summary>
        public abstract void GetXML();

        /// <summary>
        /// 將中介資料寫入資料庫
        /// </summary>
        public abstract void WriteDatabase();

        public abstract XDocument GetTotalXml(string tableName);

        public XElement ChangeNull(string name,string content)
        {
            content = content.Trim();
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            return new XElement(name, content);
        }
    }
}
