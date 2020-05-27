using AmortizorModel.Enums;

namespace AmortizorModel.Interfaces
{
    public interface ILoan
    {
        public string Name { get; set; }
        public decimal InterestRate { get; set; }
        public decimal PrincipalBalance { get; set; }
        public decimal AccruedInterest { get; set; }
        public decimal MinimumMonthlyPayment { get; set; }

        public decimal DailyInterest { get; }
        public LoanState State { get; }

        public decimal GetAccruedInterest(int days);
    }
}
