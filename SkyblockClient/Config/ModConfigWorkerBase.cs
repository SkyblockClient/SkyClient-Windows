using SkyblockClient.Options;
using System.Threading.Tasks;

namespace SkyblockClient.Config
{
	public abstract class ModConfigWorkerBase
	{
		public string id { get; set; }
		public ModOption mod { get; set; }
		public ModConfigHelper helper { get; set; }

		public virtual async Task Work() { throw new System.NotImplementedException("ModConfigWorkerBase.Work is not implemented"); }
	}
}
