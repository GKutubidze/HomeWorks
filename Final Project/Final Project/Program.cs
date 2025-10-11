using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Final_Project.Models;
using Final_Project.Services;
using NLog;

namespace Final_Project
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            BankCardRoot bankCardRoot = FileService.LoadBankCards();
            User bankCard = CheckCardDetails(bankCardRoot);

            if (bankCard != null)
            {
                HandleMenuChoice(bankCard, bankCardRoot);
            }

            logger.Info("პროგრამა დასრულდა.");
        }

        static User CheckCardDetails(BankCardRoot bankCardRoot)
        {
            Console.WriteLine("Enter card number:");
            var cardNumber = Console.ReadLine().Trim();

            Console.WriteLine("Enter expiration date (MM/YY):");
            var expirationDate = Console.ReadLine().Trim();

            var card = bankCardRoot.users.FirstOrDefault(c =>
                c.cardDetails.cardNumber == cardNumber &&
                c.cardDetails.expirationDate == expirationDate
            );

            if (card != null)
            {
                Console.WriteLine("Enter CVC code:");
                var cvc = Console.ReadLine().Trim();

                if (card.cardDetails.CVC != cvc)
                {
                    Console.WriteLine("Invalid CVC code.");
                    return null;
                }

                Console.WriteLine("Enter PIN code:");
                var pinCode = Console.ReadLine().Trim();

                if (card.cardDetails.pinCode == pinCode)
                {
                    Console.WriteLine($"Access granted. Welcome, {card.firstName} {card.lastName}!");
                    return card;
                }
                else
                {
                    Console.WriteLine("Invalid PIN code.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Invalid card number or expiration date.");
                return null;
            }
        }

        static void HandleMenuChoice(User user, BankCardRoot bankCardRoot)
        {
            while (true)
            {
                Console.WriteLine("\n=== მთავარი მენიუ ===");
                Console.WriteLine("1. ნაშთის ნახვა");
                Console.WriteLine("2. თანხის გამოტანა");
                Console.WriteLine("3. თანხის შეტანა");
                Console.WriteLine("4. ბოლო 5 ოპერაცია");
                Console.WriteLine("5. PIN კოდის შეცვლა");
                Console.WriteLine("6. ვალუტის კონვერტაცია");
                Console.WriteLine("7. გამოსვლა");

                Console.Write("\nირჩიეთ ოპერაცია: ");
                string choice = Console.ReadLine()?.Trim();

                ExecuteMenuOperation(choice, user, bankCardRoot);
            }
        }

        static void ExecuteMenuOperation(string choice, User user, BankCardRoot bankCardRoot)
        {
            switch (choice)
            {
                case "1":
                    BankService.ViewBalance(user);
                    break;
                case "2":
                    BankService.Withdraw(user.cardDetails, bankCardRoot);
                    break;
                case "3":
                    BankService.Deposit(user.cardDetails, bankCardRoot);
                    break;
                case "4":
                    BankService.ViewLastOperations(user);
                    break;
                case "5":
                    BankService.ChangePIN(user, bankCardRoot);
                    break;
                case "6":
                    BankService.ConvertCurrency(user, bankCardRoot);
                    break;
                case "7":
                    Console.WriteLine("გამოსვლა...");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("არასწორი არჩევანი, სცადეთ თავიდან.");
                    break;
            }
        }

    

     
       
    }
}