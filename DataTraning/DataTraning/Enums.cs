using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTraning
{
    public class Enums
    {
        public enum Fund : int
        {
            NO_BUSINESS_DAY,
            COMPANY_ID,
            TAX_ID,
            FUND_NAME
        }

        public enum Futures : int
        {
            TRANSACTION_DATE = 0,
            CONTRACT = 1,
            EXPIRY_MONTH = 2,
            OPENING_PRICE = 3,
            HIGHEST_PRICE = 4,
            LOWEST_PRICE = 5,
            CLOSING_PRICE = 6,
            TRADING_HOURS = 17
        }
    }
}
