using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FundNoBusinessClass
{
    [ExportMetadata("TableName", "基金非營業日統計")]
    [Export(typeof(IDataSheet))]
    public class FundNoBusinessStatistics : DataBaseTable
    {
        public FundNoBusinessStatistics() : base("基金非營業日統計_luann")
        {
        }

        public override List<OriginalWeb> GetWebs()
        {
            throw new NotImplementedException();
        }
    }
}
