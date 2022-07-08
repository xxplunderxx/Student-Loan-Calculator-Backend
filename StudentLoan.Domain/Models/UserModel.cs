using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using StudentLoan.Domain.Data;

namespace StudentLoan.Domain.Models
{
    /*
    * This class definition is used to store user data in the database
    */
    public class UserModel
    {
        public Guid Id { get; set; }
        public string username { get; set; }
        public double investments { get; set; }
        public double assets { get; set; }
        public double monthlyInvestmentContribution { get; set; }
        public double debt { get; set; }
        public double monthlyDebtPayment { get; set; }
        public double discretionaryIncome { get; set; }
        // rates
        public double investmentGrowth { get; set; }
        public double inflationDepreciation { get; set; }
        public double debtAppreciation { get; set; }

        public UserModel(string username, double investments, double assets, double monthlyInvestmentContribution, double debt, double monthlyDebtPayment, double discretionaryIncome)
        {
            // information specified before a user is added
            this.username = username;
            this.investments = investments;
            this.assets = assets;
            this.monthlyInvestmentContribution = monthlyInvestmentContribution;
            this.debt = debt;
            this.monthlyDebtPayment = monthlyDebtPayment;
            this.discretionaryIncome = discretionaryIncome;

            //default rates
            investmentGrowth = .10; 
            inflationDepreciation = .965; 
            debtAppreciation = .08;   
        }   

        /*
         * This method is used to connect other methods to the the database
         * 
         * @RETURN returns a MongoCRUD object that represents the connection
         */
        public MongoCRUD UserConnection()
        {
            return new MongoCRUD("studentLoanCalculator");
        }

        /*
         * This method creates a new user inside the database it takes several params
         * that are used as things to store. It also stores 3 default interest rates as
         * a result of the User class definition.
         * 
         * @PARAM username stores a unqique ID for every user
         * @PARAM investments holds assets specifically in the stock market
         * @PARAM assets holds other assets not in the stock market
         * @PARAM monthlyInvestmentContribution holds the amount the user 
         * contributes to their investment accounts monthly
         * @PARAM debt holds the total amount of student loan debt user has
         * @PARAM monthlyDebtPayment holds the amount the user contributes
         * to their debt monthly
         */
        public void newUser(string username, double investments, double assets, int monthlyInvestmentContribution, double debt, double monthlyDebtPayment, double discretionaryIncome)
        {
            // connection
            MongoCRUD db = UserConnection();

            // collection name
            string collection = "user";

            // insert new user into DB
            db.InsertRecord(collection, new UserModel(username, investments, assets, monthlyInvestmentContribution, debt, monthlyDebtPayment,discretionaryIncome));
        }

        /*
         * This method is used to gather an existing user's information
         * 
         * @PARAM collection specifies whcih collectino to conenct to
         * @PARAM username specifies which user's data should be collected
         * 
         * @RETURN returns a object that holds the user's information
         */
        public UserModel getUser(string collection, string username)
        {
            // connection
            MongoCRUD db = UserConnection();

            var record = db.LoadRecordByusername<UserModel>(collection, username);

            return record;
        }

        /*
         * This method is used to update existing information inside
         * a specific user account.
         * 
         * @PARAM collection specifies whcih collectino to conenct to
         * @PARAM username specifies which user's data should be collected
         * @PARAM arguement specifiec which feild should be updated
         * @PARAM value sepcifies what the feild should contain after update
         */
        public void updateUser<UserModel>(string collection, string username, string arguement, double value)
        {
            // connection
            MongoCRUD db = UserConnection();

            var onerec = getUser(collection, username);

            switch (arguement)      // decides which field (arguement) to update via a query
            {
                case "investments":
                    onerec.investments = value;
                    break;
                case "assets":
                    onerec.assets = value;
                    break;
                case "monthlyInvestmentContribution":
                    onerec.monthlyInvestmentContribution = (int)value;
                    break;
                case "debt":
                    onerec.debt = value;
                    break;
                case "monthlyDebtPayment":
                    onerec.monthlyDebtPayment = (int)value;
                    break;
                case "investmentGrowth":
                    onerec.investmentGrowth = value;
                    break;
                case "inflationDepreciation":
                    onerec.inflationDepreciation = value;
                    break;
                case "debtAppreciation":
                    onerec.debtAppreciation = value;
                    break;
                default:
                    Console.WriteLine("invalid arguement selection");   //error message
                    break;
            }
            db.UpsertRecord(collection, username, onerec);      //update command
        }

        /*
         * This method is used to remove an exisitng user and 
         * all of their information
         * 
         * @PARAM collection specifies whcih collectino to conenct to
         * @PARAM username specifies which user's data should be collected
         */
        public void deleteUser<UserModel>(string collection, string username)
        {
            // connection
            MongoCRUD db = UserConnection();

            db.DeleteRecord<UserModel>(collection, username);
        }
    }
}
