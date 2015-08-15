using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSMidiControl
{
    public enum BridgeControl
    {
        ChangeVolumeDesktop = 0,
        ChangeVolumeMic,
        MuteDesktop,
        MuteMic,
        ChangeScene,
    }

    public class OBSControl
    {
        public OBSControl(BridgeControl control, int channel, float value, string name)
        {
            _control = control;
            _channel = channel;
            _value = value;
            _name = name;
        }

        private BridgeControl _control;

        public BridgeControl Control
        {
            get { return _control; }
            set { _control = value; } 
        }

        private float _value;

        public float Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _channel;

        public int Channel
        {
            get { return _channel; }
            set { _channel = value; }
        }
        
        
    }
}
