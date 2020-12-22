using System;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Windows;

namespace SkyblockClient
{
	class Utils
	{
		internal static bool copyEndpointToClipboardOnDebug = true;
		internal static bool updateProfileOnDebug = false;

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
						if (!AvailableModOptions.ContainsKey(item.id))
						{
							AvailableModOptions.Add(item.id, item);
						}
					}
				}
				return AvailableModOptions;
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

					foreach (var item in Globals.resourceOptions)
					{
						if (!AvailablePackOptions.ContainsKey(item.id))
						{
							AvailablePackOptions.Add(item.id, item);
						}
					}
				}
				return _availablePackOptions;
			}
		}
		private static Dictionary<string, Options.PackOption> _availablePackOptions;

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
			Clipboard.SetText(value);
		}
	}
}
