using AmortizorModel.Enums;
using System;

namespace AmortizorModel
{
    public readonly struct AmortizationModel
    {
        public AmortizationModel(decimal interestRate,
            decimal principalBalance,
            uint daysToCalculate,
            InterestType interestType,
            decimal payment)
        {
            InterestRate = interestRate;
            PrincipalBalance = principalBalance;
            DaysToCalculate = daysToCalculate;
            InterestType = interestType;
            Payment = payment;
        }

        public decimal AccruedInterest
        {
            get
            {
                return InterestType switch
                {
                    InterestType.Simple => DailyInterest * DaysToCalculate,
                    InterestType.Compound => throw new NotImplementedException(),
                    _ => throw new NotImplementedException(),
                };
            }
        }
        public decimal FinalBalance
        {
            get
            {
                return PrincipalBalance + AccruedInterest - Payment;
            }
        }

        private decimal DailyInterest => PrincipalBalance * InterestRate / DAYS_IN_YEAR;

        private const decimal DAYS_IN_YEAR = 365.25m;
        private readonly decimal InterestRate;
        private readonly decimal PrincipalBalance;
        private readonly uint DaysToCalculate;
        private readonly InterestType InterestType;
        private readonly decimal Payment;
    }
}
