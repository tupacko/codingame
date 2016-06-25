using System;
using System.Collections.Generic;

internal class PowerOfThorEp1
{
	private static void Main(string[] args)
	{
		var response = new Dictionary<int, string>
		{
			{-1, "W"},
			{1, "E"},
			{-10, "N"},
			{10, "S"},
			{-11, "NW"},
			{-9, "NE"},
			{11, "SE"},
			{9, "SW"}
		};

		string[] inputs = Console.ReadLine().Split(' ');
		int LX = int.Parse(inputs[0]); // the X position of the light of power
		int LY = int.Parse(inputs[1]); // the Y position of the light of power
		int TX = int.Parse(inputs[2]); // Thor's starting X position
		int TY = int.Parse(inputs[3]); // Thor's starting Y position

		// game loop
		while (true)
		{
			int E = int.Parse(Console.ReadLine()); // The level of Thor's remaining energy, representing the number of moves he can still make.

			// Write an action using Console.WriteLine() To debug: Console.Error.WriteLine("Debug messages...");
			int h = LX - TX;
			int v = LY - TY;
			int hDiff = h > 0 ? 1 : h < 0 ? -1 : 0;
			int vDiff = v > 0 ? 1 : v < 0 ? -1 : 0;
			TX += hDiff;
			TY += vDiff;
			Console.WriteLine("{0}", response[hDiff + vDiff * 10]); // A single line providing the move to be made: N NE E SE S SW W or NW
		}
	}
}