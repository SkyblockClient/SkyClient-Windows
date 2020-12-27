using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace SkyblockClient.Options
{
	public class ModOption : Option, IEquatable<ModOption>, IEquatable<string>
	{
		public override IDownloadUrl downloadUrl => remote ? (IDownloadUrl)new RemoteDownloadUrl(url) : new InternalDownloadUrl("mods/" + file);

		[JsonIgnore]
		public bool caution => IsSet(warning.Trim());

		[DefaultValue("")]
		public string warning { get; set; }
		[JsonIgnore]
		public bool dispersed => IsSet(dependency.Trim());
		[DefaultValue("")]
		public string dependency { get; set; }

		public bool config { get; set; }

		public List<string> packs { get; set; }


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
			var checkBox = sender as CheckBoxMod;
			var isChecked = checkBox.IsChecked;

			if (caution && isChecked)
			{
				MessageBoxResult result = MessageBox.Show(warning + "\n\nUse anyway?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
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

            if (!(packs is null) && Globals.Settings.enableModDependentPacksOnEnable)
            {
                foreach (var pack in packs)
                {
                    foreach (var packOption in Globals.packOptions)
                    {
                        if (packOption.id == pack)
                        {
							packOption.enabled = enabled;
                        }
                    }
                }
				Globals.CauseRefreshPacks();
			}
		}
	}
}
