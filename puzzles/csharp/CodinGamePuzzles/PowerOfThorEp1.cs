using System;
using System.Collections.Generic;
using System.Linq;

internal class PowerOfThorEp1
{
	private class Light
	{
		public Light(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int X { get; private set; }
		public int Y { get; private set; }
	}

	private class Thor
	{
		public Thor(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int X { get; private set; }
		public int Y { get; private set; }

		/// <summary>
		/// Reads the level of Thor's remaining energy, representing the number of moves he can still make.
		/// </summary>
		public void ReadEnergy()
		{
			int.Parse(Console.ReadLine());
		}

		public void Move(Light light)
		{
			int horizontalDistance = light.X - X;
			int verticalDistance = light.Y - Y;

			int horizontalStep = Aim(horizontalDistance);
			int verticalStep = Aim(verticalDistance);

			X += horizontalStep;
			Y += verticalStep;
			this.direction = horizontalStep + verticalStep * 10;
		}

		private int Aim(int distanceToTarget)
		{
			return distanceToTarget > 0 ? 1 : distanceToTarget < 0 ? -1 : 0;
		}

		public void TellDirection()
		{
			Console.WriteLine("{0}", ResponseMap[this.direction]);
		}

		private int direction;

		private static readonly Dictionary<int, string> ResponseMap = new Dictionary<int, string>
		{
			{0, string.Empty },
			{-1, "W"},
			{1, "E"},
			{-10, "N"},
			{10, "S"},
			{-11, "NW"},
			{-9, "NE"},
			{11, "SE"},
			{9, "SW"}
		};
	}

	private static void Main(string[] args)
	{
		int[] inputs = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
		var light = new Light(inputs[0], inputs[1]);
		var thor = new Thor(inputs[2], inputs[3]);

		while (true)
		{
			thor.Move(light);
			thor.TellDirection();
		}
	}
}