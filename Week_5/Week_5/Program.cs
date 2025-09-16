namespace Week_5;

class Program
{
    static void Main(string[] args)
    {
        
        // #1 
        Console.WriteLine("შეიყვანე რიცხვი");
        int number = Convert.ToInt16(Console.ReadLine());
        if (number % 5 == 0)
        {
            Console.WriteLine("Yes");
        }
        else
        {
            Console.WriteLine("No");

        }
        


        // #2
        Console.WriteLine("შეიყვანე პირველი რიცხვი");
        int x = Convert.ToInt16(Console.ReadLine());
        Console.WriteLine("შეიყვანე მეორე რიცხვი");
        int y = Convert.ToInt16(Console.ReadLine());
        int max = x > y ? x : y;
        int min= x < y ? x : y;

        
        
        Console.WriteLine($"X+Y = {x + y}");
        Console.WriteLine($"X-Y = {max - min}");
        Console.WriteLine($"X*Y = {x*y}");
        if (min==0)
        {
            Console.WriteLine("შეცდომა: 0-ზე გაყოფა არ შეიძლება.");
        }
        else
        {
            int division = max / min; // მთელზე გაყოფა
            Console.WriteLine($"გაყოფა: {max} / {min} = {division}");
        }
        
        
        
        
        //#3  
        Console.WriteLine("შეიყვანე პირველი რიცხვი");
        int firstNum = Convert.ToInt16(Console.ReadLine());
        Console.WriteLine("შეიყვანე მეორე რიცხვი");
        int secondNum= Convert.ToInt16(Console.ReadLine());
        int temp = firstNum;
        firstNum = secondNum;
        secondNum = temp;
        Console.WriteLine($"პირველი რიცხვი:{firstNum} მეორე რიცხვი:{secondNum}");
        
        
        //#4 
        Console.WriteLine("შეიყვანე პირველი რიცხვი");
        int num = Convert.ToInt16(Console.ReadLine());
        for (int i = 1; i < 10; i++)
        {
            Console.WriteLine($"{num} * {i} = {num * i}");
        }
        
        //#5 ლუწი რიცხვები 1-დან n-მდე
        Console.WriteLine("შეიყვანე რიცხვი");
        int num2 = Convert.ToInt16(Console.ReadLine());
        for (int i = 1; i <= num2; i++)
        {
            if (i % 2 == 0)
            {
                Console.WriteLine($"{i*i}");

            }
        }

    }
}