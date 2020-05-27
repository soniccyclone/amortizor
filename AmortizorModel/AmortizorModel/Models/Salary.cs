using System;

namespace AmortizorModel.Models
{
    public class Salary
    {
        public decimal AnnualAmount { get; set; }
        public int AnnualRaiseMonth { get; set; }
        public decimal AnnualRaisePercent { get; set; }
        public decimal PercentOfRaiseForRepayment { get; set; }
    }
}
