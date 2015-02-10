using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSMidiControl.MidiControl
{
    public delegate void ControlChangedEventHandler(ControlChangedEventArgs e);

    public class ControlChangedEventArgs : EventArgs
    {
        public ControlChangedEventArgs(OBSControls control, float value, string name)
        {
            _control = control;
            _val = value;
            _name = name;
        }

        private OBSControls _control;

        public OBSControls Control
        {
            get { return _control; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }  
        }

        private float _val;

        public float Val
        {
            get { return _val; }
        }  
    }
}
