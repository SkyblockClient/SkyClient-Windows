using Newtonsoft.Json;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.Generic;
using SkyblockClient.Options.Events;

namespace SkyblockClient.Options
{
    public abstract class Option
	{
		public string Id { get; set; }

		[DefaultValue(false)]
        public bool Enabled { get; set; }

		public string File { get; set; }

		[DefaultValue("")]
		public string Display { get; set; }

		[DefaultValue("")]
		public string Description { get; set; }

		[DefaultValue(false)]
		public bool Hidden { get; set; }

		[JsonIgnore]
		public bool Remote => Utils.IsPropSet(Url.Trim());

		[DefaultValue("")]
		public string Url { get; set; }

		[DefaultValue(false)]
		public bool Guide { get; set; }

		[JsonIgnore]
		public bool HasIcon => Utils.IsPropSet(Icon.Trim());

		[DefaultValue("")]
		public string Icon { get; set; }

		public List<OptionAction> Actions { get; set; }

		[JsonIgnore]
		public bool HasWarning => Utils.IsPropSet(Warning);

		public ActionWarning Warning { get; set; }

		[JsonIgnore]
		public virtual IDownloadUrl DownloadUrl { get; }


		[JsonIgnore]
		public CheckBoxMod CheckBox
		{
			get
			{
				CheckBoxMod checkBox = new CheckBoxMod();

				checkBox.Click += ComboBoxChecked;
				checkBox.HoverEnter += CheckBox_HoverEnter;
				checkBox.HoverLeave += CheckBox_HoverLeave;

				checkBox.GuideRequest += CheckBox_GuideRequest;

				checkBox.Content = Display;
				checkBox.IsChecked = Enabled;
				checkBox.Tag = this;
				checkBox.Actions = Actions;
				checkBox.Icon = Icon;

				ToolTip toolTip = new ToolTip();
				toolTip.Content = Description;
				toolTip.Tag = checkBox;

				checkBox.ToolTip = toolTip;


				return checkBox;
			}
		}

        public virtual async void CheckBox_GuideRequest(object sender, GuideRequestEventArgs e)
        {
			var HasDocument = Utils.IsPropSet(e.OptionAction.Document) && e.OptionAction.Document != "invalid";

			if (HasDocument)
            {
                try
                {
					var documentText = await Globals.DownloadFileStringAsync(new RemoteDownloadUrl(e.OptionAction.Document));
					Utils.Debug("Loaded guide for " + e.OptionAction.Document);
					e.CheckBoxMod.DocumentText = documentText;
					return;
				}
				catch (System.Exception ex)
                {
					Utils.Debug(ex);
					Utils.Error("Error loading guide " + e.OptionAction.Document);
					Utils.Log(ex, "Error loading guide " + e.OptionAction.Document);
				}
            }
			Utils.Debug("Object has no Document");
			e.CheckBoxMod.DocumentText = "invalid";
		}

        public abstract void CheckBox_HoverLeave(object sender, TextMouseEventArgs e);
		public abstract void CheckBox_HoverEnter(object sender, TextMouseEventArgs e);

        public void OpenGuide()
		{
			if (Guide)
			{
				Utils.OpenLinkInBrowser($"{Globals.URL}guides/{Id}.md");
			}
			else
			{
				Utils.Error("How did we get here?");
			}
		}

		public virtual void ComboBoxChecked(object sender, RoutedEventArgs e)
		{
			var checkBox = sender as CheckBoxMod;
			var isChecked = checkBox.IsChecked;

			if (HasWarning && isChecked)
			{
				var messageBoxText = Warning.Message;

				MessageBoxResult result = MessageBox.Show(messageBoxText, Warning.Title, MessageBoxButton.YesNo, Warning.MessageBoxImage);
				var res = false;
				switch (result)
				{
                    case MessageBoxResult.Yes: res = true; break;
				}
				checkBox.IsChecked = res;
				this.Enabled = res;
			}
			else
			{
				this.Enabled = isChecked;
			}
		}
	}
}
