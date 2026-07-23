using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Common
{
    public class ConsoleHelper
    {
        public int ShowArrowMenu(string title,string[] options)
        {
            int chosenRow = 0;

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
    }
}
