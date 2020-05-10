using System;

namespace AmortizorModel
{
    public class AmortizationModel
    {
        private const decimal DAYS_IN_YEAR = 365.25m;
        private readonly decimal InterestRate;
        private readonly decimal PrincipalBalance;
        private readonly int DaysToCalculate;
        private readonly InterestType InterestType;

        public AmortizationModel(decimal interestRate,
            decimal principalBalance,
            int daysToCalculate,
            InterestType interestType)
        {
            InterestRate = interestRate;
            PrincipalBalance = principalBalance;
            DaysToCalculate = daysToCalculate;
            InterestType = interestType;
        }

        public decimal AccruedInterest
        {
            get
            {
                return InterestType switch
                {
                    InterestType.Simple => PrincipalBalance * InterestRate / DAYS_IN_YEAR * DaysToCalculate,
                    InterestType.Compound => throw new NotImplementedException(),
                    _ => throw new NotImplementedException(),
                };
            }
        }
    }

    public enum InterestType
    {
        Simple = 0,
        Compound = 1
    }
}
