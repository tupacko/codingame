using System;

internal class TheBridgeEp1
{
	private static void Main(string[] args)
	{
		var environment = Environment.Load();
		var motorbike = new Bike();

		// game loop
		while (true)
		{
			motorbike.LoadState();
			motorbike.Move(environment);
		}
	}

	private class Environment
	{
		private Environment()
		{
		}

		public int RoadLength
		{
			get;
			private set;
		}

		public int GapLength
		{
			get;
			private set;
		}

		public int PlatformLength
		{
			get;
			private set;
		}

		public static Environment Load()
		{
			var environment = new Environment
			{
				RoadLength = int.Parse(Console.ReadLine()),
				GapLength = int.Parse(Console.ReadLine()),
				PlatformLength = int.Parse(Console.ReadLine())
			};

			return environment;
		}
	}

	private class Bike
	{
		public void LoadState()
		{
			this.speed = int.Parse(Console.ReadLine());
			this.position = int.Parse(Console.ReadLine());
		}

		public void Move(Environment environment)
		{
			if (IsOver(environment))
			{
				Console.WriteLine("SLOW");
				return;
			}

			if (ShouldJump(environment))
			{
				Console.WriteLine("JUMP");
				return;
			}

			if (IsTooFast(environment))
			{
				Console.WriteLine("SLOW");
				return;
			}

			if (!IsFastEnough(environment))
			{
				Console.WriteLine("SPEED");
				return;
			}

			Console.WriteLine("WAIT");
		}

		private bool IsOver(Environment environment)
		{
			return environment.RoadLength + environment.GapLength <= this.position;
		}

		private bool ShouldJump(Environment environment)
		{
			return this.position + this.speed > environment.RoadLength;
		}

		private bool IsTooFast(Environment environment)
		{
			return environment.GapLength + 1 < this.speed;
		}

		private bool IsFastEnough(Environment environment)
		{
			return environment.GapLength < this.speed;
		}

		private int speed;

		private int position;
	}
}