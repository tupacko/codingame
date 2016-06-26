using System;
using System.Linq;

internal class MarsLanderEp1
{
	private static void Main(string[] args)
	{
		TrashInitialData();
		while (true)
		{
			var data = ReadArray();
			var currentTurn = new MarsLander(data);

			currentTurn.Move();
		}
	}

	private static void TrashInitialData()
	{
		int initialDataCount = int.Parse(Console.ReadLine());
		for (int i = 0; i < initialDataCount; i++)
		{
			Console.ReadLine();
		}
	}

	private static int[] ReadArray()
	{
		return Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
	}

	private class MarsLander
	{
		public MarsLander(int[] data)
		{
			this.verticalSpeed = data[VERTICAL_SPEED_INDEX];
		}

		public void Move()
		{
			var power = CalculatePower();

			Communicate(power);
		}

		private int CalculatePower()
		{
			if (Math.Abs(this.verticalSpeed) < VERTICAL_SPEED_LIMIT)
			{
				return 0;
			}

			return MAX_POWER;
		}

		private void Communicate(int power)
		{
			Console.WriteLine("0 {0}", power);
		}

		private readonly int verticalSpeed;

		private const int VERTICAL_SPEED_INDEX = 3;

		private const int VERTICAL_SPEED_LIMIT = 40;

		private const int MAX_POWER = 4;
	}
}