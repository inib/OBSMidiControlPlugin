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
        int PresetsAvailable { get; }
        int ScenesAvailable { get; }
        bool HasMaster { get; }
        string PresetXML { get; }

        event ControlChangedEventHandler ControlChanged;        
        void SetControl(OBSControl control);
        void SetAll(Presets.Preset preset);
        void Dispose();
    }
}
