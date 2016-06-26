using System;
using System.Linq;

internal class Temperatures
{
	private static void Main(string[] args)
	{
		int numberOfTemperatures = int.Parse(Console.ReadLine());
		if (0 == numberOfTemperatures)
		{
			Console.WriteLine("0");
			return;
		}

		string temperaturesText = Console.ReadLine();
		var finder = new Finder(temperaturesText);

		Console.Write(finder.FindResult());
	}

	private class Finder
	{
		public Finder(string temperatures)
		{
			this.temps = temperatures.Split(' ').Select(int.Parse).ToArray();
		}

		public int FindResult()
		{
			var closestTwo = this.temps.OrderBy(Math.Abs).Take(2).ToArray();
			var closest = closestTwo[0];

			if (ShouldTakePositive(closestTwo))
			{
				return Math.Abs(closest);
			}

			return closest;
		}

		private bool ShouldTakePositive(int[] closeTemps)
		{
			if (1 == closeTemps.Length)
			{
				return false;
			}

			return 0 == (closeTemps[0] + closeTemps[1]);
		}

		private readonly int[] temps;
	}
}