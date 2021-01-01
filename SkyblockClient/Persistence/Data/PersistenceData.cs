using Newtonsoft.Json;
using SkyblockClient.Options;

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
		public virtual Option Option { get; }
        public static PersistenceMod CreateData(ModOption option) => new PersistenceMod(option);
        public static PersistencePack CreateData(PackOption option) => new PersistencePack(option);
		public static PersistenceData CreateData(Option option)
        {
            if (option is ModOption)
				return new PersistenceMod(option as ModOption);
			else if (option is PackOption)
				return new PersistencePack(option as PackOption);
			else
				throw new System.NotImplementedException("Type of option is neither ModOption nor PackOption");
		}
	}
}