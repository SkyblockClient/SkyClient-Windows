using System;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Windows;
using SkyblockClient.Persistence.Data;
using SkyblockClient.Options;
using System.Windows.Media.Imaging;

namespace SkyblockClient
{
    class Utils
	{
		private static TextWriter ErrorOut = Console.Error;
		private static TextWriter Out = Console.Out;

		public static string exeLocation = Assembly.GetEntryAssembly().Location;

		public static Dictionary<string, Options.ModOption> AvailableModOptions
		{
			get
			{
				if (_availableModOptions is null)
				{
					_availableModOptions = new Dictionary<string, Options.ModOption>();
					foreach (var item in Globals.modOptions)
					{
						if (!AvailableModOptions.ContainsKey(item.Id))
						{
							AvailableModOptions.Add(item.Id, item);
						}
					}
				}
				return _availableModOptions;
			}
		}
		private static Dictionary<string, Options.ModOption> _availableModOptions;

		public static Dictionary<string, Options.PackOption> AvailablePackOptions
		{
			get
			{
				if (_availablePackOptions is null)
				{
					_availablePackOptions = new Dictionary<string, Options.PackOption>();

					foreach (var item in Globals.packOptions)
					{
						if (!AvailablePackOptions.ContainsKey(item.Id))
						{
							AvailablePackOptions.Add(item.Id, item);
						}
					}
				}
				return _availablePackOptions;
			}
		}
		private static Dictionary<string, Options.PackOption> _availablePackOptions;
		public static bool OptionDataExists(PersistenceMod persistenceMod)
		{
			return Utils.AvailableModOptions?.ContainsKey(persistenceMod.id) ?? false;
		}

		public static bool OptionDataExists(PersistencePack persistencePack)
		{
			return Utils.AvailablePackOptions?.ContainsKey(persistencePack.id) ?? false;
		}

		public static ModOption GetOptionData(PersistenceMod persistenceMod)
		{
			return Utils.AvailableModOptions[persistenceMod.id];
		}

		public static PackOption GetOptionData(PersistencePack persistencePack)
		{
			return Utils.AvailablePackOptions[persistencePack.id];
		}

        public static string DownloadFileTempFolderLocation(Option option) => DownloadFileTempFolderLocation(option.File);

		public static string DownloadFileTempFolderLocation(string option)
		{
			var advancedFileName = RandomString() + "_" + option;
			return Path.Combine(Globals.tempFolderLocation, advancedFileName);
		}

		public static string RandomString(int length = 5) => RandomString(length, "0123456789");
		public static string RandomString(string charset) => RandomString(5, charset);

		public static string RandomString(int length, string charset)
		{
			var stringChars = new char[length];
			var random = new Random();

			for (int i = 0; i < stringChars.Length; i++)
			{
				stringChars[i] = charset[random.Next(charset.Length)];
			}
			return new String(stringChars);
		}

		private static string logFileName
		{
			get
			{
				DateTime dateTime = DateTime.Now;
				return $"{exeLocation}-{dateTime.ToString("yyyy-MM-dd")}.log";
			}
		}
		
		public static void Error(params string[] msgs)
		{
			Thread thread = new Thread(Error);
			thread.Start(msgs);
		}

		private static void Error(object obj)
		{
			string[] msgs = (string[])obj;
			Console.ForegroundColor = ConsoleColor.Red;
			foreach (var msg in msgs)
				ErrorOut.WriteLine(msg);
			Console.ResetColor();
		}

		public static void Debug(Exception exception)
		{
			if (Globals.isDebugEnabled)
			{
				Info(exception.Message, exception.StackTrace);
			}
		}
		public static void Debug(params string[] msgs)
		{
			if (Globals.isDebugEnabled)
			{
				Info(msgs);
			}
		} 

		public static void Info(params string[] msgs)
		{
			Thread thread = new Thread(Info);
			thread.Start(msgs);
		}

		private static void Info(object obj)
		{
			string[] msgs = (string[])obj;

			foreach (var msg in msgs)
				Out.WriteLine(msg);
		}

		public static Task InitLog()
		{
			// causes errors for some reason
			/* 
			bool exists = File.Exists(logFileName);
			if (exists)
			{
				try
				{
					File.Delete(logFileName);
					File.Create(logFileName);
				}
				catch (Exception e)
				{
					Log(e);
				}
			}
			else
			{
				File.Create(logFileName);
			}
			*/
			return Task.CompletedTask;
		}

