using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyblockClient
{
	class ModOptionStructure
	{
		public string Display { get; }
		public string Name { get; }

		public bool Enabled { get; }

		public ModOptionStructure(string name = "",string display = "", bool enabled = true)
		{
			Name = name;
			Enabled = enabled;
			Display = display;
		}

	}
}
