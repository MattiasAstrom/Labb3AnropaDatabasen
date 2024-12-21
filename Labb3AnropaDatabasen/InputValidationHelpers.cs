using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3AnropaDatabasen
{
    static internal class InputValidationHelpers
    {
        static public int GetValidMenuChoice(int min, int max)
        {
            int choice;
            while (true)
            {
                ConsoleKeyInfo input = Console.ReadKey();
                if (int.TryParse(input.KeyChar.ToString(), out choice) && choice >= min && choice <= max)
                {
                    return choice;
                }
                Console.WriteLine($"Invalid input! Please select a number between {min} and {max}.");
            }
        }

        static public string GetValidSortingOption<T>(T first, T second)
        {
            string option;
            while (true)
            {
                option = Console.ReadLine();
                
                if (option == "1" || option == "2")
                {
                    return option;
                }

                Console.WriteLine($"Invalid choice. Please select one: \n 1. {first}) \n ({second}).");
            }
        }

        static public int GetValidClassChoice(int max)
        {
            int choice;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= max)
                {
                    return choice;
                }
                Console.WriteLine($"Invalid class choice! Please select a number between 1 and {max}.");
            }
        }

        static public string GetNonEmptyString(string prompt)
        {
            string input;
            while (true)
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    return input;
                }
                Console.WriteLine("Input cannot be empty. Please try again.");
            }
        }
    }
}
