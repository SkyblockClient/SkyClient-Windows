using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace SkyblockClient
{
	class Utils
	{

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
			Console.ForegroundColor = ConsoleColor.Red;
			foreach (var msg in msgs)
				Console.WriteLine(msg);
			Console.ResetColor();
		}

		public static void Info(params string[] msgs)
		{
			foreach (var msg in msgs)
				Console.WriteLine(msg);
		}

		public static void InitLog()
		{
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
				File.AppendAllText(logFileName, message);

			File.AppendAllText(logFileName, e.Message);
			File.AppendAllText(logFileName, e.StackTrace);

		}


	}
}
