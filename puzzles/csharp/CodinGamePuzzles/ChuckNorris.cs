using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class ChuckNorris
{
	private static void Main(string[] args)
	{
		string message = Console.ReadLine();

		Console.WriteLine(Encode(message));
	}

	private static string Encode(string message)
	{
		var chars = message.ToCharArray();
		var binaryChars = GetBinaryCharacters(chars);
		var encodedChars = GetEncodedCharacters(binaryChars).ToArray();

		return GetEncodedMessage(encodedChars);
	}

	private static byte[] GetBinaryCharacters(char[] chars)
	{
		return chars.Select(Convert.ToByte).ToArray();
	}

	private static IEnumerable<EncodedChar> GetEncodedCharacters(byte[] chars)
	{
		var bitEnumerator = GetAllBits(chars).GetEnumerator();
		if (!bitEnumerator.MoveNext())
		{
			yield break;
		}

		bool bitType = bitEnumerator.Current;
		int bitCount = 1;
		while (bitEnumerator.MoveNext())
		{
			if (bitType == bitEnumerator.Current)
			{
				bitCount++;
				continue;
			}

			yield return new EncodedChar(bitType, bitCount);
			bitType = bitEnumerator.Current;
			bitCount = 1;
		}

		yield return new EncodedChar(bitType, bitCount);
	}

	private static IEnumerable<bool> GetAllBits(byte[] chars)
	{
		return chars.SelectMany(GetBits).Select(x => Equals('1', x));
	}

	private static char[] GetBits(byte character)
	{
		return Convert.ToString(character, 2).PadLeft(7, '0').ToCharArray();
	}

	private static string GetEncodedMessage(EncodedChar[] chars)
	{
		return chars.Aggregate(new StringBuilder(), (sb, c) => sb.AppendFormat(" {0}", c.ToString()), sb => sb.ToString().Substring(1));
	}

	private class EncodedChar
	{
		public EncodedChar(bool type, int repeat)
		{
			this.value = type;
			this.count = repeat;
		}

		public override string ToString()
		{
			var typeMarker = GetTypeMarker();
			var countMarker = GetCountMarker();

			return GetStringRepresentation(typeMarker, countMarker);
		}

		private string GetTypeMarker()
		{
			if (value)
			{
				return "0";
			}

			return "00";
		}

		private string GetCountMarker()
		{
			return new String('0', this.count);
		}

		private static string GetStringRepresentation(string typeMarker, string countMarker)
		{
			return string.Format("{0} {1}", typeMarker, countMarker);
		}

		private readonly bool value;

		private readonly int count;
	}
}