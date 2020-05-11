using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmortizorModel
{
    public class EntityOnTheMerryGoRoundOfDebt
    {
        //private readonly float AnnualRaisePercent;
        //private readonly bool DebtSnowball;
        private IList<Loan> Loans;
        private DateTime CurrentDate;
        //private decimal Salary;
        private decimal ExtraLoanRepayment;

        public EntityOnTheMerryGoRoundOfDebt(IList<Loan> loans,
            DateTime startDate,
            decimal extraLoanRepayment)
        {
            Loans = loans;
            //AnnualRaisePercent = annualRaisePercent;
            CurrentDate = startDate;
            CurrentDate = startDate;
            //Salary = salary;
            ExtraLoanRepayment = extraLoanRepayment;
            //DebtSnowball = debtSnowball;
        }

        public DateTime FreedomDate
        {
            get
            {
                while (TotalDebt > 0)
                {
                    var nextDate = CurrentDate.AddMonths(1);
                    var days = (nextDate - CurrentDate).Days;
                    foreach (Loan loan in Loans) {
                        var dailyInterest = loan.PrincipalBalance * loan.InterestRate / DAYS_IN_YEAR;
                        var newAccruedInterest = dailyInterest * days;
                        newAccruedInterest -= loan.MinimumMonthlyPayment;
                        if (loan.Id == ExtraPaymentLoan.Id)
                            newAccruedInterest -= ExtraLoanRepayment;
                        loan.AccruedInterest += newAccruedInterest;

                        //Only apply payment to principal balance if we have paid off all of our accrued interest
                        if (loan.AccruedInterest < 0)
                        {
                            loan.PrincipalBalance += loan.AccruedInterest;
                            loan.AccruedInterest = 0;
                        }
                    }
                    CurrentDate = nextDate;
                }
                return CurrentDate;
            }
        }

        private decimal TotalDebt => Loans.Sum(l => l.PrincipalBalance);
        //TODO: Add logic for DebtSnowball vs going after highest accruing interest loan
        private Loan ExtraPaymentLoan => Loans.OrderByDescending(l => l.PrincipalBalance).ThenBy(l => l.Id).First();

        private const decimal DAYS_IN_YEAR = 365.25m;
    }
}
