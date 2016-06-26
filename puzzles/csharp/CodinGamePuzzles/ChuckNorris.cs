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
		var enumerator = GetBitsFlow(chars).GetEnumerator();
		if (!enumerator.MoveNext())
		{
			yield break;
		}

		bool type = enumerator.Current;
		int count = 1;
		while (enumerator.MoveNext())
		{
			if (type == enumerator.Current)
			{
				count++;
				continue;
			}

			yield return new EncodedChar(type, count);
			type = enumerator.Current;
			count = 1;
		}

		yield return new EncodedChar(type, count);
	}

	private static IEnumerable<bool> GetBitsFlow(byte[] chars)
	{
		for (int i = 0, max = chars.Length; i < max; i++)
		{
			foreach (var bit in GetBits(chars[i]))
			{
				yield return '1' == bit ? true : false;
			}
		}
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