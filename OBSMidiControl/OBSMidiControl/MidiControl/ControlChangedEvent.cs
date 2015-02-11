using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSMidiControl.MidiControl
{
    public delegate void ControlChangedEventHandler(Devices.IDevice d, ControlChangedEventArgs e);

    public class ControlChangedEventArgs : EventArgs
    {
        public ControlChangedEventArgs(OBSControl control)
        {
            _control = control;
        }

        private OBSControl _control;

        public OBSControl Control
        {
            get { return _control; }
        }

    }
}
