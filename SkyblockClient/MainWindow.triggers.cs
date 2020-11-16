using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SkyblockClient.Option;

namespace SkyblockClient
{
	public partial class MainWindow
	{
		private async void BtnInstallPacksClick(object sender, RoutedEventArgs e)
		{
			ButtonsEnabled(false);
			await InitializeInstall();
			await Installer(skyblockResourceLocation, enabledResourcepackOptions.ToArray(), "resourcepacks");

			var enabled = enabledResourcepackOptions;
			enabled.Reverse();

			string result = "resourcePacks:[";
			foreach (var pack in enabled)
			{
				result += $"\"{pack.file}\",";
			}
			result = result.Remove(result.Length - 1);
			result += "]";

			File.WriteAllText(skyblockRootLocation + "options.txt", result);
			ButtonsEnabled(true);

			NotifyCompleted("All the texturepacks have been installed");
		}

		private async void BtnInstallModsClick(object sender, RoutedEventArgs e)
		{
			ButtonsEnabled(false);
			await InitializeInstall();
			await Installer(skyblockModsLocation, neededMods.ToArray(), "mods");
			ButtonsEnabled(true);

			NotifyCompleted("All the mods have been installed");
		}

		private async void BtnInstallForgeClick(object sender, RoutedEventArgs e)
		{
			ButtonsEnabled(false);
			await InitializeInstall();
			await ForgeInstaller();
			ButtonsEnabled(true);
		}

		private async void BtnInstallModsAndForgeClick(object sender, RoutedEventArgs e)
		{
			ButtonsEnabled(false);
			await StartForgeAndModsInstaller();
			ButtonsEnabled(true);
		}

		private void BtnAdvancedSettinsClick(object sender, RoutedEventArgs e)
		{
			var frmAdvancedSettings = new FrmAdvancedSettings(this);
			frmAdvancedSettings.Show();
		}
	}
}
