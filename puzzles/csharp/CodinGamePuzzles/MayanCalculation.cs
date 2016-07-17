using System;
using System.Linq;

namespace CodinGamePuzzles
{
	internal class MayanCalculation
	{
		private static void Main(string[] args)
		{
			string[] inputs = Console.ReadLine().Split(' ');
			int numeralWidth = int.Parse(inputs[0]);
			int numeralHeight = int.Parse(inputs[1]);

			var numerals = new NumeralSet(numeralWidth, numeralHeight);
			numerals.Load();

			int numberOfLines = int.Parse(Console.ReadLine());
			var firstNumber = new Number(numberOfLines / numeralHeight, numerals);
			firstNumber.Load();

			numberOfLines = int.Parse(Console.ReadLine());
			var secondNumber = new Number(numberOfLines / numeralHeight, numerals);
			secondNumber.Load();

			string operation = Console.ReadLine();
			var result = Calculate(firstNumber, operation, secondNumber);
		}

		private static Number Calculate(Number firstNumber, string operation, Number secondNumber)
		{
			switch (operation)
			{
				case "+":
					return firstNumber + secondNumber;

				case "-":
					return firstNumber - secondNumber;

				case "*":
					return firstNumber * secondNumber;

				case "/":
					return firstNumber / secondNumber;

				default:
					throw new Exception("Invalid operation");
			}
		}

		private class NumeralSet
		{
			public NumeralSet(int numberWidth, int numberHeight)
			{
				this.width = numberWidth;
				this.height = numberHeight;

				this.numerals = new Numeral[NUMBER_OF_NUMERALS];
			}

			public void Load()
			{
				var rawData = ReadRawData();
				LoadNumbers(rawData);
			}

			public Numeral LoadNumeral()
			{
				var numeralData = ReadRawData();

				return FindNumeral(numeralData);
			}

			private string[] ReadRawData()
			{
				var rawData = new string[this.height];
				for (int i = 0; i < this.height; i++)
				{
					rawData[i] = Console.ReadLine();
				}

				return rawData;
			}

			private void LoadNumbers(string[] rawData)
			{
				for (int i = 0; i < NUMBER_OF_NUMERALS; i++)
				{
					this.numerals[i] = new Numeral(i, DequeueNextNumberData(rawData));
				}
			}

			public string[] DequeueNextNumberData(string[] rawData)
			{
				var data = new string[this.height];
				for (int i = 0; i < this.height; i++)
				{
					data[i] = rawData[i].Substring(0, this.width);
					rawData[i] = rawData[i].Substring(this.width);
				}

				return data;
			}

			private Numeral FindNumeral(string[] numeralData)
			{
				return this.numerals.First(x => x.IsMatch(numeralData));
			}

			private const int NUMBER_OF_NUMERALS = 20;

			private readonly int width;
			private readonly int height;

			private readonly Numeral[] numerals;
		}

		private class Numeral
		{
			public Numeral(int value, string[] numeralData)
			{
				this.value = value;
				this.data = numeralData;
			}

			public static explicit operator int(Numeral numeral)
			{
				return numeral.value;
			}

			public bool IsMatch(string[] numeralData)
			{
				return this.data.SequenceEqual(numeralData);
			}

			private readonly int value;
			private readonly string[] data;
		}

		private class Number
		{
			public Number(int numeralsCount, NumeralSet numerals)
			{
				this.numeralsNumber = numeralsCount;
				this.numeralsSet = numerals;
			}

			private Number(int value)
			{
				this.value = value;
			}

			public static Number operator +(Number first, Number second)
			{
				return new Number(first.value + second.value);
			}

			public static Number operator -(Number first, Number second)
			{
				return new Number(first.value - second.value);
			}

			public static Number operator *(Number first, Number second)
			{
				return new Number(first.value * second.value);
			}

			public static Number operator /(Number first, Number second)
			{
				return new Number(first.value / second.value);
			}

			public void Load()
			{
				int powerOfBase = this.numeralsNumber - 1;
				int localValue = 0;

				for (int i = 0; i < this.numeralsNumber; i++)
				{
					var numeralValue = GetNextNumeralValue();

					localValue += numeralValue * (int)Math.Pow(BASE, powerOfBase);
					powerOfBase--;
				}

				Console.Error.WriteLine(localValue);

				this.value = localValue;
			}

			private int GetNextNumeralValue()
			{
				var numeral = this.numeralsSet.LoadNumeral();

				return (int)numeral;
			}

			private const int BASE = 20;

			private readonly int numeralsNumber;
			private readonly NumeralSet numeralsSet;

			private int value;
		}
	}
}