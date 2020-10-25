using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SkyblockClient
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public string URL = "https://github.com/nacrt/SkyblockClient/blob/main/files/";
		//public string URL = "http://localhost/files/";

		public string tempFolderLocation => Utils.exeLocation + @".temp\";
		public string minecraftLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\.minecraft\";

		public string skyblockResourceLocation => minecraftLocation + @"";
		//public string skyblockResourceLocation => minecraftLocation + @"skyblockclient\";

		public string skyblockModsLocation => skyblockResourceLocation + @"mods\";
		public string skyblockTextureLocation => skyblockResourceLocation + @"ressourcepacks\";

		public bool clearModsFolder => btnClearModsFolder.IsChecked ?? false;

		public List<ModOption> modsList = new List<ModOption>();

		public List<ModOption> enabledModsList => modsList.Where(mod => mod.enabled).ToList();

		public MainWindow()
		{
			InitializeComponent();
			PostConstruct();
		}

		public async void PostConstruct()
		{
			Utils.InitLog();

			try
			{
				string response = await DownloadFileString("info.txt");
				modsList = ModOption.Read(response);
			}
			catch (Exception e)
			{
				Utils.Error("ERROR CONNECTING TO GITHUB", "\tAre you using a proxy?");
				Utils.Log(e, "error connecting to github");
			}

			foreach (var mod in modsList)
			{
				CheckBox checkBox = new CheckBox();
				checkBox.Content = mod.display;
				checkBox.IsChecked = mod.enabled;
				checkBox.Tag = mod;
				checkBox.Click += comboBoxIsChecked;
				stpMods.Children.Add(checkBox);
			}
		}

		private void comboBoxIsChecked(object sender, RoutedEventArgs e)
		{
			var cmb = (CheckBox)sender;
			var tag = (ModOption)cmb.Tag;
			tag.enabled = cmb.IsChecked ?? false;
		}

		private async void BtnInstallModsClick(object sender, RoutedEventArgs e)
		{
			ButtonsEnabled(false);
			await InitializeInstall();
			await ModsInstaller();
			ButtonsEnabled(true);
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

		private void ButtonsEnabled(bool enabled)
		{
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
			tasks.Add(ModsInstaller());
			await Task.WhenAll(tasks.ToArray());
		}

		private async Task ModsInstaller()
		{
			List<ModOption> mods = enabledModsList;
			List<ModOption> firstHalf = mods.Take((mods.Count() + 1) / 2).ToList();
			List<ModOption> secondHalf = mods.Skip((mods.Count() + 1) / 2).ToList();

			List<Task> tasks = new List<Task>();
			tasks.Add(DownloadIndividualMods(firstHalf));
			tasks.Add(DownloadIndividualMods(secondHalf));
			await Task.WhenAll(tasks.ToArray());

			try
			{
				bool skyblockModsLocationExists = Directory.Exists(skyblockModsLocation);
				if (skyblockModsLocationExists)
					Utils.Info("skyblock mods folder exists");
				else
					Utils.Info("skyblock mods folder does not exist");

				if (clearModsFolder)
				{
					if (skyblockModsLocationExists)
						Directory.Delete(skyblockModsLocation, true);
					Directory.CreateDirectory(skyblockModsLocation);
				}
				else
				{
					if (!skyblockModsLocationExists)
						Directory.CreateDirectory(skyblockModsLocation);
				}

				foreach (var file in enabledModsList)
				{
					Utils.Info("Moving " + file.fileName);
					try
					{
						File.Move(tempFolderLocation + file.fileName, skyblockModsLocation + file.fileName);
						Utils.Info("Finished Moving " + file.fileName);
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
				Utils.Log(e, "unkown error in ModsInstaller()");
			}
		}

		private async Task ForgeInstaller()
		{
			Console.WriteLine("Downloading Forge");
			const string FORGE = "forge.exe";
			await DownloadFileByte(FORGE, tempFolderLocation + FORGE);
			Console.WriteLine("Finished Downloading Forge");

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
					byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
					using (FileStream lxFS = new FileStream(fileDestination, FileMode.CreateNew))
					{
						lxFS.Write(lnByte, 0, lnByte.Length);
					}
				}
			}
		}


		private async Task DownloadIndividualMods(List<ModOption> webResponseInfos)
		{
			foreach (var file in webResponseInfos)
			{
				Console.WriteLine("Downloading " + file.display);
				await DownloadFileByte(file.fileName, tempFolderLocation + file.fileName);
				Console.WriteLine("Finished Downloading " + file.display);
			}
		}
	}
}
