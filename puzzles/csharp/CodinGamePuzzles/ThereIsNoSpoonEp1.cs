using System;

namespace CodinGamePuzzles
{
	internal class ThereIsNoSpoonEp1
	{
		private static void Main(string[] args)
		{
			int width = int.Parse(Console.ReadLine()); // the number of cells on the X axis
			int height = int.Parse(Console.ReadLine()); // the number of cells on the Y axis
			for (int i = 0; i < height; i++)
			{
				string line = Console.ReadLine(); // width characters, each either 0 or .
			}

			// Write an action using Console.WriteLine() To debug: Console.Error.WriteLine("Debug messages...");

			// Three coordinates: a node, its right neighbor, its bottom neighbor
			Console.WriteLine("0 0 1 0 0 1");
		}
	}
}