using System;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Reflection;

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
				string result = "";
				result += exeLocation;
				DateTime dateTime = DateTime.Now;
				result += dateTime.ToString("-yyyy-MM-dd");
				result += ".log";
				return result;
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
	}
}
