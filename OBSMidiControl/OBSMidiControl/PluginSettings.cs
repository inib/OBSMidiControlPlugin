using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLROBS;

namespace OBSMidiControl
{
    class PluginSettings : AbstractWPFSettingsPane
    {
        public PluginSettings()
        {
            Category = "Midi Control";
        }

        public override void ApplySettings()
        {
            //throw new NotImplementedException();
        }

        public override void CancelSettings()
        {
            //throw new NotImplementedException();
        }

        public override System.Windows.UIElement CreateUIElement()
        {
            return new PluginSettingsWindow();
        }

        public override bool HasDefaults()
        {
            return false;
        }

        public override void SetDefaults()
        {
            //throw new NotImplementedException();
        }

    }
}
