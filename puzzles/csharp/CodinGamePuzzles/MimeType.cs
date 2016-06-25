using System;
using System.Collections.Generic;
using System.IO;

internal class MimeType
{
	private static void Main(string[] args)
	{
		var lookupTable = new Dictionary<string, string>();

		int N = int.Parse(Console.ReadLine()); // Number of elements which make up the association table.
		int Q = int.Parse(Console.ReadLine()); // Number Q of file names to be analyzed.
		for (int i = 0; i < N; i++)
		{
			string[] inputs = Console.ReadLine().Split(' ');
			string EXT = inputs[0]; // file extension
			string MT = inputs[1]; // MIME type.

			lookupTable[EXT.ToLower()] = MT;
		}

		for (int i = 0; i < Q; i++)
		{
			string FNAME = Console.ReadLine(); // One file name per line.
			var ext = Path.GetExtension(FNAME);
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