using System;
using System.IO;
using System.Text.Json;
using NLog;
using Final_Project.Models;

namespace Final_Project.Services;

public class FileService
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public static BankCardRoot LoadBankCards()
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

    public static void SaveBankCards(BankCardRoot bankCardRoot)
    {
        try
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "../../../Data/cards.json");
            var options = new JsonSerializerOptions 
            { 
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
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