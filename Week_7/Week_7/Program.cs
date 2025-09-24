namespace Week_7;

class Program
{
    static void Main(string[] args)
    {
        #region პირველი დავალება

        Console.WriteLine("შეიყვანეთ დიდი წრის რადიუსი");
        double radius;
        double.TryParse(Console.ReadLine(), out radius);
        double areaofBigSquare = Math.Pow(2*radius, 2);
        double areaofSmallSquare = Math.Pow(2*radius, 2)/2;
        Console.WriteLine($"სხვაობა არის: {areaofBigSquare - areaofSmallSquare}");

        #endregion

        #region მეორე დავალება
        Console.WriteLine("შეიყვანე სიმბოლოები");
        string userInput = Console.ReadLine().Trim();
        if (string.IsNullOrEmpty(userInput))
        {
            Console.WriteLine("ცარიელი სტრინგი");
        }
        else
        {
            char firstSymbol = userInput[0];
            var answer=userInput.Count(c => c == firstSymbol) == userInput.Length;
            Console.WriteLine(answer ? "Yes" : "No");

        }
      

        #endregion

        #region მესამე დავალება

        Console.WriteLine("შეიყვანე მოგების, ფრის და წაგების რაოდენობა");
        
        Console.Write("მოგება: ");
        int win;
        int.TryParse(Console.ReadLine(), out win);
        
        Console.Write("ფრე: ");
        int draw;
        int.TryParse(Console.ReadLine(), out draw);
        
        Console.Write("წაგება: ");
        int loss;
        int.TryParse(Console.ReadLine(), out loss);
        
        Console.WriteLine($"ჯამური ქულა: {3 * win + draw}");
        
        

        #endregion

        #region მეოთხე დავალება
        
        string[] numbers = Console.ReadLine().Split(",");
        int[] nums = Array.ConvertAll(numbers,int.Parse);
        int satSalary = nums[nums.Length - 2]*20;
        int sunSalary = nums[nums.Length - 1]*20;
        int weekDaysSalary=0;
        for (int i = 0; i < 5; i++)
        {
            if (nums[i] <= 8)
            {
                weekDaysSalary+=nums[i]*10;
            }
            else
            {
                weekDaysSalary+=80+(nums[i]-8)*15;
        
            }
             
        }
        
        Console.WriteLine(sunSalary+satSalary+weekDaysSalary);


        #endregion

        #region მეხუთე დავალება

        int[] marathon = Array.ConvertAll(Console.ReadLine().Split(","),int.Parse);
        
        int countDays = 0;
        for (int i = 1; i < marathon.Length; i++)
        {
            if (marathon[i] > marathon[i-1])
            {
                countDays++;
            }
        }
        
        Console.WriteLine(countDays);

        #endregion

        #region მეექვსე დავალება
        Console.WriteLine("შეიყვანე სიტყვის სიგრძე");
        int N;
        int.TryParse(Console.ReadLine(), out N);
        Console.WriteLine("შეიყვნე სიტყვების მიმდევრობა");
        string[] words = Console.ReadLine().Split(",").Select(x=>x.Trim()).ToArray();

        var temp = words.Where(x => x.Length == N);

        if (temp.Any())
        {
            foreach (var item in temp)
            {
                Console.WriteLine(item);
            }
        }
        else
        {
            Console.WriteLine("No elements found");

        }
        
        


        #endregion

    }
}




 