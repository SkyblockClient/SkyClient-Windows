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
        public string Text;
        public TextMouseEventArgs(string text, MouseDevice mouse, int timestamp) : base(mouse, timestamp)
        {
            this.Text = text;
        }
        public TextMouseEventArgs(string text, MouseEventArgs e) : base(e.MouseDevice, e.Timestamp)
        {
            this.Text = text;
        }
    }
}
