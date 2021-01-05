using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace SkyblockClient.Options
{
	public class ModOption : Option, IEquatable<ModOption>, IEquatable<string>
	{
		public override IDownloadUrl DownloadUrl => Remote ? (IDownloadUrl)new RemoteDownloadUrl(Url) : new InternalDownloadUrl("mods/" + File);

		[JsonIgnore]
		public bool Caution => Utils.IsPropSet(Warning.Trim());

		[DefaultValue("")]
		public string Warning { get; set; }
		[JsonIgnore]
		public bool Dispersed => Utils.IsPropSet(Dependency.Trim());
		[DefaultValue("")]
		public string Dependency { get; set; }

		public bool Config { get; set; }

		[JsonIgnore]
		public bool HasPacks => Utils.IsPropSet(Packs);	
		public List<string> Packs { get; set; }

		public override string ToString()
		{
			string result = $"{Id}-{Display}\n";
			result += $"\tfile: {File}\n";
			result += $"\tenabled: {Enabled}\n";
			result += $"\tdescription: {Description}\n";
			result += $"\twarning: {Warning}\n";
			result += $"\tremote: {Remote}\n";
			result += $"\turl: {Url}\n";

			return result;
		}

		public bool Equals(string other)
		{
			return Id.Equals(other);
		}

		bool IEquatable<ModOption>.Equals(ModOption other)
		{
			return Id.Equals(other.Id);
		}

		public override void ComboBoxChecked(object sender, RoutedEventArgs e)
		{
			var checkBox = sender as CheckBoxMod;
			var isChecked = checkBox.IsChecked;

			if (Caution && isChecked)
			{
				MessageBoxResult result = MessageBox.Show(Warning + "\n\nUse anyway?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
				switch (result)
				{
					case MessageBoxResult.Yes:
						checkBox.IsChecked = true;
						this.Enabled = true;
						break;
					case MessageBoxResult.No:
						checkBox.IsChecked = false;
						this.Enabled = false;
						break;
					default:
						checkBox.IsChecked = false;
						this.Enabled = false;
						break;
				}
			}
			else
			{	
				this.Enabled = isChecked;
			}

            if (HasPacks && Globals.Settings.enableModDependentPacksOnEnable)
            {
                foreach (var pack in Packs)
                {
                    foreach (var packOption in Globals.packOptions)
                    {
                        if (packOption.Id == pack)
                        {
							packOption.Enabled = Enabled;
                        }
                    }
                }
				Globals.CauseRefreshPacks();
			}
		}
	}
}
