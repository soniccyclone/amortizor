using AmortizorModel.Interfaces;
using AmortizorModel.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AmortizorModel.Services
{
    public class DebtCalendar
    {
        public DebtCalendar(Person person)
        {
            Person = person;
        }

        public IList<MonthlyDecisionsModel> GenerateDebtRepaymentPlan(DateTime startDate)
        {
            var currentDate = startDate;
            var monthlyDecisions = new List<MonthlyDecisionsModel>();
            while (Person.TotalDebt > 0)
            {
                var nextDate = currentDate.AddMonths(1);
                monthlyDecisions.Add(new MonthlyDecisionsModel() {
                    Month = currentDate,
                    Decisions = ProcessMonth(currentDate, nextDate)
                });
                currentDate = nextDate;
            }
            return monthlyDecisions;
        }

        //This function represents the thought process a person would go through within a month to decide how to pay off their debts most efficiently
        //Long term goal would be to clean this up by using more math, but abstracting the process this way lets me avoid lots of stuff I'd have to
        //handle otherwise (e.g. leap years, rounding errors, incredibly complex differential equations)
        private IList<DebtDecisionModel> ProcessMonth(DateTime currentDate, DateTime nextDate)
        {
            var decisions = new List<DebtDecisionModel>();
            var daysInCurrentMonth = (nextDate - currentDate).Days;
            //Grab the information on the extra payment for the month and which loan to apply it to for
            //that month here so that we don't let any of this logic change in the middle of the month
            var extraPaymentLoanForMonth = Person.ExtraPaymentLoan;
            if (currentDate.Month == Person.Salary.AnnualRaiseMonth)
                ApplyRaise();
            var extraLoanPaymentForMonth = Person.ExtraLoanPayment;
            //We only want to consider loans that haven't already been paid off
            foreach (ILoan loan in Person.ApplicableLoans)
            {
                var loanDecision = new DebtDecisionModel { LoanName = loan.Name, CurrentPrincipal = loan.PrincipalBalance };
                loanDecision.CurrentPayment = ProcessLoan(loan, extraPaymentLoanForMonth, daysInCurrentMonth, extraLoanPaymentForMonth);
                decisions.Add(loanDecision);
            }
            return decisions;
        }

        private decimal ProcessLoan(ILoan loan, ILoan extraPaymentLoanForMonth, int daysInCurrentMonth, decimal extraLoanPaymentForMonth)
        {
            var newAccruedInterest = loan.GetAccruedInterest(daysInCurrentMonth);
            var currentPayment = loan.MinimumMonthlyPayment;
            if (loan.Name == extraPaymentLoanForMonth.Name)
                currentPayment += extraLoanPaymentForMonth;
            loan.AccruedInterest += newAccruedInterest - currentPayment;

            //Only apply payment to principal balance if we have paid off all of our accrued interest
            if (loan.AccruedInterest < 0)
            {
                loan.PrincipalBalance += loan.AccruedInterest;
                ProcessSurplusLoanPayment(loan.PrincipalBalance);
                loan.AccruedInterest = 0;
            }

            return currentPayment;
        }

        private void ProcessSurplusLoanPayment(decimal loanNegativePrincipal)
        {
            var leftoverPayment = loanNegativePrincipal;
            while (Person.ApplicableLoans.Any() && leftoverPayment < 0)
            {
                //While we have surplus payments from paid off loans, apply them to whatever is the loan we want to pay extra towards in the moment
                var loanToPayTowards = Person.ExtraPaymentLoan;
                var newPrincipalBalance = loanToPayTowards.PrincipalBalance + leftoverPayment;
                leftoverPayment = newPrincipalBalance;
                loanToPayTowards.PrincipalBalance = newPrincipalBalance;
            }
        }

        private void ApplyRaise()
        {
            var raise = Person.Salary.AnnualAmount * Person.Salary.AnnualRaisePercent;
            Person.Salary.AnnualAmount += raise;
            var monthlyRaise = raise / 12 * Person.Salary.PercentOfRaiseForRepayment;
            Person.ExtraLoanPaymentFromRaises += monthlyRaise;
        }

        private Person Person { get; }
    }
}
