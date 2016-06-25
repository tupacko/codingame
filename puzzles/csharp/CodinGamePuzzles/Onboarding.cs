using System;
using System.Collections.Generic;
using System.Linq;

/**
 * The code below will read all the game information for you.
 * On each game turn, information will be available on the standard input, you will be sent:
 * -> the total number of visible enemies
 * -> for each enemy, its name and distance from you
 * The system will wait for you to write an enemy name on the standard output.
 * Once you have designated a target:
 * -> the cannon will shoot
 * -> the enemies will move
 * -> new info will be available for you to read on the standard input.
 **/

internal class Onboarding
{
	private class EnemyInfo
	{
		public string Name { get; set; }
		public int Distance { get; set; }
	}

	private static IEnumerable<EnemyInfo> GetEnemies()
	{
		int count = int.Parse(Console.ReadLine()); // The number of current enemy ships within range
		for (int i = 0; i < count; i++)
		{
			string[] inputs = Console.ReadLine().Split(' ');
			string enemy = inputs[0]; // The name of this enemy
			int distance = int.Parse(inputs[1]); // The distance to your cannon of this enemy

			yield return new EnemyInfo { Name = enemy, Distance = distance };
		}
	}

	private static void Main(string[] args)
	{
		// game loop
		while (true)
		{
			var closestThreat = GetEnemies().OrderBy(e => e.Distance).First();

			// Write an action using Console.WriteLine() To debug: Console.Error.WriteLine("Debug messages...");

			Console.WriteLine(closestThreat.Name); // The name of the most threatening enemy (HotDroid is just one example)
		}
	}
}