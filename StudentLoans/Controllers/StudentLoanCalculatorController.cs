using Microsoft.AspNetCore.Mvc;
using StudentLoan.Domain;
using StudentLoan.Domain.Models;

namespace StudentLoans.API.Controllers
{
    [Route("[controller]")]
    public class StudentLoanCalculatorController : Controller
    {
        private LoanCalculator calculator;

        // endpoints for caclulation based api calls
        public StudentLoanCalculatorController ()
        {
            calculator = new LoanCalculator ();
        }

        public string Index()
        {
            return "From Loan Calculator Controller Index Method";
        }

        [HttpGet("minLoanPayment")]
        public double MinLoanPayment(double loanAmount, double rate, int years)
        {
            return calculator.minLoanPayment(loanAmount, rate, years);
        }

        [HttpGet("zeroBalanceDate")]
        public double ZeroBalanceDate(double loanAmount, double monthlyPayment, float interestRate)
        {
            return calculator.zeroBalanceDate(loanAmount, monthlyPayment, interestRate);
        }

        [HttpGet("netWorth")]
        public OutputModel NetWorth(double investments, double assets, double monthlyInvestmentContribution, double debt, double monthlyDebtPayment, int years, string username)
        {
            return calculator.netWorth(investments, assets, monthlyInvestmentContribution, debt, monthlyDebtPayment, years, username);
        }
    }
}
