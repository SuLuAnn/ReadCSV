using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LinqUserInterface.Global;
using LinqUserInterface.Dto;
using System.Diagnostics;
using System.Windows.Forms;

namespace LinqUserInterface
{
    /// <summary>
    /// 第四題查詢於總統大選特定天數過後，類股報酬率以及股票成交量等相關資料
    /// </summary>
    public class PresidentialPerformance
    {
        /// <summary>
        /// 資料庫物件
        /// </summary>
        private StockDBEntities StockDB;

        /// <summary>
        /// 建構子，傳入資料庫物件
        /// </summary>
        /// <param name="stockDB">資料庫物件</param>
        public PresidentialPerformance(StockDBEntities stockDB)
        {
            StockDB = stockDB;
        }

        /// <summary>
        /// 查總統大選後到指定天數交易日的範圍
        /// </summary>
        /// <param name="lastDayPosition">指定天數</param>
        /// <returns>範圍內的交易日</returns>
        private List<string> GetPresidentialDay(int lastDayPosition)
        {
            List<string> days = PRESIDENTIAL_DATE.Select(targetDay => StockDB.日收盤.Where(day => day.股票代號 == "TWA00" //以TWA00有交易的日期為交易日
                                                                                                                                                                                          && string.Compare(day.日期, targetDay) > 0)//取大於總統大選的日期
                                                                                                                                                            .OrderBy(day => day.日期)//排序
                                                                                                                                                            .Select(day => day.日期)
                                                                                                                                                            .Take(lastDayPosition))//取指定範圍的交易日
                                                                                         .SelectMany(day => day).ToList(); //將每個指定日期結果彙總
            return days;
        }

        /// <summary>
        /// 查詢總統大選後第一個交易日以及第30個交易日類股收盤價以及漲跌
        /// </summary>
        /// <param name="lastDayPosition">選後幾個交易日</param>
        /// <returns>類股收盤價以及漲跌</returns>
        public List<RiseStockDto> GetRiseStocks(int lastDayPosition)
        {
            List<string> dayInterval = GetPresidentialDay(lastDayPosition); //取得總統大選後的指定交易日
            IQueryable<KindStock> kindStocks = GetKindStocks(); //取得類股資料
            return dayInterval.GroupBy(day => day.Substring(0, YEAR_WORDS))
                                             .Select(stock => new
                                             {
                                                 年度 = stock.Key,
                                                 FirstDay = stock.First(),
                                                 LastDay = stock.Last()
                                             })//依年度分類指定日期並找出該年度的起始與結束
                                             .SelectMany(target => StockDB.日收盤.Where(day => day.日期 == target.FirstDay
                                                                                                                                                   || day.日期 == target.LastDay)//找出在起始與結束日的股票
                                                                                                                      .Join(kindStocks, day => day.股票代號,
                                                                                                                                                      code => code.代號,
                                                                                                                                                      (day, code) => new
                                                                                                                                                      {
                                                                                                                                                          target.年度,
                                                                                                                                                          day.日期,
                                                                                                                                                          code.代號,
                                                                                                                                                          code.名稱,
                                                                                                                                                          day.收盤價
                                                                                                                                                      })//與類股join，取得在指定期間的類股
                                                                                                                    .GroupBy(day => day.代號, (id, days) => new //依股票代號分群，將起始與結束取為同一個物件
                                                                                                                    {
                                                                                                                        年度 = days.FirstOrDefault().年度,
                                                                                                                        類股股票代號 = days.FirstOrDefault().代號,
                                                                                                                        類股股票名稱 = days.FirstOrDefault().名稱,
                                                                                                                        第30個交易日日期 = days.FirstOrDefault(day => day.日期 == target.LastDay).日期,
                                                                                                                        第30個交易日收盤價 = days.FirstOrDefault(day => day.日期 == target.LastDay).收盤價,
                                                                                                                        第1個交易日日期 = days.FirstOrDefault().日期,
                                                                                                                        第1個交易日收盤價 = days.FirstOrDefault().收盤價,
                                                                                                                    })
                                                                                                                    .Select(stock => new RiseStockDto
                                                                                                                    {
                                                                                                                        年度 = stock.年度,
                                                                                                                        類股股票代號 = stock.類股股票代號,
                                                                                                                        類股股票名稱 = stock.類股股票名稱,
                                                                                                                        第30個交易日日期 = stock.第30個交易日日期,
                                                                                                                        第30個交易日收盤價 = stock.第30個交易日收盤價,
                                                                                                                        第1個交易日日期 = stock.第1個交易日日期,
                                                                                                                        第1個交易日收盤價 = stock.第1個交易日收盤價,
                                                                                                                        股價漲跌 = stock.第30個交易日收盤價 - stock.第1個交易日收盤價//計算股價漲跌
                                                                                                                    })
                                                                                                                    .Where(day => day.股價漲跌 > 0))
                                                                                                                    .OrderBy(day => day.年度).ThenBy(day => day.類股股票代號).ToList();
        }

