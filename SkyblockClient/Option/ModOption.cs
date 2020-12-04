using System;
using System.Windows;

namespace SkyblockClient.Option
{
	public class ModOption : Option, IEquatable<ModOption>, IEquatable<string>
	{
		public bool caution { get; set; }
		public string warning { get; set; }
		public bool dispersed { get; set; }
		public string dependency { get; set; }
		public bool config { get; set; }
		public bool remote { get; set; }
		public string url { get; set; }

		public override IDownloadUrl downloadUrl => remote ? new RemoteDownloadUrl(url) : (IDownloadUrl)new InternalDownloadUrl(file);

		public override void Create(string line)
		{
			var helper = new OptionHelper(line, 13);

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
			remote = helper.Boolean(Index.Remote, "Remote");
			url = helper.String(Index.Url);
		}

		public override string ToString()
		{
			string result = $"{id}-{display}\n";
			result += $"\tfile: {file}\n";
			result += $"\tenabled: {enabled}\n";
			result += $"\tdescription: {description}\n";
			result += $"\twarning: {warning}\n";
			result += $"\tremote: {remote}\n";
			result += $"\turl: {url}\n";

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

		public override void ComboBoxChecked(object sender, RoutedEventArgs e)
		{
			var checkBox = sender as System.Windows.Controls.CheckBox;
			var isChecked = checkBox.IsChecked ?? false;

			if (this.caution && isChecked)
			{
				MessageBoxResult result = MessageBox.Show(this.warning + "\n\nUse anyway?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
				switch (result)
				{
					case MessageBoxResult.Yes:
						checkBox.IsChecked = true;
						this.enabled = true;
						break;
					case MessageBoxResult.No:
						checkBox.IsChecked = false;
						this.enabled = false;
						break;
					default:
						checkBox.IsChecked = false;
						this.enabled = false;
						break;
				}
			}
			else
			{
				this.enabled = isChecked;
			}
		}

		private enum Index
		{
			ID, Enabled, File, Display, Description, Caution, Warning, Hidden, Dispersed, Dependency, Config, Remote, Url
		}
	}
}
