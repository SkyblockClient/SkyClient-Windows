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
			var helper = new OptionHelper(line, 7);

			id = helper.String(Index.ID);
			enabled = helper.Boolean(Index.Enabled, "Enabled");
			file = helper.String(Index.File);
			display = helper.String(Index.Display);
			description = helper.String(Index.Description);
			caution = helper.Boolean(Index.Caution, "Caution");
			warning = helper.String(Index.Warning);
		}

		public override string ToString()
		{
			string result = $"{id}-{display}\n";
			result += $"\tfile: {file}\n";
			result += $"\tenabled: {enabled}\n";
			result += $"\tdescription: {description}\n";
			result += $"\twarning: {warning}\n";

			return result;
		}

		private enum Index
		{
			ID, Enabled, File, Display, Description, Caution, Warning
		}
	}
}
