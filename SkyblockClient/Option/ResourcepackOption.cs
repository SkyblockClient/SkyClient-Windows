using System.Windows;

namespace SkyblockClient.Option
{
	public class ResourcepackOption : Option
	{

		public override IDownloadUrl downloadUrl => new InternalDownloadUrl(file);

		public override void Create(string line)
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

		public override void ComboBoxChecked(object sender, RoutedEventArgs e)
		{
			var checkBox = sender as System.Windows.Controls.CheckBox;
			enabled = checkBox.IsChecked ?? false;
		}

		private enum Index
		{
			ID, Hidden, Enabled, File, Display, Description 
		}
	}
}
