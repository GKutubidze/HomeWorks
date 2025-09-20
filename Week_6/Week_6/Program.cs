namespace Week_6;

class Program
{
    static void Main(string[] args)
    {   
        
        
        #region პირველი დავალება
        Console.WriteLine("პირველი დავალება");
        Console.WriteLine("შეიყვნაე მასივის ზომა:");
        int N;
        Int32.TryParse(Console.ReadLine(), out N);
        Console.WriteLine(N);
        double[] numbers = new double[N];

        for (int i = 0; i < N; i++)
        {
            
            Console.WriteLine($"{i+1} Number :");
            double num;
            double.TryParse(Console.ReadLine(), out num);
            numbers[i] = num;
            
        }
        List<double> evens = new List<double>();
        List<double> odds = new List<double>();

        foreach (var num in numbers)
        {
            if (num % 2 == 0)
                evens.Add(num);
            else
                odds.Add(num);
        }
        Console.WriteLine("კენტი რიცხვები:");
        foreach (var VARIABLE in evens)
        {   
            
            Console.WriteLine(VARIABLE);
        }
        Console.WriteLine("ლუწი რიცხვები:");

        foreach (var VARIABLE in odds)
        {
            Console.WriteLine(VARIABLE);
        }
        Console.WriteLine("###########");

       #endregion



       #region მესამე/მეოთხე დავალება

       Console.WriteLine("მესამე/მეოთხე დავალება");
       Console.WriteLine("შეიყვნაე მასივის ზომა:");
       int arrSize;
       if (!int.TryParse(Console.ReadLine(), out arrSize) || arrSize <= 0)
       {
           Console.WriteLine("❌ Invalid array size. Must be positive number.");
           return;
       }
       double[] arr = new double[arrSize];
       for (int i = 0; i < arrSize; i++)
       {
           Console.WriteLine($"{i + 1} რიცხვი");
           double temp;
           double.TryParse(Console.ReadLine(), out temp);
           arr[i] = temp;
       }
       Console.WriteLine("#####");
       var linq = arr
           .GroupBy(x => x)
           .Select(g => new { Number = g.Key, Count = g.Count(), Sum = g.Key * g.Count() });

       foreach (var item in linq)
       {
           Console.WriteLine($"{item.Number} appears {item.Count} time{(item.Count > 1 ? "s" : "")}, sum = {item.Sum}");
       }

       Console.WriteLine("შეიყვანე ტოპ N");
       int topN;
       int.TryParse(Console.ReadLine(), out topN);

       var top = arr.OrderByDescending(x => x).Take(topN);
       foreach (var item in top)
       {
           Console.WriteLine(item);
       }

        Console.WriteLine("############");
       #endregion
        
        
        
        #region მეორე დავალება
        Dictionary<string,string> contacts =new Dictionary<string, string>();

        while (true)
        {
            Console.WriteLine("შეიყვანე ციფრი");
            Console.WriteLine("1-კონტაქტის დამატება");
            Console.WriteLine("2-კონტაქტის წაშლა");
            Console.WriteLine("3-კონტაქტის განახლება");
            Console.WriteLine("4-გამოსვლა");
            int n;
            Int32.TryParse(Console.ReadLine(), out n);
            switch (n)
            {
                case 1: 
                    Console.WriteLine("შეიყვანე კონტაქტის სახელი");
                    string contactName = Console.ReadLine();
                    if (!contacts.ContainsKey(contactName))
                    {
                        Console.WriteLine("შეიყვანე კონტაქტის ნომერი");
                        string contactPhone = Console.ReadLine();
                        contacts.Add(contactName, contactPhone);

                    }
                    else
                    {
                        Console.WriteLine("ასეთი კონტაქტი უკვე არსებობს");
                    }

                        
                    break;
                case 2:
                    Console.WriteLine("შეიყვანე კონტაქტის სახელი");
                    string removeContact= Console.ReadLine();
                    if (contacts.Remove(removeContact))
                    {
                        Console.WriteLine("კონტაქტი წაშლილია");
                    }
                    else
                    {
                        Console.WriteLine("ასეთი კონტაქტი არ არსებობს");
                    }
                    break;
                case 3:
                    Console.WriteLine("შეიყვანე კონტაქტის სახელი");
                    string changeContact= Console.ReadLine();
                    if (contacts.ContainsKey(changeContact))
                    {
                        Console.WriteLine("შეიყვანე ახლაი ნომერი");
                        string changedNumber= Console.ReadLine();
                        contacts[changeContact] = changedNumber;
                        
                    }

                    else
                    {
                        Console.WriteLine("ასეთი კონტაქტი არ არსებობს");
                    }
                    
                    break;
                case 4:
                    return;
                     
            }
            
            
        }
        #endregion

    }
    }
