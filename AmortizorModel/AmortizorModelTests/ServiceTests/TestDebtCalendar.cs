using Amortizor.Interfaces;
using Amortizor.Models;
using Amortizor.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace AmortizorTests
{
    [TestClass]
    public class TestDebtCalendar
    {
        //TODO: Make sure this is actually a robust test suite (need a more direct test for interest rate?)
        [TestMethod]
        public void Test_GenerateDebtRepaymentPlan_NoExtraPayment()
        {
            var loans = new Loan[] {
                new Loan() {
                    AccruedInterest = 0,
                    PrincipalBalance = 25000,
                    InterestRate = 0.068m,
                    MinimumMonthlyPayment = 287.7m,
                    Name = "a"
                }
            };
            var startDate = new DateTime(2020, 1, 1);
            var salary = new Salary();

            var model = new Person(loans, 0, salary);
            var service = new DebtCalendar(model);
            var result = service.GenerateDebtRepaymentPlan(startDate);

            Assert.AreEqual(startDate.AddMonths(119), result.Last().Month);
        }

        [TestMethod]
        public void Test_GenerateDebtRepaymentPlan_ExtraPayment()
        {
            var loans = new Loan[] {
                new Loan() {
                    AccruedInterest = 0,
                    PrincipalBalance = 25000,
                    InterestRate = 0.068m,
                    MinimumMonthlyPayment = 287.7m,
                    Name = "a"
                }
            };
            var startDate = new DateTime(2020, 1, 1);
            var salary = new Salary();

            var model = new Person(loans, 25, salary);
            var service = new DebtCalendar(model);
            var result = service.GenerateDebtRepaymentPlan(startDate);

            Assert.AreEqual(startDate.AddMonths(106), result.Last().Month);
        }

        [TestMethod]
        public void Test_GenerateDebtRepaymentPlan_ExtraPaymentLoan()
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
            var salary = new Salary();

            var model = new Person(loans, 25, salary);
            var service = new DebtCalendar(model);
            var result = service.GenerateDebtRepaymentPlan(startDate);

            Assert.AreEqual(startDate.AddMonths(5), result.Last().Month);
        }

        [TestMethod]
        public void Test_GenerateDebtRepaymentPlan_ExtraPaymentLoan_MinimumPaymentRollover()
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
            var salary = new Salary();

            var model = new Person(loans, 25, salary);
            var service = new DebtCalendar(model);
            var result = service.GenerateDebtRepaymentPlan(startDate);

            Assert.AreEqual(startDate.AddMonths(4), result.Last().Month);
        }

        [TestMethod]
        public void Test_GenerateDebtRepaymentPlan_ProcessSurplusLoanPayment()
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
            var salary = new Salary();

            var model = new Person(loans, 25, salary);
            var service = new DebtCalendar(model);
            var result = service.GenerateDebtRepaymentPlan(startDate);

            Assert.AreEqual(startDate, result.Last().Month);
        }

        [TestMethod]
        public void Test_GenerateDebtRepaymentPlan_ApplyRaise()
        {
            var loans = new Loan[] {
                new Loan() {
                    AccruedInterest = 0,
                    PrincipalBalance = 300,
                    InterestRate = 0.0m,
                    MinimumMonthlyPayment = 25m,
                    Name = "a"
                }
            };

            var startDate = new DateTime(2020, 1, 1);

            var salary = new Salary()
            {
                AnnualAmount = 100m,
                AnnualRaiseMonth = 4,
                AnnualRaisePercent = 0.25m,
                PercentOfRaiseForRepayment = 1m
            };

            var model = new Person(loans, 25, salary);
            var service = new DebtCalendar(model);
            var result = service.GenerateDebtRepaymentPlan(startDate);

            Assert.AreEqual(startDate.AddMonths(5), result.Last().Month);
        }

        [TestMethod]
        public void Test_GenerateDebtRepaymentPlan_Decisions()
        {
            var loans = new Loan[] {
                new Loan() {
                    AccruedInterest = 0,
                    PrincipalBalance = 100,
                    InterestRate = 0.0m,
                    MinimumMonthlyPayment = 25m,
                    Name = "a"
                },
                new Loan() {
                    AccruedInterest = 0,
                    PrincipalBalance = 100,
                    InterestRate = 0.0m,
                    MinimumMonthlyPayment = 25m,
                    Name = "b"
                }
            };

            var startDate = new DateTime(2020, 1, 1);

            var salary = new Salary();

            var model = new Person(loans, 25, salary);
            var service = new DebtCalendar(model);
            var result = service.GenerateDebtRepaymentPlan(startDate);

            Assert.AreEqual(3, result.Count);

            Assert.AreEqual(startDate, result[0].Month);
            Assert.AreEqual(2, result[0].Decisions.Count);
            AssertDecision(result[0].Decisions[0], "a", 100, 50);
            AssertDecision(result[0].Decisions[1], "b", 100, 25);

            Assert.AreEqual(startDate.AddMonths(1), result[1].Month);
            Assert.AreEqual(2, result[1].Decisions.Count);
            AssertDecision(result[1].Decisions[0], "a", 50, 50);
            AssertDecision(result[1].Decisions[1], "b", 75, 25);

            Assert.AreEqual(startDate.AddMonths(2), result[2].Month);
            Assert.AreEqual(1, result[2].Decisions.Count);
            AssertDecision(result[2].Decisions[0], "b", 50, 75);
        }

        private void AssertDecision(IDebtDecisionModel decision, string loanName, decimal principal, decimal payment)
        {
            Assert.AreEqual(loanName, decision.LoanName);
            Assert.AreEqual(principal, decision.CurrentPrincipal);
            Assert.AreEqual(payment, decision.CurrentPayment);
        }
    }
}
