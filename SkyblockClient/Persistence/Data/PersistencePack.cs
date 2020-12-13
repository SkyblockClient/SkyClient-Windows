using System;
using SkyblockClient.Options;

namespace SkyblockClient.Persistence.Data
{
	class PersistencePack : PersistenceData, IEquatable<PackOption>, IEquatable<PersistencePack>
	{
		public PersistencePack() { }
		public PersistencePack(Option option)
		{
			id = option.id;
			file = option.file;
		}

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
