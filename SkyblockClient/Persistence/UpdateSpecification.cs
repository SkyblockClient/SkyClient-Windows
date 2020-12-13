using System.Collections.Generic;
using Newtonsoft.Json;
using SkyblockClient.Options;
using SkyblockClient.Persistence.Data;

namespace SkyblockClient.Persistence
{
	class UpdateSpecification
	{
		public List<PersistenceMod> mods { get; set; }
		public List<PersistencePack> packs { get; set; }
		

		public void AddRangeSafe(List<ModOption> mods)
		{
			if (this.mods is null)
				this.mods = new List<PersistenceMod>();
			foreach (var item in mods)
			{
				PersistenceMod persistenceItem = new PersistenceMod(item);
				if (!this.mods.Contains(persistenceItem))
					this.mods.Add(persistenceItem);
			}
		}

		public void AddRangeSafe(List<PackOption> packs)
		{
			if (this.packs is null)
				this.packs = new List<PersistencePack>();
			foreach (var item in packs)
			{
				PersistencePack persistenceItem = new PersistencePack(item);
				if (!this.packs.Contains(persistenceItem))
					this.packs.Add(persistenceItem);
			}
		}
	}
}
