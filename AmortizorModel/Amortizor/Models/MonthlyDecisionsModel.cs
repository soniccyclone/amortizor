using System;
using System.Collections.Generic;

namespace Amortizor.Models
{
    public class MonthlyDecisionsModel
    {
        public DateTime Month { get; set; }
        public IList<DebtDecisionModel> Decisions { get; set; }
    }

    public class DebtDecisionModel
    {
        public string LoanName { get; set; }
        public decimal CurrentPayment { get; set; }
        public decimal CurrentPrincipal { get; set; }
    }
}
