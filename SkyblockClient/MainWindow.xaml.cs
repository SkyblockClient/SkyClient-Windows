using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SkyblockClient
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		//public string URL = "https://github.com/nacrt/SkyblockClient/blob/main/files";
		public string URL = "http://localhost/files";

		public string exeLocation = Assembly.GetEntryAssembly().Location;
		public string tempFolderLocation => exeLocation + ".temp/";
		public string minecraftLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\.minecraft\";

		public string skyblockResourceLocation => minecraftLocation + @"";
		//public string skyblockResourceLocation => minecraftLocation + @"skyblockclient\";

		public string skyblockModsLocation => skyblockResourceLocation + @"mods\";
		public string skyblockTextureLocation => skyblockResourceLocation + @"ressourcepacks\";


		public List<WebResponseInfo> downloadableFiles = new List<WebResponseInfo>();

		public MainWindow()
		{
			InitializeComponent();
			PostConstruct();
			Console.WriteLine(minecraftLocation);
		}

		public async void PostConstruct()
		{
			string response = await DownloadFileString("info.txt");
			downloadableFiles = WebResponseInfo.Read(response);
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			btnStartInstallation.IsEnabled = false;
			Directory.Delete(tempFolderLocation, true);
			Directory.CreateDirectory(tempFolderLocation);

			List<Task> tasks = new List<Task>();
			tasks.Add(ForgeInstaller());
			tasks.Add(ModsInstaller());
			await Task.WhenAll(tasks.ToArray());

			btnStartInstallation.IsEnabled = true;
		}

		public async Task<string> DownloadFileString(string file)
		{
			WebResponse myWebResponse = await WebRequest.Create(URL + file + "?raw=true").GetResponseAsync();
			return await new StreamReader(myWebResponse.GetResponseStream()).ReadToEndAsync();
		}

		public async Task DownloadFileByte(string file,string fileDestination)
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

		private async Task ModsInstaller()
		{
			foreach (var file in downloadableFiles)
			{
				Console.WriteLine("Downloading " + file.display);
				await DownloadFileByte(file.fileName, tempFolderLocation + file.fileName);
				Console.WriteLine("Finished Downloading " + file.display);
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
			startInfo.WindowStyle = ProcessWindowStyle.Normal;
			startInfo.FileName = "cmd.exe";
			startInfo.Arguments = $"/C {tempFolderLocation}forge.exe";
			process.StartInfo = startInfo;
			process.Start();
			process.WaitForExit();
		}
	}

	public class WebResponseInfo
	{
		public static List<WebResponseInfo> Read(string text)
		{
			var lines = new List<string>(text.Split(new string[1] { "\n" }, StringSplitOptions.None));
			var result = new List<WebResponseInfo>();
			foreach (var line in lines)
			{
				if (line != "")
				{
					var wri = new WebResponseInfo(line);
					result.Add(wri);
				}
			}
			return result;
		}

		public string key = "";
		public string fileName = "";
		public bool defaultValue = true;
		public string display = "";

		public WebResponseInfo(string respLine)
		{
			const int KEY_INDEX = 0, DEFAULT_INDEX = 1, FILE_NAME_INDEX = 2, DISPLAY_INDEX = 3;

			var split = respLine.Split(':');
			if (split.Length != 4)
			{
				throw new ArgumentException("Line is either malformed or Empty");
			}

			string boolpart = split[DEFAULT_INDEX].Trim().ToLower();

			if(boolpart == "true")
			{
				defaultValue = true;
			}
			else if (boolpart == "false")
			{
				defaultValue = false;
			}
			else
			{
				throw new ArgumentException("Default Value is neither true or false");
			}

			key = split[KEY_INDEX].Trim().ToLower();
			fileName = split[FILE_NAME_INDEX].Trim();
			display = split[DISPLAY_INDEX].Trim();
		}
	}
}
