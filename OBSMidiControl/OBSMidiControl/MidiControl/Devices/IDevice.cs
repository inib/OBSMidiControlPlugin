using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSMidiControl.MidiControl.Devices
{
    public interface IDevice
    {  
        string Name { get; }
        bool IsConnected { get; }
        //Dictionary<OBSControls, Midi.Control> CCMap();

        event ControlChangedEventHandler ControlChanged;        
        void SetControl(OBSControls control);
        void SetAll(Presets.Preset preset);
        void Dispose();
    }
}
