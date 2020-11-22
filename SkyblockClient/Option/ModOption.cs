using System;

namespace SkyblockClient.Option
{
	public class ModOption : IOption, IEquatable<ModOption>, IEquatable<string>
	{
		public string id { get; set; }
		public bool enabled { get; set; }
		public string file { get; set; }
		public string display { get; set; }
		public string description { get; set; }
		public bool caution { get; set; }
		public string warning { get; set; }
		public bool hidden { get; set; }
		public bool dispersed { get; set; }
		public string dependency { get; set; }
		public bool config { get; set; }

		public void Create(string line)
		{
			var helper = new OptionHelper(line, 11);

			id = helper.String(Index.ID);
			enabled = helper.Boolean(Index.Enabled, "Enabled");
			file = helper.String(Index.File);
			display = helper.String(Index.Display);
			description = helper.String(Index.Description);
			caution = helper.Boolean(Index.Caution, "Caution");
			warning = helper.String(Index.Warning);
			hidden = helper.Boolean(Index.Hidden, "Hidden");
			dispersed = helper.Boolean(Index.Dispersed, "Dispersed");
			dependency = helper.String(Index.Dependency);
			config = helper.Boolean(Index.Config, "Config");

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

		public bool Equals(string other)
		{
			return id.Equals(other);
		}

		bool IEquatable<ModOption>.Equals(ModOption other)
		{
			return id.Equals(other.id);
		}

		private enum Index
		{
			ID, Enabled, File, Display, Description, Caution, Warning, Hidden, Dispersed, Dependency, Config
		}
	}
}
