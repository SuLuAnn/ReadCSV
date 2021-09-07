using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 主要是為了匯出 IDataSheet物件所代表的資料表
    /// </summary>
    public interface IDataSheetNews
    {
        /// <summary>
        /// 資料表名
        /// </summary>
        string TableName { get; }
    }
}
