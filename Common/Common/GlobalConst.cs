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
        public const string DATE_FORMAT = "yyyyMMdd";
        public const string DATA = "data";
        public const string NAME_LINK = "nameLink";
        public const string CONVENER_LINK = "convenerLink";
        public const string CONVENER = "convener";
        public const string MEETING_DATE = "meetingDate";
        public const string VOTE_START_DAY = "voteStartDay";
        public const string VOTE_END_DAY = "voteEndDay";
        public const string AGENCY = "agency";
        public const string PHONE = "phone";
        public const string SLASH = @"/";
        public const string STOCK_CODE = "證券代號";
        public const string STOCK_NAME = "證券名稱";
        public const string CHINESS_CONVENER = "召集人";
        public const string CHINESS_MEETING_DATE = "股東會日期";
        public const string CHINESS_VOTE_START_DAY = "投票起日";
        public const string CHINESS_VOTE_END_DAY = "投票迄日";
        public const string CHINESS_AGENCY = "發行代理機構";
        public const string CHINESS_PHONE = "聯絡電話";
        public const string VOTE_DATE = "投票日期";
        public const string STOCK_VOTE_DATA = "yyyyMMdd/股東會投票資料表";
        public const string STOCK_VOTE_DETAIL = "yyyyMMdd/股東會投票日明細";
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
        /// <summary>
        /// 原始資料切割後的陣列與陣列內容的對應
        /// </summary>
        public const int TRANSACTION_DATE = 0;
        public const int CONTRACT = 1;
        public const int EXPIRY_MONTH = 2;
        public const int OPENING_PRICE = 3;
        public const int HIGHEST_PRICE = 4;
        public const int LOWEST_PRICE = 5;
        public const int CLOSING_PRICE = 6;
        public const int TRADING_HOURS = 17;
    }
}
