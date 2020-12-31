using SkyblockClient.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace SkyblockClient
{
	public static class Globals
	{
		public const string GITHUB_RELEASES = "https://github.com/nacrt/SkyblockClient/releases/latest";
		public const string PERSISTANCE_JSON_NAME = "skyclient.json";
		public static Version assembyVersion => Assembly.GetExecutingAssembly().GetName().Version;

		public static GlobalSettings Settings
        {
			get
            {
                if (_settings is null)
					_settings = new GlobalSettings();
				return _settings;
            }
			set => _settings = value;
        }
		private static GlobalSettings _settings;
		
		public static bool isDebugEnabled
		{
			get
			{
				if (!setValues)
				{
					InitializeValues();
				}
				return _isDebugEnabled;
			}
			set
			{
				_isDebugEnabled = value;
			}
		}
		private static bool _isDebugEnabled = false;

		public static string URL
		{
			get
			{
				if (!setValues)
				{
					InitializeValues();
				}
				return _url;
			}
			set
			{
				_url = value;
			}
		}
		private static string _url = "";

		private static bool setValues = false;

		public static string tempFolderLocation => Utils.exeLocation + @".temp\";
		public static string minecraftRootLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft");
		public static string launcherProfilesLocation = Path.Combine(minecraftRootLocation, "launcher_profiles.json");
		public static string minecraftLibrariesLocation = Path.Combine(minecraftRootLocation, "libraries");
		public static string minecraftVersionsLocation = Path.Combine(minecraftRootLocation, "versions");

		public static string gameDirectory = "skyclient";
		public static string skyblockRootLocation => Path.Combine(minecraftRootLocation, gameDirectory);

		public static string skyblockConfigLocation => Path.Combine(skyblockRootLocation, "config");
		public static string skyblockModsLocation => Path.Combine(skyblockRootLocation, "mods");
		public static string skyblockResourceLocation => Path.Combine(skyblockRootLocation, "resourcepacks");

		public static string skyblockPersistenceLocation => Path.Combine(skyblockRootLocation, PERSISTANCE_JSON_NAME);

		public static void CauseRefreshPacks()
        {
			MainWindow.RefreshPacks();
		}

		public static void CauseRefreshMods() 
		{
			MainWindow.RefreshMods();
		}

		public static void ShowInfo(string text, string title, System.Windows.MessageBoxButton messageBoxButton = System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage messageBoxImage = System.Windows.MessageBoxImage.Information)
        {
			MainWindow.ShowInfo(text, title, messageBoxButton, messageBoxImage);
        }

		public static string skyblockOptionsLocation => Path.Combine(skyblockRootLocation, "options.txt");

		public static List<ModOption> modOptions = new List<ModOption>();
		public static List<PackOption> packOptions = new List<PackOption>();

        public static List<ModOption> enabledModOptions => modOptions.Where(mod => mod.enabled).ToList();
		public static List<PackOption> enabledPackOptions => packOptions.Where(pack => pack.enabled).ToList();

		public static List<ModOption> neededMods
		{
			get
			{
				var enabled = enabledModOptions;
				var result = new List<ModOption>();
				result.AddRange(enabled);

				var libraries = new List<ModOption>();

				foreach (var mod in enabled)
				{
					if (mod.Dispersed)
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

		public static List<PackOption> neededPacks
		{
			get
			{
				var enabled = enabledPackOptions;
				var result = new List<PackOption>();
				result.AddRange(enabled);
				return result;
			}
		}

        public static MainWindow MainWindow { get; internal set; }

        public static void InitializeValues()
		{
			string result = "https://github.com/nacrt/SkyblockClient-REPO/blob/main/files/";

			try
			{
				var directory = Path.GetFileName(Directory.GetCurrentDirectory());
				if (directory == "Debug")
				{
					Globals.setValues = true;
					result = "http://localhost/files/";
					isDebugEnabled = true;
					gameDirectory = "skytest";
					Utils.Info("Connect domain:", result);
					Utils.Info("Debug mode enabled");
				}
			}
			catch (Exception e)
			{
				Utils.Log(e, "Exception in getting current directory", "assuming non-debug");
			}
			URL = result;
		}

		public static async Task<string> DownloadFileStringAsync(string file)
		{
			WebResponse myWebResponse = await WebRequest.Create(Globals.URL + file + "?raw=true").GetResponseAsync();
			return await new StreamReader(myWebResponse.GetResponseStream()).ReadToEndAsync();
		}

		public static string DownloadFileString(string file)
		{
			WebResponse myWebResponse = WebRequest.Create(Globals.URL + file + "?raw=true").GetResponse();
			return new StreamReader(myWebResponse.GetResponseStream()).ReadToEnd();
		}

		public static async Task DownloadFileByte(string file, string fileDestination)
		{
			IDownloadUrl downloadUrl = new InternalDownloadUrl(file);
			await DownloadFileByte(downloadUrl, fileDestination);
		}

		public static async Task DownloadFileByte(IDownloadUrl downloadUrl, string fileDestination)
		{
			string endpoint = downloadUrl.Url;

			Utils.Debug(endpoint);
            if (Globals.isDebugEnabled && Globals.Settings.copyAllEndpointsToClipboardOnDebug)
            {
				Utils.SetClipboard(endpoint);
            }

			var errorAt = string.Empty;
			try
            {
				errorAt = "using (WebResponse webResponse = await Task.Run(() => WebRequest.Create(endpoint).GetResponse()))";
				using (WebResponse webResponse = await Task.Run(() => WebRequest.Create(endpoint).GetResponse()))
				{
					errorAt = "using (BinaryReader reader = new BinaryReader(webResponse.GetResponseStream()))";
					using (BinaryReader reader = new BinaryReader(webResponse.GetResponseStream()))
					{
						errorAt = "bool exists = File.Exists(fileDestination);";
						bool exists = File.Exists(fileDestination);
						if (exists)
                        {
							errorAt = "File.Delete(fileDestination);";
							File.Delete(fileDestination);
						}

						try
						{
							errorAt = "using (FileStream lxFS = new FileStream(fileDestination, FileMode.Create))";
							using (FileStream lxFS = new FileStream(fileDestination, FileMode.Create))
							{
								while (true)
								{
									errorAt = "byte[] lnByte = reader.ReadBytes(1024 * 1024);";
									byte[] lnByte = reader.ReadBytes(1024 * 1024); // 1 mb each package
									if (lnByte.Length == 0)
										break;
									errorAt = "lxFS.Write(lnByte, 0, lnByte.Length);";
									lxFS.Write(lnByte, 0, lnByte.Length);
								}
							}
						}
						catch (Exception e)
						{
							Utils.Error(e.Message, "Failed at creating or Writing to FileStream in Globals.DownloadFileByte");
							Utils.Log(e, "Failed at creating or Writing to FileStream in Globals.DownloadFileByte", "errorAt:" + errorAt);
						}
					}
				}
			}
            catch (Exception e)
            {
				if (Globals.isDebugEnabled && Globals.Settings.copyEndpointToClipboardOnDebugOnError)
                {
					Utils.SetClipboard(endpoint);
                }
				Utils.Error("Error sending WebRequest");
				Utils.Log(e, "Error sending WebRequest", "errorAt:" + errorAt);
				throw new Exception("Error sending WebRequest", e);
            }

		}
	}
}
