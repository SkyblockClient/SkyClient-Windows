using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SkyblockClient
{
	public partial class MainWindow
	{
		private async void BtnInstallPacksClick(object sender, RoutedEventArgs e)
		{
			ButtonsEnabled(false);
			await InitializeInstall();
			await Installer(Globals.skyblockResourceLocation, enabledResourcepackOptions.ToArray(), "resourcepacks", false);

			try
			{
				var enabled = enabledResourcepackOptions;
				enabled.Reverse();

				string optionsLocation = Path.Combine(Globals.skyblockRootLocation, "options.txt");

				List<string> lines;

				if (File.Exists(optionsLocation))
				{
					lines = new List<string>(File.ReadAllLines(optionsLocation));
				}
				else
				{
					lines = new List<string>();
				}

				string packLine = "resourcePacks:[";
				foreach (var pack in enabled)
				{
					packLine += $"\"{pack.file}\",";
				}
				packLine = packLine.Remove(packLine.Length - 1);
				packLine += "]";

				bool foundPackLine = false;
				int indexPackLine = -1;

				for (int i = 0; i < lines.Count; i++)
				{
					if (lines[i].StartsWith("resourcePacks:"))
					{
						if (!foundPackLine)
						{
							foundPackLine = true;
							indexPackLine = i;
						}
						else
						{
							lines[i] = "";
						}
					}
				}

				if (foundPackLine)
				{
					lines[indexPackLine] = packLine;
				}
				else
				{
					lines.Add(packLine);
				}

				File.WriteAllLines(optionsLocation, lines);
			}
			catch (IOException ex)
			{
				Utils.Error("Failed Reading or Writing options.txt -> Skipping");
				Utils.Log(ex, "Failed Reading or Writing options.txt");
			}

			ButtonsEnabled(true);

			NotifyCompleted("All the texturepacks have been installed");
		}

		private async void BtnInstallModsClick(object sender, RoutedEventArgs e)
		{
			ButtonsEnabled(false);
			await InitializeInstall();
			await Task.Run(() => Installer(Globals.skyblockModsLocation, neededMods.ToArray(), "mods", true));
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
			OpenAdvancedSettings();
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				Rect rect = new Rect(this.rectDragRectangle.RenderSize);
				if (rect.Contains(e.GetPosition(this)))
				{
					ReleaseCapture();
					SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
				}
			}
		}

		private void BtnClose_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void BtnFullscreen_Click(object sender, RoutedEventArgs e)
		{
			if (WindowState == WindowState.Normal)
			{
				WindowState = WindowState.Maximized;
			}
			else
			{
				WindowState = WindowState.Normal;
			}
		}

		private void BtnMinimize_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

		private void Window_StateChanged(object sender, EventArgs e)
		{
			if (WindowState == WindowState.Maximized)
			{
				BtnFullscreen.Content = "❒";
			}
			else
			{
				BtnFullscreen.Content = "▢";
			}
		}
	}
}
