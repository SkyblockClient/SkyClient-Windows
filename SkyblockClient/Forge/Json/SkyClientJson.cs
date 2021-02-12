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

			SkyClientJson skyClientJson = new SkyClientJson();
			skyClientJson.gameDir = Globals.skyblockRootLocation;
			skyClientJson.javaArgs = Globals.Settings.javaArgs;
			skyClientJson.name = "SkyClient";
			skyClientJson.type = "custom";
			skyClientJson.lastVersionId = "1.8.9-forge1.8.9-11.15.1.2318-1.8.9";

			if (Globals.Settings.SkyClientJsonUseCloudImage)
			{
				const string IMAGE_NAME = "skyclient128apng.png";
				await Globals.DownloadFileByte($"images/{IMAGE_NAME}", System.IO.Path.Combine(Globals.tempFolderLocation, IMAGE_NAME));
				skyClientJson.icon = "data:image/png;base64," + Utils.Base64Image(System.IO.Path.Combine(Globals.tempFolderLocation, IMAGE_NAME));
			}
			else
			{
				skyClientJson.icon = Globals.Settings.imageBase64;
			}
			return skyClientJson;
		}
	}
}
