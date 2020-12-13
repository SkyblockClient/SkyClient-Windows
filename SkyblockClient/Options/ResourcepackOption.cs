using System.Windows;

namespace SkyblockClient.Options
{
	public class PackOption : Option
	{
		public override IDownloadUrl downloadUrl => new InternalDownloadUrl("packs/" + file);

		public override void Create(string line)
		{
			var helper = new OptionHelper(line, Index.Description);

			id = helper.String(Index.ID);
			hidden = helper.Boolean(Index.Hidden, "Hidden");
			enabled = helper.Boolean(Index.Enabled, "Enabled");
			file = helper.String(Index.File);
			display = helper.String(Index.Display);
			description = helper.String(Index.Description);

			HasGuide = false;
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

		public override void ComboBoxChecked(object sender, RoutedEventArgs e)
		{
			var checkBox = sender as CheckBoxMod;
			enabled = checkBox.IsChecked;
		}

		private enum Index
		{
			ID, Hidden, Enabled, File, Display, Description
		}
	}
}
