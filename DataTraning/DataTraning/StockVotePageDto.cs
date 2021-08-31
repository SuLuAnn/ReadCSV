using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataTraning
{
    public class StockVotePageDto
    {
        public int PageNumber { get; set; }
        public MatchCollection OnePageVoteData { get; set; }
        public StockVotePageDto(int pageNumber, MatchCollection onePageVoteData)
        {
            PageNumber = pageNumber;
            OnePageVoteData = onePageVoteData;
        }
    }
}
