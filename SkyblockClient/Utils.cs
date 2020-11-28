using System;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Drawing;

namespace SkyblockClient
{
	class Utils
	{
		private static TextWriter ErrorOut = Console.Error;
		private static TextWriter Out = Console.Out;
		private static TextReader In = Console.In;

		public static string exeLocation = Assembly.GetEntryAssembly().Location;

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

		public static string Base64Image(string path)
		{
			byte[] imageArray = File.ReadAllBytes(path);
			return Convert.ToBase64String(imageArray);
		}
	}
}
