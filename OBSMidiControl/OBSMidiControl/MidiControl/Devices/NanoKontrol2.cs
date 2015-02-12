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
            for (int i = 0; i < 128; i++)
            {
                oDev.SendControlChange(_chan,(Control)i,0);
            }
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
            for (int i = 0; i < preset.SceneArray.Length; i++)
            {
                if (preset.SceneArray[i].DeskIsMuted)
                {
                    oDev.SendControlChange(_chan, Control.Solo1 + i, 127);
                }
                else
                {
                    oDev.SendControlChange(_chan, Control.Solo1 + i, 0);
                }

                if (preset.SceneArray[i].MicIsMuted)
                {
                    oDev.SendControlChange(_chan, Control.Mute1 + i, 127);
                }
                else
                {
                    oDev.SendControlChange(_chan, Control.Mute1 + i, 0);
                }
                oDev.SendControlChange(_chan, Control.Rec1 + i, 0);
            }            
            if (preset.Master.DeskIsMuted)
            {
                oDev.SendControlChange(_chan, Control.Mute8, 127);
            }
            else
            {
                oDev.SendControlChange(_chan, Control.Mute8, 0);
            }

            if (preset.Master.MicIsMuted)
            {
                oDev.SendControlChange(_chan, Control.Mute7, 127);
            }
            else
            {
                oDev.SendControlChange(_chan, Control.Mute7, 0);
            }           
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
            if (_msg != null)
            {
                if (ControlChanged != null)
                {
                    ControlChanged(this, new ControlChangedEventArgs(_msg));
                }                
            }
            
        }

        public OBSControl CCMapperIn(MidiMsg msg)
        {
            if ((int)msg.Control <= 5)
            {
                float value = ((((int)msg.Control)*10)+((float)msg.Value/127.0f)); 
                return (new OBSControl(OBSControls.ChangeVolumeDesktop, value, "nanoKONTROL2"));
            }
            else if ((int)msg.Control == 6)
            {
                float value = ((((int)msg.Control)*10)+((float)msg.Value/127.0f)); 
                return (new OBSControl(OBSControls.ChangeVolumeMic, value, "nanoKONTROL2"));
            }
            else if ((int)msg.Control == 7)
            {
                float value = ((((int)msg.Control) * 10) + ((float)msg.Value / 127.0f));
                return (new OBSControl(OBSControls.ChangeVolumeDesktop, value, "nanoKONTROL2"));
            }
            else if ((int)msg.Control > 15 && (int)msg.Control <= 21)
            {
                float value = ((((int)msg.Control-16) * 10) + ((float)msg.Value / 127.0f));
                return (new OBSControl(OBSControls.ChangeVolumeMic, value, "nanoKONTROL2"));
            }
            else if ((32 <= (int)msg.Control) && (int)msg.Control <= 37 && msg.Value != 0)
            {
                float value = (((int)msg.Control-32) * 10);
                return (new OBSControl(OBSControls.MuteDesktop, value, "nanoKONTROL2"));
            }
            else if ((48 <= (int)msg.Control) && ((int)msg.Control) <= 54 && msg.Value != 0)
            {
                float value = (((int)msg.Control-48) * 10);
                return (new OBSControl(OBSControls.MuteMic, value, "nanoKONTROL2"));
            }
            else if (((int)msg.Control) == 55 && msg.Value != 0)
            {
                float value = (((int)msg.Control - 48) * 10);
                return (new OBSControl(OBSControls.MuteDesktop, value, "nanoKONTROL2"));
            }
            else if ((64 <= (int)msg.Control) && ((int)msg.Control) <= 69 && msg.Value != 0)
            {
                float value = ((((int)msg.Control-64) * 10) + ((float)msg.Value / 127.0f));
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
                int chan = ((int)control.Value / 10) + 32;
                if (((int)control.Value / 10) == 7)
                {
                    chan = 55;
                }
                                
                if (((int)control.Value % 10) == 0)
                {
                    list.Add(new MidiMsg(_chan, (Control)chan, 0));
                    //list.Add(new MidiMsg(_chan, Control.Mute8, 0));
                }
                else
                {
                    list.Add(new MidiMsg(_chan, (Control)chan, 127));
                    //list.Add(new MidiMsg(_chan, Control.Mute8, 127));
                }
            }
            else if (control.Control == OBSControls.MuteMic)
            {
                int chan = ((int)control.Value / 10) + 48;
                if (((int)control.Value % 10) == 0)
                {
                    list.Add(new MidiMsg(_chan, (Control)chan, 0));
                    //list.Add(new MidiMsg(_chan, Control.Mute7, 0));
                }
                else
                {
                    list.Add(new MidiMsg(_chan, (Control)chan, 127));
                    //list.Add(new MidiMsg(_chan, Control.Mute7, 127));
                }
            }
            return list;
        }



        public int PresetsAvailable
        {
            get { return 5; }
        }

        public int ScenesAvailable
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
