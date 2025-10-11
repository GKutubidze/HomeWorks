namespace Final_Project.Models;

public class CardDetails
    {
        public string cardNumber { get; set; }
        public string expirationDate { get; set; }
        public string CVC { get; set; }

        public double currentBalance { get; set; }

        public string CurrentCurrency { get; set; } = "GEL";
        public string pinCode { get; set; }
        public List<TransactionRecord> transactionHistory { get; set; } = new List<TransactionRecord>();

        public bool Withdraw(double amount)
        {
            if (amount <= 0)
                throw new ArgumentException("თანხა უნდა იყოს დადებითი რიცხვი.");

            if (amount > currentBalance)
                throw new InvalidOperationException("საკმარისი სახსრები არ არის.");

            currentBalance -= amount;
            return true;
        }

        public bool Deposit(double amount)
        {
            if (amount <= 0)
                throw new ArgumentException("შეტანილი თანხა უნდა იყოს დადებითი რიცხვი.");

            currentBalance += amount;
            return true;
        }

        public List<TransactionRecord> GetLastFiveTransactions()
        {
            return transactionHistory.TakeLast(5).ToList();
        }

        public bool ChangePinCode(int newPin)
        {
            if (newPin < 1000 || newPin > 9999)
            {
                return false;
            }

            pinCode = newPin.ToString();
            return true;
        }

        public bool ConvertCurrency(string targetCurrency)
        {
            if (string.Equals(CurrentCurrency, targetCurrency, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"თქვენ უკვე იყენებთ {CurrentCurrency}-ს.");
            }

            double gelToUsd = 0.37;
            double gelToEur = 0.34;
            double usdToGel = 2.7;
            double eurToGel = 3.0;

            double newBalance = currentBalance;

            switch (CurrentCurrency.ToUpper())
            {
                case "GEL":
                    if (targetCurrency.ToUpper() == "USD")
                        newBalance = currentBalance * gelToUsd;
                    else if (targetCurrency.ToUpper() == "EUR")
                        newBalance = currentBalance * gelToEur;
                    else
                        throw new ArgumentException("მიუთითეთ სწორი მიზნობრივი ვალუტა (USD/EUR).");
                    break;

                case "USD":
                    if (targetCurrency.ToUpper() == "GEL")
                        newBalance = currentBalance * usdToGel;
                    else if (targetCurrency.ToUpper() == "EUR")
                        newBalance = currentBalance * usdToGel * gelToEur;
                    else
                        throw new ArgumentException("მიუთითეთ სწორი მიზნობრივი ვალუტა (GEL/EUR).");
                    break;

                case "EUR":
                    if (targetCurrency.ToUpper() == "GEL")
                        newBalance = currentBalance * eurToGel;
                    else if (targetCurrency.ToUpper() == "USD")
                        newBalance = currentBalance * eurToGel * gelToUsd;
                    else
                        throw new ArgumentException("მიუთითეთ სწორი მიზნობრივი ვალუტა (GEL/USD).");
                    break;

                default:
                    throw new InvalidOperationException("მიმდინარე ვალუტა უცნობია.");
            }

            currentBalance = Math.Round(newBalance, 2);
            CurrentCurrency = targetCurrency.ToUpper();

            Console.WriteLine($"✅ კონვერტაცია წარმატებულია! ახალი ბალანსი: {currentBalance} {CurrentCurrency}");
            return true;
        }
    }