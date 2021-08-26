using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        public static async Task<string> HtmlPost(string requestUrl, MultipartFormDataContent postParams)
        {
            var handler =new HttpClientHandler(){ UseCookies = true };
                using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36");
                client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                client.DefaultRequestHeaders.Add("Keep-Alive", "timeout=600");
                client.DefaultRequestHeaders.Add("Cookie", "ASP.NET_SessionId=m2fgsn32kwv3ffbtzm0aewwg");
                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                client.DefaultRequestHeaders.Add("Host", "www.sitca.org.tw");
                HttpContent postContent = postParams;
                HttpResponseMessage response = await client.PostAsync(requestUrl, postContent);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
        }
    }
}
