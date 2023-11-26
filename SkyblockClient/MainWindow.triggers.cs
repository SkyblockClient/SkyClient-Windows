using System;
using System.Windows;
using System.Windows.Input;

namespace SkyblockClient
{
	public partial class MainWindow
	{
		private void BtnAdvancedSettinsClick(object sender, RoutedEventArgs e)
		{
			OpenAdvancedSettings();
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

		private void HeaderGrid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				//ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}

		private void Window_StateChanged(object sender, EventArgs e)
		{
			/*
			if (WindowState == WindowState.Maximized)
			{
				BtnFullscreen.Content = "❒";
			}
			else
			{
				BtnFullscreen.Content = "▢";
			}
			*/
		}

		private void BtnUpdate_Click(object sender, RoutedEventArgs e)
		{
			Utils.Info("The update function is no longer available, please use the SkyClientUpdater Mod instead!");
			Globals.ShowInfo("The update function is no longer available, please use the SkyClientUpdater Mod instead!", "Info");
		}

		private async void BtnJoinDiscord_Click(object sender, RoutedEventArgs e)
		{
			Utils.OpenLinkInBrowser(Globals.DISCORD_INVITE);
		}

        private async void BtnInstallModsAndForgeClick(object sender, RoutedEventArgs e)
		{
			ButtonsEnabled(false);
			await StartInstaller();
			ButtonsEnabled(true);
		}
	}
}
