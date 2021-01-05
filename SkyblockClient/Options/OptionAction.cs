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
        [DefaultValue("invalid.png")]
        public string Icon
        {
            set => _icon = value;
            get
            {
                if (Utils.IsPropSet(_icon) && _icon != "invalid.png")
                    return _icon;
                if (Utils.IsPropSet(Text))
                {
                    switch (Text)
                    {
                        case "Guide": return "guide.png";
                        case "Forum": return "forum.png";
                        case "Github": return "github.png";
                        case "Curseforge": return "curseforge.png";
                    }   
                }
                return "invalid.png";
            }
        }
        private string _icon;
        [DefaultValue("Invalid")]
        public string Text { get; set; }

        [DefaultValue("https://github.com/nacrt/SkyblockClient-REPO/blob/main/files/guides/invalid.md")]
        public string Link { get; set; }
        public override string ToString() => Text;

        public void Act() => Utils.OpenLinkInBrowser(Link);

        [JsonIgnore]
        public OptionActionItem Item
        {
            get
            {
                var item = new OptionActionItem();

                item.label.Content = Text;
                Utils.SetImage(item.image, Icon);

                return item;
            }
        }
    }
}
