using System.Windows;

namespace SkyblockClient.Options
{
	public class PackOption : Option
	{
		public override IDownloadUrl downloadUrl => remote ? (IDownloadUrl)new RemoteDownloadUrl(url) : new InternalDownloadUrl("packs/" + file);
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
	}
}
