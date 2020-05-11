using System;

namespace AmortizorModel
{
    public class Loan
    {
        public readonly Guid Id = Guid.NewGuid();
        public string Name { get; set; }
        public decimal InterestRate { get; set; }
        public decimal PrincipalBalance { get; set; }
        public decimal AccruedInterest { get; set; }
        public decimal MinimumMonthlyPayment { get; set; }

        public decimal DailyInterest => PrincipalBalance * InterestRate / DAYS_IN_YEAR;
        public decimal GetAccruedInterest(int days) => DailyInterest * days;

        private const decimal DAYS_IN_YEAR = 365.25m;
    }
}
