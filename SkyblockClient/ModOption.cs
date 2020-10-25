using System;
using System.Collections.Generic;

namespace SkyblockClient
{
	public class ModOption
	{
		public static List<ModOption> Read(string text)
		{ 
			var result = new List<ModOption>();

			if (text.Trim() == "")
				return result;

			var lines = new List<string>(text.Split(new string[1] { "\n" }, StringSplitOptions.None));
			foreach (var line in lines)
			{
				if (line != "")
				{
					var wri = new ModOption(line);
					result.Add(wri);
				}
			}
			return result;
		}

		public string key = "";
		public string fileName = "";
		public bool enabled = true;
		public string display = "";

		public ModOption(string respLine)
		{
			const int KEY_INDEX = 0, DEFAULT_INDEX = 1, FILE_NAME_INDEX = 2, DISPLAY_INDEX = 3;
			var split = respLine.Split(':');
			if (split.Length != 4)
			{
				throw new ArgumentException("Line is either malformed or Empty");
			}

			string boolpart = split[DEFAULT_INDEX].Trim().ToLower();

			if (boolpart == "true")
			{
				enabled = true;
			}
			else if (boolpart == "false")
			{
				enabled = false;
			}
			else
			{
				throw new ArgumentException("Default Value is neither true or false");
			}

			key = split[KEY_INDEX].Trim().ToLower();
			fileName = split[FILE_NAME_INDEX].Trim();
			display = split[DISPLAY_INDEX].Trim();
		}
	}
}
