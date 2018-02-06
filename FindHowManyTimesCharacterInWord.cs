using System;
using System.Collections.Generic;

namespace _2DArray
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter any word");
            string UserInput = Console.ReadLine();
            string OutputString = "";

            List<string> OutputArray = new List<string>();

            for (int i = 0; i < UserInput.Length; i++)
            {
                int count = 0;
                for(int j = 0; j < UserInput.Length; j++)
                {
                    if(UserInput[i] == UserInput[j])
                    {
                        count++;
                    }
                }

                string result = UserInput[i] + count.ToString();
                if (!OutputArray.Contains(result))
                {
                    OutputArray.Add(UserInput[i] + count.ToString());
                }
            }

            OutputArray.Sort();

            foreach(string item in OutputArray)
            {
                OutputString += item;
            }

            Console.WriteLine("{0}", OutputString.ToString());
            
            System.Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }
    }
}
