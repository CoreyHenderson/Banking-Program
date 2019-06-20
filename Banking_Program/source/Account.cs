//////////////////////////////////////////////////////////////////////////////////////
////    An interactive C# program that manages the use of customers and accounts  ////
////    for a bank which demonstrates the use of abstraction, encapsulation,      ////
////    inheritance and polymorphism. Written by Corey Henderson.                 ////
////        Name: Corey Henderson                                                 ////
////        ID: 215131357                                                         ////
////        Class: Account                                                        ////
//////////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Collections.Generic;

namespace Banking_Program
{
    abstract class Account
    {
        // Class attributes
        private static uint _instanceCount;
        private readonly uint _id;

        protected DateTime _openedDate;
        protected DateTime _closedDate;
        protected bool _active;
        protected decimal _balance;
        protected Customer _owner;

        // Public access methods with validation
        //-- ID --\\
        public uint ID
        {
            get { return _id; }
        }
        //-- Opened Date --\\
        public DateTime OpenedDate
        {
            get { return _openedDate; }
            set
            {
                if (value > DateTime.Now)
                    throw new Exception("Opened Date must be today or a past date!");
                _openedDate = value;
            }
        }
        //-- Closed Date --\\
        public DateTime ClosedDate
        {
            get
            {
                if (_active)
                    throw new Exception("This account is still active");
                return _closedDate;
            }
        }
        //-- Active --\\
        public bool Active
        {
            get { return _active; }
        }
        //-- Balance --\\
        public decimal Balance
        {
            get { return _balance; }
            set
            {
                if (value < 0)
                    throw new Exception("Balance cannot be negative!");
                _balance = value;
            }
        }
        //-- Owner --\\
        public Customer Owner
        {
            get { return _owner; }
        }

        // Custom Constructor 1
        public Account(Customer owner, DateTime openedDate, decimal balance = 0M)
        {
            // ID increment and initialisation
            _id = IncrementID();

            OpenedDate = openedDate;
            Balance = balance;
            _owner = owner;
            _active = true;
            owner.Add(this);
        }

        // Custom Contructor 2 - invoked from constructor 1
        public Account(Customer owner, decimal balance = 0M) : this(owner, DateTime.Now, balance)
        {

        }

        // Custom Constructor 3 - Takes in a stream reader object and a list of customers
        public Account(StreamReader sr, List<Customer> Customers)
        {
            // ID
            _id = uint.Parse(sr.ReadLine());
            _instanceCount++;
            // Owner
            _active = true;
            uint ownerID = uint.Parse(sr.ReadLine());
            foreach (Customer c in Customers)
            {
                if (c.ID == ownerID)
                {
                    _owner = c;
                    c.Add(this);
                }
            }
            // Opened Date
            string[] dateEntries = sr.ReadLine().Split('/');
            OpenedDate = new DateTime(int.Parse(dateEntries[2]), int.Parse(dateEntries[1]), int.Parse(dateEntries[0]));
            // Closed Date
            string closedDate = sr.ReadLine();
            if (string.IsNullOrWhiteSpace(closedDate) == false || string.IsNullOrEmpty(closedDate) == false)
            {
                string[] closedDateEntries = closedDate.Split('/');
                _active = false;
                _closedDate = new DateTime(int.Parse(closedDateEntries[2]), int.Parse(closedDateEntries[1]), int.Parse(closedDateEntries[0]));
            }
            // Balance
            Balance = decimal.Parse(sr.ReadLine());
        }

        // Close method
        public void Close()
        {
            if (!_active)
                throw new Exception("Account is already closed!");

            _active = false;
            _closedDate = DateTime.Now;
        }

        // Transfer abstract declaration
        public abstract void Transfer(Account account, decimal amount);

        // Calculate Interest abstract declaration
        public abstract decimal CalculateInterest();

        //Update Balance method - Adds the interest to the total balance
        public virtual void UpdateBalance()
        {
            _balance += CalculateInterest();
        }

        // ToString() output override
        public override string ToString()
        {
            if (_active)
                return string.Format("ID: {0,-5} Opened Date: {1,-13} Balance: ${2,-10:F1} Owner: {3,-7} {4,-7}", _id, _openedDate.ToString("d"), _balance, _owner.FirstName, _owner.LastName);
            else
                return string.Format("ID: {0,-5} Opened Date: {1,-13} Balance: ${2,-10:F1} Owner: {3,-7} {4,-7}\t- Closed on {5}", _id, _openedDate.ToString("d"), _balance, _owner.FirstName, _owner.LastName, _closedDate.ToString("d"));
        }

        // ID Increment for class constructor
        protected static uint IncrementID()
        {
            _instanceCount++;
            return _instanceCount;
        }
    }
}
