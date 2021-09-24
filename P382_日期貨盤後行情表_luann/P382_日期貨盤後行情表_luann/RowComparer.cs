using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P382_日期貨盤後行情表_luann
{
    /// <summary>
    /// 用來比較兩個row是否資料相同
    /// </summary>
    public class RowComparer : IEqualityComparer<DataRow>
    {
        /// <summary>
        /// 比較兩個row是否內容相同
        /// </summary>
        /// <param name="x">row1</param>
        /// <param name="y">row2</param>
        /// <returns>是否相同</returns>
        public bool Equals(DataRow x, DataRow y)
        {
            return x.Field<string>("日期") == y.Field<string>("日期") &&
            x.Field<string>("代號") == y.Field<string>("代號") &&
            x.Field<string>("名稱") == y.Field<string>("名稱") &&
            x.Field<string>("輸出代號") == y.Field<string>("輸出代號") &&
            x.Field<string>("交割月份") == y.Field<string>("交割月份") &&
            x.Field<decimal?>("開盤價") == y.Field<decimal?>("開盤價") &&
            x.Field<decimal?>("最高價") == y.Field<decimal?>("最高價") &&
            x.Field<decimal?>("最低價") == y.Field<decimal?>("最低價") &&
            x.Field<decimal?>("收盤價") == y.Field<decimal?>("收盤價") &&
            x.Field<decimal?>("漲跌") == y.Field<decimal?>("漲跌") &&
            x.Field<int?>("成交量") == y.Field<int?>("成交量") &&
            x.Field<int?>("未沖銷契約數") == y.Field<int?>("未沖銷契約數");
        }

        /// <summary>
        /// 取得內容的hashcode
        /// </summary>
        /// <param name="obj">要計算的row</param>
        /// <returns>hashcode</returns>
        public int GetHashCode(DataRow obj)
        {
            return obj.Field<string>("日期").GetHashCode() +
            obj.Field<string>("代號").GetHashCode() +
            (obj.Field<string>("名稱") == null ? 0 : obj.Field<string>("名稱").GetHashCode()) +
            (obj.Field<string>("輸出代號") == null ? 0 : obj.Field<string>("輸出代號").GetHashCode()) +
            (obj.Field<string>("交割月份").GetHashCode()) +
            (obj.Field<decimal?>("開盤價") == null ? 0 : obj.Field<decimal?>("開盤價").GetHashCode()) +
            (obj.Field<decimal?>("最高價") == null ? 0 : obj.Field<decimal?>("最高價").GetHashCode()) +
            (obj.Field<decimal?>("最低價") == null ? 0 : obj.Field<decimal?>("最低價").GetHashCode()) +
            (obj.Field<decimal?>("收盤價") == null ? 0 : obj.Field<decimal?>("收盤價").GetHashCode()) +
            (obj.Field<decimal?>("漲跌") == null ? 0 : obj.Field<decimal?>("漲跌").GetHashCode()) +
            (obj.Field<int?>("成交量") == null ? 0 : obj.Field<int?>("成交量").GetHashCode()) +
            (obj.Field<int?>("未沖銷契約數") == null ? 0 : obj.Field<int?>("未沖銷契約數").GetHashCode());
        }
    }
}
