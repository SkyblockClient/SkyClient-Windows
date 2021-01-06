using Newtonsoft.Json;
using System.ComponentModel;
using System.Windows.Controls;

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
        public Button Item
        {
            get
            {
                var item = new OptionActionItem();

                item.label.Content = Text;
                Utils.SetImage(item.image, Icon);
                var button = new Button();
                button.Content = item;
                button.Click += btnOpenLink_Click;
                button.Tag = this;
                return button;
            }
        }

        private void btnOpenLink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Utils.OpenLinkInBrowser(((sender as Button).Tag as OptionAction).Link);
        }
    }
}
