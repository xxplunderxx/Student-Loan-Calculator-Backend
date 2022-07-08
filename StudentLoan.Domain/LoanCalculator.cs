using StudentLoan.Domain.Data;
using StudentLoan.Domain.Models;

namespace StudentLoan.Domain
{
    public class LoanCalculator : ILoanCalculator
    {
 
        /*
         * This function returns the minimum payment required for a given loan
         * in order to not incur additional fees.
         * 
         * 
         * @PARAM loanAmount is the total outstanding balance of the loan
         * @PARAM rate is the rate at which the loan balance appreciates
         * @PARAM years is the total number of years over which the loan is paid
         * 
         * @RETURN returns a double representing the minimum loan payment per month
         */
        public double minLoanPayment(double loanAmount, double rate, int years)
        {
            double months = years * 12;
            double monthlyRate = rate / 12;

            double numerator = loanAmount * monthlyRate * Math.Pow((1 + monthlyRate),months);
            double denominator = Math.Pow((1 + monthlyRate), months) - 1;

            return numerator / denominator;
        }

        /*
         * This function returns the zero balance date used to determine when student loan payments
         * will be over. This number is calculated in months and it will be called by the netWorth method.
         * 
         * @PARAM loanAmount is the total outstanding balance of the loan
         * @PARAM monthlyPayment is the amount being paid into the loan in a given month
         * @PARAM interestRate is the rate at which the loan balance appreciates
         * 
         * @RETURN returns a double representing the zeroBalance date (months)
         */
        public double zeroBalanceDate(double loanAmount, double monthlyPayment, double interestRate)
        {
            // define variables
            double monthlyInterest = interestRate / 12;

            // calculate numerator first
            double numerator = -(Math.Log10(1 - ((loanAmount * monthlyInterest) / monthlyPayment)));

            // calculate denomenator second
            double denominator = Math.Log10(1 + monthlyInterest);

            // calc final answer
            return numerator / denominator;
        }

        /*
         * This figures out the users networth which will be used to figure out
         * the optimal solution in paying student loans vs investing
         * 
         * @PARAM investments the total value of user's investment accounts (stocks)
         * @PARAM assets the amount of investments somone has (!stocks)
         * @PARAM monthlyContribution the amount the user invests in their assets per month
         * @PARAM monthlyExpenses the amount of money somone spends on living per month (! investing or debt payment)
         * @PARAM debt the amount of student debt somone has
         * @PARAM years the amount of time we want to look ahead for the netWorth calculation
         * @PARAM monthlyDebtPayment the amount paid towards student loans in a given month
         * 
         * @RETURN returns an OutputModel object that holds two arrays to store calculations
         */
        public OutputModel netWorth(double investments, double assets, double monthlyInvestmentContribution, double debt, double monthlyDebtPayment, int years, string username)
        {
            // connect to user colleciton
            UserModel user = new UserModel("", 0, 0, 0, 0, 0,0);
            user = user.getUser("user", username);

            // define variables
            double investmentGrowth = user.investmentGrowth;
            double discretionaryIncome = user.discretionaryIncome;
            double inflationDepreciation = user.inflationDepreciation;
            double debtAppreciation = user.debtAppreciation;
            double yearlyInvestmentContribution = monthlyInvestmentContribution * 12;

            // error checking (left out of final demo, so it dosn't mess anyhting up)
            // make sure monthlyInvestmentContribution + monthlyDebtPayment - dicretionaryIncome
            /*if (discretionaryIncome != monthlyInvestmentContribution + monthlyInvestmentContribution)
            {
                monthlyDebtPayment = discretionaryIncome - monthlyInvestmentContribution;
            }*/

            // declare return object
            OutputModel output = new OutputModel(years);

            // initial calculations
            double zeroBalance = zeroBalanceDate(debt, monthlyDebtPayment, debtAppreciation);   //consistent in all years
            // year 1
            output.netWorthArray[0] = (int)((assets + investments) - debt); // store year 1 net worth
            output.outstandingBalanceArray[0] = (int)debt;           // store year 1 outstanding balance
            // year 2
            output.outstandingBalanceArray[1] = (int)outstandingLoanBalance(debt, 2, debtAppreciation, zeroBalance);       // store year 2 balance of debt
            double year2Rate = 1 + investmentGrowth;    //used in next equation
            output.netWorthArray[1] = (int)((((investments + yearlyInvestmentContribution) * year2Rate) + assets - output.outstandingBalanceArray[1]) * inflationDepreciation);  // store year 2 netWorth

            // at least output first and second year
            // becuase they do not allign with year 3+ pattern
            if (years == 1 || years == 2)
            {
                return output;
            }
            else // years is 3 or more
            {
                for (int i = 3; i <= years; i++)
                {
                    // loop declarations
                    output.outstandingBalanceArray[i-1] = (int)outstandingLoanBalance(debt, i, debtAppreciation, zeroBalance);  // store outstanding balance
                    double finalRate = 1 + ((i - 1) * investmentGrowth);    // final rate is unique to year 3 +

                    // calculate interest for every year
                    double investmentInterest = 0; // zero investment Interest out, so prev vals don't carry over
                    for (int j = 1; j < i - 1; j++)
                    {
                        investmentInterest += (yearlyInvestmentContribution * (1 + (investmentGrowth * j)));    // add every year's money until run out of years
                    }

                    double netWorth = ((((investments + yearlyInvestmentContribution) * finalRate) + investmentInterest) + assets - output.outstandingBalanceArray[i - 1]) * inflationDepreciation;
                    output.netWorthArray[i-1] = (int)netWorth;   // store netWorth
                }
                return output;
            }
        }

        /*
        * This figures out the users outstanding balance remaining on their debt. This method will also
        * be called by the netWorth method.
        * 
        * @PARAM debt debt the amount of student debt somone has
        * @PARAM years the amount of time we want to look ahead for the netWorth calculation
        * @PARAM rate the rate at which the student debt appreciates
        * @PARAM zeroBalanceDate the number of months until the student debt is paid off
        * 
        * @RETURN returns a double representing the sutdent loan debt remaining
        */
        public double outstandingLoanBalance(double debt, int years, double rate, double zeroBalanceDate)
        {
            //variables
            double monthlyInterest = rate / 12;
            double months = (years - 1)*12;

            // make sure we don't go past loan due date
            if (months >= zeroBalanceDate)
            {
                return 0;
            }

            //numerator
            double numerator = (Math.Pow((1 + monthlyInterest), months) - 1);
            //denominator
            double denomenator = (Math.Pow((1 + monthlyInterest), zeroBalanceDate) - 1);

            //equation
            double equation = 1 - (numerator / denomenator);

            return debt * equation;
        }
    }
}