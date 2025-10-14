using System;
using System.Collections.Generic;
using NLog;
using Final_Project.Models;

namespace Final_Project.Services;

public class BankService
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public static void ViewBalance(User user, BankCardRoot bankCardRoot)
    {
        try
        {
            Console.WriteLine("\n=== ნაშთის ნახვა ===");
            Console.WriteLine($"თქვენი ბალანსი: {user.cardDetails.currentBalance} {user.cardDetails.CurrentCurrency}");
            
            AddTransactionRecord(user.cardDetails, "BalanceInquiry", 0);
            SaveUserData(user, bankCardRoot);
            
            logger.Info($"ბალანსის ნახვა: {user.cardDetails.currentBalance} {user.cardDetails.CurrentCurrency}");
        }
        catch (Exception ex)
        {
            logger.Error($"ბალანსის ნახვის შეცდომა: {ex.Message}");
            Console.WriteLine("ბალანსის ნახვაში შეცდომა მოხდა.");
        }
    }

    public static void Withdraw(CardDetails card, BankCardRoot bankCardRoot)
    {
        try
        {
            Console.Write("\nშეიყვანეთ გამოსატანი თანხა: ");
            string input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("თანხა არ შეიძლება იყოს ცარიელი.");
                logger.Warn("ცარიელი გამოტანის თანხა.");
                return;
            }

            if (!double.TryParse(input, out double amount))
            {
                Console.WriteLine("შეცდომა: გთხოვთ შეიყვანეთ სწორი რიცხვი.");
                logger.Warn($"არასწორი რიცხვი გამოტანაში: {input}");
                return;
            }

            if (amount <= 0)
            {
                Console.WriteLine("თანხა უნდა იყოს დადებითი რიცხვი.");
                logger.Warn($"უარყოფითი ან ნულოვანი თანხა: {amount}");
                return;
            }

            bool result = card.Withdraw(amount);

            if (result)
            {
                AddTransactionRecord(card, "GetAmount", amount);
                SaveAllUserData(bankCardRoot);
                Console.WriteLine("გამოტანა წარმატებული!");
                Console.WriteLine($"გამოტანილი თანხა: {amount} {card.CurrentCurrency}");
                Console.WriteLine($"ახალი ბალანსი: {card.currentBalance} {card.CurrentCurrency}");
                logger.Info($"გამოტანა წარმატებული: {amount} {card.CurrentCurrency}. ბალანსი: {card.currentBalance}");
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"შეცდომა: {ex.Message}");
            logger.Error($"გამოტანის შეცდომა (ArgumentException): {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"{ex.Message}");
            logger.Error($"გამოტანის შეცდომა (InvalidOperationException): {ex.Message}");
        }
        catch (Exception ex)
        {
            logger.Error($"გამოტანის შეცდომა: {ex.Message}");
            Console.WriteLine($"გამოტანის დროს შეცდომა: {ex.Message}");
        }
    }

    public static void Deposit(CardDetails card, BankCardRoot bankCardRoot)
    {
        try
        {
            Console.Write("\nშეიყვანეთ შეტანილი თანხა: ");
            string input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("თანხა არ შეიძლება იყოს ცარიელი.");
                logger.Warn("ცარიელი შეტანის თანხა.");
                return;
            }

            if (!double.TryParse(input, out double amount))
            {
                Console.WriteLine("შეცდომა: გთხოვთ შეიყვანეთ სწორი რიცხვი.");
                logger.Warn($"არასწორი რიცხვი შეტანაში: {input}");
                return;
            }

            if (amount <= 0)
            {
                Console.WriteLine("თანხა უნდა იყოს დადებითი რიცხვი.");
                logger.Warn($"უარყოფითი ან ნულოვანი თანხა: {amount}");
                return;
            }

            bool result = card.Deposit(amount);

            if (result)
            {
                AddTransactionRecord(card, "FillAmount", amount);
                SaveAllUserData(bankCardRoot);
                Console.WriteLine("შეტანა წარმატებული!");
                Console.WriteLine($"შეტანილი თანხა: {amount} {card.CurrentCurrency}");
                Console.WriteLine($"ახალი ბალანსი: {card.currentBalance} {card.CurrentCurrency}");
                logger.Info($"შეტანა წარმატებული: {amount} {card.CurrentCurrency}. ბალანსი: {card.currentBalance}");
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"შეცდომა: {ex.Message}");
            logger.Error($"შეტანის შეცდომა (ArgumentException): {ex.Message}");
        }
        catch (Exception ex)
        {
            logger.Error($"შეტანის შეცდომა: {ex.Message}");
            Console.WriteLine($"შეტანის დროს შეცდომა: {ex.Message}");
        }
    }

    public static void ViewLastOperations(User user)
    {
        try
        {
            Console.WriteLine("\n=== ბოლო 5 ოპერაცია ===");
            var lastTransactions = user.cardDetails.GetLastFiveTransactions();

            if (lastTransactions.Count == 0)
            {
                Console.WriteLine("ოპერაციების ისტორია ცარიელია.");
                logger.Info("ოპერაციების ისტორია ცარიელია.");
                return;
            }

            foreach (var transaction in lastTransactions)
            {
                Console.WriteLine($"\n─────────────────────");
                Console.WriteLine($"თარიღი: {transaction.transactionDate:yyyy-MM-dd HH:mm:ss}");
                Console.WriteLine($"ტიპი: {transaction.transactionType}");
                Console.WriteLine($"GEL: {transaction.amountGEL}");
                Console.WriteLine($"USD: {transaction.amountUSD}");
                Console.WriteLine($"EUR: {transaction.amountEUR}");
            }
            Console.WriteLine($"─────────────────────\n");

            logger.Info("ბოლო 5 ოპერაციის ნახვა");
        }
        catch (Exception ex)
        {
            logger.Error($"ოპერაციების ნახვის შეცდომა: {ex.Message}");
            Console.WriteLine("ოპერაციების ნახვაში შეცდომა მოხდა.");
        }
    }

    public static void ChangePIN(User user, BankCardRoot bankCardRoot)
    {
        try
        {
            Console.WriteLine("\n=== PIN კოდის შეცვლა ===");
            Console.Write("შეიყვანეთ ძველი PIN კოდი: ");
            string oldPin = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(oldPin))
            {
                Console.WriteLine("PIN კოდი არ შეიძლება იყოს ცარიელი.");
                logger.Warn("ცარიელი PIN კოდი შეცვლაში.");
                return;
            }

            if (oldPin != user.cardDetails.pinCode)
            {
                Console.WriteLine("შეცდომა: ძველი PIN კოდი არასწორია.");
                logger.Warn("არასწორი ძველი PIN კოდი.");
                AddTransactionRecord(user.cardDetails, "ChangePin", 0);
                SaveAllUserData(bankCardRoot);
                return;
            }

            Console.Write("შეიყვანეთ ახალი PIN კოდი (4 ციფრი): ");
            string newPinStr = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(newPinStr))
            {
                Console.WriteLine("PIN კოდი არ შეიძლება იყოს ცარიელი.");
                logger.Warn("ცარიელი ახალი PIN კოდი.");
                return;
            }

            if (!int.TryParse(newPinStr, out int newPin))
            {
                Console.WriteLine("შეცდომა: PIN უნდა იყოს მხოლოდ ციფრი.");
                logger.Warn($"არასწორი PIN ფორმატი: {newPinStr}");
                return;
            }

            Console.Write("გაიმეორეთ ახალი PIN კოდი: ");
            string confirmPinStr = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(confirmPinStr))
            {
                Console.WriteLine("PIN კოდი არ შეიძლება იყოს ცარიელი.");
                logger.Warn("ცარიელი დამადასტურებელი PIN კოდი.");
                return;
            }

            if (newPinStr != confirmPinStr)
            {
                Console.WriteLine("შეცდომა: PIN კოდები არ ემთხვევა.");
                logger.Warn("PIN კოდები არ ემთხვევა.");
                return;
            }

            if (user.cardDetails.ChangePinCode(newPin))
            {
                AddTransactionRecord(user.cardDetails, "ChangePin", 0);
                SaveAllUserData(bankCardRoot);
                Console.WriteLine("PIN კოდი წარმატებით შეიცვალა!");
                logger.Info("PIN კოდი წარმატებით შეიცვალა.");
            }
            else
            {
                Console.WriteLine("შეცდომა: PIN უნდა იყოს 4 ციფრი.");
                logger.Warn("PIN კოდი არ აკმაყოფილებს მოთხოვნებს.");
            }
        }
        catch (Exception ex)
        {
            logger.Error($"PIN კოდის შეცვლის შეცდომა: {ex.Message}");
            Console.WriteLine("PIN კოდის შეცვლაში შეცდომა მოხდა.");
        }
    }

    public static void ConvertCurrency(User user, BankCardRoot bankCardRoot)
    {
        try
        {
            Console.WriteLine("\n=== ვალუტის კონვერტაცია ===");
            Console.WriteLine($"მიმდინარე ვალუტა: {user.cardDetails.CurrentCurrency}");
            Console.WriteLine($"მიმდინარე ბალანსი: {user.cardDetails.currentBalance}");
            Console.WriteLine("\nხელმისაწვდომი ვალუტები: GEL, USD, EUR");
            Console.Write("აირჩიეთ მიზნობრივი ვალუტა: ");
            string targetCurrency = Console.ReadLine()?.Trim().ToUpper();

            if (string.IsNullOrEmpty(targetCurrency))
            {
                Console.WriteLine("ვალუტა არ შეიძლება იყოს ცარიელი.");
                logger.Warn("ცარიელი ვალუტის არჩევანი.");
                return;
            }

            if (targetCurrency == user.cardDetails.CurrentCurrency)
            {
                Console.WriteLine($"თქვენი ბალანსი უკვე {targetCurrency}-ში არის.");
                logger.Warn($"იგივე ვალუტაზე კონვერტაციის მცდელობა: {targetCurrency}");
                return;
            }

            if (user.cardDetails.ConvertCurrency(targetCurrency))
            {
                AddTransactionRecord(user.cardDetails, "CurrencyConversion", user.cardDetails.currentBalance);
                SaveAllUserData(bankCardRoot);
                Console.WriteLine("ვალუტა წარმატებით გარდაიქმნა!");
                Console.WriteLine($"ახალი ვალუტა: {user.cardDetails.CurrentCurrency}");
                Console.WriteLine($"ახალი ბალანსი: {user.cardDetails.currentBalance}");
                logger.Info($"ვალუტა გარდაიქმნა: {targetCurrency}. ბალანსი: {user.cardDetails.currentBalance}");
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"{ex.Message}");
            logger.Error($"კონვერტაციის შეცდომა (ArgumentException): {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"{ex.Message}");
            logger.Error($"კონვერტაციის შეცდომა (InvalidOperationException): {ex.Message}");
        }
        catch (Exception ex)
        {
            logger.Error($"კონვერტაციის შეცდომა: {ex.Message}");
            Console.WriteLine("ვალუტის კონვერტაციაში შეცდომა მოხდა.");
        }
    }

    private static void AddTransactionRecord(CardDetails card, string type, double amount)
    {
        try
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
            logger.Info($"ტრანზაქცია ჩაწერილი: {type} - {amount} {card.CurrentCurrency}");
        }
        catch (Exception ex)
        {
            logger.Error($"ტრანზაქციის ჩაწერის შეცდომა: {ex.Message}");
        }
    }

    private static void SaveUserData(User user, BankCardRoot bankCardRoot)
    {
        try
        {
            var updatedUser = bankCardRoot.users.FirstOrDefault(u => 
                u.cardDetails.cardNumber == user.cardDetails.cardNumber);
            
            if (updatedUser != null)
            {
                updatedUser.cardDetails = user.cardDetails;
                FileService.SaveBankCards(bankCardRoot);
            }
        }
        catch (Exception ex)
        {
            logger.Error($"მომხმარებლის მონაცემების შენახვის შეცდომა: {ex.Message}");
        }
    }

    private static void SaveAllUserData(BankCardRoot bankCardRoot)
    {
        try
        {
            FileService.SaveBankCards(bankCardRoot);
            logger.Info("ყველა მომხმარებლის მონაცემი შენახულია.");
        }
        catch (Exception ex)
        {
            logger.Error($"მნიშვნელოვანი: მონაცემების შენახვის შეცდომა: {ex.Message}");
            Console.WriteLine("გაფრთხილება: მონაცემების შენახვა ვერ მოხერხდა.");
        }
    }
}
