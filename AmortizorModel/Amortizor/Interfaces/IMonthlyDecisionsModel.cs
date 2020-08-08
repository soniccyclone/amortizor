using System;
using System.Collections.Generic;

namespace Amortizor.Interfaces
{
    public interface IMonthlyDecisionsModel
    {
        DateTime Month { get; set; }
        IList<IDebtDecisionModel> Decisions { get; set; }
    }
}
