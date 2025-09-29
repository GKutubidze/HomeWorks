using System.Runtime.ExceptionServices;

namespace Week_8;

class Program
{
    static void Main(string[] args)
    {

        
        

        Console.WriteLine("=== Test 1: first() ===");
        Console.WriteLine($"first(0, 100, 2) = {first(0, 100, 2)} ");   
        Console.WriteLine($"first(49, 71, 2) = {first(49, 71, 2)} ");
        Console.WriteLine($"first(2, 27, 4) = {first(2, 27, 4)} ");  
        Console.WriteLine($"first(1, 1000, 3) = {first(1, 1000, 3)} ");  
        Console.WriteLine($"first(100, 200, 2) = {first(100, 200, 2)} ");  
        Console.WriteLine();
        
        Console.WriteLine("=== Test 2: second() === ");
        Console.WriteLine($"second(\"AA\") = {second("AA")} (Expected: 1)");
        Console.WriteLine($"second(\"AABBCC\") = {second("AABBCC")}");
        Console.WriteLine($"second(\"AABBC\") = {second("AABBC")} ");
        Console.WriteLine($"second(\"ABABC\") = {second("ABABC")} ");
        Console.WriteLine($"second(\"AAABB\") = {second("AAABB")} ");
        Console.WriteLine($"second(\"AAABBBCCCC\") = {second("AAABBBCCCC")} ");
        Console.WriteLine();

        Console.WriteLine("=== Test 3: third() === ");
        Console.WriteLine($"third(\"multiplication\", \"substraction\") = \"{third("multiplication", "substraction")}\" ");
        Console.WriteLine($"third(\"Some Random Text\", \"It is Some Random Text\") = \"{third("Some Random Text", "It is Some Random Text")}");
        Console.WriteLine($"third(\"programming\", \"gaming\") = \"{third("programming", "gaming")}\" ");
        Console.WriteLine($"third(\"hello\", \"world\") = \"{third("hello", "world")}\"");
        Console.WriteLine($"third(\"testing\", \"testing\") = \"{third("testing", "testing")}\" ");
        Console.WriteLine();

        Console.WriteLine("=== Test 4: fourth()   ===");
        Console.WriteLine("Test 4a: String List");
        List<string> stringList = new List<string>() { "test", "random", "programming", "word" };
        fourth(stringList);
        
        
        
        Console.WriteLine("\nTest 4c: Int List (More numbers)");
        List<int> intList2 = new List<int>() { 1, 2, 3, 4, 5 };
        fourth(intList2);
        
        Console.WriteLine("\nTest 4d: Bool List");
        List<bool> boolList = new List<bool>() { true, false, true, false, true, false, false };
        fourth(boolList);
        Console.WriteLine();

        Console.WriteLine("=== Test 5: fifth() ===");
        Console.Write("fifth(12345) = ");
        fifth(12345);
        Console.WriteLine();
         
        Console.Write("fifth(999) = ");
        fifth(999);
        Console.WriteLine();

        
        Console.Write("fifth(1) = ");
        fifth(1);
        Console.WriteLine();

        Console.Write("fifth(2024) = ");
        fifth(10001);
        Console.WriteLine();
        Console.WriteLine();


        Console.WriteLine("=== Test 6: sixth() ===");
        Console.WriteLine($"sixth([1,2,3,1]) = {sixth(new int[] { 1, 2, 3, 1 })} ");
        Console.WriteLine($"sixth([1,2,3,4]) = {sixth(new int[] { 1, 2, 3, 4 })}  ");
        Console.WriteLine($"sixth([1,1,1,1]) = {sixth(new int[] { 1, 1, 1, 1 })}  ");
        Console.WriteLine($"sixth([5]) = {sixth(new int[] { 5 })}  ");
        Console.WriteLine($"sixth([10,20,30,20]) = {sixth(new int[] { 10, 20, 30, 20 })}  ");







    }
    
    static int first(int a, int b, int n)
    {
            
            
        int start = (int)Math.Round(Math.Pow(a, 1.0 / n)); 
        int end   = (int)Math.Round(Math.Pow(b, 1.0 / n));
        if (Math.Pow(start, n) < a) start++;
        if (Math.Pow(end, n) > b) end--;
        return (end < start) ? 0 : end - start + 1;


    }
    
 
 
    static int second(string word)
    {
        return word.GroupBy(x => x).Sum(x => x.Count()/2);
             
    }
        
    
    static string third(string word1, string word2)
    {
        int index1 = word1.Length - 1 ;
        int index2 = word2.Length - 1;
        string result = "";
        while (index1 >= 0 && index2 >= 0 && word1[index1] == word2[index2])
        {
            result += word1[index1];
            index1--;
            index2--;
        }
        
        return new string(result.Reverse().ToArray());
    }

    static void fourth<T>(List<T> list)
    {

        if (typeof(T) == typeof(string))
        {
            foreach (var item in list)
            {
                Console.WriteLine(item.ToString().ToUpper());
            }
            
        }
        else if (typeof(T) == typeof(int))
        {
            var intList = list.Cast<int>().ToList();
            Console.WriteLine(intList.Sum());
        }
        
        else if (typeof(T) == typeof(bool))
        {
            Console.WriteLine($"First element is  {list[0] }");
            Console.WriteLine($"Last element is  {list.Last() }");
            Console.WriteLine($"Middle element is  {list[list.Count/2] }");

        }
        
    } 
        
    


 
    static void fifth(int number)
    {
        if (number < 10)
        {
            Console.Write(number);
            return;
        }

        fifth(number / 10);      
        Console.Write(" - " + (number % 10));  
    }

  
 
    static bool sixth(int[] nums)
    {
        return nums.GroupBy(x => x).Any(x => x.Count()>1);
    }

}


