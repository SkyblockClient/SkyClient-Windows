using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyblockClient
{
    public class LocalSettings
    {
        const string LOCALE_DEFAULT = "EN";

        [DefaultValue(LOCALE_DEFAULT)]
        public string locale { get; set; } = LOCALE_DEFAULT;

    }
}
