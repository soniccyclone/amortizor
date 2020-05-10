using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AmortizorModel;

namespace AmortizorModelTests
{
    [TestClass]
    public class TestAmortizationModel
    {
        [TestMethod]
        public void Test_StudentLoan()
        {
            var yearlyInterestRate = 0.045m;
            var initialLoanAmount = 20000m;
            var daysToCalculate = 30;
            var rateType = InterestType.Simple;

            var model = new AmortizationModel(yearlyInterestRate, initialLoanAmount, daysToCalculate, rateType);

            Assert.AreEqual(73.92m, Math.Round(model.AccruedInterest, 2));
        }
    }
}
