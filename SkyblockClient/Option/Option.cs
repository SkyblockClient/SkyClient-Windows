using System.Windows;
using System.Windows.Controls;
namespace SkyblockClient.Option
{
	public abstract class Option
	{
		public string id { get; set; }
		public bool enabled { get; set; }
		public string file { get; set; }
		public string display { get; set; }
		public string description { get; set; }
		public bool hidden { get; set; }

		public abstract IDownloadUrl downloadUrl { get; }

		public CheckBox CheckBox
		{
			get
			{
				CheckBox checkBox = new CheckBox();
				checkBox.Content = this.display;
				checkBox.IsChecked = this.enabled;
				checkBox.Tag = this;

				ToolTip toolTip = new ToolTip();
				toolTip.Content = description;
				toolTip.Tag = checkBox;

				checkBox.ToolTip = toolTip;
				checkBox.Click += ComboBoxChecked;

				return checkBox;
			}
		}

		public abstract void ComboBoxChecked(object sender, RoutedEventArgs e);
		public abstract void Create(string line);
	}
}
