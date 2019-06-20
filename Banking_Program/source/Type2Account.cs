//////////////////////////////////////////////////////////////////////////////////////
////    An interactive C# program that manages the use of customers and accounts  ////
////    for a bank which demonstrates the use of abstraction, encapsulation,      ////
////    inheritance and polymorphism. Written by Corey Henderson.                 ////
////        Name: Corey Henderson                                                 ////
////        ID: 215131357                                                         ////
////        Class: Type2Account                                                   ////
//////////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Collections.Generic;

namespace Banking_Program
{
    class Type2Account : Account
    {
        // Class attributes
        private decimal _monthlyDeposit = 0;
        private static decimal _annualInterestRate = 3.00M;
        private static decimal _depoistInterestRate = 4.00M;

        // Public access methods
        public decimal MonthlyDeposit
        {
            get { return _monthlyDeposit; }
            set
            {
                if (value < 0)
                    throw new Exception("Monthly deposit cannot be negative!");
                _monthlyDeposit = value;
            }
        }
        public decimal AnnualInterestRate
        {
            get { return _annualInterestRate; }
        }

        // Custom constructor 1 - Invokes base constructor
        public Type2Account(Customer owner, DateTime openedDate, decimal balance = 0M) : base(owner, openedDate, balance)
        { }

        // Custom constructor 2 - Invokes constructor 1
        public Type2Account(Customer owner, decimal balance = 0M) : base(owner, DateTime.Now, balance)
        { }

        public Type2Account(StreamReader sr, List<Customer> customers) : base(sr, customers)
        { }

        // Transfer method for Type 2 Account
        public override void Transfer(Account account, decimal amount)
        {
            // Checks if active
            if (Active == true)
            {
                // Checks if the owners are the same
                if (account.Owner == this.Owner)
                {
                    // Checks if its not a type 2 account
                    if (account.GetType() == typeof(Type1Account))
                    {
                        // Checks if this account has enough funds
                        if (this.Balance >= amount)
                        {
                            this.Balance -= amount;
                            account.Balance += amount;
                            Console.WriteLine();
                        }
                        else
                            throw new Exception("Transfer amount exceeded balance!");
                    }
                    else
                        throw new Exception("Cannot transfer to a Type 2 Account!");
                }
                else
                    throw new Exception("Cannot transfer to another account of a different customer!");
            }
            else
                throw new Exception("Account isn't active!");
        }

        // Calculate interest method for Type 2 Account
        public override decimal CalculateInterest()
        {
            // Check if the account is active
            if (!Active)
                throw new Exception("Account must be active to calculate the interest");

            // DateTime variable for the 1st day of the current month
            DateTime startofMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            TimeSpan span;

            // Checks if opened date is before start of the current month
            if (DateTime.Compare(OpenedDate, startofMonth) < 0)
                span = DateTime.Now.Subtract(startofMonth);
            else
                span = DateTime.Now.Subtract(OpenedDate);

            return (_annualInterestRate / 365 / 100) * span.Days * Balance + (_depoistInterestRate / 365 / 100) * span.Days * _monthlyDeposit;
        }

        // Overridden Update Balance method
        public override void UpdateBalance()
        {
            Balance += CalculateInterest();
            MonthlyDeposit = 0;
        }
    }
}
