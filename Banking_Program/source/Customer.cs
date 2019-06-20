//////////////////////////////////////////////////////////////////////////////////////
////    An interactive C# program that manages the use of customers and accounts  ////
////    for a bank which demonstrates the use of abstraction, encapsulation,      ////
////    inheritance and polymorphism. Written by Corey Henderson.                 ////
////        Name: Corey Henderson                                                 ////
////        ID: 215131357                                                         ////
////        Class: Customer                                                       ////
//////////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Banking_Program
{
    class Customer
    {
        // Class Attributes
        private static uint _instanceCount;
        private readonly uint _id;

        private string _firstName;
        private string _lastName;
        private string _address;
        private DateTime _dateOfBirth;
        private string _contactNumber;
        private string _email;
        private List<Account> _accounts;

        // Public access methods of variables with validation
        //-- ID --\\
        public uint ID
        {
            get { return _id; }
        }
        //-- First Name --\\
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new Exception("First name cannot be empty!");

                foreach (char c in value)
                {
                    if (char.IsDigit(c))
                        throw new Exception("You cannot have a number in you're first name!");
                    if (char.IsLetter(c) == false)
                        throw new Exception("You can only include letters for you're first name!");
                }

                _firstName = value;
            }
        }
        //-- Last Name --\\
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new Exception("Last name cannot be empty!");

                foreach (char c in value)
                {
                    if (char.IsDigit(c))
                        throw new Exception("You cannot have a number in you're last name!");
                    if (char.IsLetter(c) == false)
                        throw new Exception("You can only include letters for your last name!");
                }

                _lastName = value;
            }
        }
        //-- Address --\\
        public string Address
        {
            get { return _address; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new Exception("Address cannot be empty!");

                _address = value;
            }
        }
        //-- Date Of Birth --\\
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                if (string.IsNullOrEmpty(value.ToString()))
                    throw new Exception("Date of birth requires input!");

                if (value.Year > (DateTime.Now.Year - 16))
                    throw new Exception("Date of birth is too young to own an account!");

                _dateOfBirth = value;
            }
        }
        //-- Number --\\
        public string ContactNumber
        {
            get { return _contactNumber; }
            set
            {
                foreach (char c in value)
                    if (string.IsNullOrEmpty(value) == false && char.IsDigit(c) == false)
                        throw new Exception("Contact number must only contain numbers!");

                if (string.IsNullOrEmpty(value) == false && value.Length != 10)
                    throw new Exception("Contact number must be 10 numbers long!");

                _contactNumber = value;
            }
        }
        //-- Email --\\
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        //-- Account List --\\
        public ReadOnlyCollection<Account> Accounts
        {
            get { return _accounts.AsReadOnly(); }
        }

        // Custom Constructor
        public Customer(string firstname, string lastname, string address, DateTime dob, string number = "", string email = "")
        {
            _id = IncrementID();
            FirstName = firstname;
            LastName = lastname;
            Address = address;
            DateOfBirth = dob;
            ContactNumber = number;
            Email = email;
            _accounts = new List<Account>();
        }

        // Copy constructor
        public Customer(Customer source) : this(source._firstName,
                                                source._lastName,
                                                source._address,
                                                source._dateOfBirth,
                                                source._contactNumber,
                                                source._email)
        {
            _id = source.ID;
            _accounts = source._accounts;
        }

        public Customer(StreamReader sr)
        {
            _id = uint.Parse(sr.ReadLine());
            _instanceCount++;

            FirstName = sr.ReadLine();
            LastName = sr.ReadLine();
            Address = sr.ReadLine();
            string[] dobEntries = sr.ReadLine().Split('/');
            DateOfBirth = new DateTime(int.Parse(dobEntries[2]), int.Parse(dobEntries[1]), int.Parse(dobEntries[0]));
            ContactNumber = sr.ReadLine();
            Email = sr.ReadLine();
            _accounts = new List<Account>();
        }

        // Add Method for the list of accounts
        public void Add(Account a)
        {
            _accounts.Add(a);
        }

        // Sum Balance: total balance of all accounts
        public decimal SumBalance()
        {
            decimal totalBalance = 0M;
            foreach (Account a in _accounts)
            {
                totalBalance += a.Balance;
            }
            return totalBalance;
        }
        // ToString overide method outputting customer details
        public override string ToString()
        {
            return string.Format("ID: {0,-5} Name: {1,-7} {2,-9} Address: {3,-14} DOB: {4,-12} Contact: {5,-12} Email: {6,-18} Total Balance: ${7,-8:F1}", _id, _firstName, _lastName, _address, _dateOfBirth.ToString("d"), _contactNumber, _email, SumBalance());
        }

        // ID Auto incrementer for class constructor
        private static uint IncrementID()
        {
            _instanceCount++;
            return _instanceCount;
        }
    }
}
