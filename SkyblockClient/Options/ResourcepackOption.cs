using System.Windows;

namespace SkyblockClient.Options
{
	public class PackOption : Option
	{
		public override IDownloadUrl DownloadUrl => Remote ? (IDownloadUrl)new RemoteDownloadUrl(Url) : new InternalDownloadUrl("packs/" + File);
		public override string ToString()
		{
			string result = $"{Id}-{Display}\n";
			result += $"\tfile: {File}\n";
			result += $"\thidden: {Hidden}\n";
			result += $"\tenabled: {Enabled}\n";
			result += $"\tdescription: {Description}\n";

			return result;
		}

		public override void ComboBoxChecked(object sender, RoutedEventArgs e)
		{
			var checkBox = sender as CheckBoxMod;
			Enabled = checkBox.IsChecked;
		}
	}
}
