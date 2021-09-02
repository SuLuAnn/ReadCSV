using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public abstract class DataBaseTable : IDataSheet
    {
        public HttpClient HttpGetter { get; set; }
        public string DataTableName { get; set; }
        public DataBaseTable(string dataTableName)
        {
            DataTableName = dataTableName;
            HttpGetter = new HttpClient();
        }
        public string GetDataTableName()
        {
            return DataTableName;
        }

        public abstract List<OriginalWeb> GetWebs();
        public string GetWebPage(string url)
        {
            HttpResponseMessage responseMessage = HttpGetter.GetAsync(url).Result;
            return responseMessage.Content.ReadAsStringAsync().Result;
        }
    }
}