        /// <summary>
        /// 查所有類股的資料
        /// </summary>
        /// <returns>類股的資料</returns>
        private IQueryable<KindStock> GetKindStocks()
        {
            return StockDB.代號表指數.Where(code => (code.代號.StartsWith("TWB") || code.代號.StartsWith("TWC"))
                                                                                              && code.代號 != "TWC00"
                                                                                              && code.C200707整併後交易所產業編號 != null)
                                                             .Select(code => new KindStock
                                                             {
                                                                 代號 = code.代號,
                                                                 名稱 = code.名稱,
                                                                 C200707整併後交易所產業編號 = code.C200707整併後交易所產業編號
                                                             });
        }

        /// <summary>
        /// 取得指定期間的類股報酬率
        /// </summary>
        /// <param name="statisticsDay">起始日後幾個交易日</param>
        /// <returns>類股報酬率資訊</returns>
        private List<List<KindRateDto>> GetRate(int statisticsDay)
        {
            IQueryable<KindStock> kindStocks = GetKindStocks(); //取得類股資料
            List<string> dayInterval = GetPresidentialDay(statisticsDay); //取得大選後日期
            decimal rateOfReturn = 100;
            return dayInterval.GroupBy(day => day.Substring(0, YEAR_WORDS))
                                              .Select(stock => new
                                              {
                                                  年度 = stock.Key,
                                                  FirstDay = stock.First(),
                                                  LastDay = stock.Last()
                                              })//依年度分類指定日期並找出該年度的起始與結束
                                               .Select(target => StockDB.日收盤.Where(day => day.日期 == target.FirstDay
                                                                                                                                          || day.日期 == target.LastDay)
                                                                                                            .Join(kindStocks, day => day.股票代號, //取得符合起始與結束日的類股
                                                                                                                                            code => code.代號,
                                                                                                                                            (day, code) => new
                                                                                                                                            {
                                                                                                                                                day.日期,
                                                                                                                                                code.代號,
                                                                                                                                                code.名稱,
                                                                                                                                                code.C200707整併後交易所產業編號,
                                                                                                                                                day.收盤價,
                                                                                                                                                day.上市櫃,
                                                                                                                                            })
                                                                                                            .AsEnumerable()//報酬率計算需在程式算才會和範例相同
                                                                                                            .GroupBy(day => day.代號, (id, days) => new
                                                                                                            {//依代號分群找出第一日語最後一日
                                                                                                                年度 = target.年度,
                                                                                                                類股股票代號 = days.First().代號,
                                                                                                                類股股票名稱 = days.First().名稱,
                                                                                                                days.First().C200707整併後交易所產業編號,
                                                                                                                days.First().上市櫃,
                                                                                                                第30個交易日收盤價 = days.First(day => day.日期 == target.LastDay).收盤價,
                                                                                                                第1個交易日收盤價 = days.First(day => day.日期 == target.FirstDay).收盤價
                                                                                                            })
                                                                                                            .Select(day => new KindRateDto
                                                                                                            {
                                                                                                                年度 = day.年度,
                                                                                                                類股股票代號 = day.類股股票代號,
                                                                                                                類股股票名稱 = day.類股股票名稱,
                                                                                                                報酬率 = (day.第30個交易日收盤價 - day.第1個交易日收盤價) / day.第1個交易日收盤價 * rateOfReturn,
                                                                                                                上市櫃 = day.上市櫃,
                                                                                                                C200707整併後交易所產業編號 = day.C200707整併後交易所產業編號
                                                                                                                //計算報酬率
                                                                                                            })
                                                                                                            .OrderByDescending(day => day.報酬率).ToList()).ToList();
        }

