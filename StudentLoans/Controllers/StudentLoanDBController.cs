using Microsoft.AspNetCore.Mvc;
using StudentLoan.Domain.Models;

namespace StudentLoans.API.Controllers
{
    [Route("[controller]")]
    public class StudentLoanDBController : Controller
    {
        private UserModel user;

        // endpoints for database related api calls
        public StudentLoanDBController()
        {
            user = new UserModel("",0,0,0,0,0,0);
        }

        [HttpGet("newUser")]
        public void NewUser(string username, double investments, double assets, int monthlyInvestmentContribution, double debt, double monthlyDebtPayment, double discretionaryIncome)
        {
            user.newUser(username, investments, assets, monthlyInvestmentContribution, debt, monthlyDebtPayment, discretionaryIncome);
        }

        [HttpGet("getUser")]
        public UserModel GetUser(string collection, string username)
        {
            return user.getUser(collection, username);
        }

        [HttpGet("updateUser")]
        public void UpdateUser(string collection, string username, string arguement, double value)
        {
            user.updateUser<UserModel>(collection, username, arguement, value); 
        }

        [HttpGet("deleteUser")]
        public void DeleteRecord(string collection, string username)
        {
            user.deleteUser<UserModel>(collection, username);
        }
    }
}
