using System;

internal class TheBridgeEp1
{
	private abstract class ProcessorBase
	{
		protected int R
		{
			get;
			private set;
		}

		protected int G
		{
			get;
			private set;
		}

		protected int L
		{
			get;
			private set;
		}

		public ProcessorBase Next
		{
			get;
			set;
		}

		protected ProcessorBase(int R, int G, int L)
		{
			this.R = R;
			this.G = G;
			this.L = L;
		}

		public void Process(int S, int X)
		{
			Console.Error.WriteLine("{0}, {1}", S, X);

			if (!CanHandle(S, X))
			{
				HandleNext(S, X);
				return;
			}

			Handle();
		}

		protected abstract bool CanHandle(int S, int X);

		protected abstract void Handle();

		protected bool IsOver(int X)
		{
			return R + G <= X;
		}

		private void HandleNext(int S, int X)
		{
			if (ReferenceEquals(null, Next))
			{
				return;
			}

			Next.Process(S, X);
		}
	}

	private class Speed : ProcessorBase
	{
		public Speed(int R, int G, int L) : base(R, G, L)
		{
		}

		protected override bool CanHandle(int S, int X)
		{
			if (IsOver(X))
			{
				return false;
			}

			if (G >= S)
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
		public Slow(int R, int G, int L) : base(R, G, L)
		{
		}

		protected override bool CanHandle(int S, int X)
		{
			if (IsOver(X))
			{
				return true;
			}

			if (G + 1 < S)
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
		public Jump(int R, int G, int L) : base(R, G, L)
		{
		}

		protected override bool CanHandle(int S, int X)
		{
			if (IsOver(X))
			{
				return false;
			}

			if (X < R && X + S > R)
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

	private class Wait : ProcessorBase
	{
		public Wait(int R, int G, int L) : base(R, G, L)
		{
		}

		protected override bool CanHandle(int S, int X)
		{
			return true;
		}

		protected override void Handle()
		{
			Console.WriteLine("WAIT");
		}
	}

	private static ProcessorBase CreateProcessors(int R, int G, int L)
	{
		var speed = new Speed(R, G, L);
		var slow = new Slow(R, G, L);
		var jump = new Jump(R, G, L);
		var wait = new Wait(R, G, L);

		speed.Next = jump;
		jump.Next = slow;
		slow.Next = wait;

		return speed;
	}

	private static void Main(string[] args)
	{
		int R = int.Parse(Console.ReadLine()); // the length of the road before the gap.
		int G = int.Parse(Console.ReadLine()); // the length of the gap.
		int L = int.Parse(Console.ReadLine()); // the length of the landing platform.

		Console.Error.WriteLine("{0}, {1}, {2}", R, G, L);

		var processorChain = CreateProcessors(R, G, L);

		// game loop
		while (true)
		{
			int S = int.Parse(Console.ReadLine()); // the motorbike's speed.
			int X = int.Parse(Console.ReadLine()); // the position on the road of the motorbike.

			processorChain.Process(S, X);
		}
	}
}