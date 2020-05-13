using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AmortizorModel;
using AmortizorModel.Enums;

namespace AmortizorModelTests
{
    //TODO: Delete this once I am done fully fleshing out Person
    [TestClass]
    public class TestAmortizationModel
    {
        [TestMethod]
        public void Test_AccruedInterest()
        {
            var yearlyInterestRate = 0.045m;
            var initialLoanAmount = 20000m;
            uint days = 30;
            var rateType = InterestType.Simple;
            var payment = 0;

            var model = new AmortizationModel(yearlyInterestRate, initialLoanAmount, days, rateType, payment);

            Assert.AreEqual(73.92m, Math.Round(model.AccruedInterest, 2));
        }

        [TestMethod]
        public void Test_FinalBalance()
        {
            var yearlyInterestRate = 0.045m;
            var initialLoanAmount = 20000m;
            uint days = 30;
            var rateType = InterestType.Simple;
            var payment = 100;

            var model = new AmortizationModel(yearlyInterestRate, initialLoanAmount, days, rateType, payment);

            Assert.AreEqual(19973.92m, Math.Round(model.FinalBalance, 2));
        }
    }
}
