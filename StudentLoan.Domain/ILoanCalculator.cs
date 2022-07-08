using StudentLoan.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentLoan.Domain
{
    public interface ILoanCalculator
    {
        public double minLoanPayment(double loanAmount, double rate, int years);
        public OutputModel netWorth(double investments, double assets, double monthlyInvestmentContribution, double debt, double monthlyDebtPayment, int years, string username);
        public double outstandingLoanBalance(double debt, int years, double rate, double zeroBalanceDate);
        public double zeroBalanceDate(double loanAmount, double monthlyPayment, double interestRate);

    }
}
