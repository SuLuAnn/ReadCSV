using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayFuturesClass
{
    public abstract class DayFutures
    {
        public string DataTableName { get; set; }
        public DayFutures(string dataTableName)
        {
            DataTableName = dataTableName;
        }

        public string GetDataTableName()
        {
            return DataTableName;
        }
    }
}
