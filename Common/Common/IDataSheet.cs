using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IDataSheet
    {
        string GetDataTableName();
        void GetWebs();
        void GetXML();

        void WriteDatabase(SqlConnection SQLConnection);
    }
}
