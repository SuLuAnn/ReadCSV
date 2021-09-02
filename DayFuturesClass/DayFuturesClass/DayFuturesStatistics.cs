﻿using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayFuturesClass
{
    [ExportMetadata("TableName", "日期貨盤後統計表")]
    [Export(typeof(IDataSheet))]
    public class DayFuturesStatistics : DataBaseTable
    {
        public DayFuturesStatistics() : base("日期貨盤後統計表_luann")
        {
        }

        public override List<OriginalWeb> GetWebs()
        {
            throw new NotImplementedException();
        }
    }
}
