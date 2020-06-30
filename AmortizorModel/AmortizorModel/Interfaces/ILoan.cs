using AmortizorModel.Enums;

namespace AmortizorModel.Interfaces
{
    public interface ILoan
    {
        string Name { get; set; }
        decimal InterestRate { get; set; }
        decimal PrincipalBalance { get; set; }
        decimal AccruedInterest { get; set; }
        decimal MinimumMonthlyPayment { get; set; }

        decimal DailyInterest { get; }
        LoanState State { get; }

        decimal GetAccruedInterest(int days);
    }
}
