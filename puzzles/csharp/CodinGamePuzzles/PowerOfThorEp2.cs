using System;
using System.Collections.Generic;
using System.Linq;

internal class PowerOfThorEp2
{
	private static void Main(string[] args)
	{
		int[] thorPosition = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();

		int[] inputs = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
		var thor = new Thor(thorPosition[0], thorPosition[1], inputs[0]);
		var map = new Map();

		do
		{
			int numberOfGiants = inputs[1];
			map.RefreshMap(numberOfGiants);
			thor.TakeAction(map);

			inputs = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
		} while (true);
	}

	private class Thor
	{
		public Thor(int x, int y, int hammerStrikes)
		{
			X = x;
			Y = y;

			this.strikes = hammerStrikes;
		}

		public int X { get; private set; }
		public int Y { get; private set; }

		public void TakeAction(Map map)
		{
			var targetPosition = map.GetMaximumGiantDensity();
			var nextPosition = CalculateNextPosition(targetPosition);
			var currentPosition = new Tuple<int, int>(X, Y);

			if (map.IsSafe(nextPosition))
			{
				Move(nextPosition);
				return;
			}

			if (!map.IsSafe(currentPosition))
			{
				Strike();
				return;
			}

			Wait();
		}

		private Tuple<int, int> CalculateNextPosition(Tuple<int, int> targetPosition)
		{
			int horizontalDistance = targetPosition.Item1 - X;
			int verticalDistance = targetPosition.Item2 - Y;

			int horizontalStep = Aim(horizontalDistance);
			int verticalStep = Aim(verticalDistance);

			return new Tuple<int, int>(X + horizontalStep, Y + verticalStep);
		}

		private void Move(Tuple<int, int> targetPosition)
		{
			int horizontalDistance = targetPosition.Item1 - X;
			int verticalDistance = targetPosition.Item2 - Y;

			int horizontalStep = Aim(horizontalDistance);
			int verticalStep = Aim(verticalDistance);

			X += horizontalStep;
			Y += verticalStep;
			this.direction = horizontalStep + verticalStep * 10;

			Console.WriteLine("{0}", ResponseMap[this.direction]);
		}

		private int Aim(int distanceToTarget)
		{
			return distanceToTarget > 0 ? 1 : distanceToTarget < 0 ? -1 : 0;
		}

		private void Strike()
		{
			Console.WriteLine(STRIKE);
			this.strikes--;
		}

		private void Wait()
		{
			Console.WriteLine(WAIT);
		}

		private const string STRIKE = "STRIKE";

		private const string WAIT = "WAIT";

		private static readonly Dictionary<int, string> ResponseMap = new Dictionary<int, string>
		{
			{0, WAIT },
			{-1, "W"},
			{1, "E"},
			{-10, "N"},
			{10, "S"},
			{-11, "NW"},
			{-9, "NE"},
			{11, "SE"},
			{9, "SW"}
		};

		private int strikes;

		private int direction;
	}

	private class Map
	{
		public void RefreshMap(int numberOfGiants)
		{
			this.giants.Clear();

			for (int i = 0; i < numberOfGiants; i++)
			{
				var inputs = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
				this.giants.Add(new Tuple<int, int>(inputs[0], inputs[1]));
			}
		}

		public Tuple<int, int> GetMaximumGiantDensity()
		{
			return giants.Aggregate(new Tuple<int, int>(0, 0), (r, c) => new Tuple<int, int>(r.Item1 + c.Item1, r.Item2 + c.Item2), r => new Tuple<int, int>(r.Item1 / this.giants.Count, r.Item2 / this.giants.Count));
		}

		public bool IsSafe(Tuple<int, int> position)
		{
			foreach (var giant in this.giants)
			{
				if (Math.Abs(giant.Item1 - position.Item1) <= 1 && Math.Abs(giant.Item2 - position.Item2) <= 1)
				{
					return false;
				}
			}

			return true;
		}

		private IList<Tuple<int, int>> giants = new List<Tuple<int, int>>();
	}
}