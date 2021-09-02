using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FundNoBusinessClass
{
    [ExportMetadata("TableName", "基金非營業日明細")]
    [Export(typeof(IDataSheet))]
    public class FundNoBusinessDetail : DataBaseTable
    {
        public FundNoBusinessDetail() : base("基金非營業日明細_luann")
        {
        }

        public override List<OriginalWeb> GetWebs()
        {
            throw new NotImplementedException();
        }
    }
}
