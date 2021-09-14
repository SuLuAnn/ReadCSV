using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 專門放常數的類，這邊字串的常數多為在迴圈裡的字串，為了避免重複new出來，做成常數
    /// </summary>
    public class GlobalConst
    {
        /// <summary>
        /// 中華民國證券投資信託暨顧問商業同業公會-基金非營業日的網址
        /// </summary>
        public const string FUND_NO_BUSINESS_DAY = "https://www.sitca.org.tw/ROC/Industry/IN2107.aspx?pid=IN2213_03";

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
        /// 要存入的資料夾名稱
        /// </summary>
        public const string FOLDER_NAME = "BackupFile";

        /// <summary>
        /// xml中介資料中每一筆資料都會用Data包起來
        /// </summary>
        public const string XML_NODE_NAME = "Data";

        /// <summary>
        /// xml中介資料中所有資料會用root包起來
        /// </summary>
        public const string XML_ROOT = "Root";
        
        /// <summary>
        /// 正則的GROUP名
        /// </summary>
        public const string YEAR = "year";

        /// <summary>
        /// 正則的GROUP名
        /// </summary>
        public const string VALUE = "value";

        /// <summary>
        /// 正則的GROUP名
        /// </summary>
        public const string ID = "id";

        /// <summary>
        /// POST的參數key
        /// </summary>
        public const string QUERY = "查詢";

        /// <summary>
        /// POST的參數key
        /// </summary>
        public const string POST_DATA_YEAR = "ctl00$ContentPlaceHolder1$ddlQ_Year";

        /// <summary>
        /// POST的參數key
        /// </summary>
        public const string POST_DATA_COMID = "ctl00$ContentPlaceHolder1$ddlQ_Comid";

        /// <summary>
        /// POST的參數key
        /// </summary>
        public const string POST_DATA_FUND = "ctl00$ContentPlaceHolder1$ddlQ_Fund";

        /// <summary>
        /// POST的參數key
        /// </summary>
        public const string POST_DATA_PAGESIZE = "ctl00$ContentPlaceHolder1$ddlQ_PAGESIZE";

        /// <summary>
        /// POST的參數key
        /// </summary>
        public const string POST_DATA_QUERY = "ctl00$ContentPlaceHolder1$BtnQuery";

        /// <summary>
        /// post方法時解碼的方式
        /// </summary>
        public const string ENCODED = "UTF-8";

        /// <summary>
        /// 正則的GROUP名
        /// </summary>
        public const string DATE = "date";

        /// <summary>
        /// 正則的GROUP名
        /// </summary>
        public const string COMPANY = "company";

        /// <summary>
        /// 正則的GROUP名
        /// </summary>
        public const string TAX_ID = "taxID";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string NO_BUSINESS = "非營業日";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string COMPANY_CODE = "公司代號";

        /// <summary>
        /// 正則的GROUP名
        /// </summary>
        public const string NAME = "name";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string CHINESS_TAX_ID = "基金統編";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string FUND_NAME = "基金名稱";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string FUND_TOTAL = "基金總數";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string SORT = "排序";

        /// <summary>
        /// 要儲存的檔名
        /// </summary>
        public const string FUND_NO_BUSINESS_DETAIL = "基金非營業日明細.xml";

        /// <summary>
        /// 要儲存的檔名
        /// </summary>
        public const string FUND_NO_BUSINESS_STATISTICS = "基金非營業日統計.xml";

        /// <summary>
        /// 日期格式
        /// </summary>
        public const string DATE_FORMAT = "yyyyMMdd";

        /// <summary>
        /// 正則的GROUP名
        /// </summary>
        public const string DATA = "data";

        /// <summary>
        /// 正則的GROUP名
        /// </summary>
        public const string NAME_LINK = "nameLink";

        /// <summary>
        /// 正則的GROUP名
        /// </summary>
        public const string CONVENER_LINK = "convenerLink";

        /// <summary>
        /// 正則的GROUP名
        /// </summary>
        public const string CONVENER = "convener";

        /// <summary>
        /// 正則的GROUP名
        /// </summary>
        public const string MEETING_DATE = "meetingDate";

        /// <summary>
        /// 正則的GROUP名
        /// </summary>
        public const string VOTE_START_DAY = "voteStartDay";

        /// <summary>
        /// 正則的GROUP名
        /// </summary>
        public const string VOTE_END_DAY = "voteEndDay";

        /// <summary>
        /// 正則的GROUP名
        /// </summary>
        public const string AGENCY = "agency";

        /// <summary>
        /// 正則的GROUP名
        /// </summary>
        public const string PHONE = "phone";

        /// <summary>
        /// 日期要取代掉斜線
        /// </summary>
        public const string SLASH = "/";

        /// <summary>
        /// csv要用逗點切割
        /// </summary>
        public const char COMMA = ',';

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string STOCK_CODE = "證券代號";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string STOCK_NAME = "證券名稱";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string CHINESS_CONVENER = "召集人";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string CHINESS_MEETING_DATE = "股東會日期";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string CHINESS_VOTE_START_DAY = "投票起日";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string CHINESS_VOTE_END_DAY = "投票迄日";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string CHINESS_AGENCY = "發行代理機構";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string CHINESS_PHONE = "聯絡電話";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string VOTE_DATE = "投票日期";

        /// <summary>
        /// 要儲存的檔名
        /// </summary>
        public const string STOCK_VOTE_DATA = "yyyyMMdd/股東會投票資料表";

        /// <summary>
        /// 要儲存的檔名
        /// </summary>
        public const string STOCK_VOTE_DETAIL = "yyyyMMdd/股東會投票日明細";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string CHINESS_TRANSACTION_DATE = "交易日期";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string CHINESS_CONTRACT = "契約";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string CHINESS_EXPIRY_MONTH = "到期月份_週別";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string CHINESS_OPENING_PRICE = "開盤價";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string CHINESS_HIGHEST_PRICE = "最高價";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string CHINESS_LOWEST_PRICE = "最低價";

        /// <summary>
        /// 轉xml時的標籤名
        /// </summary>
        public const string CHINESS_CLOSING_PRICE = "收盤價";

        /// <summary>
        /// 當值等於盤後時才是需要的資料
        /// </summary>
        public const string CHINESS_TRADING_HOURS = "盤後";

        /// <summary>
        /// 原始資料切割後"交易日期"位置的對應
        /// </summary>
        public const int TRANSACTION_DATE = 0;

        /// <summary>
        /// 原始資料切割後"契約"位置的對應
        /// </summary>
        public const int CONTRACT = 1;

        /// <summary>
        /// 原始資料切割後"到期月份_週別"位置的對應
        /// </summary>
        public const int EXPIRY_MONTH = 2;

        /// <summary>
        /// 原始資料切割後"開盤價"位置的對應
        /// </summary>
        public const int OPENING_PRICE = 3;

        /// <summary>
        /// 原始資料切割後"最高價"位置的對應
        /// </summary>
        public const int HIGHEST_PRICE = 4;

        /// <summary>
        /// 原始資料切割後"最低價"位置的對應
        /// </summary>
        public const int LOWEST_PRICE = 5;

        /// <summary>
        /// 原始資料切割後"收盤價"位置的對應
        /// </summary>
        public const int CLOSING_PRICE = 6;

        /// <summary>
        /// 原始資料切割後"盤後"位置的對應
        /// </summary>
        public const int TRADING_HOURS = 17;
    }
}
