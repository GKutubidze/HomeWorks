namespace Practise;

class Program
{
    static void Main(string[] args)
    {
     Console.WriteLine(FindFirstUniqueChar("AABB") );   
     Console.WriteLine(CountWords("Hello , d ,") );
     Console.WriteLine(ThirdUniqueOrMax([3,2,1]) );
    }

    static int FindFirstUniqueChar(string s)
    {
         
        var result = 
            s.GroupBy(c => c).Where(g => g.Count() == 1).FirstOrDefault();

        return result == null ? -1 : s.IndexOf(result.Key);
    }
    
    static int CountWords(string s)
    {
        return s.Split(' ', System.StringSplitOptions.RemoveEmptyEntries)
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Count();
    }

    static int ThirdUniqueOrMax(int[] s)
    {
        var unique = s
            .GroupBy(c => c)
            .Where(g => g.Count() == 1)
            .OrderByDescending(x => x.Key)
            .Select(x => x.Key)
            .ToList();

        return unique.Count >= 3 ? unique[2] : s.Max();
    }
    
    static int AverageWordLength(string s)
    {
        return (int)s.Split(' ')
            .Average(x => x.Length);
    }
    
}




// იპოვეთ სიტყვების სიგრძეების საშუალო მნიშვნელობა წინადადებაში
// Input: &quot;Hello my friend&quot;
// Output: 5
//     (სიგრძე: 5,2,6 → საშუალო = 13/3 ≈ 5)
//
// Input: &quot;I am here&quot;
// Output: 2
//     (სიგრძე: 1,2,4 → საშუალო = 7/3 ≈ 2)

 
