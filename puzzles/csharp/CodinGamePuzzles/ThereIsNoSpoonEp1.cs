using System;

namespace CodinGamePuzzles
{
	internal class ThereIsNoSpoonEp1
	{
		public static void Main(string[] args)
		{
			var map = Map.Load();
			map.ListNeighbours();
		}

		private class Map
		{
			private Map(int width, int height)
			{
				this.width = width;
				this.height = height;

				this.nodes = new char[this.height][];
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
				for (int i = 0; i < this.height; i++)
				{
					for (int j = 0; j < this.width; j++)
					{
						WriteNodeNeighbours(j, i);
					}
				}
			}

			private void WriteNodeNeighbours(int x, int y)
			{
				if (!IsNode(x, y))
				{
					return;
				}

				int rightX, rightY;
				FindRightNeighbour(x, y, out rightX, out rightY);

				int bottomX, bottomY;
				FindBottomNeighbour(x, y, out bottomX, out bottomY);

				Console.WriteLine("{0} {1} {2} {3} {4} {5}", x, y, rightX, rightY, bottomX, bottomY);
			}

			private bool IsNode(int x, int y)
			{
				if (x >= width)
				{
					return false;
				}

				if (y >= height)
				{
					return false;
				}

				return Equals(Node, this.nodes[y][x]);
			}

			private void FindRightNeighbour(int x, int y, out int rightX, out int rightY)
			{
				rightX = -1;
				rightY = -1;

				for (int i = x + 1; i < this.width; i++)
				{
					if (IsNode(i, y))
					{
						rightX = i;
						rightY = y;
						break;
					}
				}
			}

			private void FindBottomNeighbour(int x, int y, out int bottomX, out int bottomY)
			{
				bottomX = -1;
				bottomY = -1;

				for (int i = y + 1; i < this.height; i++)
				{
					if (IsNode(x, i))
					{
						bottomX = x;
						bottomY = i;
						break;
					}
				}
			}

			private readonly int height;
			private readonly int width;

			private readonly char[][] nodes;

			private const char Node = '0';
		}
	}
}