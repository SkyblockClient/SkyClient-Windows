using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyblockClient.Option
{
	public interface IOption
	{
		string id { get; set; }
		bool enabled { get; set; }
		string file { get; set; }
		string display { get; set; }
		string description { get; set; }

		void Create(string line);
		//  { throw new NotImplementedException("OptionBase(string line) is not Implemented"); }
	}
}
