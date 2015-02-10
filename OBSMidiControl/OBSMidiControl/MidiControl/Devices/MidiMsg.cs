using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSMidiControl.MidiControl.Devices
{
    public class MidiMsg
    {
        private Midi.Control _control;
        private Midi.Channel _channel;
        private int _value;

        public Midi.Control Control { get { return _control;} }
        public Midi.Channel Channel { get { return _channel;} }
        public int Value { get { return _value;} }

        public MidiMsg(Midi.Channel channel, Midi.Control control, int value)
        {
            _channel = channel;
            _control = control;
            _value = value;
        }
    }    
}
