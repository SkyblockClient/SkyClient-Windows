using System;
using System.Threading.Tasks;

namespace SkyblockClient.Json
{
	public class SkyClientJson
	{
		public string created { get; set; }
		public string gameDir { get; set; }
		public string icon { get; set; }
		public string javaArgs { get; set; }
		public string lastUsed { get; set; }
		public string lastVersionId { get; set; }
		public string name { get; set; }
		public string type { get; set; }

		public static async Task<SkyClientJson> Create()
		{
			const string IMAGE_NAME = "SkyblockClient128.png";
			await Globals.DownloadFileByte($"images/{IMAGE_NAME}", System.IO.Path.Combine(Globals.tempFolderLocation, IMAGE_NAME));

			SkyClientJson skyClientJson = new SkyClientJson();
			skyClientJson.gameDir = Globals.skyblockRootLocation;
			skyClientJson.javaArgs = JAVA_ARGS;
			skyClientJson.name = "SkyClient";
			skyClientJson.type = "custom";
			skyClientJson.lastVersionId = "1.8.9-forge1.8.9-11.15.1.2318-1.8.9";
			skyClientJson.icon = "data:image/png;base64," + Utils.Base64Image(System.IO.Path.Combine(Globals.tempFolderLocation, IMAGE_NAME));

			return skyClientJson;
		}

		public const string JAVA_ARGS = "-Xmx4G -XX:+UnlockExperimentalVMOptions -XX:+UseG1GC -XX:G1NewSizePercent=20 -XX:G1ReservePercent=20 -XX:MaxGCPauseMillis=50 -XX:G1HeapRegionSize=32M";
	}
}
