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
            try
            {
                logger.Info("პროგრამა დაიწყო.");
                
                BankCardRoot bankCardRoot = FileService.LoadBankCards();
                
                if (bankCardRoot == null)
                {
                    logger.Error("ბანკის ბარათების ფაილი ვერ ჩაიტვირთა.");
                    Console.WriteLine("სისტემა არ შეიძლება დაიწყოს.");
                    return;
                }

                User bankCard = CheckCardDetails(bankCardRoot);

                if (bankCard != null)
                {
                    HandleMenuChoice(bankCard, bankCardRoot);
                }
                else
                {
                    logger.Warn("მომხმარებელმა ვერ გაიარა ავტორიზაცია.");
                    Console.WriteLine("ავტორიზაცია ვერ მოხერხდა. პროგრამა დაიხურება.");
                }
            }
            catch (Exception ex)
            {
                logger.Error($"პროგრამაში კრიტიკული შეცდომა: {ex.Message}");
                Console.WriteLine("პროგრამაში შეცდომა მოხდა. სცადეთ მოგვიანებით.");
            }
            finally
            {
                logger.Info("პროგრამა დასრულდა.");
            }
        }

        static User CheckCardDetails(BankCardRoot bankCardRoot)
        {
            try
            {
                Console.WriteLine("\n=== ავტორიზაცია ===");
                Console.WriteLine("შეიყვანეთ ბარათის დეტალები:");
                
                Console.Write("ბარათის ნომერი: ");
                var cardNumber = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(cardNumber))
                {
                    Console.WriteLine("ბარათის ნომერი არ შეიძლება იყოს ცარიელი.");
                    logger.Warn("ცარიელი ბარათის ნომერი.");
                    return null;
                }

                Console.Write("მოქმედების ვადა (MM/YY): ");
                var expirationDate = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(expirationDate))
                {
                    Console.WriteLine("მოქმედების ვადა არ შეიძლება იყოს ცარიელი.");
                    logger.Warn("ცარიელი მოქმედების ვადა.");
                    return null;
                }

                var card = bankCardRoot.users.FirstOrDefault(c =>
                    c.cardDetails.cardNumber == cardNumber &&
                    c.cardDetails.expirationDate == expirationDate
                );

                if (card != null)
                {
                    Console.Write("CVC კოდი: ");
                    var cvc = Console.ReadLine()?.Trim();

                    if (string.IsNullOrEmpty(cvc))
                    {
                        Console.WriteLine("CVC კოდი არ შეიძლება იყოს ცარიელი.");
                        logger.Warn("ცარიელი CVC კოდი.");
                        return null;
                    }

                    if (card.cardDetails.CVC != cvc)
                    {
                        Console.WriteLine("არასწორი CVC კოდი.");
                        logger.Warn($"არასწორი CVC კოდი ბარათისთვის: {cardNumber}");
                        return null;
                    }

                    Console.Write("PIN კოდი: ");
                    var pinCode = Console.ReadLine()?.Trim();

                    if (string.IsNullOrEmpty(pinCode))
                    {
                        Console.WriteLine("PIN კოდი არ შეიძლება იყოს ცარიელი.");
                        logger.Warn("ცარიელი PIN კოდი.");
                        return null;
                    }

                    if (card.cardDetails.pinCode == pinCode)
                    {
                        Console.WriteLine($"ავტორიზაცია წარმატებული. მოგესალმები, {card.firstName} {card.lastName}!");
                        logger.Info($"ავტორიზაცია წარმატებული: {card.firstName} {card.lastName}");
                        return card;
                    }
                    else
                    {
                        Console.WriteLine("არასწორი PIN კოდი.");
                        logger.Warn($"არასწორი PIN კოდი ბარათისთვის: {cardNumber}");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine("ბარათი ვერ მოიძებნა. არასწორი ნომერი ან მოქმედების ვადა.");
                    logger.Warn($"ბარათი ვერ მოიძებნა: {cardNumber}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error($"ავტორიზაციის შეცდომა: {ex.Message}");
                Console.WriteLine("ავტორიზაციაში შეცდომა მოხდა.");
                return null;
            }
        }

        static void HandleMenuChoice(User user, BankCardRoot bankCardRoot)
        {
            bool continueMenu = true;
            
            while (continueMenu)
            {
                try
                {
                    Console.WriteLine("\n=== მთავარი მენიუ ===\n");
                    Console.WriteLine("1. ნაშთის ნახვა");
                    Console.WriteLine("2. თანხის გამოტანა");
                    Console.WriteLine("3. თანხის შეტანა");
                    Console.WriteLine("4. ბოლო 5 ოპერაცია");
                    Console.WriteLine("5. PIN კოდის შეცვლა");
                    Console.WriteLine("6. ვალუტის კონვერტაცია");
                    Console.WriteLine("7. გამოსვლა");

                    Console.Write("\nირჩიეთ ოპერაცია: ");
                    string choice = Console.ReadLine()?.Trim();

                    if (string.IsNullOrEmpty(choice))
                    {
                        Console.WriteLine("გთხოვთ შეიყვანოთ მენიუს ნომერი.");
                        logger.Warn("ცარიელი მენიუს არჩევანი.");
                        continue;
                    }

                    continueMenu = ExecuteMenuOperation(choice, user, bankCardRoot);
                }
                catch (Exception ex)
                {
                    logger.Error($"მენიუს შეცდომა: {ex.Message}");
                    Console.WriteLine("რაიმე შეცდომა მოხდა. გთხოვთ სცადოთ თავიდან.");
                }
            }
        }

        static bool ExecuteMenuOperation(string choice, User user, BankCardRoot bankCardRoot)
        {
            try
            {
                switch (choice)
                {
                    case "1":
                        BankService.ViewBalance(user, bankCardRoot);
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
                        logger.Info("მომხმარებელი გამოსული");
                        return false;
                    default:
                        Console.WriteLine("არასწორი არჩევანი. გთხოვთ აირჩიეთ 1-დან 7-მდე.");
                        logger.Warn($"არასწორი მენიუს არჩევანი: {choice}");
                        break;
                }

                if (choice != "7")
                {
                    Console.WriteLine("\nგასაგრძელებლად დააჭირეთ Enter...");
                    Console.ReadLine();
                    Console.Clear();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error($"ოპერაციის შეცდომა ({choice}): {ex.Message}");
                Console.WriteLine("ოპერაციის შესრულებაში შეცდომა მოხდა.");
                Console.WriteLine("\nგასაგრძელებლად დააჭირეთ Enter...");
                Console.ReadLine();
                Console.Clear();
                return true;
            }
        }
    }
}
