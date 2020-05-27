using AmortizorModel.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;

namespace AmortizorModel.Services
{
    public class DebtCalendar
    {
        public DebtCalendar(Person person)
        {
            this.person = person;
        }

        public DateTime FreedomDate(DateTime startDate)
        {
            var currentDate = startDate;
            //This loop represents the thought process a person would go through on a monthly basis to decide how to pay off their debts each month
            //Long term goal would be to clean this up but abstracting the process this way lets me avoid lots of stuff I'd have to handle otherwise (e.g. leap years, rounding errors, incredibly complex differential equations)
            while (person.TotalDebt > 0)
            {
                var nextDate = currentDate.AddMonths(1);
                var daysInCurrentMonth = (nextDate - currentDate).Days;
                //Grab the information on the extra payment for the month and which loan to apply it to that
                //month here so that we don't let any of this logic change in the middle of the month
                var extraPaymentLoanForMonth = person.ExtraPaymentLoan;
                var extraLoanPaymentForMonth = person.ExtraLoanPayment;
                //We only want to consider loans that haven't already been paid off
                foreach (ILoan loan in person.ApplicableLoans)
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
                        RolloverLoanPayment(loan.PrincipalBalance);
                        loan.AccruedInterest = 0;
                    }
                    //TODO: Make this debug line be some kind of logger to let you know how to pay off your loans at each month
                    Debug.WriteLine($"Date: {nextDate}, Loan: {loan.Name}, Principal: {loan.PrincipalBalance}");
                }
                currentDate = nextDate;
            }
            return currentDate;
        }

        private void RolloverLoanPayment(decimal loanNegativePrincipal)
        {
            var leftoverPayment = loanNegativePrincipal;
            while (person.ApplicableLoans.Any() && leftoverPayment < 0)
            {
                //While we have surplus payments from paid off loans, apply them to whatever is the loan we want to pay extra towards in the moment
                var loanToPayTowards = person.ExtraPaymentLoan;
                var newPrincipalBalance = loanToPayTowards.PrincipalBalance + leftoverPayment;
                leftoverPayment = newPrincipalBalance;
                loanToPayTowards.PrincipalBalance = newPrincipalBalance;
            }
        }

        private Person person { get; }
    }
}
