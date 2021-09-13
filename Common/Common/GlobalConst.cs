using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class GlobalConst
    {
        public const string FOLDER_NAME = "BackupFile";
        public const string XML_NODE_NAME = "Data";
        public const string XML_ROOT = "Root";
        /// <summary>
        /// 中華民國證券投資信託暨顧問商業同業公會-基金非營業日的網址
        /// </summary>
        public const string FUND_NO_BUSINESS_DAY = "https://www.sitca.org.tw/ROC/Industry/IN2107.aspx?pid=IN2213_03";

        public const string YEAR = "year";

        public const string VALUE = "value";

        public const string ID = "id";

        public const string QUERY = "查詢";

        public const string POST_DATA_YEAR = "ctl00$ContentPlaceHolder1$ddlQ_Year";
        public const string POST_DATA_COMID = "ctl00$ContentPlaceHolder1$ddlQ_Comid";
        public const string POST_DATA_FUND = "ctl00$ContentPlaceHolder1$ddlQ_Fund";
        public const string POST_DATA_PAGESIZE = "ctl00$ContentPlaceHolder1$ddlQ_PAGESIZE";
        public const string POST_DATA_QUERY = "ctl00$ContentPlaceHolder1$BtnQuery";

        public const string ENCODED = "UTF-8";
        public const string DATE = "date";
        public const string COMPANY = "company";
        public const string TAX_ID = "taxID";
        public const string NO_BUSINESS = "非營業日";
        public const string COMPANY_CODE = "公司代號";
        public const string NAME = "name";
        public const string CHINESS_TAX_ID = "基金統編";
        public const string FUND_NAME = "基金名稱";
        public const string FUND_TOTAL = "基金總數";
        public const string SORT = "排序";
        public const string FUND_NO_BUSINESS_DETAIL = "基金非營業日明細.xml";
        public const string FUND_NO_BUSINESS_STATISTICS = "基金非營業日統計.xml";
        /// <summary>
        /// 日期貨盤後交易行情的網址
        /// </summary>
        public const string DAY_FUTURES = "https://www.taifex.com.tw/cht/3/dlFutDataDown";
        /// <summary>
        /// stockvote電子投票首頁
        /// </summary>
        public const string STOCK_VOTE = "https://www.stockvote.com.tw/evote/login/index/meetingInfoMore.html";

        /// <summary>
        /// stockvote電子投票取得每頁內容的網址
        /// </summary>
        public const string STOCK_VOTE_PAGE = "https://www.stockvote.com.tw/evote/login/index/meetingInfoMore.html?stockName=&orderType=0&stockId=&searchType=0&meetingDate=&meetinfo=";
    }
}
