using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Common
{
    /// <summary>
    /// 所有人共用的方法
    /// </summary>
    public class GlobalFunction
    {
        /// <summary>
        /// 爬蟲的get方法
        /// </summary>
        /// <param name="url">要爬的網址</param>
        /// <returns>get取得的網站html文字內容</returns>
        public static string GetWebPage(string url)
        {
            using (HttpClient HttpGetter = new HttpClient()) 
            {
                //直接取回字串
                return HttpGetter.GetStringAsync(url).Result;
            }
            //return responseMessage.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// 創建資料夾
        /// </summary>
        /// <param name="fileName">資料夾名稱</param>
        /// <returns>回傳資料夾名稱</returns>
        public static string CreatDirectory(string fileName)
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
        public static string HtmlPost(string requestUrl, MultipartFormDataContent content, string encoded)
        {
            using (HttpClient HttpGetter = new HttpClient()) 
            {
                HttpResponseMessage response = HttpGetter.PostAsync(requestUrl, content).Result;
                response.EnsureSuccessStatusCode();
                response.Content.Headers.ContentType.CharSet = encoded;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        /// <summary>
        /// 寫檔
        /// </summary>
        /// <param name="file">檔案內容</param>
        /// <param name="fileName">檔名</param>
        public static void SaveFile(string file, string fileName)
        {
            //組檔案路徑
            string path = Path.Combine(Environment.CurrentDirectory, GlobalConst.FOLDER_NAME, fileName);
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.Write(file);
            }
        }

        /// <summary>
        /// 讀存好的html檔
        /// </summary>
        /// <param name="fileName">檔名</param>
        /// <returns></returns>
        public static string ReadFile(string fileName)
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
        public static void SaveXml(XDocument document, string xmlName)
        {
            //組檔案路徑
            string path = Path.Combine(Environment.CurrentDirectory, GlobalConst.FOLDER_NAME, xmlName);
            using (XmlWriter writer = XmlWriter.Create(path))
            {
                document.Save(writer);
            }
        }

        /// <summary>
        /// 判斷傳入的字串是否為空，是則回傳null，否則回傳內容為該字串的XElement節點
        /// </summary>
        /// <param name="name">節點的標籤名</param>
        /// <param name="content">要判斷的字串</param>
        /// <returns>組好的節點</returns>
        public static XElement ChangeNull(string name, string content)
        {
            XElement result = null;
            content = content.Trim();
            if (!string.IsNullOrEmpty(content))
            {
                result = new XElement(name, content);
            }
            return result;
        }
    }
}
