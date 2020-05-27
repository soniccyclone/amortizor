using AmortizorModel;
using AmortizorModel.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AmortizorModelTests
{
    [TestClass]
    public class TestLoan
    {
        [TestMethod]
        public void Test_DailyInterest()
        {
            var loan = new Loan()
            {
                InterestRate = 5m,
                PrincipalBalance = 146.1m
            };

            Assert.AreEqual(2, loan.DailyInterest);
        }

        [DataTestMethod]
        [DataRow(5d, LoanState.Active)]
        [DataRow(0d, LoanState.Paid)]
        public void Test_State(double principalBalance, LoanState state)
        {
            var loan = new Loan()
            {
                PrincipalBalance = (decimal)principalBalance
            };

            Assert.AreEqual(state, loan.State);
        }

        [TestMethod]
        public void Test_GetAccruedInterest()
        {
            var loan = new Loan()
            {
                InterestRate = 5m,
                PrincipalBalance = 146.1m
            };

            Assert.AreEqual(10, loan.GetAccruedInterest(5));
        }
    }
}
