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
			helper.InitFolders("config", "config/CustomMainMenu");

			string[] custommainmenu = new string[] { "mainmenu.json", "play.json" };
			foreach (var file in custommainmenu)
			{
				string source = await helper.DownloadFileByte(file);
				helper.Move(source, Path.Combine(Globals.skyblockConfigLocation, "CustomMainMenu", file));
			}

			/*
			string[] resources = new string[] { "background.png", "skyclient.png", "bg_menu.png" };
			foreach (var file in resources)
			{
				string source = await helper.DownloadFileByte($"cmm{file}");
				helper.Move(source, Path.Combine(Globals.gameDirectory, "resources", "mainmenu", file));
			}
			*/
		}
	}
}
