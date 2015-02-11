using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSMidiControl.Presets
{
    public class Scene
    {
        public string Name { get; set; }
        public float MicVol { get; set; }
        public float DeskVol { get; set; }
        public bool DeskIsMuted { get; set; }
        public bool MicIsMuted { get; set; }

        public Scene()
        {
            Name = "default";
            DeskVol = 1.0f;
            DeskIsMuted = false;
            MicVol = 1.0f;
            MicIsMuted = false;
        }

        public Scene(string name, float deskVol, bool deskIsMuted, float micVol, bool micIsMuted)
        {
            Name = name;
            DeskVol = deskVol;
            DeskIsMuted = deskIsMuted;
            MicVol = micVol;
            MicIsMuted = micIsMuted;
        }
    }
}
