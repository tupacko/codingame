using System;
using System.Linq;

internal class Temperatures
{
	private class Finder
	{
		public Finder(string temperatures)
		{
			if (string.IsNullOrWhiteSpace(temperatures))
			{
				this.temps = new int[0];
				return;
			}

			this.temps = temperatures.Split(' ').Select(s => int.Parse(s)).ToArray();
		}

		public int FindResult()
		{
			if (0 == this.temps.Length)
			{
				return 0;
			}

			return FindClosestToZero();
		}

		private int FindClosestToZero()
		{
			var firstOrderedTemps = this.temps.OrderBy(t => Math.Abs(t)).Take(2).ToArray();
			var closest = firstOrderedTemps[0];

			if (ShouldTakePositive(firstOrderedTemps))
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

	private static void Main(string[] args)
	{
		int N = int.Parse(Console.ReadLine()); // the number of temperatures to analyse
		string TEMPS = Console.ReadLine(); // the N temperatures expressed as integers ranging from -273 to 5526

		var finder = new Finder(TEMPS);

		Console.Write(finder.FindResult());
	}
}