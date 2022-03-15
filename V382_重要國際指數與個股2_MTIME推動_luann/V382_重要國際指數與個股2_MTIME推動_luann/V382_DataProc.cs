using CMoney.Kernel;
using DataProc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static V382_重要國際指數與個股2_MTIME推動_luann.Global;

namespace V382_重要國際指數與個股2_MTIME推動_luann
{
    /// <summary>
    /// 處理主程式
    /// </summary>
    public class V382_DataProc : IDataProc
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
        /// 實作資料轉換成CMoney表處理。這個需求是新增美股的中文新聞來源，因為新聞資料量龐大，所以不進mongoDB，
        /// 不跑一般的統一架構，而在處理程式才去叫用解析設定表的方法，使用解析擴充解析，解完接處理程序
        /// </summary>
        /// <param name="ps">程式相關設定</param>
        /// <returns>DataProc資料處理結果</returns>
        public DataResult GetProc(ProgramSetting ps)
        {
            string cycle = ps.DateRange.FirstOrDefault();
            string message = $"CMID: {ps.CMenuID} Task編號:{ps.ProcSerialNumber.ToString()} 週期:{cycle} 樣本:{string.Join(",", ps.Samples)}{Environment.NewLine}";
            using (StreamWriter writer = new StreamWriter(PATH, true))
            {
                writer.WriteLine(message);
            }
            DataResult result = new DataResult
            {
                Success = true
            };
            return result;
        }
    }
}
