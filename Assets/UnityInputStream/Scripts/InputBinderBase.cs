using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Toguchi.UnityInputStream
{
    public abstract class InputBinderBase : MonoBehaviour
    {
        public List<InputProcess> buttonInputProcesses = new List<InputProcess>();
        public List<InputProcess> floatInputProcesses = new List<InputProcess>();
        
        

        protected abstract void OnButtonPushed(string processName, int processIndex);
        protected abstract void OnButtonReleased(string processName, int processIndex);
        protected abstract void OnFloatChanged(string processName, int processIndex, float value);
        
        private void Start()
        {
            var sources = InputSourceContainer.Instance.InputSourceBases;

            foreach (var source in sources)
            {
                foreach (var process in buttonInputProcesses)
                {
                    source.OnButtonChanged
                        .Where(x => process.isBinding && x.Value)
                        .Subscribe(x =>
                        {
                            process.DeviceName = x.DeviceName;
                            process.Id = x.Id;
                            process.isBinding = false;
                        });

                    source.OnButtonChanged
                        .Where(x => process.DeviceName == x.DeviceName && process.Id == x.Id)
                        .Subscribe(x =>
                        {
                            int index = buttonInputProcesses.IndexOf(process);
                            if (x.Value)
                            {
                                OnButtonPushed(process.ProcessName, index);
                            }
                            else
                            {
                                OnButtonReleased(process.ProcessName, index);
                            }
                        });
                }
                
                foreach(var process in floatInputProcesses)
                {
                    source.OnFloatChanged
                        .Where(x => process.isBinding)
                        .Subscribe(x =>
                        {
                            process.DeviceName = x.DeviceName;
                            process.Id = x.Id;
                            process.isBinding = false;
                        });

                    source.OnFloatChanged
                        .Where(x => process.DeviceName == x.DeviceName &&
                                    process.Id == x.Id)
                        .Subscribe(x =>
                        {
                            int index = floatInputProcesses.IndexOf(process);
                            OnFloatChanged(process.ProcessName, index, x.Value);
                        });
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

        public InputProcess(string processName)
        {
            ProcessName = processName;
        }
    }
}
