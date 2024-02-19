using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMoneyManager.Shared.ViewModels
{
    public class FinancialStatusViewModel
    {
        public DateTime TimePeriod { get; set; }
        public Decimal FundChange { get; set; }
        public Decimal CurrentBalance { get; set; }
    }
}
