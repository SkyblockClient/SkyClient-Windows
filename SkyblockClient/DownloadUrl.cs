using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyblockClient
{
	public interface IDownloadUrl
	{
		string Url { get; }
	}

	public class InternalDownloadUrl : IDownloadUrl
	{
		public string Url => Globals.URL + Resource + "?raw=true";

		private string Resource;

		public InternalDownloadUrl(string resource)
		{
			this.Resource = resource;
		}
	}

	public class RemoteDownloadUrl : IDownloadUrl
	{
		public string Url => Resource;

		private string Resource;

		public RemoteDownloadUrl(string resource)
		{
			this.Resource = resource;
		}
	}
}
