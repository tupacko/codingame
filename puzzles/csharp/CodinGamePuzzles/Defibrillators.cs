using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

internal class Defibrillators
{
	private static class Parser
	{
		static Parser()
		{
			NumberFormatInfo provider = new NumberFormatInfo();
			provider.NumberDecimalSeparator = ",";

			NumberFormatProvider = provider;
		}

		public static double ToDouble(string value)
		{
			return Convert.ToDouble(value, NumberFormatProvider);
		}

		private static readonly IFormatProvider NumberFormatProvider;
	}

	private class Position
	{
		public Position(double lon, double lat)
		{
			this.lon = lon;
			this.lat = lat;
		}

		public double GetDistance(Position position)
		{
			var x = (position.lon - this.lon) * Math.Cos((position.lat + this.lat) / 2);
			var y = position.lat - this.lat;

			return Math.Sqrt(x * x + y * y) * EarthRadsInKm;
		}

		public override string ToString()
		{
			return string.Format("Lon: {0}, Lat: {1}", this.lon, this.lat);
		}

		private readonly double lon;

		private readonly double lat;

		private const int EarthRadsInKm = 6371;
	}

	private class UserReader
	{
		public Position GetUserPosition()
		{
			double lon = Parser.ToDouble(Console.ReadLine());
			double lat = Parser.ToDouble(Console.ReadLine());

			return new Position(lon, lat);
		}
	}

	private class Defibrillator
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Address { get; set; }

		public string Phone { get; set; }

		public Position Position { get; set; }
	}

	private class DefibrillatorReader
	{
		public IEnumerable<Defibrillator> GetDefibrillators()
		{
			int records = int.Parse(Console.ReadLine());
			for (int i = 0; i < records; i++)
			{
				yield return GetDefibrillator();
			}
		}

		private Defibrillator GetDefibrillator()
		{
			var data = new List<string>();

			while (!IsDataComplete(data))
			{
				var piece = Console.ReadLine();
				if (string.IsNullOrEmpty(piece))
				{
					return null;
				}

				data.Add(piece);
			}

			var record = data.Aggregate(new StringBuilder(), (sb, p) => sb.Append(p), sb => sb.ToString());
			var pieces = record.Split(DataSeparator);

			var position = new Position(Parser.ToDouble(pieces[4]), Parser.ToDouble(pieces[5]));

			return new Defibrillator
			{
				Id = int.Parse(pieces[0]),
				Name = pieces[1],
				Address = pieces[2],
				Phone = pieces[3],
				Position = position
			};
		}

		private static bool IsDataComplete(IEnumerable<string> data)
		{
			var separatorCount = data.Aggregate(0, (count, piece) => count + piece.Count(ch => DataSeparator == ch));

			return DataSeparatorCount == separatorCount;
		}

		private const char DataSeparator = ';';

		private const int DataSeparatorCount = 5;
	}

	private class Finder
	{
		public Finder(IEnumerable<Defibrillator> defibrillators)
		{
			this.instruments = GetSafe(defibrillators).ToArray();
		}

		public string GetClosest(Position userPosition)
		{
			return this.instruments.OrderBy(i => userPosition.GetDistance(i.Position)).First().Name;
		}

		private static IEnumerable<Defibrillator> GetSafe(IEnumerable<Defibrillator> defibrillators)
		{
			if (ReferenceEquals(null, defibrillators))
			{
				yield break;
			}

			foreach (var instrument in defibrillators)
			{
				if (ReferenceEquals(null, instrument))
				{
					continue;
				}

				yield return instrument;
			}
		}

		private readonly IEnumerable<Defibrillator> instruments;
	}

	private static void Main(string[] args)
	{
		var userReader = new UserReader();
		var userPosition = userReader.GetUserPosition();

		var defibrillatorReader = new DefibrillatorReader();

		var finder = new Finder(defibrillatorReader.GetDefibrillators());

		Console.WriteLine(finder.GetClosest(userPosition));
	}
}