using System;

namespace SkyblockClient.Config
{
	public class ModConfigWorkerAttribute : Attribute
	{
		public string id;
		public ModConfigWorkerAttribute(string id)
		{
			this.id = id;
		} 
	}
}
