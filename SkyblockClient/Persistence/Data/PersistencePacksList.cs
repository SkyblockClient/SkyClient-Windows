using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyblockClient.Persistence.Data
{
	class PersistencePacksList
	{
		private List<PersistencePack> _packs;
		public List<PersistencePack> Packs
		{
			get
			{
				if (_packs is null)
				{
					_packs = new List<PersistencePack>();
				}
				return _packs;
			}
		}

		public PersistencePacksList() { }

		public PersistencePacksList(List<Options.PackOption> packs)
		{
			foreach (var pack in packs)
			{
				Packs.Add(new PersistencePack(pack));
			}
		}

		public static explicit operator List<PersistencePack>(PersistencePacksList packsList) => packsList.Packs;
	}
}
