using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSMidiControl.MidiControl
{
    public delegate void ControlChangedEventHandler(ControlChangedEventArgs e);

    interface IDevice
    {  
        string Name { get; }
        bool IsConnected { get; }

        event ControlChangedEventHandler ControlChanged;
        void SetRawControl(Midi.Control control, int value);
        void SetControl(OBSControl control, )
        void SetAll(Presets.Preset preset);
        void Dispose();
    }
}
