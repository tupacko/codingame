using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

internal class Defibrillators
{
	private static void Main(string[] args)
	{
		var userPosition = Position.ReadPosition();
		var finder = new DefibrillatorFinder();
		finder.LoadData();

		Console.WriteLine(finder.GetClosest(userPosition));
	}

	private class Position
	{
		public Position(double lon, double lat)
		{
			this.lon = lon;
			this.lat = lat;
		}

		public static Position ReadPosition()
		{
			double lon = GpsCoordinateParser.ToDouble(Console.ReadLine());
			double lat = GpsCoordinateParser.ToDouble(Console.ReadLine());

			return new Position(lon, lat);
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

	private static class GpsCoordinateParser
	{
		static GpsCoordinateParser()
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

	private class DefibrillatorFinder
	{
		public void LoadData()
		{
			int defibrillatorsCount = int.Parse(Console.ReadLine());
			this.instruments = new List<Defibrillator>(defibrillatorsCount);

			for (int i = 0; i < defibrillatorsCount; i++)
			{
				var instrument = Defibrillator.Read();
				if (ReferenceEquals(null, instrument))
				{
					continue;
				}

				this.instruments.Add(instrument);
			}
		}

		public string GetClosest(Position userPosition)
		{
			return this.instruments.OrderBy(i => userPosition.GetDistance(i.Position)).First().Name;
		}

		private IList<Defibrillator> instruments;
	}

	private class Defibrillator
	{
		private Defibrillator(string data)
		{
			var pieces = data.Split(DATA_SEPARATOR);
			var position = new Position(GpsCoordinateParser.ToDouble(pieces[4]), GpsCoordinateParser.ToDouble(pieces[5]));

			Id = int.Parse(pieces[0]);
			Name = pieces[1];
			Address = pieces[2];
			Phone = pieces[3];
			Position = position;
		}

		public int Id { get; private set; }

		public string Name { get; private set; }

		public string Address { get; private set; }

		public string Phone { get; private set; }

		public Position Position { get; private set; }

		public static Defibrillator Read()
		{
			var data = string.Empty;
			while (!IsDataComplete(data))
			{
				var piece = Console.ReadLine();
				if (string.IsNullOrEmpty(piece))
				{
					return null;
				}

				data += piece;
			}

			return new Defibrillator(data);
		}

		private static bool IsDataComplete(string data)
		{
			return DATA_SEPARATOR_COUNT == data.Count(ch => DATA_SEPARATOR == ch);
		}

		private const char DATA_SEPARATOR = ';';
		private const int DATA_SEPARATOR_COUNT = 5;
	}
}