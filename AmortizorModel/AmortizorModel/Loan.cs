using AmortizorModel.Enums;
using System;

namespace AmortizorModel
{
    public class Loan
    {
        public string Name { get; set; }
        public decimal InterestRate { get; set; }
        public decimal PrincipalBalance { get; set; }
        public decimal AccruedInterest { get; set; }
        public decimal MinimumMonthlyPayment { get; set; }

        public decimal DailyInterest => PrincipalBalance * InterestRate / DAYS_IN_YEAR;
        public LoanState State => PrincipalBalance <= 0 ? LoanState.Paid : LoanState.Active;

        public decimal GetAccruedInterest(int days) => DailyInterest * days;

        private const decimal DAYS_IN_YEAR = 365.25m;
    }
}
