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

		//public bool clearModsFolder => btnClearModsFolder.IsChecked ?? false;
		public bool clearModsFolder =>  false;

		public List<ModOption> modOptions = new List<ModOption>();
		public List<ResourcepackOption> resourceOptions = new List<ResourcepackOption>();

		public List<ModOption> enabledModOptions => modOptions.Where(mod => mod.enabled).ToList();
		public List<ResourcepackOption> enabledResourcepackOptions => resourceOptions.Where(pack => pack.enabled).ToList();

		public List<ModOption> neededMods
		{ 
			get
			{
				var enabled = enabledModOptions;
				var result = new List<ModOption>();
				result.AddRange(enabled);

				var libraries = new List<ModOption>();

				foreach (var mod in enabled)
				{
					if (mod.dispersed)
					{
						foreach (var library in modOptions)
						{
							if (mod.dependency == library.id)
							{
								if (!libraries.Contains(library))
								{
									libraries.Add(library);
								}
								break;
							}
						}
					}
				}

				result.AddRange(libraries);

				return result;
			}
		}

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

			foreach (ModOption mod in modOptions)
			{
				if (!mod.hidden)
				{
					Utils.Debug(mod.display);
					lbMods.Items.Add(mod.CheckBox);
				}
			}

			foreach (ResourcepackOption pack in resourceOptions)
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
			resourceOptions = OptionHelper.Read<ResourcepackOption>(response);
		}

		private async Task DownloadModsFile()
		{
			string response = await Globals.DownloadFileString("mods.txt");
			modOptions = OptionHelper.Read<ModOption>(response);
		}

		private void ButtonsEnabled(bool enabled)
		{
			btnInstallPacks.IsEnabled = enabled;
			btnInstall.IsEnabled = enabled;
		}

		private async Task InitializeInstall()
		{
			bool exists = await Task.Run(() => Directory.Exists(Globals.tempFolderLocation));
			if (exists)
				Directory.Delete(Globals.tempFolderLocation, true);
			Directory.CreateDirectory(Globals.tempFolderLocation);
		}

		public async Task StartForgeAndModsInstaller()
		{
			await InitializeInstall();

			List<Task> tasks = new List<Task>();
			tasks.Add(ForgeInstaller());
			tasks.Add(Installer(Globals.skyblockModsLocation, enabledModOptions.ToArray(), "mods", true));
			await Task.WhenAll(tasks.ToArray());

			NotifyCompleted("Finished installing.\nAll that's left is to start your vanilla launcher and play the game.");
		}

		private async Task ForgeInstaller()
		{
			
			bool valid = Utils.ValidateMinecraftDirectory(Globals.minecraftRootLocation);
			if (!valid)
			{
				string msg = $"\"{Globals.minecraftRootLocation}\" is not a valid minecraft directory.\nMake sure you run the minecraft launcher at least once.";
				MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}

			const string JAVA_LINK = "https://www.java.com/en/download/manual.jsp";
			bool correctJavaVersion = false;

			var javaProcess = Process.Start(Utils.CreateProcessStartInfo("java.exe", "-version"));

			List<string> jLines = new List<string>();

			while (!javaProcess.StandardError.EndOfStream)
			{
				jLines.Add(javaProcess.StandardError.ReadLine());
			}

			if (jLines.Count == 3 && jLines[0].Split('"')[1].StartsWith("1.8."))
			{
				correctJavaVersion = true;
			}
			else
			{
				correctJavaVersion = false;
				Utils.Error("You are Using the wrong version of Java.");
				Utils.Error("Download the newest version here:");
				Utils.Error(JAVA_LINK);
				Utils.Error("Select \"Windows Offline (64-Bit)\"");
				Process.Start(Utils.CreateProcessStartInfo("cmd.exe", $"/c start {JAVA_LINK}"));
			}
			
			if (correctJavaVersion)
			{
				await SkyblockClient.Forge.ForgeInstaller.Work();
			}

			await Task.CompletedTask;
		}

		private async Task DownloadIndividualMods(List<Options.Option> modOptions)
		{
			foreach (var mod in modOptions)
			{
				try
				{
					Utils.Info("Downloading " + mod.display);
					await Globals.DownloadFileByte(mod.downloadUrl, Globals.tempFolderLocation + mod.file);
					Utils.Info("Finished Downloading " + mod.display);
				}
				catch (WebException webE)
				{
					var msg = "Error while Downloading " + mod.display;
					Utils.Error(msg);
					Utils.Log(webE, "msg:"+msg, "webE.Status:" + webE.Status, "webE.Data:" + webE.Data.ToString());
				}
				catch (Exception e)
				{
					var msg = "Error while Downloading " + mod.display;
					Utils.Error(msg);
					Utils.Log(e, msg);
				}
			}
		}

		private async Task Installer(string location, Options.Option[] enabledOptions, string foldername, bool isMods)
		{
			List<Options.Option> firstHalf = enabledOptions.Take((enabledOptions.Count() + 1) / 2).ToList();
			List<Options.Option> secondHalf = enabledOptions.Skip((enabledOptions.Count() + 1) / 2).ToList();

			List<Task> tasks = new List<Task>();
			tasks.Add(DownloadIndividualMods(firstHalf));
			tasks.Add(DownloadIndividualMods(secondHalf));
			await Task.WhenAll(tasks.ToArray());

			try
			{
				bool locationExists = Directory.Exists(location);
				if (locationExists)
					Utils.Info($"skyblock {foldername} folder exists");
				else
					Utils.Info($"skyblock {foldername} folder does not exist");

				if (locationExists)
				{
					DirectoryInfo di = new DirectoryInfo(location);

					foreach (FileInfo file in di.GetFiles())
					{
						file.Delete();
					}

				}
				Directory.CreateDirectory(location);

				foreach (var file in enabledOptions)
				{
					Utils.Info("Moving " + file.file);
					try
					{
						File.Move(Path.Combine(Globals.tempFolderLocation, file.file), Path.Combine(location, file.file));
						Utils.Info("Finished Moving " + file.file);
					}
					catch (Exception e)
					{
						Utils.Error("Failed Moving " + file.display);
						Utils.Log(e, "failed moving " + file.display);
					}
				}

				if (isMods)
				{
					foreach (ModOption mod in enabledOptions)
					{
						List<Task> tasks1 = new List<Task>();
						try
						{
							tasks1.Add(Config.ConfigUtils.CreateModConfigWorker(mod));
						}
						catch (Exception e)
						{
							Utils.Debug(e.Message);
							Utils.Error("An Unknown error occured, please submit the log file");
							Utils.Log(e, "unkown error in Installer():if(isMods)");
						}
						await Task.WhenAll(tasks1.ToArray());
					}
				}
			}
			catch (Exception e)
			{
				Utils.Error("An Unknown error occured, please submit the log file");
				Utils.Log(e, "unkown error in Installer()");
			}
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
	}
}
