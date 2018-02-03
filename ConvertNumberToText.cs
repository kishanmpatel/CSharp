using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace coding_practice
{
	class Program
	{
		static string[] Ones = new string[] { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
		static string[] Tens = new string[] { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
		static string[] Teens = new string[] { "Ten", "Eleven", "Twele", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
		static List<int> lstPowerOfTen = new List<int> { 10000, 1000, 100, 10 };
		static void Main()
		{
			Console.WriteLine("Please enter");
			string UserInput = Console.ReadLine();

			int Number = int.Parse(UserInput);

			string NumberInTextFormat = ConvertDigitToText(Number);

			//char[] ArrNumber = (Number.ToString()).ToCharArray();

			Console.WriteLine(NumberInTextFormat);

			Console.WriteLine("Press any key to continue...");
			Console.Read();
		}

		public static string ConvertDigitToText(int num)
		{
			if (num == 0)
			{
				return "Zero";
			}
			else
			{
				return ConvertThousandToText(num);
			}
		}

		public static string ConvertThousandToText(int num)
		{
			if (num >= 1000)
			{
				return Ones[num / 1000] + "Thousand" + ConvertHundredsToText(num % 1000);
			}
			else
			{
				return ConvertHundredsToText(num);
			}

		}

		public static string ConvertHundredsToText(int num)
		{
			if (num >= 100)
			{
				return Ones[num / 100] + "Hundred" + ConvertTensToText(num % 100);
			}
			else
			{
				return ConvertTensToText(num);
			}
		}

		public static string ConvertTensToText(int num)
		{
			if (num < 10)
			{
				return Ones[num];
			}
			else if (num >= 10 && num < 20)
			{
				return Teens[num - 10];
			}
			else
			{
				return Tens[num / 10] + Ones[num % 10];
			}
		}
	}
}