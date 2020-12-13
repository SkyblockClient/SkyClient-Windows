using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using SkyblockClient.Options;
using SkyblockClient.Persistence;

namespace SkyblockClient
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;

		IntPtr Handle
		{
			get
			{
				if (_handle == IntPtr.Zero)
				{
					_handle = new System.Windows.Interop.WindowInteropHelper(this).Handle;
				}
				return _handle;
			}
		}
		IntPtr _handle = IntPtr.Zero;

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		public MainWindow()
		{
			InitializeComponent();
			PostConstruct();
			if (!Globals.isDebugEnabled)
			{
				Utils.Info("Thank you for using SkyClient", "This is the output Console and will display information important to the developer!");
			}
		}

		public async void PostConstruct()
		{
			await Utils.InitLog();

			try
			{
				List<Task> tasks = new List<Task>();
				tasks.Add(DownloadModsFile());
				tasks.Add(DownloadResourceFile());
				await Task.WhenAll(tasks.ToArray());
			}
			catch (Exception e)
			{
				Utils.Error("ERROR CONNECTING TO GITHUB", "\tAre you using a proxy?","\tYou might also be using an outdated version! Update!");
				Utils.Log(e, "error connecting to github");
				if (!Globals.isDebugEnabled)
				{
					string endpoint = $"{Globals.GITHUB_RELEASES}";
					string command = $"/c start {endpoint}";
					var processInfo = Utils.CreateProcessStartInfo("cmd.exe", command);
					Process.Start(processInfo);
				}
			}

			foreach (ModOption mod in Globals.modOptions)
			{
				if (!mod.hidden)
				{
					Utils.Debug(mod.display);
					lbMods.Items.Add(mod.CheckBox);
				}
			}

			foreach (PackOption pack in Globals.resourceOptions)
			{
				if (!pack.hidden)
				{
					lbPacks.Items.Add(pack.CheckBox);
				}
			}
		}

		private async Task DownloadResourceFile()
		{
			string response = await Globals.DownloadFileString("resourcepacks.txt");
			Globals.resourceOptions = OptionHelper.Read<PackOption>(response);
		}

		private async Task DownloadModsFile()
		{
			string response = await Globals.DownloadFileString("mods.txt");
			Globals.modOptions = OptionHelper.Read<ModOption>(response);
		}

		private void ButtonsEnabled(bool enabled)
		{
			btnUpdate.IsEnabled = enabled;
			btnInstall.IsEnabled = enabled;
		}

		private async Task InitializeInstall()
		{
			await Utils.InitializeInstall();
		}

		private async Task InstallForge()
		{
			await Forge.ForgeInstaller.Work();
		}

		private void NotifyCompleted(string message)
		{
			Thread thread = new Thread(NotifyCompletedInternal);
			thread.Start(message);
		}

		private void NotifyCompletedInternal(object obj)
		{
			MessageBox.Show((string)obj, "Completed", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		public bool frmAdvancedSettingsIsOpen = false;
		public FrmAdvancedSettings frmAdvancedSettings = null;
		private void OpenAdvancedSettings()
		{
			if (!frmAdvancedSettingsIsOpen)
			{
				frmAdvancedSettings = new FrmAdvancedSettings(this);
				frmAdvancedSettings.Show();
				frmAdvancedSettingsIsOpen = true;
			}
			else
			{
				if (frmAdvancedSettings != null)
				{
					frmAdvancedSettings.Show();
					frmAdvancedSettingsIsOpen = true;
				}
			}
		}

		public async Task Persist()
		{
			await PersistenceMain.Update();
		}

		public async Task StartInstaller()
		{
			await InitializeInstall();
			await Persist();

			await Utils.ExecuteAsyncronous(
				InstallForge(),
				InstallMods(),
				InstallPacks()
			);
		}

		public async Task InstallPacks()
		{
			await PersistenceMain.InstallPacks(Globals.neededPacks);
		}

		public async Task InstallMods()
		{
			await PersistenceMain.InstallMods(Globals.neededMods);
		}
	}
}
