using System.Collections.Generic;

namespace Amortizor.Interfaces
{
    public interface IPerson
    {
        decimal InitialExtraLoanPayment { get; }
        decimal ExtraLoanPaymentFromRaises { get; set; }
        IList<ILoan> Loans { get; }
        ISalary Salary { get; }
        decimal TotalDebt { get; }
        IList<ILoan> ApplicableLoans { get; }
        IList<ILoan> PaidLoans { get; }
        ILoan ExtraPaymentLoan { get; }
        decimal ExtraLoanPayment { get; }
    }
}
