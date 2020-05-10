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
            var newLoanInfo = model.CalculateInterest();

            Assert.AreEqual(20073.92m, Math.Round(newLoanInfo.NewBalance, 2));
            Assert.AreEqual(73.92m, Math.Round(newLoanInfo.AccruedInterest, 2));
        }
    }
}
