using System;
using SkyblockClient.Options;

namespace SkyblockClient.Persistence.Data
{
	public class PersistenceMod : PersistenceData, IEquatable<ModOption>, IEquatable<PersistenceMod>
	{
		public PersistenceMod() { }
		public PersistenceMod(Option option)
		{
			id = option.Id;
			file = option.File;
		}
		public PersistenceMod(ModOption option)
		{
			id = option.Id;
			file = option.File;
		}

        public override bool UpdateAvailable
        {
			get
            {
                if (Utils.OptionDataExists(this))
                {
					var data = Utils.GetOptionData(this);
					return data.File != this.file;
                }
				return false;
            }
        }
		public override Option Option => Utils.GetOptionData(this);

		public override bool Equals(object obj)
		{
			if (obj is PersistenceMod)
				return (obj as PersistenceMod).id == this.id;
			if (obj is ModOption)
				return (obj as ModOption).Id == this.id;
			return false;
		}

		bool IEquatable<ModOption>.Equals(ModOption other) => this.id == other.Id;
		bool IEquatable<PersistenceMod>.Equals(PersistenceMod other) => this.id == other.id;
	}
}
