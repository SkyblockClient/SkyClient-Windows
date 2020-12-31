using Newtonsoft.Json;

namespace SkyblockClient.Persistence.Data
{
	public abstract class PersistenceData
	{
		public string id { get; set; }
		public string file { get; set; }

		[JsonIgnore]
		public bool exists;

		[JsonIgnore]
		public virtual bool UpdateAvailable { get; }

		[JsonIgnore]
		public virtual Options.Option Option { get; }
	}
}