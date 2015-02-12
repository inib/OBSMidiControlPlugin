﻿using System;
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
                _device.ControlChanged += NewMidiMsg;
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
            // Debug
            for (int i = 0; i < 6; i++)
            {
                _currentPreset.SceneArray[i].Name = "Scene" + i;
            }
            //
            _currentPreset.Master.DeskIsMuted = CLROBS.API.Instance.GetDesktopMuted();
            _currentPreset.Master.MicIsMuted = CLROBS.API.Instance.GetMicMuted();          

            _device.SetAll(_currentPreset);                      
        }        

        public void SetVolume(OBSControl control)
        {
            // Output MIDI
            bool master;
            if (control.Control == OBSControls.ChangeVolumeDesktop)
            {
                int chan = (int)control.Value / 10;
                float vol = control.Value - (chan * 10);
                CLROBS.API.Instance.Log("SetVolume::VolumeDesktop::Channel: " + chan);
                CLROBS.API.Instance.Log("SetVolume::VolumeDesktop::Vol: " + vol);
                if (chan == 7)
                {
                    _currentPreset.Master.DeskVol = vol;
                    CLROBS.API.Instance.Log("SetVolume::VolumeDesktop::MasterVolSet: " + vol);
                    if (!_currentScene.DeskIsMuted && !_currentPreset.Master.DeskIsMuted)
                    {
                        CLROBS.API.Instance.SetDesktopVolume(_currentScene.DeskVol * _currentPreset.Master.DeskVol, true);
                        CLROBS.API.Instance.Log("SetVolume::VolumeDesktop->OBSVolSet: " + vol);
                    }
                }
                if (chan < _device.ScenesAvailable)
                {
                    if (_currentPreset.SceneArray[chan].DeskVol != vol)
                    {
                        _currentPreset.SceneArray[chan].DeskVol = vol;
                        CLROBS.API.Instance.Log("SetVolume::VolumeDesktop::SceneVolSet: " + vol);
                        if (_currentPreset.SceneArray[chan].Name == _currentScene.Name && !_currentScene.DeskIsMuted && !_currentPreset.Master.DeskIsMuted)
                        {
                            CLROBS.API.Instance.SetDesktopVolume(_currentScene.DeskVol * _currentPreset.Master.DeskVol, true);
                            CLROBS.API.Instance.Log("SetVolume::VolumeDesktop->OBSVolSet: " + vol);
                        }
                    }
                }
            }

            if (control.Control == OBSControls.ChangeVolumeMic)
            {
                int chan = (int)control.Value / 10;
                float vol = control.Value - (chan * 10);
                CLROBS.API.Instance.Log("SetVolume::VolumeMic::Channel: " + chan);
                CLROBS.API.Instance.Log("SetVolume::VolumeMic::Vol: " + vol);
                if (chan == 6)
                {
                    _currentPreset.Master.MicVol = vol;
                    CLROBS.API.Instance.Log("SetVolume::VolumeMic::MasterVolSet: " + vol);
                    if (!_currentScene.MicIsMuted && !_currentPreset.Master.MicIsMuted)
                    {
                        CLROBS.API.Instance.SetMicVolume(_currentScene.MicVol * _currentPreset.Master.MicVol, true);
                        CLROBS.API.Instance.Log("SetVolume::VolumeMic->OBSVolSet: " + vol);
                    }
                }
                if (chan < _device.ScenesAvailable)
                {
                    if (_currentPreset.SceneArray[chan].MicVol != vol)
                    {
                        _currentPreset.SceneArray[chan].MicVol = vol;
                        CLROBS.API.Instance.Log("SetVolume::VolumeMic::SceneVolSet: " + vol);
                        if (_currentPreset.SceneArray[chan].Name == _currentScene.Name && !_currentScene.MicIsMuted && !_currentPreset.Master.MicIsMuted)
                        {
                            CLROBS.API.Instance.SetMicVolume(_currentScene.MicVol * _currentPreset.Master.MicVol, true);
                            CLROBS.API.Instance.Log("SetVolume::VolumeMic->OBSVolSet: " + vol);
                        }
                    }
                }
            }

            if (control.Control == OBSControls.MuteDesktop)
            {
                int chan = (int)control.Value / 10;
                CLROBS.API.Instance.Log("SetVolume::MuteDesktop::Channel: " + chan);                 
                if (chan == 7)
                {
                    if (_currentPreset.Master.DeskIsMuted)
                    {
                        CLROBS.API.Instance.Log("SetVolume::MuteDesktop::Master: true");
                        _currentPreset.Master.DeskIsMuted = false;
                        if (CLROBS.API.Instance.GetDesktopMuted() && !_currentScene.DeskIsMuted)
                        {
                            CLROBS.API.Instance.SetDesktopVolume(_currentPreset.Master.DeskVol * _currentScene.DeskVol, true);
                        } 
                        _device.SetControl(new OBSControl(OBSControls.MuteDesktop, (10 * 7), ""));
                    }
                    else
                    {
                        CLROBS.API.Instance.Log("SetVolume::MuteDesktop::Master: false");
                        _currentPreset.Master.DeskIsMuted = true;
                        if (!CLROBS.API.Instance.GetDesktopMuted())
                        {
                            CLROBS.API.Instance.ToggleDesktopMute();
                        }                         
                        _device.SetControl(new OBSControl(OBSControls.MuteDesktop, (10 * 7) + 1, ""));
                    }                    
                }
                else if (chan < _device.ScenesAvailable)
                {
                    if (_currentPreset.SceneArray[chan].DeskIsMuted)
                    {
                        CLROBS.API.Instance.Log("SetVolume::MuteDesktop::Scene: true");
                        _currentPreset.SceneArray[chan].DeskIsMuted = false;
                        if (CLROBS.API.Instance.GetDesktopMuted() && !_currentScene.DeskIsMuted && !_currentPreset.Master.DeskIsMuted)
                        {
                            CLROBS.API.Instance.SetDesktopVolume(_currentPreset.Master.DeskVol * _currentScene.DeskVol,true);
                        }                        
                        _device.SetControl(new OBSControl(OBSControls.MuteDesktop, (10 * chan), ""));
                    }
                    else
                    {
                        CLROBS.API.Instance.Log("SetVolume::MuteDesktop::Scene: false");
                        _currentPreset.SceneArray[chan].DeskIsMuted = true;
                        if (_currentScene.DeskIsMuted && !CLROBS.API.Instance.GetDesktopMuted())
                        {
                            CLROBS.API.Instance.ToggleDesktopMute();
                        } 
                        _device.SetControl(new OBSControl(OBSControls.MuteDesktop, (10 * chan) + 1, ""));                        
                    }                    
                }
            }
            if (control.Control == OBSControls.MuteMic)
            {
                int chan = (int)control.Value / 10;
                CLROBS.API.Instance.Log("SetVolume::MuteMic::Channel: " + chan);
                if (chan == 6)
                {
                    if (_currentPreset.Master.MicIsMuted)
                    {
                        CLROBS.API.Instance.Log("SetVolume::MuteMic::Master: true");
                        CLROBS.API.Instance.ToggleMicMute();
                        _currentPreset.Master.MicIsMuted = false;
                        _device.SetControl(new OBSControl(OBSControls.MuteMic, (10 * 6), ""));
                    }
                    else
                    {
                        CLROBS.API.Instance.Log("SetVolume::MuteMic::Master: false");
                        CLROBS.API.Instance.ToggleMicMute();
                        _currentPreset.Master.MicIsMuted = true;
                        _device.SetControl(new OBSControl(OBSControls.MuteMic, (10 * 6) + 1, ""));
                    }
                }
                else if (chan < _device.ScenesAvailable)
                {
                    if (_currentPreset.SceneArray[chan].MicIsMuted)
                    {
                        CLROBS.API.Instance.Log("SetVolume::MuteMic::Scene: true");
                        CLROBS.API.Instance.ToggleMicMute();
                        _currentPreset.SceneArray[chan].MicIsMuted = false;
                        _device.SetControl(new OBSControl(OBSControls.MuteMic, (10 * chan), ""));
                    }
                    else
                    {
                        CLROBS.API.Instance.Log("SetVolume::MuteMic::Scene: false");
                        CLROBS.API.Instance.ToggleMicMute();
                        _currentPreset.SceneArray[chan].MicIsMuted = true;
                        _device.SetControl(new OBSControl(OBSControls.MuteMic, (10 * chan) + 1, ""));
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

        private void NewMidiMsg(object sender, ControlChangedEventArgs e)
        {
            if (e.Control.Control == OBSControls.ChangeScene)
            {
                CLROBS.API.Instance.Log("NewMidiMsg::ChangeScene::Value: " + e.Control.Value);                
                string scene = _currentPreset.SceneArray[(int)e.Control.Value/10].Name;
                CLROBS.API.Instance.Log("NewMidiMsg::ChangeScene::Scene: " + scene);
                SetCurrentScene(scene);
            }
            if (e.Control.Control == OBSControls.ChangeVolumeDesktop)
            {
                CLROBS.API.Instance.Log("NewMidiMsg::ChangeVolumeDesk::Value: " + e.Control.Value);
                SetVolume(new OBSControl(e.Control.Control, e.Control.Value, e.Control.Name));                
            }
            if (e.Control.Control == OBSControls.ChangeVolumeMic)
            {
                CLROBS.API.Instance.Log("NewMidiMsg::ChangeVolumeMic::Value: " + e.Control.Value);
                SetVolume(new OBSControl(e.Control.Control, e.Control.Value, e.Control.Name));
            }
            if (e.Control.Control == OBSControls.MuteDesktop)
            {
                CLROBS.API.Instance.Log("NewMidiMsg::MuteDesktop::Value: " + e.Control.Value);
                SetVolume(new OBSControl(e.Control.Control, e.Control.Value, e.Control.Name)); 
            }
            if (e.Control.Control == OBSControls.MuteMic)
            {
                CLROBS.API.Instance.Log("NewMidiMsg::MuteMic::Value: " + e.Control.Value);
                SetVolume(new OBSControl(e.Control.Control, e.Control.Value, e.Control.Name));
            }
        }

        public void Dispose()
        {
            _device.Dispose();
        }
       
    }
}
