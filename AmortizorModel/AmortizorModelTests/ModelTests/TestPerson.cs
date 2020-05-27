using AmortizorModel.Enums;
using AmortizorModel.Interfaces;
using AmortizorModel.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace AmortizorModelTests
{
    [TestClass]
    public class TestPerson
    {
        [TestMethod]
        public void Test_Constructor()
        {
            var loans = new List<ILoan>();
            var salary = new Salary();

            var model = new Person(loans, 10, salary);

            Assert.AreEqual(loans, model.Loans);
            Assert.AreEqual(10, model.InitialExtraLoanPayment);
            Assert.AreEqual(salary, model.Salary);
        }

        [TestMethod]
        public void Test_TotalDebt()
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
            var salary = new Salary();

            var model = new Person(loans, 0, salary);

            Assert.AreEqual(400m, model.TotalDebt);
        }

        [DataTestMethod]
        [DataRow(LoanState.Active, 1)]
        [DataRow(LoanState.Paid, 0)]
        public void Test_ApplicableLoans(LoanState loanState, int expectedLoanCount)
        {
            var loan = new Mock<ILoan>();
            loan.Setup(l => l.State).Returns(loanState);
            var salary = new Salary();

            var model = new Person(new ILoan[] { loan.Object }, 0, salary);

            Assert.AreEqual(expectedLoanCount, model.ApplicableLoans.Count);
        }

        [DataTestMethod]
        [DataRow(LoanState.Active, 0)]
        [DataRow(LoanState.Paid, 1)]
        public void Test_PaidLoans(LoanState loanState, int expectedLoanCount)
        {
            var loan = new Mock<ILoan>();
            loan.Setup(l => l.State).Returns(loanState);
            var salary = new Salary();

            var model = new Person(new ILoan[] { loan.Object }, 0, salary);

            Assert.AreEqual(expectedLoanCount, model.PaidLoans.Count);
        }

        [TestMethod]
        public void Test_ExtraPaymentLoan()
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
                },
                new Loan() {
                    AccruedInterest = 0,
                    PrincipalBalance = 100m,
                    InterestRate = 0.0m,
                    MinimumMonthlyPayment = 25m,
                    Name = "c"
                }
            };
            var salary = new Salary();

            var model = new Person(loans, 0, salary);

            Assert.AreEqual(loans[1], model.ExtraPaymentLoan);
        }

        [TestMethod]
        public void Test_ExtraLoanPayment()
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
                    PrincipalBalance = 0m,
                    InterestRate = 0.0m,
                    MinimumMonthlyPayment = 25m,
                    Name = "b"
                },
                new Loan() {
                    AccruedInterest = 0,
                    PrincipalBalance = 0m,
                    InterestRate = 0.0m,
                    MinimumMonthlyPayment = 25m,
                    Name = "c"
                }
            };
            var salary = new Salary();

            var model = new Person(loans, 25, salary);

            Assert.AreEqual(75, model.ExtraLoanPayment);
        }
    }
}
