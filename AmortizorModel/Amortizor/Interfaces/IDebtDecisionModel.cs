namespace Amortizor.Interfaces
{
    public interface IDebtDecisionModel
    {
        string LoanName { get; set; }
        decimal CurrentPayment { get; set; }
        decimal CurrentPrincipal { get; set; }
    }
}
