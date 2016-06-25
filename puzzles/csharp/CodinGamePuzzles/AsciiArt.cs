using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class AsciiArt
{
	private class Character
	{
		public Character(string[] charData)
		{
			this.data = charData;
		}

		public void AppendTo(StringBuilder[] text)
		{
			for (int i = 0, max = text.Length; i < max; i++)
			{
				text[i].Append(data[i]);
			}
		}

		private readonly string[] data;
	}

	private class Chopper
	{
		public Chopper(int charWidth, string[] characters)
		{
			this.width = charWidth;
			this.data = characters;
		}

		public Character GetNext()
		{
			var height = this.data.Length;
			var charData = new string[height];

			for (int i = 0; i < height; i++)
			{
				charData[i] = this.data[i].Substring(0, this.width);
				this.data[i] = this.data[i].Substring(this.width);
			}

			return new Character(charData);
		}

		private readonly int width;
		private readonly string[] data;
	}

	private class Art
	{
		public Art(int height, Chopper chopper)
		{
			this.height = height;
			this.characters = InitCharacters(chopper);
		}

		public string Transform(string text)
		{
			var safeText = GetSafeText(text).ToArray();
			var appenders = CreateAppenders().ToArray();
			var lines = GetTransformedLines(appenders, safeText);

			return PrepareResult(lines);
		}

		private IEnumerable<char> GetSafeText(string text)
		{
			var chars = text.ToUpper().ToCharArray();
			foreach (var character in chars)
			{
				if (!this.characters.Keys.Contains(character))
				{
					yield return '?';
					continue;
				}

				yield return character;
			}
		}

		private IEnumerable<StringBuilder> CreateAppenders()
		{
			for (int i = 0; i < this.height; i++)
			{
				yield return new StringBuilder();
			}
		}

		private string[] GetTransformedLines(StringBuilder[] appenders, char[] safeText)
		{
			foreach (var character in safeText)
			{
				this.characters[character].AppendTo(appenders);
			}

			return appenders.Select(x => x.ToString()).ToArray();
		}

		private static string PrepareResult(string[] lines)
		{
			var builder = new StringBuilder();
			for (int i = 0, max = lines.Length; i < max; i++)
			{
				builder.AppendLine(lines[i]);
			}

			return builder.ToString();
		}

		private IDictionary<char, Character> InitCharacters(Chopper chopper)
		{
			var characters = new Dictionary<char, Character>();

			LoadCharacters(chopper, characters);

			return characters;
		}

		private static void LoadCharacters(Chopper chopper, IDictionary<char, Character> target)
		{
			Character character;
			for (int i = (int)'A', max = (int)'Z'; i <= max; i++)
			{
				character = chopper.GetNext();
				PutCharacter(target, (char)i, character);
			}

			character = chopper.GetNext();
			PutCharacter(target, '?', character);
		}

		private static void PutCharacter(IDictionary<char, Character> target, char plainChar, Character artChar)
		{
			target[plainChar] = artChar;
		}

		private readonly int height;
		private readonly IDictionary<char, Character> characters;
	}

	private static void Main(string[] args)
	{
		int L = int.Parse(Console.ReadLine());
		int H = int.Parse(Console.ReadLine());

		string T = Console.ReadLine();

		string[] characters = new string[H];
		for (int i = 0; i < H; i++)
		{
			string ROW = Console.ReadLine();
			characters[i] = ROW;
		}

		var chopper = new Chopper(L, characters);
		var asciiArt = new Art(H, chopper);

		Console.WriteLine(asciiArt.Transform(T));
	}
}