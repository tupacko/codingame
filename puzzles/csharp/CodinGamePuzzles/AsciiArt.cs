using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class AsciiArt
{
	private static void Main(string[] args)
	{
		int letterWidth = int.Parse(Console.ReadLine());
		int letterHeight = int.Parse(Console.ReadLine());

		string encodedText = Console.ReadLine();
		var fontFamily = new FontFamily(letterWidth, letterHeight);
		fontFamily.Load();

		var asciiArt = new Art(fontFamily);
		Console.WriteLine(asciiArt.Transform(encodedText));
	}

	private class FontFamily
	{
		public FontFamily(int letterWidth, int letterHeight)
		{
			this.width = letterWidth;
			this.height = letterHeight;

			this.characters = new Dictionary<char, Character>();
		}

		public int Height
		{
			get
			{
				return this.height;
			}
		}

		public void Load()
		{
			var rawFontsData = ReadRawFontsData();
			LoadCharacters(rawFontsData);
		}

		public Character GetCharacter(char plainCharacter)
		{
			if (!this.characters.ContainsKey(plainCharacter))
			{
				return this.characters['?'];
			}

			return this.characters[plainCharacter];
		}

		private string[] ReadRawFontsData()
		{
			var rawFontsData = new string[this.height];
			for (int i = 0; i < this.height; i++)
			{
				rawFontsData[i] = Console.ReadLine();
			}

			return rawFontsData;
		}

		private void LoadCharacters(string[] rawFontsData)
		{
			LoadLetters(rawFontsData);
			LoadSpecialCharacters(rawFontsData);
		}

		private void LoadLetters(string[] rawFontsData)
		{
			for (int i = (int)'A', max = (int)'Z'; i <= max; i++)
			{
				Character character = DequeueNextCharacter(rawFontsData);
				PutCharacter((char)i, character);
			}
		}

		private void LoadSpecialCharacters(string[] rawFontsData)
		{
			Character character = DequeueNextCharacter(rawFontsData);
			PutCharacter('?', character);
		}

		public Character DequeueNextCharacter(string[] rawFontsData)
		{
			var charData = new string[this.height];
			for (int i = 0; i < this.height; i++)
			{
				charData[i] = rawFontsData[i].Substring(0, this.width);
				rawFontsData[i] = rawFontsData[i].Substring(this.width);
			}

			return new Character(charData);
		}

		private void PutCharacter(char plainChar, Character artChar)
		{
			this.characters[plainChar] = artChar;
		}

		private readonly int width;
		private readonly int height;

		private readonly IDictionary<char, Character> characters;
	}

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

	private class Art
	{
		public Art(FontFamily fontFamily)
		{
			this.characters = fontFamily;
		}

		public string Transform(string text)
		{
			var uppercaseText = text.ToUpper().ToCharArray();
			var appenders = CreateAppenders().ToArray();
			var lines = GetTransformedLines(appenders, uppercaseText);

			return PrepareResult(lines);
		}

		private IEnumerable<StringBuilder> CreateAppenders()
		{
			for (int i = 0; i < this.characters.Height; i++)
			{
				yield return new StringBuilder();
			}
		}

		private string[] GetTransformedLines(StringBuilder[] appenders, char[] uppercaseText)
		{
			foreach (var plainCharacter in uppercaseText)
			{
				this.characters.GetCharacter(plainCharacter).AppendTo(appenders);
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

		private readonly FontFamily characters;
	}
}