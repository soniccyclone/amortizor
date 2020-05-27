using AmortizorModel.Enums;
using AmortizorModel.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AmortizorModel.Models
{
    //TODO: Decouple with interface. That should protect me from the service tests becoming a nightmare later
    public class Person
    {
        public Person(IList<ILoan> loans, decimal extraLoanRepayment)
        {
            Loans = loans;
            InitialExtraLoanPayment = extraLoanRepayment;
            //TODO: Use Salary model
            //Salary = salary;
        }

        public decimal InitialExtraLoanPayment { get; }
        public IList<ILoan> Loans { get; }
        //private Salary salary { get; }

        public decimal TotalDebt => ApplicableLoans.Sum(l => l.PrincipalBalance);
        public IList<ILoan> ApplicableLoans => Loans.Where(l => l.State == LoanState.Active).ToList();
        public IList<ILoan> PaidLoans => Loans.Where(l => l.State == LoanState.Paid).ToList();
        public ILoan ExtraPaymentLoan => ApplicableLoans.OrderBy(l => l.PrincipalBalance).ThenBy(l => l.Name).First();
        public decimal ExtraLoanPayment => InitialExtraLoanPayment + PaidLoans.Sum(l => l.MinimumMonthlyPayment);
    }
}
