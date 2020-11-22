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
		public string configFolder => Globals.skyblockConfigLocation + "itlt/";

		public override async Task Work()
		{
			helper.InitFolders("config", "config/itlt");

			string sourceIcon = await helper.DownloadFileByte("icon.png");
			helper.Move(sourceIcon, $"{configFolder}/icon.png");

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
}
