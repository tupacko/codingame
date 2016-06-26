using System;

internal class TheBridgeEp1
{
	private static void Main(string[] args)
	{
		var environment = Environment.Load();
		var processorChain = CreateProcessors(environment);
		var motorbike = new Bike();

		// game loop
		while (true)
		{
			motorbike.LoadState();

			processorChain.Process(motorbike);
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

		public bool IsOver(Environment environment)
		{
			return environment.RoadLength + environment.GapLength <= this.position;
		}

		public bool IsFastEnough(Environment environment)
		{
			return environment.GapLength >= this.speed;
		}

		public bool IsTooFast(Environment environment)
		{
			return environment.GapLength + 1 < this.speed;
		}

		public bool ShouldJump(Environment environment)
		{
			return this.position + this.speed > environment.RoadLength;
		}

		private int speed;

		private int position;
	}

	private static ProcessorBase CreateProcessors(Environment environment)
	{
		var speed = new Speed(environment);
		var slow = new Slow(environment);
		var jump = new Jump(environment);
		var wait = new Wait();

		speed.SetNextInChain(jump);
		jump.SetNextInChain(slow);
		slow.SetNextInChain(wait);

		return speed;
	}

	private interface IProcessor
	{
		void Process(Bike bike);
	}

	private abstract class ProcessorBase : IProcessor
	{
		protected ProcessorBase(Environment environment)
		{
			Environment = environment;
		}

		protected Environment Environment
		{
			get;
			private set;
		}

		public void SetNextInChain(IProcessor next)
		{
			this.nextProcessor = next;
		}

		public void Process(Bike bike)
		{
			if (!CanHandle(bike))
			{
				HandleNext(bike);
				return;
			}

			Handle();
		}

		protected abstract bool CanHandle(Bike bike);

		protected abstract void Handle();

		private void HandleNext(Bike bike)
		{
			if (ReferenceEquals(null, this.nextProcessor))
			{
				return;
			}

			this.nextProcessor.Process(bike);
		}

		private IProcessor nextProcessor;
	}

	private class Speed : ProcessorBase
	{
		public Speed(Environment environment) : base(environment)
		{
		}

		protected override bool CanHandle(Bike bike)
		{
			if (bike.IsOver(Environment))
			{
				return false;
			}

			if (bike.IsFastEnough(Environment))
			{
				return true;
			}

			return false;
		}

		protected override void Handle()
		{
			Console.WriteLine("SPEED");
		}
	}

	private class Slow : ProcessorBase
	{
		public Slow(Environment environment) : base(environment)
		{
		}

		protected override bool CanHandle(Bike bike)
		{
			if (bike.IsOver(Environment))
			{
				return true;
			}

			if (bike.IsTooFast(Environment))
			{
				return true;
			}

			return false;
		}

		protected override void Handle()
		{
			Console.WriteLine("SLOW");
		}
	}

	private class Jump : ProcessorBase
	{
		public Jump(Environment environment) : base(environment)
		{
		}

		protected override bool CanHandle(Bike bike)
		{
			if (bike.IsOver(Environment))
			{
				return false;
			}

			if (bike.ShouldJump(Environment))
			{
				return true;
			}

			return false;
		}

		protected override void Handle()
		{
			Console.WriteLine("JUMP");
		}
	}

	private class Wait : IProcessor
	{
		public void Process(Bike bike)
		{
			Console.WriteLine("WAIT");
		}
	}
}