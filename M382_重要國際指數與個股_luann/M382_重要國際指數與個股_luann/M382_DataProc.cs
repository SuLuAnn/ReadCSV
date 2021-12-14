using CMoney.Kernel;
using DataProc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static M382_重要國際指數與個股_luann.Global;

namespace M382_重要國際指數與個股_luann
{
    /// <summary>
    /// 主要dll類
    /// </summary>
    public class M382_DataProc : IDataProc
    {
        /// <summary>
        /// 實作檢查機制
        /// </summary>
        /// <param name="CheckList">檢查項目</param>
        /// <param name="Cycle">週期</param>
        /// <param name="Sample">樣本</param>
        /// <returns>檢查結果</returns>
        public List<CheckResult> DataCheck(List<int> CheckList, string Cycle, string Sample)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 實作檢查機制
        /// </summary>
        /// <param name="CheckList">檢查項目</param>
        /// <param name="CheckData">檢查資料表</param>
        /// <returns>檢查結果</returns>
        public List<CheckResult> DataCheck(List<int> CheckList, DataTable CheckData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 實作資料轉換成CMoney表處理。
        /// </summary>
        /// <param name="ps">程式相關設定</param>
        /// <returns>DataProc資料處理結果</returns>
        public DataResult GetProc(ProgramSetting ps)
        {
            string cycle = ps.DateRange.FirstOrDefault();
            DataResult result = new DataResult();
            List<string> sampleList = GetAllSample(ps, cycle);

            if (sampleList != null)
            {
                DataTable targetTable = GetTargetTable(ps, cycle);
                DateTime ctime = DateTime.Now;
                int mtime = DB.GetMTime();
                //紀錄錯誤訊息
                string message = string.Empty;

                foreach (SourceInfo sourceInfo in ps.RunSourceInfo)
                {
                    foreach (string sample in sampleList)
                    {
                        ParseResult sourceData = sourceInfo.GetResult(cycle, sample);
                        DataRow targetRow = targetTable.Rows.Find(new object[] { cycle, sample });

                        if (targetRow == null)
                        {
                            targetRow = targetTable.NewRow();
                            targetRow[CTIME] = ctime;
                            targetRow[MTIME] = mtime;
                            targetRow[SOURCE_ID] = sourceInfo.SourceID;
                            targetRow[DATE] = cycle;
                            targetRow[STOCK_ID] = sample;
                            AddData(sourceData, targetRow, COLUMN_NAMES, ref message);
                            targetTable.Rows.Add(targetRow);
                        }
                        else
                        {
                            UpdateData(sourceData, targetRow, COLUMN_NAMES, ref message, mtime);
                        }
                    }
                }
                if (message.Length != 0)
                {
                    SendMessage(ps, MessagePart.ERROR, message);
                }
                result.Success = true;
                result.dtData = targetTable.GetChanges();
            }

            return result;
        }

        /// <summary>
        /// 取得所有樣本
        /// </summary>
        /// <param name="programSetting">程式相關設定</param>
        /// <param name="cycle">週期</param>
        /// <returns>所有樣本</returns>
        private List<string> GetAllSample(ProgramSetting programSetting, string cycle)
        {
            //判斷是否傳入樣本
            bool isAllSample = (string.IsNullOrEmpty(programSetting.Samples.FirstOrDefault()) && programSetting.Samples.Count == 1) || (programSetting.Samples.Count == 0);
            List<string> sampleList = new List<string>();

            if (isAllSample)
            {
                ParseResult sourceData = programSetting.RunSourceInfo.FirstOrDefault().GetResult(cycle, string.Empty);
                if (sourceData == null)
                {
                    return null;
                }
                sampleList = sourceData.RowSamples;
            }
            else
            {
                sampleList = programSetting.Samples;
            }

            return sampleList;
        }

        /// <summary>
        /// 取得要更新的目標資料表
        /// </summary>
        /// <param name="programSetting">程式相關設定</param>
        /// <param name="cycle">週期</param>
        /// <returns>要更新的目標資料表</returns>
        private DataTable GetTargetTable(ProgramSetting programSetting, string cycle)
        {
            string connectString = DB.GetSysDataSetting(UPPER_PROGRAM, programSetting.SysDataDB, CONNECT_STRING);
            string sqlCommand = $"SELECT * FROM [StockDB].[dbo].[重要國際指數與個股_luann] WHERE [日期] = '{cycle}'";
            return DB.DoQuerySQLWithSchema(sqlCommand, connectString);
        }

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="sourceData">來源資料</param>
        /// <param name="newRow">要插入資料的新行</param>
        /// <param name="columnNames">要被新增資料的欄位名稱</param>
        /// <param name="message">轉換失敗時紀錄訊息</param>
        private void AddData(ParseResult sourceData, DataRow newRow, List<string> columnNames, ref string message)
        {
            foreach (string columnName in columnNames)
            {
                if (decimal.TryParse(sourceData.GetValue(columnName), out decimal price))
                {
                    newRow[columnName] = price;
                }
                else
                {
                    message = ConvertErrorMessage(newRow, message);
                }
            }
        }

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="sourceData">來源資料</param>
        /// <param name="targetRow">要更新資料的目標行</param>
        /// <param name="columnNames">欄位名稱</param>
        /// <param name="message">轉換失敗時紀錄訊息</param>
        /// <param name="mtime">資料變動時間</param>
        private void UpdateData(ParseResult sourceData, DataRow targetRow, List<string> columnNames, ref string message, int mtime)
        {
            bool isUpdated = false;
            foreach (string columnName in columnNames)
            {
                string priceString = sourceData.GetValue(columnName);
                //priceString = null;
                if (priceString == null)
                {
                    continue;
                }
                if (decimal.TryParse(priceString, out decimal price))
                {
                    //price = 0;
                    if (targetRow[columnName] == DBNull.Value || targetRow.Field<decimal>(columnName) != price)
                    {
                        targetRow[columnName] = price;
                        isUpdated = true;
                    }
                }
                else
                {
                    message = ConvertErrorMessage(targetRow, message);
                }
            }
            if (isUpdated)
            {
                targetRow[MTIME] = mtime;
            }
        }

        /// <summary>
        /// 記送訊息
        /// </summary>
        /// <param name="programSetting">程式相關設定</param>
        /// <param name="part">信件等級</param>
        /// <param name="message">信件內容</param>
        private void SendMessage(ProgramSetting programSetting, MessagePart part, string message)
        {
            ProcessChangeEventArgs eventArgs = new ProcessChangeEventArgs();
            eventArgs.Part = (int)part;
            eventArgs.ProcessMessage = message;
            programSetting.SendMessage(eventArgs);
        }

        /// <summary>
        /// 將轉換失敗的資料的KEY加入訊息中
        /// </summary>
        /// <param name="targetRow">要加入的資料內容</param>
        /// <param name="message">被加入的訊息</param>
        /// <returns></returns>
        private string ConvertErrorMessage(DataRow targetRow, string message)
        {
            return $"{message}{Environment.NewLine}週期:{targetRow[DATE]},樣本:{targetRow[STOCK_ID]} 轉換為dacimal失敗";
        }
    }
}
