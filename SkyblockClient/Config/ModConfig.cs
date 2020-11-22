using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
