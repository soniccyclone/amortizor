using Amortizor.Enums;
using Amortizor.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Amortizor.Models
{
    public class Person: IPerson
    {
        public Person(IList<ILoan> loans, decimal extraLoanRepayment, ISalary salary)
        {
            Loans = loans;
            InitialExtraLoanPayment = extraLoanRepayment;
            ExtraLoanPaymentFromRaises = 0;
            Salary = salary;
        }

        public decimal InitialExtraLoanPayment { get; }
        public decimal ExtraLoanPaymentFromRaises { get; set; }
        public IList<ILoan> Loans { get; }
        public ISalary Salary { get; }

        public decimal TotalDebt => ApplicableLoans.Sum(l => l.PrincipalBalance);
        public IList<ILoan> ApplicableLoans => Loans.Where(l => l.State == LoanState.Active).ToList();
        public IList<ILoan> PaidLoans => Loans.Where(l => l.State == LoanState.Paid).ToList();
        public ILoan ExtraPaymentLoan => ApplicableLoans.OrderBy(l => l.PrincipalBalance).ThenBy(l => l.Name).First();
        public decimal ExtraLoanPayment => InitialExtraLoanPayment + PaidLoans.Sum(l => l.MinimumMonthlyPayment) + ExtraLoanPaymentFromRaises;
    }
}
