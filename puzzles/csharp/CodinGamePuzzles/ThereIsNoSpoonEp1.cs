using System;

namespace CodinGamePuzzles
{
	internal class ThereIsNoSpoonEp1
	{
		private static void Main(string[] args)
		{
			var map = Map.Load();
		}

		private class Map
		{
			private Map(int width, int height)
			{
				this.width = width;
				this.height = height;

				this.nodes = new char[this.width][];
			}

			public static Map Load()
			{
				int width = int.Parse(Console.ReadLine());
				int height = int.Parse(Console.ReadLine());

				var map = new Map(width, height);
				map.LoadNodes();

				return map;
			}

			private void LoadNodes()
			{
				for (int i = 0; i < height; i++)
				{
					string line = Console.ReadLine();
					this.nodes[i] = line.ToCharArray();
				}
			}

			public void ListNeighbours()
			{
				// Three coordinates: a node, its right neighbor, its bottom neighbor
				Console.WriteLine("0 0 1 0 0 1");
			}

			private readonly int height;
			private readonly int width;

			private readonly char[][] nodes;

			private const char Node = '0';
			private const char Empty = '.';
		}
	}
}