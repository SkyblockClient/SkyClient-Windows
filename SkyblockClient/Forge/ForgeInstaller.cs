using Newtonsoft.Json;
using SkyblockClient.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace SkyblockClient.Forge
{
	class ForgeInstaller
	{
		public async static Task Work()
		{
			bool valid = Utils.ValidateMinecraftDirectory(Globals.minecraftRootLocation);
			if (!valid)
			{
				string msg = $"\"{Globals.minecraftRootLocation}\" is not a valid minecraft directory.\nMake sure you run the minecraft launcher at least once.";
				//MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}

			try
			{
				const string JAVA_LINK = "https://www.java.com/en/download/manual.jsp";
				bool correctJavaVersion = false;

				var javaProcess = Process.Start(Utils.CreateProcessStartInfo("java.exe", "-version"));

				List<string> jLines = new List<string>();

				while (!javaProcess.StandardError.EndOfStream)
				{
					jLines.Add(javaProcess.StandardError.ReadLine());
				}

				if (jLines.Count == 3 && jLines[0].Split('"')[1].StartsWith("1.8."))
				{
					correctJavaVersion = true;
				}
				else
				{
					correctJavaVersion = false;
				}


				if (!correctJavaVersion)
				{
					Utils.Error("You are Using the wrong version of Java.");
					Utils.Error("Download the newest version here:");
					Utils.Error(JAVA_LINK);
					Utils.Error("Select \"Windows Offline (64-Bit)\"");
					Process.Start(Utils.CreateProcessStartInfo("cmd.exe", $"/c start {JAVA_LINK}"));
				}
			}
			catch (System.ComponentModel.Win32Exception e)
			{
				Utils.Error(e.Message, "Win32Exception in ForgeInstaller.Work");
				Utils.Log(e, "Win32Exception in ForgeInstaller.Work");
			}
			catch (Exception e)
			{
				Utils.Error(e.Message, "Unknown Exception in ForgeInstaller.Work - please publish the log file to the author");
				Utils.Log(e, "Win32Exception in ForgeInstaller.Work");
			}

			List<Task> tasks = new List<Task>();
			tasks.Add(Task.Run(() => Profile()));
			tasks.Add(Task.Run(() => Libraries()));
			await Task.WhenAll(tasks.ToArray());
		}

		private static async Task Profile()
		{
			SkyClientJson skyClientJson = null;

			try
			{
				string text = File.ReadAllText(Globals.launcherProfilesLocation);
				var launcherProfiles = JsonConvert.DeserializeObject<LauncherProfiles>(text);

				skyClientJson = await SkyClientJson.Create();

				if (launcherProfiles.profiles.ContainsKey("SkyClient"))
				{
					if (Globals.isDebugEnabled && !Globals.Settings.updateProfileOnDebug)
					{
						Utils.Error("Profile Exists -> Debug mode is enabled -> not updating");
					}
					else
					{
						Utils.Info("Profile Exists -> Updating");
						launcherProfiles.profiles["SkyClient"] = skyClientJson;
					}
				}
				else
				{
					Utils.Info("SkyClient does not exist -> Creating");
					launcherProfiles.profiles.Add("SkyClient", skyClientJson);
				}
				string outp = JsonConvert.SerializeObject(launcherProfiles, Formatting.Indented);
				File.WriteAllText(Globals.launcherProfilesLocation, outp);
			}
			catch (System.ComponentModel.Win32Exception win32Exception)
			{
				string errorDetails = string.Empty;
				string errorWhileErrorPart = "Part 0";

				try
				{
					bool launcherProfilesExists = File.Exists(Globals.launcherProfilesLocation);
					errorDetails += $"launcherProfilesExists:{launcherProfilesExists}|";
					errorWhileErrorPart = "Part 1";
					
					bool skyClientJsonCreated = !(skyClientJson is null);
					if (launcherProfilesExists)
					{
						errorDetails += " FAILED CREATING SkyClientJson";
					}
					errorWhileErrorPart = "Part 2";
				}
				catch (Exception e)
				{
					Utils.Error("An unkown Exception occured while getting Crash information");
					Utils.Log(e, "An unkown Exception occured while getting Crash information", "ForgeInstaller.Profile()", $"Error Part:'{errorWhileErrorPart}'" );
				}

				Utils.Debug();
				Utils.Log(win32Exception, $"errorDetails:{errorDetails}");
			}
			catch(Exception e)
			{
				Utils.Error("An unkown Exception occured while installing the SkyClient profile to your launcher");
				Utils.Log(e, "An unkown Exception occured while installing the SkyClient profile to launcher", "ForgeInstaller.Profile()");
			}


		}

		private static async Task Libraries()
		{
			string fileForge = @"forge-1.8.9-11.15.1.2318-1.8.9.jar";
			string libraryDirectory = Path.Combine(Globals.minecraftLibrariesLocation, "net", "minecraftforge", "forge", "1.8.9-11.15.1.2318-1.8.9");

			string nextCommand = string.Empty;

			try
			{
				nextCommand = "if (!File.Exists(Path.Combine(libraryDirectory, fileForge)))";
				if (!File.Exists(Path.Combine(libraryDirectory, fileForge)))
				{
					nextCommand = "if (!Directory.Exists(libraryDirectory))";
					if (!Directory.Exists(libraryDirectory))
					{
						nextCommand = "Directory.CreateDirectory(libraryDirectory);";
						Directory.CreateDirectory(libraryDirectory);
					}

					nextCommand = "await Globals.DownloadFileByte(\"forge/\" + fileForge, Path.Combine(Globals.tempFolderLocation, fileForge));";
					await Globals.DownloadFileByte("forge/" + fileForge, Path.Combine(Globals.tempFolderLocation, fileForge));

					nextCommand = "File.Move(Path.Combine(Globals.tempFolderLocation, fileForge), Path.Combine(libraryDirectory, fileForge));";
					File.Move(Path.Combine(Globals.tempFolderLocation, fileForge), Path.Combine(libraryDirectory, fileForge));
				}
				else
				{
					nextCommand = "Utils.Debug(Path.Combine(libraryDirectory, fileForge) + \" exists\");";
					Utils.Debug(Path.Combine(libraryDirectory, fileForge) + " exists");
				}
			}
			catch (System.ComponentModel.Win32Exception win32Exception)
			{
				const string CRASH_MESSAGE = "A Win32Exception occured while installing the Forge to your .minecraft";
				Utils.Error(CRASH_MESSAGE);
				Utils.Log(win32Exception, CRASH_MESSAGE, "ForgeInstaller.Libraries().Part:Forge", $"nextCommand:'{nextCommand}'");
			}
			catch (Exception e)
			{
				const string CRASH_MESSAGE = "An unkown Exception occured while installing the Forge to your .minecraft";
				Utils.Error(CRASH_MESSAGE);
				Utils.Log(e, CRASH_MESSAGE, "ForgeInstaller.Profile().Part:Forge", $"nextCommand:'{nextCommand}'");
			}

			string fileJson = "1.8.9-forge1.8.9-11.15.1.2318-1.8.9.json";
			string versionDirectory = Path.Combine(Globals.minecraftVersionsLocation, "1.8.9-forge1.8.9-11.15.1.2318-1.8.9");

			try
			{
				nextCommand = "if (!File.Exists(Path.Combine(versionDirectory, fileJson)))";
				if (!File.Exists(Path.Combine(versionDirectory, fileJson)))
				{
					nextCommand = "if (!Directory.Exists(versionDirectory))";
					if (!Directory.Exists(versionDirectory))
					{
						nextCommand = "Directory.CreateDirectory(versionDirectory);";
						Directory.CreateDirectory(versionDirectory);
					}

					nextCommand = "await Globals.DownloadFileByte(\"forge/\" + fileJson, Path.Combine(Globals.tempFolderLocation, fileJson));";
					await Globals.DownloadFileByte("forge/" + fileJson, Path.Combine(Globals.tempFolderLocation, fileJson));

					nextCommand = "File.Move(Path.Combine(Globals.tempFolderLocation, fileJson), Path.Combine(versionDirectory, fileJson));";
					File.Move(Path.Combine(Globals.tempFolderLocation, fileJson), Path.Combine(versionDirectory, fileJson));
				}
				else
				{
					nextCommand = "Utils.Debug(Path.Combine(versionDirectory, fileJson) + \" exists\");";
					Utils.Debug(Path.Combine(versionDirectory, fileJson) + " exists");
				}
			}
			catch (System.ComponentModel.Win32Exception win32Exception)
			{
				const string CRASH_MESSAGE = "A Win32Exception occured while installing the Forge to your .minecraft";
				Utils.Error(CRASH_MESSAGE);
				Utils.Log(win32Exception, CRASH_MESSAGE, "ForgeInstaller.Libraries().Part:Json", $"nextCommand:'{nextCommand}'");
			}
			catch (Exception e)
			{
				const string CRASH_MESSAGE = "An unkown Exception occured while installing the Forge to your .minecraft";

				Utils.Error(CRASH_MESSAGE);
				Utils.Log(e, CRASH_MESSAGE, "ForgeInstaller.Libraries().Part:Json", $"nextCommand:'{nextCommand}'");
			}
		}

		private class ForgeFile
		{
			public string full;
			public string directory;
			public string file;
		
			public bool isVersion;
			public bool isLibrary;

			public ForgeFile(string line)
			{
				var parts = line.Split(':');
				if (parts[0] == "libraries")
				{
					isLibrary = true;
				}
				else if (parts[0] == "versions")
				{
					isVersion = true;
				}
				full = parts[1];
				directory = Path.GetDirectoryName(full);
				file = Path.GetFileName(full);
			}

			public static List<ForgeFile> CreateFromFile(string file)
			{

				List<ForgeFile> forgeFiles = new List<ForgeFile>();
				string[] lines = file.Split('\n');
				foreach (var line in lines)
				{
					forgeFiles.Add(new ForgeFile(line));
				}
				return forgeFiles;
			}
		}
	}
}
