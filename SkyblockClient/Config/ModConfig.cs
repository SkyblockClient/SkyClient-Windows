using System;

namespace SkyblockClient.Config
{
	public class ModConfig
	{
		string id;
		string[] response;

		public string abc = "abc";

		public ModConfig(string responseString)
		{
			response = responseString.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
		}
	}
}
