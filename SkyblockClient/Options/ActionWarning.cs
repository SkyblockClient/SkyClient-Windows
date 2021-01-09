using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SkyblockClient.Options
{
    public class ActionWarning
    {

		public List<string> Lines { get; set; }

		[DefaultValue("Warning")]
		public string Title { get; set; }

        [DefaultValue("")]
        public string Image { get; set; }

        [JsonIgnore]
        public bool HasImage => Utils.IsPropSet(Image);
        [JsonIgnore]
        public bool HasLines => Utils.IsPropSet(Lines);
        
        [JsonIgnore]
        public MessageBoxImage MessageBoxImage
        {
            get
            {
                if (HasImage)
                {
                    // https://docs.microsoft.com/en-us/dotnet/api/system.windows.messageboximage?view=net-5.0
                    switch (Image.ToLower().Trim())
                    {
                        case "warning": return MessageBoxImage.Warning;
                        case "none": return MessageBoxImage.None;
                        case "hand": return MessageBoxImage.Hand;
                        case "question": return MessageBoxImage.Question;
                        case "exclamation": return MessageBoxImage.Exclamation;
                        case "asterisk": return MessageBoxImage.Asterisk;
                        case "stop": return MessageBoxImage.Stop;
                        case "error": return MessageBoxImage.Error;
                        case "information": return MessageBoxImage.Information;
                        default: return MessageBoxImage.Warning;
                    }
                }
                return MessageBoxImage.Warning;
            }
        }

        [JsonIgnore]
        public string Message
        {
            get
            {
                if (HasLines)
                {
                    var message = string.Empty;
                    foreach (var line in Lines)
                    {
                        message += line + "\n";
                    }
                    return message.Substring(0, message.Length - 1);
                }
                return "INVALID";
            }
        }
    }
}
