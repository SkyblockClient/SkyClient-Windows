using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkyblockClient.Option;

namespace SkyblockClient.Config
{
	[ModConfigWorker("itlt")]
	class ModConfigWorkerITLT : ModConfigWorkerBase
	{
		public string configFolder => Path.Combine(Globals.skyblockConfigLocation, "itlt");

		public override async Task Work()
		{
			helper.InitFolders("config", "config/itlt");

			string sourceIcon = await helper.DownloadFileByte("icon.png");
			helper.Move(sourceIcon, Path.Combine(configFolder,"icon.png"));

			string sourceCfg = await helper.DownloadFileByte("itlt.cfg");
			helper.Move(sourceCfg, Path.Combine(Globals.skyblockConfigLocation, "itlt.cfg"));
		}
	}

	[ModConfigWorker("hytilities")]
	class ModConfigWorkerHytilities : ModConfigWorkerBase
	{
		public override async Task Work()
		{
			helper.InitFolders("config");

			string sourceCfg = await helper.DownloadFileByte("hytilities.toml");
			helper.Move(sourceCfg, Path.Combine(Globals.skyblockConfigLocation, "hytilities.toml"));
		}
	}

	[ModConfigWorker("cmm")]
	class ModConfigWorkerCmm : ModConfigWorkerBase
	{
		public override async Task Work()
		{
			helper.InitFolders("resources", "resources/mainmenu", "config", "config/CustomMainMenu");

			string source = await helper.DownloadFileByte("mainmenu.json");
			helper.Move(source, Path.Combine(Globals.skyblockConfigLocation, "CustomMainMenu/mainmenu.json"));

			string[] files = new string[] { "background.png", "skyclient.png", "bg_menu.png" };
			foreach (var file in files)
			{
				source = await helper.DownloadFileByte(file);
				helper.Move(source, Path.Combine(Globals.gameDirectory, "resources", "mainmenu", file));
			}
		}
	}
}
