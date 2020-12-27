using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;
using SkyblockClient.Options;
using SkyblockClient.Persistence;
using Newtonsoft.Json;
using System.IO;

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
			Globals.MainWindow = this;
			Utils.LoadSettings();
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

			RefreshMods();
			RefreshPacks();
		}

		private async Task DownloadResourceFile()
		{
			string response = await Globals.DownloadFileStringAsync("packs.json");
			Globals.packOptions = JsonConvert.DeserializeObject<List<PackOption>>(response, Utils.JsonSerializerSettings);
		}

		public void RefreshPacks()
        {
			lbPacks.Items.Clear();
			foreach (PackOption pack in Globals.packOptions)
			{
				if (!pack.hidden)
				{
					lbPacks.Items.Add(pack.CheckBox);
				}
			}
		}
		public void RefreshMods()
		{
			lbMods.Items.Clear();
			foreach (ModOption mod in Globals.modOptions)
			{
				if (!mod.hidden)
				{
					lbMods.Items.Add(mod.CheckBox);
				}
			}
		}

		private async Task DownloadModsFile()
		{
			string response = await Globals.DownloadFileStringAsync("mods.json");
			Globals.modOptions = JsonConvert.DeserializeObject<List<ModOption>>(response, Utils.JsonSerializerSettings);
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
			var crashedAt = string.Empty;
			try
			{
				crashedAt = "await InitializeInstall();";
				await InitializeInstall();
				crashedAt = "await Persist();";
				await Persist();

				crashedAt = "await Utils.ExecuteAsyncronous(";
				await Utils.ExecuteAsyncronous(
					InstallForge(),
					InstallMods(),
					InstallPacks()
				);
				crashedAt = "NotifyCompleted";
				NotifyCompleted("The installation is done!\nNow all that's left is to start the the minecraft launcher and pick the SkyClient profile");

			}
			catch (Exception e)
			{
				Utils.Error(e.Message, "An exception occured during the Installation, the program might not have installed correctly, please submit the log file to the author");
				Utils.Log(e, "crashAt:"+ crashedAt, "Exception occured at MainWindow.StartInstaller()");
			}
		}

		public async Task InstallPacks()
		{
			await PersistenceMain.InstallPacks(Globals.neededPacks);
		}

		public async Task InstallMods()
		{
			await PersistenceMain.InstallMods(Globals.neededMods);
		}

		private void CreateJson()
		{
			/*
			var jsonString = JsonConvert.SerializeObject(Globals.resourceOptions, Utils.JsonSerializerSettings);
			File.WriteAllText("packs.json", jsonString);
			Utils.Error(jsonString);
			*/
		}
	}
}
