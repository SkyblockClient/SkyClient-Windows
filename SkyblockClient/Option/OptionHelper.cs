using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyblockClient.Option
{
	public class OptionHelper
	{

		public static List<IOption> Read<TOption>(string text) where TOption : IOption, new()
		{
			var result = new List<IOption>();
			text = text.Trim();
			if (text == "")
				return result;

			var lines = new List<string>(text.Split('\n'));
			foreach (var line in lines)
			{
				string ln = line.Trim();
				if (ln != "" && !ln.StartsWith("#"))
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
			parts = line.Split(':');
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

		public string String(int index)
		{
			return parts[index].Replace(@"\n", "\n").Trim();
		}

	}
}
