using System;

internal class TheDescent
{
	private static void Main(string[] args)
	{
		const int NUMBER_OF_MOUNTAINS = 8;

		while (true)
		{
			var maxHeight = 0;
			var attackPosition = 0;
			for (int i = NUMBER_OF_MOUNTAINS; 0 != i; i--)
			{
				int height = int.Parse(Console.ReadLine());

				if (maxHeight < height)
				{
					maxHeight = height;
					attackPosition = NUMBER_OF_MOUNTAINS - i;
				}
			}

			Console.WriteLine(attackPosition);
		}
	}
}