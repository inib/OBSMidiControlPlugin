using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLROBS;

namespace OBSMidiControl
{
    public class MidiControlPlugin : AbstractPlugin
    {
        private MidiControl.OBSMidiBridge bridge;
        public MidiControlPlugin()
        {
            Name = "Midi Control";
            Description = "Control OBS with Midi Power";

            // Resolve embedded libraries
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        #region OBSCallbacks
        public override bool LoadPlugin()
        {
            API.Instance.AddSettingsPane(new PluginSettings());
            bridge = new MidiControl.OBSMidiBridge(MidiControl.Devices.KnownDevices.nanoKONTROL2);
            return true;
        } 

        public override void UnloadPlugin()
        {
            bridge.Dispose();
            base.UnloadPlugin();
        }

        public override void OnDesktopVolumeChanged(float level, bool muted, bool finalValue)
        {
            bridge.SetVolume(new OBSControl(OBSControls.ChangeVolumeDesktop, level, "Master"));
            if (muted)
            {
                bridge.SetVolume(new OBSControl(OBSControls.MuteDesktop, 1, "Master"));
            }
            else 
            {
                bridge.SetVolume(new OBSControl(OBSControls.MuteDesktop, 0, "Master"));
            }
        }

        public override void OnMicVolumeChanged(float level, bool muted, bool finalValue)
        {
            bridge.SetVolume(new OBSControl(OBSControls.ChangeVolumeMic, level, "Master"));
            if (muted)
            {
                bridge.SetVolume(new OBSControl(OBSControls.MuteMic, 1, "Master"));
            }
            else
            {
                bridge.SetVolume(new OBSControl(OBSControls.MuteMic, 0, "Master"));
            }
        }

        public override void OnSceneSwitch(string scene)
        {
            bridge.SetCurrentScene(scene);
        }
        #endregion

        #region internal
        // Method to resolve embedded libraries
        System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");

            dllName = dllName.Replace(".", "_");

            if (dllName.EndsWith("_resources")) return null;

            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(GetType().Namespace + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());

            byte[] bytes = (byte[])rm.GetObject(dllName);

            return System.Reflection.Assembly.Load(bytes);
        } 
        #endregion

    }
}
