using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSMidiControl.MidiControl
{
    public class OBSMidiBridge
    {
        private Devices.IDevice _device;
        private Presets.Preset _currentPreset;
        private Presets.Scene _currentScene;

        public OBSMidiBridge(Devices.KnownDevices device)
        {
            _device = Devices.DeviceSelector.GetDevice(device);
            if (_device != null)
            {
                SetCurrentPreset(_device.ScenesAvailable);
                _currentScene = _currentPreset.SceneArray[0];
            }
        }

        private void SetCurrentPreset(int length)
        {
            Presets.Scene[] scArr = new Presets.Scene[_device.ScenesAvailable];
            for (int i = 0; i < scArr.Length; i++)
            {	
                scArr[i] = new Presets.Scene();
            }
            _currentPreset = new Presets.Preset("test",scArr);
            _device.SetAll(_currentPreset);                      
        }

        public void SetVolume(OBSControl control)
        {
            // Input OBS

            // Output MIDI
            bool master;
            if (control.Control == OBSControls.ChangeVolumeDesktop)
            {
                _currentScene.DeskVol = control.Value;
            }

            if (control.Control == OBSControls.MuteDesktop)
            {
                if (_currentScene.DeskIsMuted == (control.Value != 1))
                {
                    int chan = Array.FindIndex(_currentPreset.SceneArray, a => a.Name == _currentScene.Name);
                    if (control.Value == 1)
                    {
                        _currentScene.DeskIsMuted = true;
                        _currentPreset.Master.DeskIsMuted = true;
                        _device.SetControl(new OBSControl(OBSControls.MuteDesktop, (10 * chan) + 1, ""));
                    }
                    else
                    {
                        _currentScene.DeskIsMuted = false;
                        _currentPreset.Master.DeskIsMuted = false;
                        _device.SetControl(new OBSControl(OBSControls.MuteDesktop, (10 * chan), ""));
                    }
                }
            }
        }

        public void SetCurrentScene(string scene)
        {
            // Input OBS

            // Output MIDI

            int sceneNr;
            if (scene != _currentScene.Name)
            {
                for (int i = 0; i < _device.ScenesAvailable; i++)
                {
                    _device.SetControl(new OBSControl(OBSControls.ChangeScene, (i * 10) + 0, scene));
                }

                if (_currentPreset.SceneArray.Any(a => a.Name == scene))
                {
                    _currentScene = _currentPreset.SceneArray.First(a => a.Name == scene);
                    sceneNr = Array.FindIndex(_currentPreset.SceneArray, a => a.Name == scene);
                    for (int i = 0; i < _device.ScenesAvailable; i++)
                    {
                        if (i == sceneNr)
                        {
                            _device.SetControl(new OBSControl(OBSControls.ChangeScene, (i * 10) + 1, scene));
                        }
                    }                    
                }

            }
        }

        public void Dispose()
        {
            _device.Dispose();
        }
       
    }
}
