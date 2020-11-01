using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyblockClient.Option
{
	public class ResourcepackOption : IOption
	{
		public string id { get; set; }
		public bool hidden { get; set; }
		public bool enabled { get; set; }
		public string file { get; set; }
		public string display { get; set; }
		public string description { get; set; }

		public void Create(string line)
		{
			const int ID_INDEX = 0, HIDDEN_INDEX = 1, ENABLED_INDEX = 2, FILE_INDEX = 3, DISPLAY_INDEX = 4, DESCRIPTION_INDEX = 5;
			var helper = new OptionHelper(line, 6);

			id = helper.String(ID_INDEX);
			hidden = helper.Boolean(HIDDEN_INDEX, "Hidden");
			enabled = helper.Boolean(ENABLED_INDEX, "Enabled");
			file = helper.String(FILE_INDEX);
			display = helper.String(DISPLAY_INDEX);
			description = helper.String(DESCRIPTION_INDEX);
		}

		public override string ToString()
		{
			string result = $"{id}-{display}\n";
			result += $"\tfile: {file}\n";
			result += $"\thidden: {hidden}\n";
			result += $"\tenabled: {enabled}\n";
			result += $"\tdescription: {description}\n";

			return result;
		}
	}
}
