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
            iDev.ControlChange += new InputDevice.ControlChangeHandler(Receive);
            iDev.StartReceiving(null);
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

        public void SetControl(OBSControl control)
        {
            var list = CCMapperOut(control);
            sendCCList(list);
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

        public void SetControl(OBSControls control)
        {
            throw new NotImplementedException();
        }

        private void sendCCList(List<MidiMsg> list)
        {
            if (oDev.IsOpen)
            {
                foreach (var item in list)
                {
                    oDev.SendControlChange(item.Channel, item.Control, item.Value);
                }
            }
        }

        public void Receive(Midi.ControlChangeMessage msg)
        {
            var _msg = CCMapperIn(new MidiMsg(_chan, msg.Control, msg.Value));
            ControlChanged(new ControlChangedEventArgs(_msg));
        }

        public OBSControl CCMapperIn(MidiMsg msg)
        {
            if ((int)msg.Control <= 7)
            {
                float value = ((((int)msg.Control+1)*10)+((float)msg.Value/127.0f)); 
                return (new OBSControl(OBSControls.ChangeVolumeDesktop, value, "nanoKONTROL2"));
            }
            else if ((int)msg.Control <= 23)
            {
                float value = ((((int)msg.Control-15) * 10) + ((float)msg.Value / 127.0f));
                return (new OBSControl(OBSControls.ChangeVolumeMic, value, "nanoKONTROL2"));
            }
            else if ((int)msg.Control <= 39)
            {
                float value = ((((int)msg.Control-31) * 10) + ((float)msg.Value / 127.0f));
                return (new OBSControl(OBSControls.MuteDesktop, value, "nanoKONTROL2"));
            }
            else if ((48 <= (int)msg.Control) && ((int)msg.Control) <= 55)
            {
                float value = ((((int)msg.Control-47) * 10) + ((float)msg.Value / 127.0f));
                return (new OBSControl(OBSControls.MuteMic, value, "nanoKONTROL2"));
            }
            else if ((64 <= (int)msg.Control) && ((int)msg.Control) <= 71)
            {
                float value = ((((int)msg.Control-63) * 10) + ((float)msg.Value / 127.0f));
                return (new OBSControl(OBSControls.ChangeScene, value, "nanoKONTROL2"));
            }
            else {
                return null;
            }
        }

        private List<MidiMsg> CCMapperOut(OBSControl control)
        {
            var list = new List<MidiMsg>();
            if (control.Control == OBSControls.ChangeScene)
            {
                int scene = ((int)control.Value / 10) + 64;
                if (((int)control.Value % 10) == 0)
                {
                    list.Add(new MidiMsg(_chan, (Control)scene, 0));                    
                }
                else
                {
                    list.Add(new MidiMsg(_chan, (Control)scene, 127));                    
                }
            }
            else if (control.Control == OBSControls.MuteDesktop)
            {
                int chan = ((int)control.Value / 10) + 48;                
                if (((int)control.Value % 10) == 0)
                {
                    list.Add(new MidiMsg(_chan, (Control)chan, 0));
                    list.Add(new MidiMsg(_chan, Control.Mute8, 0));
                }
                else
                {
                    list.Add(new MidiMsg(_chan, (Control)chan, 127));
                    list.Add(new MidiMsg(_chan, Control.Mute8, 127));
                }
            }
            else if (control.Control == OBSControls.MuteMic)
            {
                int chan = ((int)control.Value / 10) + 32;
                if (((int)control.Value % 10) == 0)
                {
                    list.Add(new MidiMsg(_chan, (Control)chan, 0));
                    list.Add(new MidiMsg(_chan, Control.Mute7, 0));
                }
                else
                {
                    list.Add(new MidiMsg(_chan, (Control)chan, 127));
                    list.Add(new MidiMsg(_chan, Control.Mute7, 127));
                }
            }
            return list;
        }



        public int PresetsAvailable
        {
            get { return 5; }
        }

        public int ScenesAivailable
        {
            get { return 6; }
        }

        public bool HasMaster
        {
            get { return true; }
        }

        public string PresetXML
        {
            get { return "nanoKONTROL.xml"; }
        }
    }

}
