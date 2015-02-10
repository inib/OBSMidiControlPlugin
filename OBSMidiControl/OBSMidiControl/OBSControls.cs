using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSMidiControl
{
    enum OBSControls
    {
        ChangeVolumeDesktop = 0,
        ChangeVolumeMic,
        MuteDesktop,
        MuteMic,
        ChangeScene,
    }

    public class Control
    {
        public Control(OBSControls control, float value, string name)
        {

        }

        public int MyProperty { get; private set; }
    }
}
