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
			var helper = new OptionHelper(line, 6);

			id = helper.String(Index.ID);
			hidden = helper.Boolean(Index.Hidden, "Hidden");
			enabled = helper.Boolean(Index.Enabled, "Enabled");
			file = helper.String(Index.File);
			display = helper.String(Index.Display);
			description = helper.String(Index.Description);
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

		private enum Index
		{
			ID, Hidden, Enabled, File, Display, Description 
		}
	}
}