        /// <summary>
        /// 取得兩年中報酬率排名相同的類股
        /// </summary>
        /// <returns>類股資料</returns>
        public List<RateReturn> GetSameStock()
        {
            return GetRate(TRADING_DAY_30TH).Select(stocks => stocks.Take(20)//取20筆
                                                                                                                               .Zip(Enumerable.Range(1, 20), (stock, index) => new RateReturn
                                                                                                                                   {
                                                                                                                                       年度 = stock.年度,
                                                                                                                                       類股股票代號 = stock.類股股票代號,
                                                                                                                                       類股股票名稱 = stock.類股股票名稱,
                                                                                                                                       報酬率 = stock.報酬率,
                                                                                                                                       報酬率排名 = index, //index初始會是0，但我要從1開始
                                                                                                                                   }))
                                                                                .SelectMany(day => day)//將兩年資料變成一個list
                                                                                .GroupBy(day => new { day.類股股票代號, day.報酬率排名 } )//當股票代號相同
                                                                                .Where(day => day.Count() > 1)
                                                                                .SelectMany(day => day)
                                                                                .OrderBy(day => day.年度)
                                                                                .ThenBy(day => day.報酬率)
                                                                                .ToList();
        }

        /// <summary>
        /// 為第三題找相同報酬率排名的類股
        /// </summary>
        /// <returns>相同報酬率排名的類股</returns>
        public IEnumerable<RiseFallDto> GetSameForTop5()
        {
            return GetRate(TRADING_DAY_30TH).Select(stocks => stocks.Take(20)
                                                                                                                                .Select((stock, index) => new
                                                                                                                                {
                                                                                                                                    年度 = stock.年度,
                                                                                                                                    類股股票代號 = stock.類股股票代號,
                                                                                                                                    類股股票名稱 = stock.類股股票名稱,
                                                                                                                                    報酬率 = stock.報酬率,
                                                                                                                                    報酬率排名 = index + 1, //index初始會是0，但我要從1開始
                                                                                                                                    stock.C200707整併後交易所產業編號,
                                                                                                                                    stock.上市櫃
                                                                                                                                }))
                                                                                .SelectMany(day => day)//將兩年資料變成一個list
                                                                                .GroupBy(day => new { day.類股股票代號, day.報酬率排名 })//當股票代號相同
                                                                                .Where(day => day.Count() > 1)
                                                                                .SelectMany(day => day)
                                                                                .Select(day => new
                                                                                {
                                                                                    day.年度,
                                                                                    day.類股股票代號,
                                                                                    day.類股股票名稱,
                                                                                    day.上市櫃,
                                                                                    產業編號 = day.C200707整併後交易所產業編號.Replace("\'", string.Empty).Split(',')
                                                                                }).SelectMany(day => day.產業編號, (day, codes) => new RiseFallDto
                                                                                {
                                                                                    年度 = day.年度,
                                                                                    上市櫃 = day.上市櫃,
                                                                                    股票代號 = day.類股股票代號,
                                                                                    股票名稱 = day.類股股票名稱,
                                                                                    產業代號 = codes
                                                                                });
        }

        /// <summary>
        /// 查詢總統大選報酬率排名相同的類股中的成分個股資訊，找出時間區間內，各類股中平均成交量前五大的個股
        /// </summary>
        /// <param name="lastDayPosition">總統大選選後幾天</param>
        /// <returns>類股中平均成交量前五大的個股</returns>
        public List<Top5VolumeDto> GetTop5Volume(int lastDayPosition)
        {
            return PRESIDENTIAL_DATE.Select(targetDay => StockDB.日收盤.Where(day => day.股票代號 == "TWA00"
                                                                                                                                                                    && string.Compare(day.日期, targetDay) > 0)//加一年後時間為500多毫秒
                                                                                                                                       .Select(day => day.日期)
                                                                                                                                       .OrderBy(day => day)//排序
                                                                                                                                       .Take(lastDayPosition)//日收盤加where 700多毫秒
                                                                                                                                       .Join(StockDB.日收盤, day => day, stock => stock.日期, (day, stock) => new
                                                                                                                                       {
                                                                                                                                           stock.股票代號,
                                                                                                                                           stock.產業代號,
                                                                                                                                           stock.上市櫃,
                                                                                                                                           stock.股票名稱,
                                                                                                                                           stock.成交量
                                                                                                                                       })
                                                                                                                                       .Where(stock => stock.產業代號 != null)
                                                                                                                                       .GroupBy(stock => new
                                                                                                                                       {
                                                                                                                                           stock.股票代號,
                                                                                                                                           stock.產業代號,
                                                                                                                                           stock.上市櫃
                                                                                                                                       },
                                                                                                                                       (key, data) => new
                                                                                                                                       {
                                                                                                                                           年度 = targetDay.Substring(0, YEAR_WORDS),
                                                                                                                                           key.上市櫃,
                                                                                                                                           key.股票代號,
                                                                                                                                           data.FirstOrDefault().股票名稱,
                                                                                                                                           key.產業代號,
                                                                                                                                           平均成交量 = data.Average(item => item.成交量)
                                                                                                                                       }))
                                                                .SelectMany(day => day)
                                                                .Join(GetSameForTop5(),
                                                                         stock => new { stock.年度, stock.產業代號, stock.上市櫃 },
                                                                         same => new { same.年度, same.產業代號, same.上市櫃 },
                                                                         (stock, same) => new Top5VolumeDto
                                                                         {
                                                                             年度 = same.年度,
                                                                             類股股票代號 = same.股票代號,
                                                                             類股股票名稱 = same.股票名稱,
                                                                             股票代號 = stock.股票代號,
                                                                             股票名稱 = stock.股票名稱,
                                                                             產業代號 = stock.產業代號,
                                                                             平均成交量 = stock.平均成交量
                                                                         })
                                                                .OrderByDescending(stock => stock.平均成交量)
                                                                .GroupBy(stock => stock.年度)
                                                                .AsEnumerable()//放這最快
                                                                .SelectMany(stock => stock.Take(TOP_5))//取前5名
                                                                .ToList();
        }

