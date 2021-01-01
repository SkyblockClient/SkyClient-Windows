using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SkyblockClient.Persistence.Data;

namespace SkyblockClient.Persistence
{
    public class UpdateMain
    {
        public static async Task Update()
        {
            Utils.Info("Starting update");
            await Utils.InitializeInstall();
            var jsonLocation = Path.Combine(Globals.skyblockRootLocation, Globals.PERSISTANCE_JSON_NAME);
            var update = new UpdateMain(jsonLocation);
            if (!update.IsSkyClientDirectory)
            {
                Globals.ShowInfo(Globals.skyblockRootLocation + " is not a SkyClient directoy!", "Error");
                return;
            }
            List<Task> tasks = new List<Task>();

            if (Directory.Exists(Globals.skyblockModsLocation))
            {
                tasks.Add(PersitFiles(update, Globals.skyblockModsLocation, update.PersitenceFile.mods, Globals.modOptions));
            }
            else
                Globals.ShowInfo("No mods folder found in " + Globals.skyblockRootLocation, "Error");

            if (Directory.Exists(Globals.skyblockResourceLocation))
            {
                tasks.Add(PersitFiles(update, Globals.skyblockResourceLocation, update.PersitenceFile.packs, Globals.packOptions));
                tasks.Add(PersistenceMain.UpdateMinecraftConfigForPacks(update.PersitenceFile.packs));
            }
            else
                Globals.ShowInfo("No resourcepacks folder found in " + Globals.skyblockRootLocation, "Error");
            
            await Task.WhenAll(tasks.ToArray());

            await PersistenceMain.PersistSpecificationsAsync(update.PersitenceFile);

            Utils.Info("Finised Updating");

            if (update.UnmanagedFiles.Count > 0)
            {
                var result = "Warning: there are unmanaged files:\n";
                foreach (var item in update.UnmanagedFiles)
                {
                    result += item + "\n";
                }
                Utils.Error(result);
            }
        }

        private static async Task PersitFiles<TData, TOption>(UpdateMain update, string location, List<TData> collection, List<TOption> options) where TData : PersistenceData where TOption : Options.Option
        {
            var localFiles = Directory.GetFiles(location).Select((s, i) => Path.GetFileName(s)).ToArray();
            
            var trackedFiles = new List<PersistenceData>();
            var untrackedFiles = new List<string>(localFiles);
            foreach (var item in collection)
            {
                item.exists = untrackedFiles.Contains(item.file);

                if (item.exists)
                {
                    untrackedFiles.Remove(item.file);
                    trackedFiles.Add(item);
                }
            }

            foreach (var item in options)
            {
                if (untrackedFiles.Count == 0)
                    break;
                if (untrackedFiles.Contains(item.file))
                {
                    bool contained = false;
                    foreach (var data in collection)
                    {
                        if (data.id == item.id)
                        {
                            data.file = item.file;
                            contained = true;
                            untrackedFiles.Remove(item.file);
                            trackedFiles.Add(data);
                            break;
                        }
                    }

                    if (!contained)
                    {
                        var data = PersistenceData.CreateData(item);
                        untrackedFiles.Remove(item.file);
                        trackedFiles.Add(data);
                        collection.Add(data as TData);
                    }
                }
            }

            update.UnmanagedFiles.AddRange(untrackedFiles);


            foreach (var item in trackedFiles)
            {
                string errorAt = string.Empty;
                try
                {
                    errorAt = "if (item.UpdateAvailable)";
                    if (item.UpdateAvailable)
                    {
                        Utils.Info("Updating " + item.file);
                        errorAt = "var fileLocation = Path.Combine(location, item.file);";
                        var fileLocation = Path.Combine(location, item.file);
                        errorAt = "File.Delete(fileLocation);";
                        File.Delete(fileLocation);
                        errorAt = "var option = item.Option;";
                        var option = item.Option;
                        errorAt = "var tempfileLocation = Utils.DownloadFileTempFolderLocation(option);";
                        var tempfileLocation = Utils.DownloadFileTempFolderLocation(option);
                        errorAt = "await Globals.DownloadFileByte(option.downloadUrl, tempfileLocation);";
                        await Globals.DownloadFileByte(option.downloadUrl, tempfileLocation);
                        errorAt = "File.Move(tempfileLocation, Path.Combine(location, option.file));";
                        File.Move(tempfileLocation, Path.Combine(location, option.file));
                        errorAt = "item.file = option.file;";
                        item.file = option.file;
                    }
                }
                catch (Exception e)
                {
                    Utils.Debug(e);
                    Utils.Debug("errorAt: " + errorAt);
                    Utils.Debug("item.file: " + item.file);
                    Utils.Log(e, "Error Updating File" + item.file, "errorAt: " + errorAt);
                    Utils.Error("Error Updating " + item.file);
                }
            }
        }

        public string jsonLocation;

        public List<string> UnmanagedFiles = new List<string>();
       
        public PersistenceFile PersitenceFile
        {
            get
            {
                if (_file is null)
                {
                    try
                    {
                        _file = PersistenceFile.ReadData(jsonLocation);
                    }
                    catch (Exception e)
                    {

                        var file = new PersistenceFile();
                        file.mods = new List<PersistenceMod>();
                        file.packs = new List<PersistencePack>();
                        _file = file;

                        Utils.Debug(e.Message);
                        Utils.Debug(e.StackTrace);
                        Utils.Error($"Error reading {Globals.PERSISTANCE_JSON_NAME}");
                        Utils.Log(e, $"Error reading {Globals.PERSISTANCE_JSON_NAME}");
                    }
                }
                return _file;
            }
            set => _file = value;
        }


        private PersistenceFile _file;

        public UpdateMain(string jsonLocation)
        {
            this.jsonLocation = jsonLocation;
        }

        public bool IsSkyClientDirectory
        {
            get
            {
                if (File.Exists(jsonLocation))
                {
                    try
                    {
                        PersitenceFile = PersistenceFile.ReadData(jsonLocation);
                        return true;
                    }
                    catch (Exception e)
                    {
                        Utils.Debug(e.Message);
                        Utils.Debug(e.StackTrace);
                        Utils.Error($"Error reading {Globals.PERSISTANCE_JSON_NAME}");
                        Utils.Log(e, $"Error reading {Globals.PERSISTANCE_JSON_NAME}");
                    }
                }
                return false;
            }
        }
    }
}
