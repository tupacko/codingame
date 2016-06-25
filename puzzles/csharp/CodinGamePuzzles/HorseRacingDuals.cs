using System;
using System.Collections.Generic;
using System.Linq;

internal class HorseRacingDuals
{
	private static IEnumerable<int> GetPowers()
	{
		int N = int.Parse(Console.ReadLine());
		for (int i = 0; i < N; i++)
		{
			yield return int.Parse(Console.ReadLine());
		}
	}

	private static int GetPowerDifference()
	{
		var orderedPowers = GetPowers().OrderBy(x => x).ToArray();
		var shiftPowers = orderedPowers.Skip(1).ToArray();
		var differences = shiftPowers.Zip(orderedPowers, (x, y) => x - y);

		return differences.OrderBy(x => x).FirstOrDefault();
	}

	private static void Main(string[] args)
	{
		var powerDiff = GetPowerDifference();

		Console.WriteLine("{0}", powerDiff);
	}
}