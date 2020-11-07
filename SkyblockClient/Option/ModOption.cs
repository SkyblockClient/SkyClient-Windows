using System;
using System.Collections.Generic;

namespace SkyblockClient.Option
{
	public class ModOption : IOption
	{
		public string id { get; set; }
		public bool enabled { get; set; }
		public string file { get; set; }
		public string display { get; set; }
		public string description { get; set; }
		public bool caution { get; set; }
		public string warning { get; set; }

		public void Create(string line)
		{
			const int ID_INDEX = 0, ENABLED_INDEX = 1, FILE_INDEX = 2, DISPLAY_INDEX = 3, DESCRIPTION_INDEX = 4, CAUTION_INDEX = 5, WARNING_INDEX = 6;
			var helper = new OptionHelper(line, 7);

			id = helper.String(ID_INDEX);
			enabled = helper.Boolean(ENABLED_INDEX, "Enabled");
			file = helper.String(FILE_INDEX);
			display = helper.String(DISPLAY_INDEX);
			description = helper.String(DESCRIPTION_INDEX);
			caution = helper.Boolean(CAUTION_INDEX, "Caution");
			warning = helper.String(WARNING_INDEX);
		}

		public override string ToString()
		{
			string result = $"{id}-{display}\n";
			result += $"\tfile: {file}\n";
			result += $"\tenabled: {enabled}\n";
			result += $"\tdescription: {description}\n";

			return result;
		}
	}
}
