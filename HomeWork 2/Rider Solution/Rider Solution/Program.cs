using System;

namespace Homework2
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear(); // 
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Giorgi Kutubidze");

            Console.Write("Write something: ");
            string input = Console.ReadLine();

            Console.WriteLine(input);
 
        }
    }
}