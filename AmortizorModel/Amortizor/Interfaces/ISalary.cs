namespace Amortizor.Interfaces
{
    public interface ISalary
    {
        decimal AnnualAmount { get; set; }
        int AnnualRaiseMonth { get; set; }
        decimal AnnualRaisePercent { get; set; }
        decimal PercentOfRaiseForRepayment { get; set; }
    }
}
