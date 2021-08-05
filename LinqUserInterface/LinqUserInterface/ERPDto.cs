﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqUserInterface
{
    public class ERPDto
    {
        private int stockMinName;
        private IEnumerable<日收盤> dayStocks;
        public string 股票代號 { get; set; }
        public string 股票名稱 { get; set; }
        public decimal? 平均本益比 { get; set; }
    }
}