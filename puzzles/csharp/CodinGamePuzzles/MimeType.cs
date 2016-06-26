using System;
using System.Collections.Generic;
using System.IO;

internal class MimeType
{
	private static void Main(string[] args)
	{
		var lookupTable = new Dictionary<string, string>();

		int fileTypesCount = int.Parse(Console.ReadLine());
		int fileNamesCount = int.Parse(Console.ReadLine());

		LoadMimeTypes(lookupTable, fileTypesCount);
		AnalyzeFileNames(lookupTable, fileNamesCount);
	}

	private static void LoadMimeTypes(Dictionary<string, string> lookupTable, int fileTypesCount)
	{
		for (int i = 0; i < fileTypesCount; i++)
		{
			string[] inputs = Console.ReadLine().Split(' ');
			string extension = inputs[0];
			string mimeType = inputs[1];

			lookupTable[extension.ToLower()] = mimeType;
		}
	}

	private static void AnalyzeFileNames(Dictionary<string, string> lookupTable, int fileNamesCount)
	{
		for (int i = 0; i < fileNamesCount; i++)
		{
			string fileName = Console.ReadLine();
			var ext = Path.GetExtension(fileName);

			if (!string.IsNullOrEmpty(ext))
			{
				ext = ext.ToLower().Substring(1);
			}

			if (!lookupTable.ContainsKey(ext))
			{
				Console.WriteLine("UNKNOWN");
				continue;
			}

			Console.WriteLine(lookupTable[ext]);
		}
	}
}