//////////////////////////////////////////////////////////////////////////////////////
////    An interactive C# program that manages the use of customers and accounts  ////
////    for a bank which demonstrates the use of abstraction, encapsulation,      ////
////    inheritance and polymorphism. Written by Corey Henderson.                 ////
////        Name: Corey Henderson                                                 ////
////        ID: 215131357                                                         ////
////        Class: Type1Account                                                   ////
//////////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Collections.Generic;

namespace Banking_Program
{
    class Type1Account : Account
    {
        // Attributes and public access methods
        private static decimal _annualInterestRate = 2.00M;

        public decimal InterestRate
        {
            get { return _annualInterestRate; }
        }

        // Custom constructor 1 - Invokes base contructor
        public Type1Account(Customer owner, DateTime openedDate, decimal balance = 0M) : base(owner, openedDate, balance)
        { }

        // Custom constructor 2 - Invokes base constructor
        public Type1Account(Customer owner, decimal balance = 0M) : base(owner, DateTime.Now, balance)
        { }

        public Type1Account(StreamReader sr, List<Customer> customers) : base(sr, customers)
        { }

        // Deposit method
        public void Deposit(decimal amount)
        {
            if (Active == true)
            {
                if (amount > 0)
                {
                    Balance += amount;
                    Console.WriteLine();
                }
                else
                    throw new Exception("Amount must be a positive number!");
            }
            else
                throw new Exception("Account must be active to deposit!");
        }

        // Withdraw method
        public void Withdraw(decimal amount)
        {
            if (Active == true)
            {
                if (amount > 0)
                {
                    if (amount <= Balance)
                    {
                        Balance -= amount;
                        Console.WriteLine();
                    }
                    else
                        throw new Exception("Withdraw amount exceeded balance!");
                }
                else
                    throw new Exception("Amount must be a positive number!");
            }
            else
                throw new Exception("Account must be active to withdraw!");
        }

        // Transfer method for Type 1 Account
        public override void Transfer(Account account, decimal amount)
        {
            // Checks if its active
            if (Active == true)
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
                throw new Exception("Account isn't active!");
        }

        // Calculate interest method for Type 1 Account
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

            return (_annualInterestRate / 365 / 100) * span.Days * Balance;
        }
    }
}