		public static bool ValidateMinecraftDirectory(string path)
		{
			try
			{
				if (File.Exists(Path.Combine(path, "launcher_profiles.json")))
				{
					return true;
				}
			}
			catch (IOException e)
			{
				Utils.Log(e, "Error accessing" + Path.Combine(path, "launcher_profiles.json"));
			}
			return false;
		}

		public static async Task InitializeInstall(bool skipTempFolder = false)
		{
			if (skipTempFolder)
			{
				if (await Task.Run(() => Directory.Exists(Globals.tempFolderLocation)))
					await Task.Run(() => Directory.Delete(Globals.tempFolderLocation, true));
				await Task.Run(() => Directory.CreateDirectory(Globals.tempFolderLocation));
			}

			if (!await Task.Run(() => Directory.Exists(Globals.skyblockRootLocation)))
				await Task.Run(() => Directory.CreateDirectory(Globals.skyblockRootLocation));
		}

		public static void Log(Exception e)
		{
			File.AppendAllText(logFileName, e.Message);
			File.AppendAllText(logFileName, e.StackTrace);
		}

		public static void Log(params string[] messages)
		{
			foreach (var message in messages)
				File.AppendAllText(logFileName, message);
		}

		public static void Log(Exception e, params string[] messages)
		{
			foreach (var message in messages)
				File.AppendAllText(logFileName, message + " ");

			File.AppendAllText(logFileName, "\n");

			File.AppendAllText(logFileName, e.Message + "\n");
			File.AppendAllText(logFileName, e.StackTrace);

			File.AppendAllText(logFileName, "\n\n");
		}

		public static ProcessStartInfo CreateProcessStartInfo(string exe, string command) => new ProcessStartInfo
		{
			FileName = $"{exe}",
			Arguments = $"{command}",
			UseShellExecute = false,
			RedirectStandardOutput = true,
			RedirectStandardError = true
		};

		public static JsonSerializerSettings JsonSerializerSettings => new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore,
			Formatting = Formatting.Indented,
			DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
		};


		public static string Base64Image(string path)
		{
			byte[] imageArray = File.ReadAllBytes(path);
			return Convert.ToBase64String(imageArray);
		}

		public static async Task ExecuteAsyncronous(params Task[] tasks) => await Task.WhenAll(tasks);

		/// <summary>
		/// Sets clipboard to value.
		/// </summary>
		/// <param name="value">String to set the clipboard to.</param>
		public static void SetClipboard(string value)
		{
            try
			{
				Clipboard.SetText(value);
			}
            catch (Exception e)
            {
				Utils.Error("Could not set Clipboard text to " + value);
				Utils.Debug(e.Message);
            }
		}

		public static void OpenLinkInBrowser(string link)
        {
			Utils.Info($"Opening {link}");
			var info = Utils.CreateProcessStartInfo("cmd.exe", $"/c start {link}");
			Process process = new Process() { StartInfo = info };
			process.Start();
        }
		public static void LoadSettings()
        {
			string errorAt = "try";
			try
            {
				errorAt = "var response = Globals.DownloadFileString(\"settings.json\");";
				var response = Globals.DownloadFileString("settings.json");
				errorAt = "var settings = JsonConvert.DeserializeObject<GlobalSettings>(response, Utils.JsonSerializerSettings);";
				var settings = JsonConvert.DeserializeObject<GlobalSettings>(response, Utils.JsonSerializerSettings);
				errorAt = "Globals.Settings = settings;";
				Globals.Settings = settings;

            }
            catch (Exception e)
            {
				Utils.Error("Error loading Config from Repository");
				Utils.Log(e, "Error loading Config from Repository", "errorAt:" + errorAt);
			}
        }

		public static BitmapImage GetImageIcon(string icon)
        {
			BitmapImage bitmap = new BitmapImage();
			bitmap.BeginInit();
			bitmap.UriSource = new Uri(new InternalDownloadUrl($"icons/{icon}").Url, UriKind.Absolute);
			bitmap.EndInit();
			return bitmap;
		}

		public static void SetImage(System.Windows.Controls.Image img, string icon)
		{
			img.Source = GetImageIcon(icon);
		}

		public static void SetImage(System.Windows.Controls.Button img, string icon)
		{
			var image = new System.Windows.Controls.Image();
			image.Source = GetImageIcon(icon);
			img.Content = image;
		}

		public static bool IsPropSet(string value) => !(value == null || value == "" || value == "None" || value == "none");
		public static bool IsPropSet<T>(List<T> value) => !(value == null || value.Count == 0);
		public static bool IsPropSet<T>(T[] value) => !(value == null || value.Length == 0);
		public static bool IsPropSet(object value) => !(value is null);
	}
}
