using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqUserInterface.Dto
{
    public class RateReturn
    {
        public string 年度 { get; set; }
        public string 類股股票代號 { get; set; }
        public string 類股股票名稱 { get; set; }
        public decimal? 報酬率 { get; set; }
        public int 報酬率排名 { get; set; }

        public RateReturn SetRank(int rank)
        {
            報酬率排名 = rank;
            return this;
        }
    }
}
