using System;
using System.Collections.Generic;

namespace SkyblockClient.Option
{
	public class OptionHelper
	{
		public const char SPLIT_CHARACTER = ';';
		public const string COMMENT_CHARACTER = "#";

		public static List<TOption> Read<TOption>(string text) where TOption : IOption, new()
		{
			var result = new List<TOption>();
			text = text.Trim();
			if (text == "")
				return result;

			var lines = new List<string>(text.Split('\n'));
			foreach (var line in lines)
			{
				string ln = line.Trim();
				if (ln != "" && !ln.StartsWith(COMMENT_CHARACTER))
				{
					TOption opt = new TOption();
					opt.Create(ln);
					result.Add(opt);
				}
			}
			return result;
		}

		string[] parts;

		public OptionHelper(string line, int length)
		{
			parts = line.Split(SPLIT_CHARACTER);
			if (parts.Length != length)
				throw new ArgumentException("Line is either malformed or Empty");

		}

		public bool Boolean(int index, string name)
		{
			if (parts[index] == "true")
				return true;
			else if (parts[index] == "false")
				return false;
			else
				throw new ArgumentException($"Value of '{name}' is neither true or false");
		}

		public bool Boolean(Enum index, string name)
		{
			return Boolean(Convert.ToInt32(index), name);
		}

		public string String(int index)
		{
			return parts[index].Replace(@"\n", "\n").Trim();
		}

		public string String(Enum index)
		{
			return String(Convert.ToInt32(index));
		}
	}
}
