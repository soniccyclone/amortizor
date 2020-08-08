using Amortizor.Models;
using System;
using System.Collections.Generic;

namespace Amortizor.Interfaces
{
    public interface IDebtCalendar
    {
        IList<IMonthlyDecisionsModel> GenerateDebtRepaymentPlan(DateTime startDate);
    }
}