        /// <summary>
        /// 取得報酬率大於0前5名類股
        /// </summary>
        /// <param name="kinds">所有類股資料</param>
        /// <returns>報酬率大於0前5名類股</returns>
        private List<List<RiseFallTop5Dto>> RiseFall(List<List<KindRateDto>> kinds)
        {
            return kinds.Select(kind => kind.Where(data => data.報酬率 > 0)
                                                                       .Take(5).Select((data, index) => new RiseFallTop5Dto//取報酬率最高5筆
                                                                       {
                                                                           年度 = data.年度,
                                                                           類股股票代號 = data.類股股票代號,
                                                                           類股股票名稱 = data.類股股票名稱,
                                                                           報酬率 = data.報酬率,
                                                                           項目 = "最高",
                                                                           排名 = index + 1
                                                                       })
                                                                       .Union(kind.Where(data => data.報酬率 < 0)
                                                                                             .Reverse()
                                                                                             .Take(5)//取報酬率最低5筆
                                                                                             .Select((data, index) => new RiseFallTop5Dto
                                                                                             {
                                                                                                 年度 = data.年度,
                                                                                                 類股股票代號 = data.類股股票代號,
                                                                                                 類股股票名稱 = data.類股股票名稱,
                                                                                                 報酬率 = data.報酬率,
                                                                                                 項目 = "最低",
                                                                                                 排名 = index + 1
                                                                                             })).ToList()).ToList();
        }

        /// <summary>
        /// 取得上漲表現遞減，股票代號遞增，取得10檔個股，以及包含這些個股的類股
        /// </summary>
        /// <param name="riseFall">個股資訊</param>
        /// <param name="kinds">類股資訊</param>
        /// <returns>計算完上漲的10檔個股資訊</returns>
        private List<List<OriginalDto>> GetOriginalData(List<RiseFallDto> riseFall, List<List<KindRateDto>> kinds)
        {
            List<List<OriginalDto>> riseResult = riseFall.GroupBy(stock => stock.年度)
                                                                                                .ToList()
                                                                                                .Select(stocks => 
                                                                                                {
                                                                                                    List<KindRateDto> kindYear = kinds.First(kind => kind.First().年度 == stocks.Key).ToList();
                                                                                                    return stocks
                                                                                                    .OrderByDescending(rise => rise.上漲表現)
                                                                                                    .ThenBy(rise => rise.股票代號).Take(10)//依上漲前10
                                                                                                    .Union(stocks.OrderByDescending(rise => rise.下跌表現)
                                                                                                                             .ThenBy(rise => rise.股票代號).Take(10))//取下跌前10
                                                                                                    .Select(rise => new OriginalDto
                                                                                                    {
                                                                                                        年度 = rise.年度,
                                                                                                        股票代號 = rise.股票代號,
                                                                                                        股票名稱 = rise.股票名稱,
                                                                                                        上漲表現 = rise.上漲表現,
                                                                                                        下跌表現 = rise.下跌表現,
                                                                                                        Kind = kindYear
                                                                                                        .Where(kind => kind.C200707整併後交易所產業編號.Contains(rise.產業代號)
                                                                                                                                       && kind.上市櫃 == rise.上市櫃)//取得該股票產業代號所對應的類股
                                                                                                        .OrderBy(kind => kind.類股股票代號)
                                                                                                    })
                                                                                                    .ToList();
                                                                                                })
                                                                                                .ToList();

            return riseResult;
        }

