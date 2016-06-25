using System;
using System.Linq;

internal class MarsLanderEp1
{
	private class Point
	{
		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int X
		{
			get;
			private set;
		}

		public int Y
		{
			get;
			private set;
		}
	}

	private class Reader
	{
		public int Read()
		{
			return int.Parse(Console.ReadLine());
		}

		public int[] ReadArray()
		{
			return Console.ReadLine().Split(' ').Select(s => int.Parse(s)).ToArray();
		}
	}

	private class MarsLander
	{
		public MarsLander(Point[] surface, int[] data)
		{
			this.surface = surface;

			this.position = new Point(data[0], data[1]);
			this.horizontalSpeed = data[2];
			this.verticalSpeed = data[3];
			this.fuel = data[4];
			this.rotation = data[5];
			this.power = data[6];
		}

		public void Move()
		{
			var rotation = CalculateRotation();
			var power = CalculatePower();

			Communicate(rotation, power);
		}

		private int CalculatePower()
		{
			//var landingDistance = GetCrashDistance();
			//Console.Error.Write(landingDistance);

			// more realistic would be: landingDistance > timeToDecelerateToVerticalLimit
			if (Math.Abs(this.verticalSpeed) < VerticalSpeedLimit)
			{
				return 0;
			}

			return 4;
		}

		private int GetCrashDistance()
		{
			Point left = null;
			Point right = null;

			for (int i = 0, max = this.surface.Length; i < max; i++)
			{
				if (this.surface[i].X > this.position.X)
				{
					right = this.surface[i];
					left = this.surface[i - 1];
					break;
				}
			}

			// straight landing, Y's are equal
			if (left.Y != right.Y)
			{
				Console.Error.WriteLine("Error: Y's are not equal when straight landing!!!");
			}

			// Console.Error.WriteLine("{0}, {1} | {2}, {3} | {4}, {5}", left.X, left.Y,
			// this.position.X, this.position.Y, right.X, right.Y);

			return this.position.Y - right.Y;
		}

		private int CalculateRotation()
		{
			return 0; // straight landing
		}

		private void Communicate(int rotation, int power)
		{
			Console.WriteLine("{0} {1}", rotation, power);
		}

		private readonly Point[] surface;
		private readonly Point position;
		private readonly int horizontalSpeed;
		private readonly int verticalSpeed;
		private readonly int fuel;
		private readonly int rotation;
		private readonly int power;

		private const int VerticalSpeedLimit = 40;
		private const int HorizontalSpeedLimit = 20;

		private const double GravityAcceleration = 3.711;
	}

	private MarsLanderEp1()
	{
		InitLocals();
		InitSurface();
		LoadSurface();
	}

	public void Play()
	{
		// game loop
		while (true)
		{
			var data = this.reader.ReadArray();
			var currentTurn = new MarsLander(this.surface, data);

			currentTurn.Move();
		}
	}

	private void InitLocals()
	{
		this.reader = new Reader();
	}

	private void InitSurface()
	{
		int N = this.reader.Read(); // the number of points used to draw the surface of Mars.
		this.surface = new Point[N];
	}

	private void LoadSurface()
	{
		var points = this.surface.Length;
		for (int i = 0; i < points; i++)
		{
			var current = this.reader.ReadArray();

			this.surface[i] = new Point(current[0], current[1]);
		}
	}

	private static void Main(string[] args)
	{
		var player = new MarsLanderEp1();
		player.Play();
	}

	private Reader reader;

	private Point[] surface;
}