using System;

namespace AmortizorModel
{
    public class AmortizationModel
    {
        public AmortizationModel(decimal interestRate,
            decimal principalBalance,
            uint daysToCalculate,
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

        private const decimal DAYS_IN_YEAR = 365.25m;
        private readonly decimal InterestRate;
        private readonly decimal PrincipalBalance;
        private readonly uint DaysToCalculate;
        private readonly InterestType InterestType;
    }

    public enum InterestType
    {
        Simple = 0,
        Compound = 1
    }
}
