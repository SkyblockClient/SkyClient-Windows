using System;
using SkyblockClient.Options;

namespace SkyblockClient.Persistence.Data
{
	public class PersistencePack : PersistenceData, IEquatable<PackOption>, IEquatable<PersistencePack>
	{
		public PersistencePack() { }
		public PersistencePack(Option option)
		{
			id = option.id;
			file = option.file;
		}
		public PersistencePack(PackOption option)
		{
			id = option.id;
			file = option.file;
		}

		public override bool UpdateAvailable
		{
			get
			{
				if (Utils.OptionDataExists(this))
				{
					var data = Utils.GetOptionData(this);
					return data.file != this.file;
				}
				return false;
			}
		}

        public override Option Option => Utils.GetOptionData(this);

		public override bool Equals(object obj)
		{
			if (obj is PersistencePack)
				return (obj as PersistencePack).id == this.id;
			if (obj is PackOption)
				return (obj as PackOption).id == this.id;
			return false;
		}

		bool IEquatable<PackOption>.Equals(PackOption other) => this.id == other.id;
		bool IEquatable<PersistencePack>.Equals(PersistencePack other) => this.id == other.id;
	}
}
