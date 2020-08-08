using Amortizor.Interfaces;

namespace Amortizor.Models
{
    public class DebtDecisionModel : IDebtDecisionModel
    {
        public string LoanName { get; set; }
        public decimal CurrentPayment { get; set; }
        public decimal CurrentPrincipal { get; set; }
    }
}
