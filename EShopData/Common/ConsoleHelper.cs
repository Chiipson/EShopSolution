using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace EShopData.Common
{
    public class ConsoleHelper
    {
        public int ShowArrowMenu(string title,string[] options, int arrowPosition = 0)
        {
            int chosenRow = arrowPosition;

            while (true)
            {
                Console.Clear();

                Console.WriteLine(title + '\n');

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == chosenRow)
                    {
                        Console.Write("> ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                    Console.WriteLine(options[i]);
                }

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        chosenRow--;
                        if (chosenRow < 0)
                        {
                            chosenRow = options.Length - 1;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        chosenRow++;
                        if (chosenRow >= options.Length)
                        {
                            chosenRow = 0;
                        }
                        break;

                    case ConsoleKey.Enter:
                        return chosenRow;
                }
            }
        }

        public void PrintUserMessage(string massage, int durationInSeconds)
        {
            Console.Clear();

            Console.WriteLine(massage);

            Thread.Sleep(durationInSeconds * 1000);
        }

        public string GetString(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        public T GetNumber<T>(string message) where T : INumber<T>
        {
            while (true)
            {
                Console.Write(message);
                if(T.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture, out T number))
                {
                    return number;
                }
            }
        }
    }
}
