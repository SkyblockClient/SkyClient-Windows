using Newtonsoft.Json;
using System.Diagnostics;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System;

namespace SkyblockClient.Options
{
	public abstract class Option
	{
		public string id { get; set; }

		[DefaultValue(false)]
		public bool enabled { get; set; }

		public string file { get; set; }

		[DefaultValue("")]
		public string display { get; set; }

		[DefaultValue("")]
		public string description { get; set; }

		[DefaultValue(false)]
		public bool hidden { get; set; }

		[JsonIgnore]
		public bool remote => IsSet(url.Trim());

		[DefaultValue("")]
		public string url { get; set; }

		[DefaultValue(false)]
		public bool guide { get; set; }

		[JsonIgnore]
		public bool hasIcon => IsSet(icon.Trim());

		[DefaultValue("")]
		public string icon { get; set; }

		[JsonIgnore]
		public virtual IDownloadUrl downloadUrl { get; }


		[JsonIgnore]
		public CheckBoxMod CheckBox
		{
			get
			{
				CheckBoxMod checkBox = new CheckBoxMod();
				checkBox.Content = display;
				checkBox.IsChecked = enabled;
				checkBox.Tag = this;
				checkBox.HasGuide = guide;

				Utils.Debug("modID:" + id);
				Utils.Debug("hasIcon:" + hasIcon);
                if (hasIcon)
				{
					BitmapImage bitmap = new BitmapImage();
					bitmap.BeginInit();
					bitmap.UriSource = new Uri(new InternalDownloadUrl($"icons/{icon}").Url, UriKind.Absolute);
					bitmap.EndInit();

					checkBox.image.Source = bitmap;
				}
				else
                {
                    if (!Globals.appendMissingOptionIcon)
					{
						checkBox.gridCol0.Width = new GridLength(0);
						checkBox.gridCol1.Width = new GridLength(0);
					}
				}

				ToolTip toolTip = new ToolTip();
				toolTip.Content = description;
				toolTip.Tag = checkBox;

				checkBox.ToolTip = toolTip;
				checkBox.Click += ComboBoxChecked;

				return checkBox;
			}
		}

		public abstract void ComboBoxChecked(object sender, RoutedEventArgs e);

		public void OpenGuide()
		{
			if (guide)
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

		protected bool IsSet(string value) => !(value == "" || value == "None" || value == "none");
	}
}
