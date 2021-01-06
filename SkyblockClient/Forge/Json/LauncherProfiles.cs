using System.Collections.Generic;

namespace SkyblockClient.Json
{
	public class LauncherProfiles
	{
		public object authenticationDatabase { get; set; }
		public object clientToken { get; set; }
		public object launcherVersion { get; set; }
		public Dictionary<string, object> profiles { get; set; }
		public object selectedUser { get; set; }
		public object settings { get; set; }
	}
}
