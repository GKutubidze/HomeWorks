using System;
using System.IO;
using System.Text.Json;
using NLog;
using Final_Project.Models;

namespace Final_Project.Services;

public class FileService
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../Data/cards.json");

    public static BankCardRoot LoadBankCards()
    {
        try
        {
            if (!File.Exists(FilePath))
            {
                logger.Error($"ფაილი ვერ მოიძებნა: {FilePath}");
                Console.WriteLine("❌ ბანკის ბარათების ფაილი ვერ მოიძებნა.");
                return null;
            }

            string jsonContent = File.ReadAllText(FilePath);

            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                logger.Error("ფაილი ცარიელია.");
                Console.WriteLine("❌ ფაილი ცარიელია.");
                return null;
            }

            BankCardRoot bankCardRoot = JsonSerializer.Deserialize<BankCardRoot>(jsonContent);

            if (bankCardRoot == null || bankCardRoot.users == null || bankCardRoot.users.Count == 0)
            {
                logger.Warn("ფაილში ხელმისაწვდომი ბარათები არ არის.");
                Console.WriteLine("⚠️ სისტემაში ბარათები ვერ მოიძებნა.");
                return null;
            }

            logger.Info($"ბანკის ბარათები წარმატებით ჩაიტვირთა. ბარათების რაოდენობა: {bankCardRoot.users.Count}");
            return bankCardRoot;
        }
        catch (JsonException ex)
        {
            logger.Error($"JSON ფაილის გახსნის შეცდომა: {ex.Message}");
            Console.WriteLine("❌ ფაილის ფორმატი არასწორია.");
            return null;
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.Error($"ფაილზე წვდომა არ არის: {ex.Message}");
            Console.WriteLine("❌ ფაილზე წვდომა უარი თქვა.");
            return null;
        }
        catch (FileNotFoundException ex)
        {
            logger.Error($"ფაილი ვერ მოიძებნა: {ex.Message}");
            Console.WriteLine("❌ ფაილი ვერ მოიძებნა.");
            return null;
        }
        catch (Exception ex)
        {
            logger.Error($"ფაილის ჩატვირთვის შეცდომა: {ex.Message}");
            Console.WriteLine("❌ ფაილი ვერ ჩაიტვირთა.");
            return null;
        }
    }

    public static void SaveBankCards(BankCardRoot bankCardRoot)
    {
        try
        {
            if (bankCardRoot == null)
            {
                logger.Error("მცდელობა: null მონაცემების შენახვა.");
                Console.WriteLine("❌ მონაცემების შენახვა ვერ მოხერხდა (null მონაცემი).");
                return;
            }

            if (bankCardRoot.users == null || bankCardRoot.users.Count == 0)
            {
                logger.Warn("მცდელობა: ცარიელი მომხმარებლების სიის შენახვა.");
                Console.WriteLine("⚠️ არ არის შენახვისთვის მომხმარებელი.");
                return;
            }

            // დირექტორიის შექმნა თუ ის არ არსებობს
            string directory = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                logger.Info($"დირექტორია შეიქმნა: {directory}");
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string jsonContent = JsonSerializer.Serialize(bankCardRoot, options);
            File.WriteAllText(FilePath, jsonContent);

            logger.Info($"მონაცემები წარმატებით შენახულია. მომხმარებლების რაოდენობა: {bankCardRoot.users.Count}");
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.Error($"ფაილის წვდომა უარი: {ex.Message}");
            Console.WriteLine("❌ ფაილზე წვდომა უარი თქვა.");
        }
        catch (DirectoryNotFoundException ex)
        {
            logger.Error($"დირექტორია ვერ მოიძებნა: {ex.Message}");
            Console.WriteLine("❌ დირექტორია ვერ მოიძებნა.");
        }
        catch (IOException ex)
        {
            logger.Error($"I/O შეცდომა ფაილის შენახვისას: {ex.Message}");
            Console.WriteLine("❌ ფაილის შენახვაში I/O შეცდომა მოხდა.");
        }
        catch (JsonException ex)
        {
            logger.Error($"JSON სერიალიზაციის შეცდომა: {ex.Message}");
            Console.WriteLine("❌ მონაცემების ფორმატირებაში შეცდომა.");
        }
        catch (Exception ex)
        {
            logger.Error($"მონაცემების შენახვის შეცდომა: {ex.Message}");
            Console.WriteLine("❌ მონაცემების შენახვა ვერ მოხერხდა.");
        }
    }

    // public static bool IsFileValid()
    // {
    //     try
    //     {
    //         if (!File.Exists(FilePath))
    //             return false;
    //
    //         string content = File.ReadAllText(FilePath);
    //         if (string.IsNullOrWhiteSpace(content))
    //             return false;
    //
    //         var data = JsonSerializer.Deserialize<BankCardRoot>(content);
    //         return data != null && data.users != null && data.users.Count > 0;
    //     }
    //     catch (Exception ex)
    //     {
    //         logger.Error($"ფაილის ვალიდაციის შეცდომა: {ex.Message}");
    //         return false;
    //     }
    // }
}