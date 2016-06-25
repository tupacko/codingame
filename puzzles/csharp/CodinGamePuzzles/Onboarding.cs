using System;
using System.Collections.Generic;
using System.Linq;

internal class Onboarding
{
	private class EnemyInfo
	{
		public EnemyInfo(string name, string distance)
		{
			Name = name;
			Distance = int.Parse(distance);
		}

		public string Name { get; private set; }
		public int Distance { get; private set; }
	}

	private static IEnumerable<EnemyInfo> GetEnemies()
	{
		yield return new EnemyInfo(Console.ReadLine(), Console.ReadLine());
		yield return new EnemyInfo(Console.ReadLine(), Console.ReadLine());
	}

	private static void Main(string[] args)
	{
		while (true)
		{
			var closestThreat = GetEnemies().OrderBy(e => e.Distance).First();

			Console.WriteLine(closestThreat.Name);
		}
	}
}