using AmortizorModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AmortizorModelTests
{
    [TestClass]
    public class TestPerson
    {
        [TestMethod]
        public void Test_FreedomDate_NoExtraPayment()
        {
            var loans = new Loan[] {
                new Loan() {
                    AccruedInterest = 0,
                    PrincipalBalance = 25000,
                    InterestRate = 0.068m,
                    MinimumMonthlyPayment = 287.7m,
                    Name = "a"
                } };
            var startDate = new DateTime(2020, 1, 1);

            var model = new Person(loans, startDate, 0);

            Assert.AreEqual(startDate.AddMonths(120), model.FreedomDate);
        }

        [TestMethod]
        public void Test_FreedomDate_ExtraPayment()
        {
            var loans = new Loan[] {
                new Loan() {
                    AccruedInterest = 0,
                    PrincipalBalance = 25000,
                    InterestRate = 0.068m,
                    MinimumMonthlyPayment = 287.7m,
                    Name = "a"
                } };
            var startDate = new DateTime(2020, 1, 1);

            var model = new Person(loans, startDate, 25);

            Assert.AreEqual(startDate.AddMonths(107), model.FreedomDate);
        }

        [TestMethod]
        public void Test_FreedomDate_ExtraPaymentLoan()
        {
            var loans = new Loan[] {
                new Loan() {
                    AccruedInterest = 0,
                    PrincipalBalance = 200m,
                    InterestRate = 0.0m,
                    MinimumMonthlyPayment = 25m,
                    Name = "a"
                },
                new Loan() {
                    AccruedInterest = 0,
                    PrincipalBalance = 200m,
                    InterestRate = 0.0m,
                    MinimumMonthlyPayment = 25m,
                    Name = "b"
                }
            };
            var startDate = new DateTime(2020, 1, 1);

            var model = new Person(loans, startDate, 25);

            Assert.AreEqual(startDate.AddMonths(6), model.FreedomDate);
        }

        [TestMethod]
        public void Test_FreedomDate_ExtraPaymentLoan_MinimumPaymentRollover()
        {
            var loans = new Loan[] {
                new Loan() {
                    AccruedInterest = 0,
                    PrincipalBalance = 275m,
                    InterestRate = 0.0m,
                    MinimumMonthlyPayment = 25m,
                    Name = "a"
                },
                new Loan() {
                    AccruedInterest = 0,
                    PrincipalBalance = 100m,
                    InterestRate = 0.0m,
                    MinimumMonthlyPayment = 25m,
                    Name = "b"
                }
            };
            var startDate = new DateTime(2020, 1, 1);

            var model = new Person(loans, startDate, 25);

            Assert.AreEqual(startDate.AddMonths(5), model.FreedomDate);
        }

        [TestMethod]
        public void Test_FreedomDate_RolloverPayment()
        {
            var loans = new Loan[] {
                new Loan() {
                    AccruedInterest = 0,
                    PrincipalBalance = 50,
                    InterestRate = 0.0m,
                    MinimumMonthlyPayment = 25m,
                    Name = "a"
                },
                new Loan() {
                    AccruedInterest = 0,
                    PrincipalBalance = 25m,
                    InterestRate = 0.0m,
                    MinimumMonthlyPayment = 25m,
                    Name = "b"
                }
            };
            var startDate = new DateTime(2020, 1, 1);

            var model = new Person(loans, startDate, 25);

            Assert.AreEqual(startDate.AddMonths(1), model.FreedomDate);
        }
    }
}
