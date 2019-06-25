//////////////////////////////////////////////////////////////////////////////////////
////    An interactive C# program that manages the use of customers and accounts  ////
////    for a bank which demonstrates the use of abstraction, encapsulation,      ////
////    inheritance and polymorphism. Written by Corey Henderson.                 ////
////                                                                              ////
////        Class: Program                                                        ////
//////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;

namespace Banking_Program
{
    public enum State
    {
        MainMenu,
        CreateCustomer,
        SearchCustomer,
        CreateAccount,
        SearchAccount,
        Transfer,
        Desposit,
        Withdraw,
        MonthlyDeposit,
        Exit
    }
    public enum SearchState
    {
        firstNameInput,
        lastNameInput,
        addressInput,
        dateOfBirthInput,
        contactInput,
        emailInput,
        accountIDInput,
        ownerIDInput
    }

    class Program
    {
        // enum variable of State represents the programs state for the main() program loop
        static State menuState = State.MainMenu;
        // enum variable for the state of the search - used for detecting what type of input is being received in Display Search functions
        static SearchState searchState;
        // Heading menu line
        static string formattingLine = new string('_', 150);

        //---  Read File Function             ---\\
        public static void ReadFile(string fileName, List<Customer> CustomerList, List<Account> AccountList)
        {
            try
            {
                StreamReader sr = new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read));

                // Gets the amount of objects in the file
                int objectAmount = Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < objectAmount; i++)
                {
                    if (fileName == "customers.txt")
                    {
                        CustomerList.Add(new Customer(sr));
                    }
                    else if (fileName == "accounts.txt")
                    {
                        // Checks the type of account on the next line and creates the object based off that result
                        string accType = sr.ReadLine();
                        if (accType == "1")
                        {
                            AccountList.Add(new Type1Account(sr, CustomerList));
                        }
                        else if (accType == "2")
                        {
                            AccountList.Add(new Type2Account(sr, CustomerList));
                        }
                    }
                }

