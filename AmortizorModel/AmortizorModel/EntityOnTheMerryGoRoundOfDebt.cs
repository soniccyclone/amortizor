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
                    //Grab this here so we don't apply extra payment to multiple loans in one month
                    var extraPaymentLoanForMonth = ExtraPaymentLoan;
                    foreach (Loan loan in ApplicableLoans) {
                        var dailyInterest = loan.PrincipalBalance * loan.InterestRate / DAYS_IN_YEAR;
                        var newAccruedInterest = dailyInterest * days;
                        newAccruedInterest -= loan.MinimumMonthlyPayment;
                        if (loan.Name == extraPaymentLoanForMonth.Name)
                            //TODO: Make this loop log which extra loan payments go to which loan so I know which ones to apply extra repayment to while doing this irl
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

        private decimal TotalDebt => ApplicableLoans.Where(l => l.PrincipalBalance > 0).Sum(l => l.PrincipalBalance);
        //We only want to consider loans that haven't already been paid off
        //TODO: Make things smarter so leftover extra payments that would have gone towards these loans will go towards other loans
        //TODO: Make minimum repayments on laons rollover into other loans once the current loan is paid off
        private List<Loan> ApplicableLoans => Loans.Where(l => l.PrincipalBalance > 0).ToList();
        //TODO: Add logic for DebtSnowball vs going after highest accruing interest loan
        private Loan ExtraPaymentLoan => Loans.Where(l => l.PrincipalBalance > 0).OrderBy(l => l.Name).ThenByDescending(l => l.PrincipalBalance).First();

        private const decimal DAYS_IN_YEAR = 365.25m;
    }
}
