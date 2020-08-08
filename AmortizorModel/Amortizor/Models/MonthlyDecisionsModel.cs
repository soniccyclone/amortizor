using Amortizor.Interfaces;
using System;
using System.Collections.Generic;

namespace Amortizor.Models
{
    public class MonthlyDecisionsModel : IMonthlyDecisionsModel
    {
        public DateTime Month { get; set; }
        public IList<IDebtDecisionModel> Decisions { get; set; }
    }
}
