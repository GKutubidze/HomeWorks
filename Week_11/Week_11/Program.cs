using System.Text.Json;
using System.Xml;

namespace Week_11;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== 1. CreateFile ===");
        CreateFile("data.txt", 5);
        Console.WriteLine();

        Console.WriteLine("=== 2. Multiplication Table ===");
        MultiplicationTableToFile("table.txt", 3);
        Console.WriteLine("გამრავლების ტაბულა ჩაიწერა ფაილში 'table.txt'\n");

        Console.WriteLine("=== 3. StringToN (XML generator) ===");
        StringToN("programming", 2);
        Console.WriteLine();

        Console.WriteLine("=== 4. BirthDayFromJson ===");
        BirthDayFromJson("data.json");
        Console.WriteLine();

        Console.WriteLine("=== 5. Caesar Cipher from JSON ===");
        CaesarFromJson("temp.json");
        Console.WriteLine();
    }

    static void CreateFile(string path,int n)
    {
        if (File.Exists(path))
        {
            Console.WriteLine("File already exists!");
            return; 
        }

        File.Create(path).Close();

        using (StreamWriter writer = new StreamWriter(path))
        {
            for (int i = 0; i < n; i++)
            {
                Console.Write($"Enter line {i + 1}: "); 
                string line = Console.ReadLine();
                writer.WriteLine(line);
            }
        }

        var lines=File.ReadAllLines(path);
        Console.WriteLine(lines[lines.Length-1]);
    }

    static void MultiplicationTableToFile(string path, int n)
    {
        if (File.Exists(path))
        {
            Console.WriteLine("File already exists!");
            return; 
        }
        File.Create(path).Close();

        using (StreamWriter writer = new StreamWriter(path))
        {
            for (int i = 1; i <= 9; i++)
            {
                 
                writer.Write($"1*{i} = {i}");
            
                 
                for (int j = 2; j <= n; j++)
                {
                    writer.Write($" | {j}*{i} = {j * i}");
                }
                writer.WriteLine();
            
                 
                if (i < 9)
                {
                    for (int j = 1; j <= n; j++)
                    {
                        writer.Write("........");
                        if (j < n)
                            writer.Write(" | ");
                    }
                    writer.WriteLine();
                }
            }
        }
    }
    static void StringToN(string str, int n)
    {
        string fileName = "user.xml";
    
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
    
        using (XmlWriter writer = XmlWriter.Create(fileName, settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("XML"); 
        
            int index = 1;
            for (int i = 0; i < str.Length; i += n)
            {
                 
                int remainingChars = Math.Min(n, str.Length - i);
            
                 
                string part = str.Substring(i, remainingChars);
            
                 
                writer.WriteElementString($"string{index}", part);
                index++;
            }
        
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
    
        Console.WriteLine($"XML ფაილი შეიქმნა: {fileName}");
    }

    static string CaesarCipher(string input, int shift)
    {
        string result = "";
        for (int i = 0; i < input.Length; i++)
        {
            int index=((int)input[i]-65+shift)%26;
            char c=(char)(index+65);
            result+=c;
        }

        return $"Cipher: {result}";
    }


    static void BirthDayFromJson(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine("File does not exist!");
            return;
        }

        string jsonContent = File.ReadAllText(path);

        using (JsonDocument doc = JsonDocument.Parse(jsonContent))
        {
            string currentDateStr = doc.RootElement.GetProperty("currentDate").GetString();
            string birthdayStr = doc.RootElement.GetProperty("birthday").GetString();

             
            DateTime currentDate = DateTime.Parse(currentDateStr);
            DateTime birthday = DateTime.Parse(birthdayStr);

            
            if (birthday < currentDate)
            {
                birthday = birthday.AddYears(1);
            }
            
            Console.WriteLine(birthday);

             
            int daysLeft = (birthday - currentDate).Days;

            Console.WriteLine($"{daysLeft}");
        }
    }
    
    
    static void CaesarFromJson(string jsonPath)
    {
        if (!File.Exists(jsonPath))
        {
            Console.WriteLine("File does not exist!"); 
        }

        string jsonContent = File.ReadAllText(jsonPath);
        using (JsonDocument doc = JsonDocument.Parse(jsonContent))
        {
            string word=doc.RootElement.GetProperty("word").GetString();
            int shift=doc.RootElement.GetProperty("key").GetInt32();
            Console.WriteLine( CaesarCipher(word,shift));
        }
    }
    
    
}

 


