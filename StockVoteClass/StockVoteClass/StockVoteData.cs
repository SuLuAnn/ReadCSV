using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockVoteClass
{
    [ExportMetadata("TableName", "股東會投票資料表")]
    [Export(typeof(IDataSheet))]
    public class StockVoteData : StockVote
    {
        public StockVoteData() : base("股東會投票資料表_luann")
        {
        }
    }
}
