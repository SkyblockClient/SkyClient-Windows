using System;

namespace SkyblockClient.Options.Events
{
    public class GuideRequestEventArgs : EventArgs
    {
        public readonly CheckBoxMod CheckBoxMod;
        public readonly OptionAction OptionAction;

        public GuideRequestEventArgs(CheckBoxMod checkBoxMod, OptionAction optionAction)
        {
            CheckBoxMod = checkBoxMod;
            OptionAction = optionAction;
        }
    }
}
