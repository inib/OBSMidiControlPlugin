using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSMidiControl
{
    public enum OBSControls
    {
        ChangeVolumeDesktop = 0,
        ChangeVolumeMic,
        MuteDesktop,
        MuteMic,
        ChangeScene,
    }

    public class OBSControl
    {
        public OBSControl(OBSControls control, float value, string name)
        {
            _control = control;
            _value = value;
            _name = name;
        }

        private OBSControls _control;

        public OBSControls Control
        {
            get { return _control; }
        }

        private float _value;

        public float Value
        {
            get { return _value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
        }
        
        
        
    }
}
