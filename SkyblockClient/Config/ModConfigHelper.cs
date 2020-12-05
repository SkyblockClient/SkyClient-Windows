using SkyblockClient.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyblockClient.Config
{
	public class ModConfigHelper
	{
		ModOption modOption;

		private ModConfig _config;

		public ModConfigHelper(ModOption modOption)
		{
			this.modOption = modOption;
		}

		public async Task<string> DownloadFileByte(string file)
		{
			string path = $"config/{file}";

			string localPath = Globals.tempFolderLocation + file;
			await Globals.DownloadFileByte(path, localPath);
			return localPath;
		}

		public async Task<string> DownloadFileString(string file)
		{
			string path = $"config/{file}";
			return await Globals.DownloadFileString(path);
		}

		public async Task<ModConfig> GetModConfig()
		{
			if (_config is null)
			{
				string responseString = await Globals.DownloadFileString($"config/{modOption.id}.config");
				_config = new ModConfig(modOption.id);

			}
			return _config;
		}
		public void InitFolders(params string[] locations)
		{
			try
			{
				foreach (var location in locations)
				{
					string path = Path.Combine(Globals.skyblockRootLocation, location);
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
				}
			}
			catch (Exception e)
			{
				Utils.Log(e, "Error Creating or Deleting folders");
			}
		}

		public void Move(string source, string destination)
		{
			if (File.Exists(destination))
				File.Delete(destination);
			File.Move(source, destination);
		}
	}
}