                sr.Close();
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }

        //---  Write File Function            ---\\
        public static void WriteFile(string fileName, List<Customer> CustomerList, List<Account> AccountList)
        {
            StreamWriter sw = new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write));

            if (fileName == "customers.txt")
            {
                // Write's the amount of customers
                sw.WriteLine(CustomerList.Count);
                foreach (Customer c in CustomerList)
                {
                    sw.WriteLine(c.ID);
                    sw.WriteLine(c.FirstName);
                    sw.WriteLine(c.LastName);
                    sw.WriteLine(c.Address);
                    sw.WriteLine(c.DateOfBirth.ToString("d"));
                    sw.WriteLine(c.ContactNumber);
                    sw.WriteLine(c.Email);
                }
            }
            else if (fileName == "accounts.txt")
            {
                // Write's the amount of accounts
                sw.WriteLine(AccountList.Count);

                foreach (Account a in AccountList)
                {
                    // Write the acount type
                    if (a.GetType() == typeof(Type1Account))
                        sw.WriteLine("1");
                    else
                        sw.WriteLine("2");
                    // Write ID
                    sw.WriteLine(a.ID);
                    // Write Owner ID
                    sw.WriteLine(a.Owner.ID);
                    // Wrrite Opened date
                    sw.WriteLine(a.OpenedDate.ToString("d"));
                    // Write Closed date if's its closed
                    if (a.Active == false)
                        sw.WriteLine(a.ClosedDate);
                    else
                        sw.WriteLine("");
                    // Write balance
                    sw.WriteLine(a.Balance);
                }
            }

            sw.Close();
        }

        //---  Main Menu                      ---\\
        static void DisplayMainMenu()
        {
            // Header
            Console.WriteLine(formattingLine);
            Console.WriteLine("\tInteractive Banking Program\t\t\t\t\t\t\t\t\tDesigned By Corey Henderson");
            Console.WriteLine(formattingLine);

            //Display Main menu
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("|    Main Menu                                           |");
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("| 1. Create a new customer                               |");
            Console.WriteLine("| 2. Search for a customer                               |");
            Console.WriteLine("| 3. Open a new account                                  |");
            Console.WriteLine("| 4. Search for an account                               |");
            Console.WriteLine("|                                                        |");
            Console.WriteLine("| 5. Transfer money                                      |");
            Console.WriteLine("| 6. Deposit                                             |");
            Console.WriteLine("| 7. Withdraw                                            |");
            Console.WriteLine("| 8. Set monthly deposit                                 |");
            Console.WriteLine("|                                                        |");
            Console.WriteLine("| Enter any other key to exit                            |");
            Console.WriteLine("----------------------------------------------------------");

            Console.Write("Selection: ");
            string menuSelection = Console.ReadLine();
            Console.Clear();
            switch (menuSelection)
            {
                case "1":
                    menuState = State.CreateCustomer;
                    break;
                case "2":
                    menuState = State.SearchCustomer;
                    break;
                case "3":
                    menuState = State.CreateAccount;
                    break;
                case "4":
                    menuState = State.SearchAccount;
                    break;
                case "5":
                    menuState = State.Transfer;
                    break;
                case "6":
                    menuState = State.Desposit;
                    break;
                case "7":
                    menuState = State.Withdraw;
                    break;
                case "8":
                    menuState = State.MonthlyDeposit;
                    break;
                default:
                    menuState = State.Exit;
                    break;
            }
        }

        //---  Create a customer menu         ---\\
        static void CreateCustomerMenu(List<Customer> CustomerList)
        {
            Console.WriteLine(formattingLine);
            Console.WriteLine("\tCreate A Customer\t\t\t\t\t\t\t\t\t\t\t\tDesigned By Corey Henderson");
            Console.WriteLine(formattingLine);

            Console.Write("\n\nEnter First Name: ");
            string firstNameInput = Console.ReadLine();

            Console.Write("Enter Last Name: ");
            string lastNameInput = Console.ReadLine();

            Console.Write("Enter Address: ");
            string addressInput = Console.ReadLine();

            Console.Write("Enter Date of Birth (DD/MM/YYYY): ");
            string[] inputArray = Console.ReadLine().Split('/');

            Console.Write("Enter Contact number: ");
            string contactInput = Console.ReadLine();

            Console.Write("Enter Email: ");
            string emailInput = Console.ReadLine();

            Console.WriteLine("\n\nPress \"s\" or \"S\" to submit");
            Console.WriteLine("Press any other key to cancel");

            try
            {
                var selection = Console.ReadKey();
                switch (selection.Key)
                {
                    case ConsoleKey.S:
                        // Checks whether a customer with that name, contact or email already exists
                        foreach (Customer c in CustomerList)
                        {
                            if (firstNameInput.ToLower() == c.FirstName.ToLower() && lastNameInput.ToLower() == c.LastName.ToLower())
                                throw new Exception("A customer with that name already exists!");
                        }

                        // Adds the user to the list
                        CustomerList.Add(new Customer(firstNameInput, lastNameInput, addressInput, new DateTime(int.Parse(inputArray[2]), int.Parse(inputArray[1]), int.Parse(inputArray[0])), contactInput, emailInput));

                        // Clear's the screen and prints out feedback result
                        Console.Clear();
                        Console.WriteLine(formattingLine);
                        Console.WriteLine("\tCreate A Customer\t\t\t\t\t\t\t\t\t\t\t\tDesigned By Corey Henderson");
                        Console.WriteLine(formattingLine);

                        Console.WriteLine("\nThank you {0}, you're Customer account has been successfully created.", firstNameInput);

                        Console.WriteLine("\n\nPress any keys to go back to Main menu");
                        Console.ReadKey();

                        menuState = State.MainMenu;
                        break;
                    default:
                        menuState = State.MainMenu;
                        break;
                }
            }
            // Exception catching
            catch (IndexOutOfRangeException re)
            {
                Console.Clear();
                Console.WriteLine(re.Message);
                Console.WriteLine("\nPlease ensure all entries are fully entered. e.g. 4/10/1995");
                Console.WriteLine("Please try again.");
                CreateCustomerMenu(CustomerList);
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Console.WriteLine("\nPlease try again.");
                CreateCustomerMenu(CustomerList);
            }
        }

        //---  Search Customer Menu           ---\\
        static string TruncateCustomerValue(string source, int length)
        {
            // This function converts the customer information to all lowercases and truncates it to the size of the search input
            string result = source.ToLower();

            if (result.Length > length)
            {
                result = result.Substring(0, length);
            }
            return result;
        }
        static void DisplayCustomerSearch(List<Customer> CustomerList, string input)
        {
            Console.WriteLine(formattingLine);
            Console.WriteLine("\tSearch A Customer\t\t\t\t\t\t\t\t\t\t\t\tDesigned By Corey Henderson");
            Console.WriteLine(formattingLine);

            Console.WriteLine("\nSearch results:");

            // An integer that increments if a matching customer is found for the purpose of printing a response (Line 326) if none is found
            int FoundACustomerCount = 0;

            foreach (Customer c in CustomerList)
            {
                // For this search method: The SearchState enum is for checking which input the user has entered from. (firstname/lastname/address etc.)
                //                         The TruncateCustomerValue() function converts the customer information to lowercase and truncates the string to the same length as the users input length.
                //                         The users input is also converted to all lowercase letters as well.
                //
                //                         In turn, this is an advanced search form that allows the user to enter a portion of the customers information no matter if its uppercase or lower case,
                //                         and still finds the desired customer even if its a single letter input from the user.
                if (input.ToLower() == TruncateCustomerValue(c.FirstName, input.Length) && searchState == SearchState.firstNameInput || input.ToLower() == TruncateCustomerValue(c.LastName, input.Length) && searchState == SearchState.lastNameInput || input.ToLower() == TruncateCustomerValue(c.Address, input.Length) && searchState == SearchState.addressInput || input.ToLower() == TruncateCustomerValue(c.DateOfBirth.ToString("d"), input.Length) && searchState == SearchState.dateOfBirthInput || input.ToLower() == TruncateCustomerValue(c.ContactNumber, input.Length) && searchState == SearchState.contactInput || input.ToLower() == TruncateCustomerValue(c.Email, input.Length) && searchState == SearchState.emailInput)
                {
                    FoundACustomerCount++;
                    Console.WriteLine(c);
                }
            }

            if (FoundACustomerCount == 0)
                Console.WriteLine("Sorry, no customers match that search.");

            Console.WriteLine("\n\nPress any keys to go back to Main menu");
            Console.ReadKey();
            menuState = State.MainMenu;
        }
        static void SearchCustomerMenu(List<Customer> CustomerList)
        {
            Console.WriteLine(formattingLine);
            Console.WriteLine("\tSearch A Customer\t\t\t\t\t\t\t\t\t\t\t\tDesigned By Corey Henderson");
            Console.WriteLine(formattingLine);
            Console.WriteLine("Please enter one of the following customer details:");

            Console.Write("\nEnter First Name: ");
            string firstNameInput = Console.ReadLine();

            Console.Write("Enter Last Name: ");
            string lastNameInput = Console.ReadLine();

            Console.Write("Enter Address: ");
            string addressInput = Console.ReadLine();

            Console.Write("Enter Date of Birth (DD/MM/YYYY): ");
            string dobInput = Console.ReadLine();

            Console.Write("Enter Contact number: ");
            string contactInput = Console.ReadLine();

            Console.Write("Enter Email: ");
            string emailInput = Console.ReadLine();

            Console.WriteLine("\n\nPress \"s\" or \"S\" to search");
            Console.WriteLine("Press any other key to cancel");

            try
            {
                var selection = Console.ReadKey();
                Console.Clear();
                switch (selection.Key)
                {
                    case ConsoleKey.S:
                        // Checks whether the user inputs have a value, if so, it displays the search based off the input
                        if (string.IsNullOrEmpty(firstNameInput) == false)
                        {
                            searchState = SearchState.firstNameInput;
                            DisplayCustomerSearch(CustomerList, firstNameInput);
                            break;
                        }
                        if (string.IsNullOrEmpty(lastNameInput) == false)
                        {
                            searchState = SearchState.lastNameInput;
                            DisplayCustomerSearch(CustomerList, lastNameInput);
                            break;
                        }
                        if (string.IsNullOrEmpty(addressInput) == false)
                        {
                            searchState = SearchState.addressInput;
                            DisplayCustomerSearch(CustomerList, addressInput);
                            break;
                        }
                        if (string.IsNullOrEmpty(dobInput) == false)
                        {
                            searchState = SearchState.dateOfBirthInput;
                            DisplayCustomerSearch(CustomerList, dobInput);
                            break;
                        }
                        if (string.IsNullOrEmpty(contactInput) == false)
                        {
                            searchState = SearchState.contactInput;
                            DisplayCustomerSearch(CustomerList, contactInput);
                            break;
                        }
                        if (string.IsNullOrEmpty(emailInput) == false)
                        {
                            searchState = SearchState.emailInput;
                            DisplayCustomerSearch(CustomerList, emailInput);
                            break;
                        }

                        throw new Exception("No information was entered!");
                    default:
                        menuState = State.MainMenu;
                        break;
                }
            }
            // Exception catching
            catch (IndexOutOfRangeException re)
            {
                Console.Clear();
                Console.WriteLine(re.Message);
                Console.WriteLine("\nPlease ensure at lease one of the entries are fully entered. e.g. 4/10/1995");
                Console.WriteLine("Please try again.");
                SearchCustomerMenu(CustomerList);
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Console.WriteLine("\nPlease try again.");
                SearchCustomerMenu(CustomerList);
            }
        }

        //---  Create Account Menu            ---\\
        static void CreateAccountMenu(List<Account> AccountList, List<Customer> CustomerList)
        {
            Console.WriteLine(formattingLine);
            Console.WriteLine("\tOpen An Account\t\t\t\t\t\t\t\t\t\t\t\t\tDesigned By Corey Henderson");
            Console.WriteLine(formattingLine);

            Console.Write("\n\nEnter Owner ID: ");
            string ownerIDInput = Console.ReadLine();

            Console.Write("Enter Account Type (1 or 2): ");
            string accountTypeInput = Console.ReadLine();

            Console.Write("Enter Initial Balance: ");
            string balanceInput = Console.ReadLine();

            Console.WriteLine("\n\nPress \"s\" or \"S\" to submit");
            Console.WriteLine("Press any other key to cancel");

            try
            {
                var selection = Console.ReadKey();
                switch (selection.Key)
                {
                    case ConsoleKey.S:
                        // Checks if the ID input fits within the amount of customers
                        if (int.Parse(ownerIDInput) > CustomerList.Count)
                            throw new ArgumentException("There is no Customer matching that ID!");

                        // Checks which customer the owner ID input matches
                        foreach (Customer c in CustomerList)
                        {
                            // Checks the ID
                            if (c.ID == uint.Parse(ownerIDInput))
                            {
                                if (accountTypeInput == "1")
                                    AccountList.Add(new Type1Account(c, decimal.Parse(balanceInput)));
                                else if (accountTypeInput == "2")
                                    AccountList.Add(new Type2Account(c, decimal.Parse(balanceInput)));
                            }
                        }

                        // Clear's the screen and prints out feedback result
                        Console.Clear();
                        Console.WriteLine(formattingLine);
                        Console.WriteLine("\tOpen An Account\t\t\t\t\t\t\t\t\t\t\t\t\tDesigned By Corey Henderson");
                        Console.WriteLine(formattingLine);

                        Console.WriteLine("\nThank you {0}, you're {1} Bank Account has been successfully created.", CustomerList[int.Parse(ownerIDInput) - 1].FirstName, accountTypeInput == "1" ? "Type 1" : "Type 2");

                        Console.WriteLine("\n\nPress any keys to go back to Main menu");
                        Console.ReadKey();

                        menuState = State.MainMenu;
                        break;
                    default:
                        menuState = State.MainMenu;
                        break;
                }
            }
            catch (IndexOutOfRangeException re)
            {
                Console.Clear();
                Console.WriteLine(re.Message);
                Console.WriteLine("\nPlease ensure all entries are fully entered. e.g. 4/10/1995");
                Console.WriteLine("Please try again.");
                CreateAccountMenu(AccountList, CustomerList);
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Console.WriteLine("\nPlease try again.");
                CreateAccountMenu(AccountList, CustomerList);
            }
        }

        //---  Search Account Menu            ---\\
        static void DisplayAccountSearch(List<Account> AccountList, List<Customer> CustomerList, uint input)
        {
            Console.WriteLine(formattingLine);
            Console.WriteLine("\tSearch An Account\t\t\t\t\t\t\t\t\t\t\t\tDesigned By Corey Henderson");
            Console.WriteLine(formattingLine);

            Console.WriteLine("\nSearch results:");
            foreach (Account a in AccountList)
            {
                // Prints out the accounts if the account ID matches and the SearchState matches
                if (input == a.ID && searchState == SearchState.accountIDInput || input == a.Owner.ID && searchState == SearchState.ownerIDInput)
                {
                    Console.WriteLine(a);
                }
            }

            // Displays message if there is nothing printed
            if (input > AccountList.Count && searchState == SearchState.accountIDInput || input > CustomerList.Count && searchState == SearchState.ownerIDInput)
                Console.WriteLine("Sorry, no accounts match that search");

            Console.WriteLine("\n\nPress any keys to go back to Main menu");
            Console.ReadKey();
            menuState = State.MainMenu;
        }
        static void SearchAccountMenu(List<Account> AccountList, List<Customer> CustomerList)
        {
            Console.WriteLine(formattingLine);
            Console.WriteLine("\tSearch An Account\t\t\t\t\t\t\t\t\t\t\t\tDesigned By Corey Henderson");
            Console.WriteLine(formattingLine);
            Console.WriteLine("Please enter one of the following account details:");

            Console.Write("\nEnter Account ID: ");
            string accIDInput = Console.ReadLine();

            Console.Write("Enter Owner ID: ");
            string ownerIDInput = Console.ReadLine();

            Console.WriteLine("\n\nPress \"s\" or \"S\" to search");
            Console.WriteLine("Press any other key to cancel");

            try
            {
                var selection = Console.ReadKey();
                Console.Clear();
                switch (selection.Key)
                {
                    case ConsoleKey.S:
                        // Checks whether the user inputs have a value, if so, it displays the search based off the input
                        if (string.IsNullOrEmpty(accIDInput) == false)
                        {
                            searchState = SearchState.accountIDInput;
                            DisplayAccountSearch(AccountList, CustomerList, uint.Parse(accIDInput));
                            break;
                        }
                        if (string.IsNullOrEmpty(ownerIDInput) == false)
                        {
                            searchState = SearchState.ownerIDInput;
                            DisplayAccountSearch(AccountList, CustomerList, uint.Parse(ownerIDInput));
                            break;
                        }

                        throw new Exception("No information was entered!");
                    default:
                        menuState = State.MainMenu;
                        break;
                }
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Console.WriteLine("\nPlease try again.");
                SearchAccountMenu(AccountList, CustomerList);
            }
        }

        //---  Transfer Menu                  ---\\
        static void TransferMenu(List<Account> AccountList)
        {
            Console.WriteLine(formattingLine);
            Console.WriteLine("\tTransfer\t\t\t\t\t\t\t\t\t\t\t\t\tDesigned By Corey Henderson");
            Console.WriteLine(formattingLine);

            // Receive inputs
            Console.Write("\n\nEnter Source Account ID: ");
            string sourceIDInput = Console.ReadLine();
            Console.Write("Enter Destination Account ID: ");
            string destinationIDInput = Console.ReadLine();
            Console.Write("Enter Amount:");
            string amountInput = Console.ReadLine();

            Console.WriteLine("\n\nPress \"s\" or \"S\" to submit transfer");
            Console.WriteLine("Press any other key to cancel");

            try
            {
                var selection = Console.ReadKey();
                switch (selection.Key)
                {
                    case ConsoleKey.S:
                        // Get's the two accounts' position in the list
                        int sourceAccountPosition = 999;
                        int destinationAccountPosition = 999;
                        foreach (Account a in AccountList)
                        {
                            if (uint.Parse(sourceIDInput) == a.ID)
                                sourceAccountPosition = int.Parse(sourceIDInput) - 1;

                            if (uint.Parse(destinationIDInput) == a.ID)
                                destinationAccountPosition = int.Parse(destinationIDInput) - 1;
                        }

                        if (sourceAccountPosition > AccountList.Count - 1)
                            throw new Exception("Source Account ID does not exist!");
                        if (destinationAccountPosition > AccountList.Count - 1)
                            throw new Exception("Destination Account ID does not exist!");
                        if (sourceAccountPosition == destinationAccountPosition)
                            throw new Exception("Why would you want to transfer to the same account!");

                        // Transfer's using the accounts in the list
                        AccountList[sourceAccountPosition].Transfer(AccountList[destinationAccountPosition], decimal.Parse(amountInput));

                        // Clear and display success output
                        Console.Clear();
                        Console.WriteLine(formattingLine);
                        Console.WriteLine("\tTransfer\t\t\t\t\t\t\t\t\t\t\t\t\tDesigned By Corey Henderson");
                        Console.WriteLine(formattingLine);

                        // Checks the types of the source and destination accounts for usage in the Successful transfer prompt line
                        string sourceType = "";
                        string destinationType = "";
                        if (AccountList[sourceAccountPosition].GetType() == typeof(Type1Account))
                            sourceType = "1";
                        else
                            sourceType = "2";
                        if (AccountList[destinationAccountPosition].GetType() == typeof(Type1Account))
                            destinationType = "1";
                        else
                            destinationType = "2";

                        // Successful transfer prompt line
                        if (AccountList[sourceAccountPosition].Owner.ID == AccountList[destinationAccountPosition].Owner.ID)
                            Console.WriteLine("\nSuccessful Transfer from {0}'s type {1} account to {0}'s type {2} account.", AccountList[sourceAccountPosition].Owner.FirstName, sourceType, destinationType);
                        else
                            Console.WriteLine("\nSuccessful Transfer from {0}'s account to {1}'s account.", AccountList[sourceAccountPosition].Owner.FirstName, AccountList[destinationAccountPosition].Owner.FirstName);

                        Console.WriteLine("\n\nPress any keys to go back to Main menu");
                        Console.ReadKey();
                        menuState = State.MainMenu;
                        break;
                    default:
                        menuState = State.MainMenu;
                        break;
                }
            }
            // Exception handling - displays error then loops back to the start of the transferMenu()
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Console.WriteLine("\nPlease try again.");
                TransferMenu(AccountList);
            }
        }

        //---  Deposit Menu                   ---\\
        static void DepositMenu(List<Account> AccountList)
        {
            Console.WriteLine(formattingLine);
            Console.WriteLine("\tDeposit\t\t\t\t\t\t\t\t\t\t\t\t\t\tDesigned By Corey Henderson");
            Console.WriteLine(formattingLine);

            Console.Write("\nEnter Account ID: ");
            string accountIDInput = Console.ReadLine();
            Console.Write("Enter Amount: ");
            string amountInput = Console.ReadLine();

            Console.WriteLine("\n\nPress \"s\" or \"S\" to submit despoit");
            Console.WriteLine("Press any other key to cancel");

            try
            {
                var selection = Console.ReadKey();
                switch (selection.Key)
                {
                    case ConsoleKey.S:
                        // Checks if the inputs are entered
                        if (string.IsNullOrEmpty(amountInput))
                            throw new Exception("Amount must been entered!");

                        // Gets the account position in the list from the ID input - I just used 999 as a default value with the assumption that there will not be more than 1000 accounts in the list
                        int accountPosition = 999;
                        foreach (Account a in AccountList)
                        {
                            // Checks if the input is of a type 2 account
                            if (uint.Parse(accountIDInput) == a.ID && a.GetType() == typeof(Type2Account))
                                throw new Exception("You can only deposit into a type 1 account!");

                            // Finds the account in the list
                            if (uint.Parse(accountIDInput) == a.ID)
                            {
                                accountPosition = int.Parse(accountIDInput) - 1;

                                // Make sure that the accounts position in the list is within the range of the list.
                                if (accountPosition > AccountList.Count - 1)
                                    throw new Exception("Account ID does not exist!");

                                ((Type1Account)a).Deposit(decimal.Parse(amountInput));
                            }
                        }

                        Console.Clear();
                        Console.WriteLine(formattingLine);
                        Console.WriteLine("\tDeposit\t\t\t\t\t\t\t\t\t\t\t\t\t\tDesigned By Corey Henderson");
                        Console.WriteLine(formattingLine);

                        Console.WriteLine("\nSuccessful Deposit into {0}'s account", AccountList[accountPosition].Owner.FirstName);

                        Console.WriteLine("\n\nPress any keys to go back to Main menu");
                        Console.ReadKey();

                        menuState = State.MainMenu;
                        break;
                    default:
                        menuState = State.MainMenu;
                        break;
                }
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Console.WriteLine("\nPlease try again.");
                DepositMenu(AccountList);
            }
        }

        //---  Withdraw Menu                  ---\\
        static void WithdrawMenu(List<Account> AccountList)
        {
            Console.WriteLine(formattingLine);
            Console.WriteLine("\tWithdraw\t\t\t\t\t\t\t\t\t\t\t\t\tDesigned By Corey Henderson");
            Console.WriteLine(formattingLine);

            Console.Write("\nEnter Account ID: ");
            string accountIDInput = Console.ReadLine();
            Console.Write("Enter Amount: ");
            string amountInput = Console.ReadLine();

            Console.WriteLine("\n\nPress \"s\" or \"S\" to submit withdraw");
            Console.WriteLine("Press any other key to cancel");

            try
            {
                var selection = Console.ReadKey();
                switch (selection.Key)
                {
                    case ConsoleKey.S:
                        // Checks if the inputs are entered
                        if (string.IsNullOrEmpty(amountInput))
                            throw new Exception("Amount must been entered!");

                        // Variable for the chosen accounts position in the list - I just used 999 as a default value with the assumption that there will not be more than 1000 accounts in the list
                        int accountPosition = 999;
                        foreach (Account a in AccountList)
                        {
                            // Compares input ID and account ID to find the matching account
                            if (uint.Parse(accountIDInput) == a.ID)
                            {
                                // Checks if the input is of a type 2 account
                                if (a.GetType() == typeof(Type2Account))
                                    throw new Exception("You can only withdraw from a type 1 account!");

                                accountPosition = int.Parse(accountIDInput) - 1;

                                // Make sure that the accounts position in the list is within the range of the list.
                                if (accountPosition > AccountList.Count - 1)
                                    throw new Exception("Account ID does not exist!");

                                ((Type1Account)a).Withdraw(decimal.Parse(amountInput));
                            }
                        }

                        Console.Clear();
                        Console.WriteLine(formattingLine);
                        Console.WriteLine("\tWithdraw\t\t\t\t\t\t\t\t\t\t\t\t\tDesigned By Corey Henderson");
                        Console.WriteLine(formattingLine);

                        Console.WriteLine("\nSuccessful Withdraw from {0}'s account", AccountList[accountPosition].Owner.FirstName);

                        Console.WriteLine("\n\nPress any keys to go back to Main menu");
                        Console.ReadKey();

                        menuState = State.MainMenu;
                        break;
                    default:
                        menuState = State.MainMenu;
                        break;
                }
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Console.WriteLine("\nPlease try again.");
                WithdrawMenu(AccountList);
            }
        }

        //---  Set Monthly Deposit Menu       ---\\
        static void MonthlyDepositMenu(List<Account> AccountList)
        {
            Console.WriteLine(formattingLine);
            Console.WriteLine("\tSet Monthly Deposit\t\t\t\t\t\t\t\t\t\t\t\tDesigned By Corey Henderson");
            Console.WriteLine(formattingLine);

            Console.Write("\nEnter Account ID: ");
            string accountIDInput = Console.ReadLine();
            Console.Write("Enter Amount: ");
            string amountInput = Console.ReadLine();

            Console.WriteLine("\n\nPress \"s\" or \"S\" to submit monthly deposit");
            Console.WriteLine("Press any other key to cancel");

            try
            {
                var selection = Console.ReadKey();
                switch (selection.Key)
                {
                    case ConsoleKey.S:
                        // Gets the account position in the list from the ID input - I just used 999 as a default value with the assumption that there will not be more than 1000 accounts in the list
                        int accountPosition = 999;
                        foreach (Account a in AccountList)
                        {
                            if (uint.Parse(accountIDInput) == a.ID)
                            {
                                // Checks if the input is of a type 1 account
                                if (a.GetType() == typeof(Type1Account))
                                    throw new Exception("You can only set the monthly deposit for a type 2 account!");

                                accountPosition = int.Parse(accountIDInput) - 1;

                                // Make sure that the accounts position in the list is within the range of the list.
                                if (accountPosition > AccountList.Count - 1)
                                    throw new Exception("Account ID does not exist!");

                                ((Type2Account)a).MonthlyDeposit = decimal.Parse(amountInput);
                            }
                        }

                        // Checks if the inputs are entered
                        if (string.IsNullOrEmpty(accountIDInput) && string.IsNullOrEmpty(amountInput))
                            throw new Exception("No information has been entered!");

                        Console.Clear();
                        Console.WriteLine(formattingLine);
                        Console.WriteLine("\tSet Monthly Deposit\t\t\t\t\t\t\t\t\t\t\t\tDesigned By Corey Henderson");
                        Console.WriteLine(formattingLine);

                        Console.WriteLine("\nSuccessfully set monthly deposit for {0}'s account", AccountList[accountPosition].Owner.FirstName);

                        Console.WriteLine("\n\nPress any keys to go back to Main menu");
                        Console.ReadKey();

                        menuState = State.MainMenu;
                        break;
                    default:
                        menuState = State.MainMenu;
                        break;
                }
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Console.WriteLine("\nPlease try again.");
                MonthlyDepositMenu(AccountList);
            }
        }

        //-----------------------------------------------------------------\\
        //  Main Function                                                  \\
        //-----------------------------------------------------------------\\
        static void Main(string[] args)
        {
            Console.SetWindowSize(150, 40); // Original is: 120, 30

            // List of Customers and Accounts
            List<Customer> Customers = new List<Customer>();
            List<Account> Accounts = new List<Account>();

            ReadFile("customers.txt", Customers, Accounts);
            ReadFile("accounts.txt", Customers, Accounts);

            // Main System Loop using an Enum class to make a state machine
            do
            {
                switch (menuState)
                {
                    case State.MainMenu:
                        DisplayMainMenu();
                        break;
                    case State.CreateCustomer:
                        CreateCustomerMenu(Customers);
                        break;
                    case State.SearchCustomer:
                        SearchCustomerMenu(Customers);
                        break;
                    case State.CreateAccount:
                        CreateAccountMenu(Accounts, Customers);
                        break;
                    case State.SearchAccount:
                        SearchAccountMenu(Accounts, Customers);
                        break;
                    case State.Transfer:
                        TransferMenu(Accounts);
                        break;
                    case State.Desposit:
                        DepositMenu(Accounts);
                        break;
                    case State.Withdraw:
                        WithdrawMenu(Accounts);
                        break;
                    case State.MonthlyDeposit:
                        MonthlyDepositMenu(Accounts);
                        break;
                }
                // Clears the console everytime it loops through the menus
                Console.Clear();

            } while (menuState != State.Exit);

            // Recreate customer and account files based off the objects within the list
            WriteFile("customers.txt", Customers, Accounts);
            WriteFile("accounts.txt", Customers, Accounts);
        }
    }
}
