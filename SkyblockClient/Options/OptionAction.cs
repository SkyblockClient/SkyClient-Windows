using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyblockClient.Options
{
    public class OptionAction
    {

        [DefaultValue("Invalid")]
        public string text { get; set; }

        [DefaultValue("https://github.com/nacrt/SkyblockClient-REPO/blob/main/files/guides/invalid.md")]
        public string link { get; set; }
        public override string ToString() => text;

        public void Act() => Utils.OpenLinkInBrowser(link);
    }
}
