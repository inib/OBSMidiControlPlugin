using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSMidiControl.MidiControl.Devices
{
    public static class DeviceSelector
    {
        public static IDevice GetDevice(Devices.KnownDevices device)
        {   
            if (CheckDevice(device) && device.ToString() == "nanoKONTROL2")
            {
                return new NanoKONTROL2();
            }
            else
            {
                return null;
            }
        }

        public static bool CheckDevice(Devices.KnownDevices device)
        {
            bool check = (Midi.InputDevice.InstalledDevices.Any(a => a.Name == device.ToString())) && (Midi.OutputDevice.InstalledDevices.Any(a => a.Name == device.ToString()));
            return check;
        }

        // Preparation future multidevice support

        //private Midi.InputDevice _inputDevices;

        //public Midi.InputDevice InputDevices
        //{
        //    get { return _inputDevices; }
        //}

        //private Midi.OutputDevice _outputDevices;

        //public List<Midi.OutputDevice> OutputDevices
        //{
        //    get { return _outputDevices; }
        //}


        //public DeviceSelector(KnownDevices device)
        //{       
        //    _inputDevices.Add(Midi.InputDevice.InstalledDevices.First(a => a.Name == device.ToString()));
        //    _outputDevices.Add(Midi.OutputDevice.InstalledDevices.First(a => a.Name == device.ToString()));

        //}

    }

    public class MidiDevice
    {
        public MidiDevice(Midi.InputDevice inputDevice, Midi.OutputDevice outputDevice, Devices.KnownDevices knownDevice)
        {
            _inputDevice = inputDevice;
            _outputDevice = outputDevice;
            _name = knownDevice;
        }

        private Midi.InputDevice _inputDevice;

        public Midi.InputDevice InputDevice
        {
            get { return _inputDevice; }
        }

        private Midi.OutputDevice _outputDevice;

        public Midi.OutputDevice OutputDevice
        {
            get { return _outputDevice; }
        }

        private Devices.KnownDevices _name;

        public Devices.KnownDevices Name
        {
            get { return _name;}
        }	
        
    }
}
