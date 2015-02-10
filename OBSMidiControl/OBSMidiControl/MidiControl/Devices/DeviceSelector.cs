using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSMidiControl.MidiControl.Devices
{
    public static class DeviceSelector
    {
        //private MidiDevice _device;

        //private bool _deviceFound = false;

        //public MidiDevice GetDevice()
        //{
        //    if (_deviceFound)
        //    {
        //        return _device;
        //    }
        //    else
        //    {
        //        return null;
        //    }
            
        //}

        //public bool CheckDevice(Devices.KnownDevices device)
        //{
        //    if (true)
        //    {
        //        Midi.InputDevice input = Midi.InputDevice.InstalledDevices.First(a => a.Name == device.ToString());
        //        Midi.OutputDevice output = Midi.OutputDevice.InstalledDevices.First(a => a.Name == device.ToString());
        //        _deviceFound = true;
        //    }
        //    return _deviceFound;
        //}

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
