using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkyblockClient.Options.Events
{
    public class TextMouseEventArgs : MouseEventArgs
    {
        public OptionPreview OptionPreview;

        public TextMouseEventArgs(Option option, string text, MouseDevice mouse, int timestamp) : base(mouse, timestamp)
        {
            this.OptionPreview = new OptionPreview(option, text);
        }
        public TextMouseEventArgs(Option option, string text, MouseEventArgs e) : base(e.MouseDevice, e.Timestamp)
		{
			this.OptionPreview = new OptionPreview(option, text);
		}
    }
}
