using Amortizor.Interfaces;

namespace Amortizor.Models
{
    public class Salary : ISalary
    {
        public decimal AnnualAmount { get; set; }
        public int AnnualRaiseMonth { get; set; }
        public decimal AnnualRaisePercent { get; set; }
        public decimal PercentOfRaiseForRepayment { get; set; }
    }
}
