using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Toguchi.UnityInputStream
{
    public abstract class InputBinderBase : MonoBehaviour
    {
        public List<InputProcess> buttonInputProcesses;
        public List<InputProcess> floatInputProcesses;
        
        

        protected abstract void OnButtonPushed(string processName, int processIndex);
        protected abstract void OnButtonReleased(string processName, int processIndex);
        protected abstract void OnFloatChanged(string processName, int processIndex, float value);
        
        private void Start()
        {
            var sources = InputSourceContainer.Instance.InputSourceBases;

            foreach (var source in sources)
            {
                for(int i = 0; i < buttonInputProcesses.Count; i++)
                {
                    source.OnButtonChanged
                        .Where(x => buttonInputProcesses[i].isBinding)
                        .Subscribe(x =>
                        {
                            buttonInputProcesses[i].DeviceName = x.DeviceName;
                            buttonInputProcesses[i].Id = x.Id;
                        });

                    source.OnButtonChanged
                        .Where(x => buttonInputProcesses[i].DeviceName == x.DeviceName && buttonInputProcesses[i].Id == x.Id)
                        .Subscribe(x =>
                        {
                            if (x.Value)
                            {
                                OnButtonPushed(buttonInputProcesses[i].ProcessName, i);
                            }
                            else
                            {
                                OnButtonReleased(buttonInputProcesses[i].ProcessName, i);
                            }
                        });
                }
                
                for(int i = 0; i < floatInputProcesses.Count; i++)
                {
                    source.OnFloatChanged
                        .Where(x => floatInputProcesses[i].isBinding)
                        .Subscribe(x =>
                        {
                            floatInputProcesses[i].DeviceName = x.DeviceName;
                            floatInputProcesses[i].Id = x.Id;
                        });

                    source.OnFloatChanged
                        .Where(x => floatInputProcesses[i].DeviceName == x.DeviceName &&
                                    floatInputProcesses[i].Id == x.Id)
                        .Subscribe(x => { OnFloatChanged(floatInputProcesses[i].ProcessName, i, x.Value); });
                }
            }
        }
    }

    [System.Serializable]
    public class InputProcess
    {
        public string ProcessName;
        public string DeviceName;
        public string Id;
        public bool isBinding;

        public InputProcess(string processName, string deviceName, string id)
        {
            ProcessName = processName;
            DeviceName = deviceName;
            Id = id;
        }
    }
}
