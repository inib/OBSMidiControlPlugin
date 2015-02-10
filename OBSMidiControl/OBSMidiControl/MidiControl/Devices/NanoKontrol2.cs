using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midi;

namespace OBSMidiControl.MidiControl.Devices
{
    public class NanoKONTROL2 : IDevice
    {
        Midi.InputDevice iDev;
        Midi.OutputDevice oDev;
        bool _deviceFound = false;
        Midi.Channel _chan = Midi.Channel.Channel1;

        public NanoKONTROL2()
        {
            iDev = Midi.InputDevice.InstalledDevices.First(a => a.Name == "nanoKONTROL2");
            oDev = Midi.OutputDevice.InstalledDevices.First(a => a.Name == "nanoKONTROL2");

            if ((iDev != null)&&(oDev != null))
            {
                _deviceFound = true;
            }

            iDev.Open();
            oDev.Open();
            oDev.SendControlChange(_chan,Control.Rec,127);            
        }

        public string Name
        {
            get { return iDev.Name; }
        }

        public bool IsConnected
        {
            get
            {
                bool state = (iDev.IsOpen && oDev.IsOpen);
                return state;
            }
        }

        public event ControlChangedEventHandler ControlChanged;

        public void SetControl(OBSControls control)
        {
            throw new NotImplementedException();
        }

        public void SetAll(Presets.Preset preset)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (_deviceFound)
            {
                if (iDev.IsOpen)
                {
                    if (iDev.IsReceiving)
                    {
                        iDev.StopReceiving();
                    }
                    iDev.Close();
                    iDev = null;
                }   
             
                if (oDev.IsOpen)
                {
                    oDev.SendControlChange(_chan, Control.Rec, 0);
                    oDev.Close();
                    oDev = null;
                }
            }
        }
    }
}
