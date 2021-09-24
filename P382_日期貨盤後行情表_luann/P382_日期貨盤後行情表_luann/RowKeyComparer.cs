using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P382_日期貨盤後行情表_luann
{
    /// <summary>
    /// 用來比較兩個row是否是相同的key
    /// </summary>
    public class RowKeyComparer : IEqualityComparer<DataRow>
    {
        /// <summary>
        /// 比較兩個row是否是相同的key
        /// </summary>
        /// <param name="x">row1</param>
        /// <param name="y">row2</param>
        /// <returns>是否相同</returns>
        public bool Equals(DataRow x, DataRow y)
        {
            return x.Field<string>("日期") == y.Field<string>("日期") &&
            x.Field<string>("代號") == y.Field<string>("代號") &&
            x.Field<string>("交割月份") == y.Field<string>("交割月份");
        }

        /// <summary>
        /// 取得key的hashcode
        /// </summary>
        /// <param name="obj">要計算的row</param>
        /// <returns>hashcode</returns>
        public int GetHashCode(DataRow obj)
        {
            return obj.Field<string>("日期").GetHashCode() +
            obj.Field<string>("代號").GetHashCode() +
            (obj.Field<string>("交割月份").GetHashCode());
        }
    }
}
