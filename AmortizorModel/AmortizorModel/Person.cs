using AmortizorModel.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AmortizorModel
{
    public class Person
    {
        public Person(IList<Loan> loans, DateTime startDate, decimal extraLoanRepayment, bool debtSnowball)
        {
            Loans = loans;
            CurrentDate = startDate;
            CurrentDate = startDate;
            InitialExtraLoanPayment = extraLoanRepayment;
            DebtSnowball = debtSnowball;
            //AnnualRaisePercent = annualRaisePercent;
            //AnnualRaiseDate = annualRaiseDate;
            //Salary = salary;
        }

        //TODO: Make this prettier
        //(Fracture into functions? Move into a DebtCalendar model? Maybe make the person a property of the DebtCalendar instead of vice versa? Most of a Person's attributes are very tied to this function so maybe just fracture this function into smaller bits instead. Or make a service?)
        public DateTime FreedomDate
        {
            get
            {
                //This loop represents the thought process a person would go through on a monthly basis to decide how to pay off their debts each month
                //Long term goal would be to clean this up but abstracting the process this way lets me avoid lots of stuff I'd have to handle otherwise (e.g. leap years, rounding errors, incredibly complex differential equations)
                while (TotalDebt > 0)
                {
                    var nextDate = CurrentDate.AddMonths(1);
                    var daysInCurrentMonth = (nextDate - CurrentDate).Days;
                    //Grab the information on the extra payment for the month and which loan to apply it to that
                    //month here so that we don't let any of this logic change in the middle of the month
                    var extraPaymentLoanForMonth = ExtraPaymentLoan(daysInCurrentMonth);
                    var extraLoanPaymentForMonth = ExtraLoanPayment;
                    //We only want to consider loans that haven't already been paid off
                    foreach (Loan loan in ApplicableLoans)
                    {
                        var newAccruedInterest = loan.GetAccruedInterest(daysInCurrentMonth);
                        newAccruedInterest -= loan.MinimumMonthlyPayment;
                        if (loan.Name == extraPaymentLoanForMonth.Name)
                            newAccruedInterest -= extraLoanPaymentForMonth;
                        loan.AccruedInterest += newAccruedInterest;

                        //Only apply payment to principal balance if we have paid off all of our accrued interest
                        if (loan.AccruedInterest < 0)
                        {
                            loan.PrincipalBalance += loan.AccruedInterest;
                            RolloverLoanPayment(loan.PrincipalBalance, daysInCurrentMonth);
                            loan.AccruedInterest = 0;
                        }
                        //TODO: Make this debug line be some kind of logger to let you know how to pay off your loans at each month
                        Debug.WriteLine($"Date: {nextDate}, Loan: {loan.Name}, Principal: {loan.PrincipalBalance}");
                    }
                    CurrentDate = nextDate;
                }
                return CurrentDate;
            }
        }

        private readonly bool DebtSnowball;
        private readonly decimal InitialExtraLoanPayment;
        private IList<Loan> Loans;
        private DateTime CurrentDate;
        //TODO: Salary model?
        //private readonly float AnnualRaisePercent;
        //private readonly DateTime AnnualRaiseDate;
        //private decimal Salary;

        private decimal TotalDebt => ApplicableLoans.Sum(l => l.PrincipalBalance);
        private List<Loan> ApplicableLoans => Loans.Where(l => l.State == LoanState.Active).ToList();
        private List<Loan> PaidLoans => Loans.Where(l => l.State == LoanState.Paid).ToList();
        private decimal ExtraLoanPayment => InitialExtraLoanPayment + PaidLoans.Sum(l => l.MinimumMonthlyPayment);

        private Loan ExtraPaymentLoan(int days)
        {
            if (DebtSnowball)
                return ApplicableLoans.OrderBy(l => l.PrincipalBalance).ThenBy(l => l.Name).First();
            else
                //For minimizing interest paid, we want to always put the extra payment towards wichever loan will accrue the most interest next
                return ApplicableLoans.OrderByDescending(l => l.GetAccruedInterest(days)).ThenBy(l => l.Name).First();
        }

        private void RolloverLoanPayment(decimal loanNegativePrincipal, int days)
        {
            var leftoverPayment = loanNegativePrincipal;
            while (ApplicableLoans.Any() && leftoverPayment < 0)
            {
                //While we have surplus payments from paid off loans, apply them to whatever is the loan we want to pay extra towards in the moment
                var loanToPayTowards = ExtraPaymentLoan(days);
                var newPrincipalBalance = loanToPayTowards.PrincipalBalance + leftoverPayment;
                leftoverPayment = newPrincipalBalance;
                loanToPayTowards.PrincipalBalance = newPrincipalBalance;
            }
        }
    }
}
