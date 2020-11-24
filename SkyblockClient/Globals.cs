using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SkyblockClient
{
	public static class Globals
	{
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

		public static string gameDirectory = "";
		public static string skyblockRootLocation => Path.Combine(minecraftRootLocation, gameDirectory);

		public static string skyblockConfigLocation => Path.Combine(skyblockRootLocation, "config");
		public static string skyblockModsLocation => Path.Combine(skyblockRootLocation, "mods");
		public static string skyblockResourceLocation => Path.Combine(skyblockRootLocation, "resourcepacks");

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
					gameDirectory = "skyblockclient";
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

		public static async Task<string> DownloadFileString(string file)
		{
			WebResponse myWebResponse = await WebRequest.Create(Globals.URL + file + "?raw=true").GetResponseAsync();
			return await new StreamReader(myWebResponse.GetResponseStream()).ReadToEndAsync();
		}

		public static async Task DownloadFileByte(string file, string fileDestination)
		{
			string endpoint = Globals.URL + file + "?raw=true";

			using (WebResponse webResponse = await Task.Run(() => WebRequest.Create(endpoint).GetResponse()))
			{
				using (BinaryReader reader = new BinaryReader(webResponse.GetResponseStream()))
				{
					bool exists = File.Exists(fileDestination);
					if (exists)
						File.Delete(fileDestination);

					using (FileStream lxFS = new FileStream(fileDestination, FileMode.Create))
					{
						while (true)
						{
							byte[] lnByte = reader.ReadBytes(1024 * 1024); // 1 mb each package
							if (lnByte.Length == 0)
								break;
							lxFS.Write(lnByte, 0, lnByte.Length);
						}
					}
				}
			}
		}
	}
}