        /// <summary>
        /// 查詢上漲下跌表現前10位的個股與類股前5位的對應關係
        /// </summary>
        /// <param name="riseTop">類股前5</param>
        /// <param name="originalTop">上漲下跌表現前10位</param>
        /// <returns>計算後的資訊</returns>
        private List<IndividualStockDto> GetIndividualStocks(IEnumerable<IEnumerable<RiseFallTop5Dto>> riseTop, IEnumerable<IEnumerable<OriginalDto>> originalTop)
        {
            return originalTop.Select(record => 
                                                                                {
                                                                                  List<RiseFallTop5Dto> rises = riseTop.First(rise => rise.First().年度 == record.First().年度).ToList();
                                                                                  List<RiseFallTop5Dto> risesTop = rises.Take(5).ToList();
                                                                                  List<RiseFallTop5Dto> risesBelow = rises.Skip(5).ToList();
                                                                                  return record.Take(10)//上漲前10
                                                                                                           .SelectMany(stock => stock.Kind, (stock, kind) => new IndividualStockDto
                                                                                                           {
                                                                                                               年度 = stock.年度,
                                                                                                               類股股票代號 = kind.類股股票代號,
                                                                                                               類股股票名稱 = kind.類股股票名稱,
                                                                                                               項目 = stock.上漲表現 > stock.下跌表現 ? "最高" : "最低",
                                                                                                               是否存在 = risesTop.Any(rise => rise.類股股票代號 == kind.類股股票代號) ? "是" : "否"
                                                                                                           })
                                                                                                          .Union(record.Skip(10)//下跌前10
                                                                                                                                    .SelectMany(stock => stock.Kind, (stock, kind) => new IndividualStockDto
                                                                                                                                    {
                                                                                                                                        年度 = stock.年度,
                                                                                                                                        類股股票代號 = kind.類股股票代號,
                                                                                                                                        類股股票名稱 = kind.類股股票名稱,
                                                                                                                                        項目 = stock.上漲表現 > stock.下跌表現 ? "最高" : "最低",
                                                                                                                                        是否存在 = risesBelow.Any(rise => rise.類股股票代號 == kind.類股股票代號) ? "是" : "否"
                                                                                                                                    }));
                                                            })
                                               .SelectMany(stock => stock)
                                               .GroupBy(stock => new { stock.年度, stock.類股股票代號, stock.項目 })//去掉重複
                                               .Select(stocks => stocks.First())
                                               .OrderBy(stock => stock.年度)
                                               .ThenByDescending(stock => stock.項目)
                                               .ThenByDescending(stock => stock.是否存在)
                                               .ThenBy(stock => stock.類股股票代號)
                                               .ToList();
        }

        /// <summary>
        /// 彙整4.d要得三個表的資料
        /// </summary>
        /// <param name="riseFall">類股報酬率最高、最低前五名分頁的資料</param>
        /// <param name="originals">計算後原始資料分頁的資料</param>
        /// <param name="individuals">共用分頁的資料</param>
        public void GetRiseFallTop5(out List<RiseFallTop5Dto> riseFall, out List<OriginalDataDto> originals, out List<IndividualStockDto> individuals)
        {
            List<RiseFallDto> riseFallData = CheckData(TRADING_DAY_100TH); //取得上漲下跌股票資料
            List<List<KindRateDto>> kindsData = GetRate(TRADING_DAY_100TH); //取得類股報酬率
            List<List<OriginalDto>> originalData = GetOriginalData(riseFallData, kindsData); //取得上漲下跌的原始資料，還未分開兩個年度及類股資料
            List<List<RiseFallTop5Dto>>riseFallResult = RiseFall(kindsData); //取得類股報酬率最高、最低前五名
            riseFall = riseFallResult.SelectMany(kind => kind).ToList();
            originals = originalData.Select(stock => stock.SelectMany(rise => rise.Kind, (rise, kind) => new OriginalDataDto
            {
                年度 = rise.年度,
                類股股票代號 = kind.類股股票代號,
                類股股票名稱 = kind.類股股票名稱,
                股票代號 = rise.股票代號,
                股票名稱 = rise.股票名稱,
                上漲表現 = rise.上漲表現,
                下跌表現 = rise.下跌表現,
            }))
                                                    .SelectMany(rise => rise)
                                                    .OrderBy(rise => rise.年度)
                                                    .ToList(); //將上漲下跌原始資料的類股及年度拆開
            individuals = GetIndividualStocks(riseFallResult, originalData); //取得共用頁資料
        }

