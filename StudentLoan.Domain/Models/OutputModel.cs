using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentLoan.Domain.Models
{
    /*
    * This class definition is used as a return type for the NetWorth() method
    */
    public class OutputModel
    {
        public int[]? netWorthArray { get; set; }
        public int[]? outstandingBalanceArray { get; set; }

        public OutputModel(int years)
        {
            // ensures there is always enough space in array
            if (years <= 2)
            {
                netWorthArray = new int[2];
                outstandingBalanceArray = new int[2];
            }
            else
            {
                netWorthArray = new int[years];
                outstandingBalanceArray = new int[years];
            }
        }
    }
}
