using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Final_Project.Models;
using NLog;

namespace Final_Project
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            BankCardRoot bankCardRoot = LoadBankCards();
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
                    ViewBalance(user);
                    break;
                case "2":
                    Withdraw(user.cardDetails, bankCardRoot);
                    break;
                case "3":
                    Deposit(user.cardDetails, bankCardRoot);
                    break;
                case "4":
                    ViewLastOperations(user);
                    break;
                case "5":
                    ChangePIN(user, bankCardRoot);
                    break;
                case "6":
                    ConvertCurrency(user, bankCardRoot);
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

        static void ViewBalance(User user)
        {
            Console.WriteLine("\n=== ნაშთის ნახვა ===");
            Console.WriteLine($"თქვენი ბალანსი: {user.cardDetails.currentBalance} {user.cardDetails.CurrentCurrency}");
            logger.Info($"ბალანსის ნახვა: {user.cardDetails.currentBalance}");
        }

        static void Withdraw(CardDetails card, BankCardRoot bankCardRoot)
        {
            try
            {
                Console.Write("\nშეიყვანეთ გამოსატანი თანხა: ");
                string input = Console.ReadLine().Trim();

                if (!double.TryParse(input, out double amount))
                {
                    Console.WriteLine("შეცდომა: გთხოვთ შეიყვანეთ სწორი რიცხვი.");
                    return;
                }

                bool result = card.Withdraw(amount);

                if (result)
                {
                    AddTransactionRecord(card, "გამოტანა", amount);
                    SaveBankCards(bankCardRoot);
                    Console.WriteLine($"✅ გამოტანა წარმატებული! გამოტანილი თანხა: {amount}");
                    Console.WriteLine($"თქვენი ახალი ბალანსი: {card.currentBalance} {card.CurrentCurrency}");
                    logger.Info($"გამოტანა წარმატებული: {amount}");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"შეცდომა: {ex.Message}");
                logger.Error($"გამოტანის შეცდომა: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"❌ {ex.Message}");
                logger.Error($"გამოტანის შეცდომა: {ex.Message}");
            }
            catch (Exception ex)
            {
                logger.Error($"გამოტანის შეცდომა: {ex.Message}");
                Console.WriteLine($"გამოტანის დროს შეცდომა: {ex.Message}");
            }
        }

        static void Deposit(CardDetails card, BankCardRoot bankCardRoot)
        {
            try
            {
                Console.Write("\nშეიყვანეთ შეტანილი თანხა: ");
                string input = Console.ReadLine().Trim();

                if (!double.TryParse(input, out double amount))
                {
                    Console.WriteLine("შეცდომა: გთხოვთ შეიყვანეთ სწორი რიცხვი.");
                    return;
                }

                bool result = card.Deposit(amount);

                if (result)
                {
                    AddTransactionRecord(card, "შეტანა", amount);
                    SaveBankCards(bankCardRoot);
                    Console.WriteLine($"✅ შეტანა წარმატებული! შეტანილი თანხა: {amount}");
                    Console.WriteLine($"თქვენი ახალი ბალანსი: {card.currentBalance} {card.CurrentCurrency}");
                    logger.Info($"შეტანა წარმატებული: {amount}");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"შეცდომა: {ex.Message}");
                logger.Error($"შეტანის შეცდომა: {ex.Message}");
            }
            catch (Exception ex)
            {
                logger.Error($"შეტანის შეცდომა: {ex.Message}");
                Console.WriteLine($"შეტანის დროს შეცდომა: {ex.Message}");
            }
        }

        static void ViewLastOperations(User user)
        {
            Console.WriteLine("\n=== ბოლო 5 ოპერაცია ===");
            var lastTransactions = user.cardDetails.GetLastFiveTransactions();

            if (lastTransactions.Count == 0)
            {
                Console.WriteLine("ოპერაციების ისტორია ცარიელია.");
                return;
            }

            foreach (var transaction in lastTransactions)
            {
                Console.WriteLine($"\nთარიღი: {transaction.transactionDate:yyyy-MM-dd HH:mm:ss}");
                Console.WriteLine($"ტიპი: {transaction.transactionType}");
                Console.WriteLine($"GEL: {transaction.amountGEL}");
                Console.WriteLine($"USD: {transaction.amountUSD}");
                Console.WriteLine($"EUR: {transaction.amountEUR}");
            }

            logger.Info("ბოლო 5 ოპერაციის ნახვა");
        }

        static void ChangePIN(User user, BankCardRoot bankCardRoot)
        {
            Console.WriteLine("\n=== PIN კოდის შეცვლა ===");
            Console.Write("შეიყვანეთ ძველი PIN კოდი: ");
            string oldPin = Console.ReadLine().Trim();

            if (oldPin != user.cardDetails.pinCode)
            {
                Console.WriteLine("❌ შეცდომა: ძველი PIN კოდი არასწორია.");
                return;
            }

            Console.Write("შეიყვანეთ ახალი PIN კოდი (4 ციფრი): ");
            string newPinStr = Console.ReadLine().Trim();

            if (!int.TryParse(newPinStr, out int newPin))
            {
                Console.WriteLine("❌ შეცდომა: PIN უნდა იყოს ციფრი.");
                return;
            }

            Console.Write("გაიმეორეთ ახალი PIN კოდი: ");
            string confirmPinStr = Console.ReadLine().Trim();

            if (newPinStr != confirmPinStr)
            {
                Console.WriteLine("❌ შეცდომა: PIN კოდები არ ემთხვევა.");
                return;
            }

            if (user.cardDetails.ChangePinCode(newPin))
            {
                SaveBankCards(bankCardRoot);
                Console.WriteLine("✅ PIN კოდი წარმატებით შეიცვალა!");
                logger.Info("PIN კოდი შეიცვალა");
            }
            else
            {
                Console.WriteLine("❌ შეცდომა: PIN უნდა იყოს 4 ციფრი.");
            }
        }

        static void ConvertCurrency(User user, BankCardRoot bankCardRoot)
        {
            Console.WriteLine("\n=== ვალუტის კონვერტაცია ===");
            Console.WriteLine($"მიმდინარე ვალუტა: {user.cardDetails.CurrentCurrency}");
            Console.WriteLine($"მიმდინარე ბალანსი: {user.cardDetails.currentBalance}");
            Console.WriteLine("\nხელმისაწვდომი ვალუტები: GEL, USD, EUR");
            Console.Write("აირჩიეთ მიზნობრივი ვალუტა: ");
            string targetCurrency = Console.ReadLine().Trim().ToUpper();

            try
            {
                if (user.cardDetails.ConvertCurrency(targetCurrency))
                {
                    SaveBankCards(bankCardRoot);
                    AddTransactionRecord(user.cardDetails, "ვალუტის კონვერტაცია", user.cardDetails.currentBalance);
                    logger.Info($"ვალუტა გარდაიქმნა: {targetCurrency}");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"❌ {ex.Message}");
                logger.Error($"კონვერტაციის შეცდომა: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"❌ {ex.Message}");
                logger.Error($"კონვერტაციის შეცდომა: {ex.Message}");
            }
        }

        static void AddTransactionRecord(CardDetails card, string type, double amount)
        {
            var transaction = new TransactionRecord
            {
                transactionDate = DateTime.Now,
                transactionType = type,
                amountGEL = card.CurrentCurrency == "GEL" ? amount : 0,
                amountUSD = card.CurrentCurrency == "USD" ? amount : 0,
                amountEUR = card.CurrentCurrency == "EUR" ? amount : 0
            };

            card.transactionHistory.Add(transaction);
        }

        static BankCardRoot LoadBankCards()
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "../../../Data/cards.json");
                string jsonContent = File.ReadAllText(path);
                BankCardRoot bankCardRoot = JsonSerializer.Deserialize<BankCardRoot>(jsonContent);
                return bankCardRoot;
            }
            catch (Exception ex)
            {
                logger.Error($"ფაილის ჩატვირთვის შეცდომა: {ex.Message}");
                Console.WriteLine("ფაილი ვერ ჩაიტვირთა.");
                return null;
            }
        }

        static void SaveBankCards(BankCardRoot bankCardRoot)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "../../../../Data/cards.json");
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonContent = JsonSerializer.Serialize(bankCardRoot, options);
                File.WriteAllText(path, jsonContent);
                logger.Info("მონაცემები წარმატებით შეინახა.");
            }
            catch (Exception ex)
            {
                logger.Error($"ფაილის შენახვის შეცდომა: {ex.Message}");
                Console.WriteLine("მონაცემების შენახვა ვერ მოხერხდა.");
            }
        }
    }
}