using Newtonsoft.Json;
using System.Diagnostics;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System;
using System.Collections.Generic;

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

		public List<OptionAction> actions { get; set; }


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
				checkBox.Actions = actions;
				checkBox.Icon = icon;

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

		protected bool IsSet(string value) => !(value == null || value == "" || value == "None" || value == "none");
	}
}
