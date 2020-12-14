using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SkyblockClient.Options;
using SkyblockClient.Persistence.Data;

namespace SkyblockClient.Persistence
{
	class PersistenceMain
	{

		public static async Task InstallPacks(List<PackOption> packs)
		{
			await UpdateMinecraftConfigForPacks(new PersistencePacksList(packs).Packs);
			await InstallUniversal(Globals.skyblockResourceLocation, packs.ToArray(), "resourcepacks", false);
		}

		public static async Task InstallMods(List<ModOption> mods)
		{
			await InstallUniversal(Globals.skyblockModsLocation, mods.ToArray(), "mods", true);
		}

		public static async Task InstallUniversal(string location, Option[] enabledOptions, string foldername, bool isMods, bool setAutoconfig = true)
		{
			List<Option> firstHalf = enabledOptions.Take((enabledOptions.Count() + 1) / 2).ToList();
			List<Option> secondHalf = enabledOptions.Skip((enabledOptions.Count() + 1) / 2).ToList();

			List<Task> tasks = new List<Task>();
			tasks.Add(DownloadIndividualMods(firstHalf));
			tasks.Add(DownloadIndividualMods(secondHalf));
			await Task.WhenAll(tasks.ToArray());

			try
			{
				bool locationExists = Directory.Exists(location);
				if (locationExists)
					Utils.Info($"skyblock {foldername} folder exists");
				else
					Utils.Info($"skyblock {foldername} folder does not exist");

				if (locationExists)
				{
					DirectoryInfo di = new DirectoryInfo(location);

					foreach (FileInfo file in di.GetFiles())
					{
						file.Delete();
					}

				}
				Directory.CreateDirectory(location);

				foreach (var file in enabledOptions)
				{
					Utils.Info("Moving " + file.file);
					try
					{
						File.Move(Path.Combine(Globals.tempFolderLocation, file.file), Path.Combine(location, file.file));
						Utils.Info("Finished Moving " + file.file);
					}
					catch (Exception e)
					{
						Utils.Error("Failed Moving " + file.display);
						Utils.Log(e, "failed moving " + file.display);
					}
				}

				if (isMods && setAutoconfig)
				{
					foreach (ModOption mod in enabledOptions)
					{
						List<Task> tasks1 = new List<Task>();
						try
						{
							tasks1.Add(Config.ConfigUtils.CreateModConfigWorker(mod));
						}
						catch (Exception e)
						{
							Utils.Debug(e.Message);
							Utils.Error("An Unknown error occured, please submit the log file");
							Utils.Log(e, "unkown error in Installer():if(isMods)");
						}
						await Task.WhenAll(tasks1.ToArray());
					}
				}
			}
			catch (Exception e)
			{
				Utils.Error("An Unknown error occured, please submit the log file");
				Utils.Log(e, "unkown error in Installer()");
			}
		}

		private static async Task DownloadIndividualMods(List<Options.Option> modOptions)
		{
			foreach (var mod in modOptions)
			{
				try
				{
					Utils.Info("Downloading " + mod.display);
					await Globals.DownloadFileByte(mod.downloadUrl, Globals.tempFolderLocation + mod.file);
					Utils.Info("Finished Downloading " + mod.display);
				}
				catch (WebException webE)
				{
					var msg = "Error while Downloading " + mod.display;
					Utils.Error(msg);
					Utils.Log(webE, "msg:" + msg, "webE.Status:" + webE.Status, "webE.Data:" + webE.Data.ToString());
				}
				catch (Exception e)
				{
					var msg = "Error while Downloading " + mod.display;
					Utils.Error(msg);
					Utils.Log(e, msg);
				}
			}
		}


