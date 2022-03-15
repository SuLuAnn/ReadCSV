using CMoney.Kernel;
using DownloadSystem.DataLibs.Interface;
using DownloadSystem.DataLibs.Models;
using M157_DownloadExtent_TEST;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace M157_DownLoadExtent
{
    /// <summary>
    /// 母任務下載(載各新聞)
    /// </summary>
    [Export(typeof(DownloadSystem.DataLibs.Interface.ITaskExtension))]
    [ExportMetadata("Name", "M157_DownloadExtent_TEST.181001001_TEST")]
    [ExportMetadata("ContainsDatabaseOperation", true)]
    [ExportMetadata("ContainsInternetOperation", false)]
    public class DownloadExtension_181009001 : ITaskExtension
    {
        /// <summary>
        /// 各股新聞下載連結table
        /// </summary>
        private DataTable Table;

        /// <summary>
        /// 取[各股新聞下載連結] (未下載) 清單
        /// </summary>
        /// <param name="extraParameter">外傳參數(股票代號開頭數字)</param>
        private void GetLinkTable(string extraParameter)
        {
#if DEBUG
            Table = DB.DoQuerySQLWithSchema($"SELECT * FROM [StockDB_TW].[dbo].[各股新聞下載連結] WHERE [已下載]=1 AND [日期] = '20210825'", KernelConst.DEF_DBCONNECTION_SRV1);
#else
            //以外部參數去找該年度所有該股票代號開頭之股票代號
            Table = DB.DoQuerySQLWithSchema($"SELECT * FROM [StockDB_TW].[dbo].[各股新聞下載連結] WHERE [已下載]=0 AND  [股票代號] LIKE '{extraParameter}%'", KernelConst.DEF_DBCONNECTION_SRV1);
#endif     
        }

        /// <summary>
        /// 將下載過之連接對應的 [各股新聞下載連結]資料表 的 [已下載] 欄位改為true
        /// </summary>
        /// <param name="taskInfo">任務</param>
        /// <param name="downloadedWebSourceDataList">下載清單</param>
        /// <param name="failedList">下載失敗清單</param>
        /// <returns></returns>
        public bool CheckContent(ref TaskInfo taskInfo, ref List<WebSourceData> downloadedWebSourceDataList, out List<WebSourceData> failedList)
        {
            failedList = new List<WebSourceData>();
            //排除錯誤下載
            List<WebSourceData> faildDatas = downloadedWebSourceDataList.Where(x => x.WebContent == null || x.WebContent.Length == 0).ToList();

            downloadedWebSourceDataList.RemoveAll(x => faildDatas.Select(y => y.URI).Contains(x.URI));

            //MTIME 資料每次異動的時間
            int mtime = DB.GetMTime();

            //update字串
            StringBuilder sb = new StringBuilder();

            foreach (WebSourceData webSourceData in downloadedWebSourceDataList)
            {
                //主鍵索引欄位(週期,樣本,流水號)
                string dataCycle = webSourceData.Cycle;
                string dataSample = webSourceData.Sample;
                string num = Regex.Match(webSourceData.URI, "(?<num>[0-9]{9}).html", RegexOptions.IgnoreCase).Groups["num"].ToString();

                //欲查詢的資料的主鍵索引
                object[] search = new object[] { dataCycle, dataSample, num };

                //以主鍵索引搜尋DataRow
                DataRow dataRow = Table.Rows.Find(search);

                //判斷主鍵索引是否有搜尋DataRow(有則將 [各股新聞下載連結] 相對應欄位 [已下載] 改為ture)
                if (dataRow != null)
                {
                    //將update串成一字串(將 [各股新聞下載連結] 相對應欄位 [已下載] 改為ture)
                    sb.AppendLine($@"UPDATE [StockDB_TW].[dbo].[各股新聞下載連結] SET [已下載] = 1
                       WHERE [日期] = '{dataCycle}' AND [股票代號] = '{dataSample}' AND [流水號] = '{num}';");
                }
            }

            //update資料庫
            DB.DoQueryExecuteScalar(sb.ToString(), KernelConst.DEF_DBCONNECTION_SRV1, out _);

            failedList = faildDatas;
            return failedList.Count == 0;
        }

        /// <summary>
        /// 取得近兩日日期之新聞的連結，並建子任務清單
        /// </summary>
        /// <param name="webSourceDataPrototypeList">原任務清單</param>
        /// <param name="cycleRange">週期</param>
        /// <param name="samples">樣本</param>
        /// <param name="extraParameter">外部參數</param>
        /// <param name="parentList">母任務(無)</param>
        /// <returns>新子任務清單(所有近兩日日期之新聞)</returns>
        public List<WebSourceData> GetList(List<WebSourceData> webSourceDataPrototypeList, string cycleRange, string samples, string extraParameter, List<WebSourceData> parentList = null)
        {
            //取[各股新聞下載連結] (未下載) 清單
            GetLinkTable(extraParameter);

            List<WebSourceData> result = new List<WebSourceData>();

            WebSourceData webSource = webSourceDataPrototypeList.First();


            //尋覽所有[各股新聞下載連結] (未下載) 清單之row
            foreach (DataRow row in Table.Rows)
            {
                string date = row["日期"].ToString();

                //新增一WebSourceData
                WebSourceData webSourceData = new WebSourceData(webSource);

                //每筆新聞的連結
                webSourceData.URI = row["連結"].ToString();

                //設定 Cycle [日期]
                webSourceData.Cycle = date;

                //設定母任務Sample [股票代號]
                webSourceData.Sample = row["股票代號"].ToString();

                result.Add(webSourceData);
            }

            return result;
        }
    }
}
