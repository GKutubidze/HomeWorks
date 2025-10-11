using System;
using System.Collections.Generic;
using NLog;
using Final_Project.Models;

namespace Final_Project.Services;

public class BankService
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public static void ViewBalance(User user)
    {
        Console.WriteLine("\n=== ნაშთის ნახვა ===");
        Console.WriteLine($"თქვენი ბალანსი: {user.cardDetails.currentBalance} {user.cardDetails.CurrentCurrency}");
        logger.Info($"ბალანსის ნახვა: {user.cardDetails.currentBalance}");
    }

    public static void Withdraw(CardDetails card, BankCardRoot bankCardRoot)
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
                FileService.SaveBankCards(bankCardRoot);
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

    public static void Deposit(CardDetails card, BankCardRoot bankCardRoot)
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
                FileService.SaveBankCards(bankCardRoot);
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

    public  static void ViewLastOperations(User user)
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

    public static  void ChangePIN(User user, BankCardRoot bankCardRoot)
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
            FileService.SaveBankCards(bankCardRoot);
            Console.WriteLine("✅ PIN კოდი წარმატებით შეიცვალა!");
            logger.Info("PIN კოდი შეიცვალა");
        }
        else
        {
            Console.WriteLine("❌ შეცდომა: PIN უნდა იყოს 4 ციფრი.");
        }
    }

    public  static void ConvertCurrency(User user, BankCardRoot bankCardRoot)
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
                FileService.SaveBankCards(bankCardRoot);
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

    private static void AddTransactionRecord(CardDetails card, string type, double amount)
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
}