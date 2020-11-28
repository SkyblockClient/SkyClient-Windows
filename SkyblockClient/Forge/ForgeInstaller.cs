using Newtonsoft.Json;
using SkyblockClient.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyblockClient.Forge
{
	class ForgeInstaller
	{

		public async static Task Work()
		{
			List<Task> tasks = new List<Task>();
			tasks.Add(Task.Run(() => Profile()));
			tasks.Add(Task.Run(() => Libraries()));
			await Task.WhenAll(tasks.ToArray());
		}

		private static async Task Profile()
		{
			string text = File.ReadAllText(Globals.launcherProfilesLocation);
			var launcherProfiles = JsonConvert.DeserializeObject<LauncherProfiles>(text);

			SkyClientJson skyClientJson = await SkyClientJson.Create();
			if (launcherProfiles.profiles.ContainsKey("SkyClient"))
			{
				Utils.Info("Profile Exists -> Updating");
				launcherProfiles.profiles["SkyClient"] = skyClientJson;
			}
			else
			{
				Utils.Info("SkyClient does not exist -> Creating");
				launcherProfiles.profiles.Add("SkyClient", skyClientJson);
			}
			string outp = JsonConvert.SerializeObject(launcherProfiles, Formatting.Indented);
			File.WriteAllText(Globals.launcherProfilesLocation, outp);
		}

		private static async Task Libraries()
		{
			string fileForge = @"forge-1.8.9-11.15.1.2318-1.8.9.jar";
			string libraryDirectory = Path.Combine(Globals.minecraftLibrariesLocation, "net", "minecraftforge", "forge", "1.8.9-11.15.1.2318-1.8.9");


			if (!File.Exists(Path.Combine(libraryDirectory, fileForge)))
			{
				if (!Directory.Exists(libraryDirectory))
				{
					Directory.CreateDirectory(libraryDirectory);
				}

				await Globals.DownloadFileByte("forge/" + fileForge, Path.Combine(Globals.tempFolderLocation, fileForge));

				File.Move(Path.Combine(Globals.tempFolderLocation, fileForge), Path.Combine(libraryDirectory, fileForge));
			}
			else
			{
				Utils.Debug(Path.Combine(libraryDirectory, fileForge) + " exists");
			}

			string fileJson = "1.8.9-forge1.8.9-11.15.1.2318-1.8.9.json";
			string versionDirectory = Path.Combine(Globals.minecraftVersionsLocation, "1.8.9-forge1.8.9-11.15.1.2318-1.8.9");

			if (!File.Exists(Path.Combine(versionDirectory, fileJson)))
			{
				if (!Directory.Exists(versionDirectory))
				{
					Directory.CreateDirectory(versionDirectory);
				}

				await Globals.DownloadFileByte("forge/" + fileJson, Path.Combine(Globals.tempFolderLocation, fileJson));

				File.Move(Path.Combine(Globals.tempFolderLocation, fileJson), Path.Combine(versionDirectory, fileJson));
			}
			else
			{
				Utils.Debug(Path.Combine(versionDirectory, fileJson) + " exists");
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
