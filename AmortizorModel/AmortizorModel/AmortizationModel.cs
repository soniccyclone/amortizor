using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace AmortizorModel
{
    public class AmortizationModel
    {
        private const decimal DAYS_IN_YEAR = 365.25m;
        private decimal InterestRate;
        private decimal PrincipalBalance;
        private int DaysToCalculate;
        private InterestType InterestType;

        public AmortizationModel(decimal interestRate,
            decimal principalBalance,
            int daysToCalculate,
            InterestType interestType)
        {
            this.InterestRate = interestRate;
            this.PrincipalBalance = principalBalance;
            this.DaysToCalculate = daysToCalculate;
            this.InterestType = interestType;
        }

        public (decimal NewBalance, decimal AccruedInterest) CalculateInterest()
        {
            switch (InterestType)
            {
                case InterestType.Simple:
                    var accruedInterest = PrincipalBalance * InterestRate / DAYS_IN_YEAR * DaysToCalculate;
                    return (accruedInterest + PrincipalBalance, accruedInterest);
                case InterestType.Compound:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
    }

    public enum InterestType
    {
        Simple = 0,
        Compound = 1
    }
}