        /// <summary>
        /// 取得上漲下跌表現
        /// </summary>
        /// <param name="lastDayPosition">總統大選後的天數</param>
        /// <returns>上漲下跌表現</returns>
        public List<RiseFallDto> CheckData(int lastDayPosition)
        {
            return PRESIDENTIAL_DATE.Select(targetDay => StockDB.日收盤.Where(day => day.股票代號 == "TWA00"
                                                                                                                                                                  && string.Compare(day.日期, targetDay) > 0)
                                                                                                                                     .Select(stock => stock.日期)
                                                                                                                                     .OrderBy(day => day)//排序
                                                                                                                                     .Take(lastDayPosition)
                                                                                                                                     .Join(StockDB.日收盤.Where(stock => stock.產業代號 != null), day => day, stock => stock.日期, (day, stock) => new
                                                                                                                                     {
                                                                                                                                         stock.日期,
                                                                                                                                         stock.股票代號,
                                                                                                                                         stock.股票名稱,
                                                                                                                                         stock.上市櫃,
                                                                                                                                         stock.漲跌,
                                                                                                                                         stock.產業代號
                                                                                                                                     })
                                                                                                                                     .AsEnumerable()
                                                                                                                                     .GroupBy(stocks => new { stocks.股票代號, stocks.上市櫃, stocks.產業代號 }, (key, value) => new
                                                                                                                                     {
                                                                                                                                         key.股票代號,
                                                                                                                                         key.產業代號,
                                                                                                                                         key.上市櫃,
                                                                                                                                         fallRises = value.OrderBy(day => day.日期)
                                                                                                                                                                        .Select(stock => stock.漲跌),
                                                                                                                                         value.First().股票名稱
                                                                                                                                     })
                                                                                                                                     .Select(stocks =>
                                                                                                                                     {
                                                                                                                                         GetUpsDowns(stocks.fallRises, out int rise, out int fall);
                                                                                                                                         return new RiseFallDto
                                                                                                                                         {
                                                                                                                                             年度 = targetDay.Substring(0, YEAR_WORDS),
                                                                                                                                             上市櫃 = stocks.上市櫃,
                                                                                                                                             股票代號 = stocks.股票代號,
                                                                                                                                             股票名稱 = stocks.股票名稱,
                                                                                                                                             產業代號 = stocks.產業代號,
                                                                                                                                             上漲表現 = rise,
                                                                                                                                             下跌表現 = fall
                                                                                                                                         };
                                                                                                                                     }))
                                                                .SelectMany(stock => stock).ToList();
        }

        /// <summary>
        /// 計算漲跌
        /// </summary>
        /// <param name="record">股票資料</param>
        /// <param name="rise">回傳上漲表現</param>
        /// <param name="fall">回傳下跌表現</param>
        private void GetUpsDowns(IEnumerable<decimal?> record, out int rise, out int fall)
        {
            int count = 0; //最大連續上漲
            int riseDay = 0; //當前連續上漲
            int fallCount = 0; //最大連續下跌
            int fallDay = 0; //當前連續下跌
            int riseAllDay = 0; //上漲總數
            int fallAllDay = 0; //下跌總數
            record.Select(riseStock =>
            {
                if (riseStock > 0)
                {
                    riseAllDay++;
                    riseDay++;
                }
                else
                {
                    if (count < riseDay)
                    {
                        count = riseDay;
                    }
                    riseDay = 0;
                }
                if (riseStock < 0)
                {
                    fallAllDay++;
                    fallDay++;
                }
                else
                {
                    if (fallCount < fallDay)
                    {
                        fallCount = fallDay;
                    }
                    fallDay = 0;
                }
                return count;
            })
            .ToList();
            if (count < riseDay)
            {
                count = riseDay;
            }
            if (fallCount < fallDay)
            {
                fallCount = fallDay;
            }
            rise = riseAllDay + count;
            fall = fallAllDay + fallCount;
        }
    }
}