		public static async Task UpdateMinecraftConfigForPacks(List<PersistencePack> packs)
		{
			try
			{
				List<string> lines;
				if (File.Exists(Globals.skyblockOptionsLocation))
				{
					lines = new List<string>(File.ReadAllLines(Globals.skyblockOptionsLocation));
				}
				else
				{
					lines = new List<string>();
				}

				string packLine = "resourcePacks:[";
				foreach (var pack in packs)
				{
					packLine += $"\"{pack.file.Replace(@"\", @"\\").Replace("\"", "\\\"")}\",";
				}
				packLine = packLine.Remove(packLine.Length - 1);
				packLine += "]";

				bool foundPackLine = false;
				int indexPackLine = -1;
				List<int> duplicateIndexes = new List<int>();

				for (int i = 0; i < lines.Count; i++)
				{
					if (lines[i].StartsWith("resourcePacks:"))
					{
						if (!foundPackLine)
						{
							foundPackLine = true;
							indexPackLine = i;
						}
						else
						{
							duplicateIndexes.Add(i);
						}
					}
				}

				for (int i = duplicateIndexes.Count - 1; i >= 0; i--)
				{
					var duplicateIndex = duplicateIndexes[i];
					lines.RemoveAt(duplicateIndex);
				}

				if (foundPackLine)
				{
					lines[indexPackLine] = packLine;
				}
				else
				{
					lines.Add(packLine);
				}

				await Task.Run(() => File.WriteAllLines(Globals.skyblockOptionsLocation, lines));

			}
			catch (IOException ex)
			{
				Utils.Error("Failed Reading or Writing options.txt -> Skipping");
				Utils.Log(ex, "Failed Reading or Writing options.txt");
			}
			catch (Exception ex)
			{
				Utils.Error("An unexpected error happend while updating mods.txt");
				Utils.Log(ex, "An unexpected error happend while updating mods.txt");
			}
		}

		public static async Task<UpdateSpecification> CreateSpecificationsAsync(bool readExistingSpecifications)
		{
			bool persistanceLocationExists = await Task.Run(() => File.Exists(Globals.skyblockPersistenceLocation));
			UpdateSpecification specification = null;
			if (persistanceLocationExists)
			{
				if (readExistingSpecifications)
				{
					try
					{
						var specificationText = await Task.Run(() => File.ReadAllText(Globals.skyblockPersistenceLocation));
						specification = await Task.Run(() => JsonConvert.DeserializeObject<UpdateSpecification>(specificationText));
					}
					catch (JsonReaderException e)
					{
						Utils.Error(e.Message, e.StackTrace);
					}
					catch (Exception e)
					{
						Utils.Error(e.Message, e.StackTrace);
					}
					catch
					{
						Utils.Error("WE DON'T KNOW WHAT WENT WRONG HERE BUT SOMETHING IS SERIOUSLY MESSED UP");
					}
				}


				if (specification is null)
					specification = new UpdateSpecification();

			}
			else
			{
				specification = new UpdateSpecification();
			}
			specification.AddRangeSafe(Globals.neededMods);
			specification.AddRangeSafe(Globals.neededPacks);
			return specification;
		}

		public static async Task PersistSpecificationsAsync(UpdateSpecification specifications)
		{
			try
			{
				await Utils.InitializeInstall(true);
				var jsonConvert = JsonConvert.SerializeObject(specifications, Formatting.Indented);

				foreach (var item in specifications.mods)
					Utils.Debug(item.file);

				foreach (var item in specifications.packs)
					Utils.Debug(item.file);

				Utils.Debug(jsonConvert);
				await Task.Run(() => File.WriteAllText(Globals.skyblockPersistenceLocation, jsonConvert));
			}
			catch (Exception e)
			{
				string message = $"Failed creating or writing {Globals.PERSISTANCE_JSON_NAME}";
				Utils.Error(message);
				Utils.Log(e, "PersistSpecificationsAsync(UpdateSpecification specifications)", message);
			}
		}

		public static async Task Update()
		{
			var specs = await CreateSpecificationsAsync(false);
			await Update(specs);
		}

		public static async Task Update(UpdateSpecification specification)
		{
			await PersistSpecificationsAsync(specification);
		}
	}
}
