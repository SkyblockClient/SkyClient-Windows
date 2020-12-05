using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace SkyblockClient.Options
{
	public abstract class Option
	{
		public string id { get; set; }
		public bool enabled { get; set; }
		public string file { get; set; }
		public string display { get; set; }
		public string description { get; set; }
		public bool hidden { get; set; }

		public bool HasGuide { get; set; }

		public abstract IDownloadUrl downloadUrl { get; }

		public CheckBoxMod CheckBox
		{
			get
			{
				CheckBoxMod checkBox = new CheckBoxMod();
				checkBox.Content = display;
				checkBox.IsChecked = enabled;
				checkBox.Tag = this;
				checkBox.HasGuide = HasGuide;

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

		public void OpenGuide()
		{
			if (HasGuide)
			{
				string endpoint = $"{Globals.URL}guides/{id}.md";
				string command = $"/c start {endpoint}";
				Utils.Debug(command);
				var processInfo = Utils.CreateProcessStartInfo("cmd.exe", command);
				Process.Start(processInfo);
			}
			else
			{
				Utils.Error("How did we get here?");
			}
		}
	}
}
