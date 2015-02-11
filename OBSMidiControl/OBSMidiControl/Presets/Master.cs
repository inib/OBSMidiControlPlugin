using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSMidiControl.Presets
{
    public class Master
    {
        public float MicVol { get; set; }
        public float DeskVol { get; set; }
        public bool DeskIsMuted { get; set; }
        public bool MicIsMuted { get; set; }

        public Master()
        {
            DeskVol = 1.0f;
            DeskIsMuted = true;
            MicVol = 1.0f;
            MicIsMuted = true;
        }

        public Master(float deskVol, bool deskIsMuted, float micVol, bool micIsMuted)
        {
            DeskVol = deskVol;
            DeskIsMuted = deskIsMuted;
            MicVol = micVol;
            MicIsMuted = micIsMuted;
        }
    }
}
