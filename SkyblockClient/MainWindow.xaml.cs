using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SkyblockClient.Option;

namespace SkyblockClient
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		//public string URL = "https://github.com/nacrt/SkyblockClient/blob/main/files/";
		public string URL = "http://localhost/files/";

		public string tempFolderLocation => Utils.exeLocation + @".temp\";
		public string minecraftLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\.minecraft\";

		public string skyblockRootLocation => minecraftLocation + gameDirectory;
		public string gameDirectory = "";

		public string skyblockModsLocation => skyblockRootLocation + @"mods\";
		public string skyblockResourceLocation => skyblockRootLocation + @"resourcepacks\";

		public bool clearModsFolder => btnClearModsFolder.IsChecked ?? false;

		public List<IOption> modOptions = new List<IOption>();
		public List<IOption> resourceOptions = new List<IOption>();

		public List<IOption> enabledModOptions => modOptions.Where(mod => mod.enabled).ToList();
		public List<IOption> enabledResourcepackOptions => resourceOptions.Where(pack => pack.enabled).ToList();

		public MainWindow()
		{
			InitializeComponent();
			PostConstruct();
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
				Utils.Error("ERROR CONNECTING TO GITHUB", "\tAre you using a proxy?");
				Utils.Log(e, "error connecting to github");
			}

			foreach (ModOption mod in modOptions)
			{
				CheckBox checkBox = new CheckBox();
				checkBox.Content = mod.display;
				checkBox.IsChecked = mod.enabled;
				checkBox.Tag = mod;
				checkBox.Click += ModComboBoxIsChecked;

				AddToolTip(checkBox, mod);

				stpMods.Children.Add(checkBox);
			}

			foreach (ResourcepackOption pack in resourceOptions)
			{
				if (!pack.hidden)
				{
					CheckBox checkBox = new CheckBox();
					checkBox.Content = pack.display;
					checkBox.IsChecked = pack.enabled;
					checkBox.Tag = pack;
					checkBox.Click += ResourceComboBoxIsChecked;
					AddToolTip(checkBox, pack);
					stpResources.Children.Add(checkBox);
				}
			}
		}

		private async Task DownloadResourceFile()
		{
			string response = await DownloadFileString("resourcepacks.txt");
			resourceOptions = OptionHelper.Read<ResourcepackOption>(response);
		}
		private async Task DownloadModsFile()
		{
			string response = await DownloadFileString("mods.txt");
			modOptions = OptionHelper.Read<ModOption>(response);
		}

		private void ModComboBoxIsChecked(object sender, RoutedEventArgs e)
		{
			var cmb = (CheckBox)sender;
			var tag = (ModOption)cmb.Tag;

			var isChecked = cmb.IsChecked ?? false;

			if (tag.caution && isChecked)
			{
				MessageBoxResult result = MessageBox.Show(tag.warning + "\n\nUse anyway?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
				switch (result)
				{
					case MessageBoxResult.Yes:
						cmb.IsChecked = true;
						tag.enabled = true;
						break;
					case MessageBoxResult.No:
						cmb.IsChecked = false;
						tag.enabled = false;
						break;
					default:
						cmb.IsChecked = false;
						tag.enabled = false;
						break;
				}
			}
			else
			{
				tag.enabled = isChecked; 
			}

		}

		private void ResourceComboBoxIsChecked(object sender, RoutedEventArgs e)
		{
			var cmb = (CheckBox)sender;
			var tag = (ResourcepackOption)cmb.Tag;
			tag.enabled = cmb.IsChecked ?? false;
		}

		private void ButtonsEnabled(bool enabled)
		{
			btnInstallPacks.IsEnabled = enabled;
			btnInstallMods.IsEnabled = enabled;
			btnInstallForge.IsEnabled = enabled;
			btnInstallModsAndForge.IsEnabled = enabled;
		}

		private async Task InitializeInstall()
		{
			bool exists = await Task.Run(() => Directory.Exists(tempFolderLocation));
			if (exists)
				Directory.Delete(tempFolderLocation, true);
			Directory.CreateDirectory(tempFolderLocation);
		}

		public async Task StartForgeAndModsInstaller()
		{
			await InitializeInstall();

			List<Task> tasks = new List<Task>();
			tasks.Add(ForgeInstaller());
			tasks.Add(Installer(skyblockModsLocation,enabledModOptions,"mods"));
			await Task.WhenAll(tasks.ToArray());
		}

		private async Task ForgeInstaller()
		{
			Utils.Info("Downloading Forge");
			const string FORGE = "forge.exe";
			await DownloadFileByte(FORGE, tempFolderLocation + FORGE);
			Utils.Info("Finished Downloading Forge");

			Process process = new Process();
			ProcessStartInfo startInfo = new ProcessStartInfo();
			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			startInfo.FileName = "cmd.exe";
			startInfo.Arguments = $"/C {tempFolderLocation}forge.exe";
			process.StartInfo = startInfo;
			process.Start();
			process.WaitForExit();
		}

		public async Task<string> DownloadFileString(string file)
		{
			WebResponse myWebResponse = await WebRequest.Create(URL + file + "?raw=true").GetResponseAsync();
			return await new StreamReader(myWebResponse.GetResponseStream()).ReadToEndAsync();
		}

		public async Task DownloadFileByte(string file, string fileDestination)
		{
			using (WebResponse webResponse = await WebRequest.Create(URL + file + "?raw=true").GetResponseAsync())
			{
				using (BinaryReader reader = new BinaryReader(webResponse.GetResponseStream()))
				{
					bool exists = File.Exists(fileDestination);
					if (exists)
						File.Delete(fileDestination);

					while (true)
					{
						byte[] lnByte = reader.ReadBytes(1024 * 1024); // 1 mb each package
						if (lnByte.Length == 0)
							break;

						using (FileStream lxFS = new FileStream(fileDestination, FileMode.Append))
						{
							lxFS.Write(lnByte, 0, lnByte.Length);
						}
					}
				}
			}
		}

		private async Task DownloadIndividualMods(List<IOption> modOptions)
		{
			foreach (var mod in modOptions)
			{
				try
				{
					Utils.Info("Downloading " + mod.display);
					await DownloadFileByte(mod.file, tempFolderLocation + mod.file);
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

		private async Task Installer(string location, List<IOption> enabledOptions, string foldername)
		{
			List<IOption> firstHalf = enabledOptions.Take((enabledOptions.Count() + 1) / 2).ToList();
			List<IOption> secondHalf = enabledOptions.Skip((enabledOptions.Count() + 1) / 2).ToList();

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
						File.Move(tempFolderLocation + file.file, location + file.file);
						Utils.Info("Finished Moving " + file.file);
					}
					catch (Exception e)
					{
						Utils.Error("Failed Moving " + file.display);
						Utils.Log(e, "failed moving " + file.display);
					}
				}
			}
			catch (Exception e)
			{
				Utils.Error("An Unknown error occured, please submit the log file");
				Utils.Log(e, "unkown error in Installer()");
			}
		}

		private void AddToolTip(CheckBox checkBox, IOption option)
		{
			AddToolTip(checkBox, option.description);
		}

		private void AddToolTip(CheckBox checkBox, string description)
		{
			ToolTip toolTip = new ToolTip();
			toolTip.Content = description;
			checkBox.ToolTip = toolTip;
		}
	}
}
