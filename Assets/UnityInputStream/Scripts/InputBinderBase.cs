using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
        
        #if UNITY_EDITOR

        [ButtonGroup("Group1")]
        private void OpenAndImport()
        {
            string path = UnityEditor.EditorUtility.OpenFilePanel("Open process json.", Application.streamingAssetsPath, "json");
            InputProcesses processes;
            Utility.ReadJson(path, out processes);

            buttonInputProcesses = new List<InputProcess>(processes.buttonProcess);
            floatInputProcesses = new List<InputProcess>(processes.floatProcess);
        }
        
        [ButtonGroup("Group1")]
        private void Save()
        {
            string path = UnityEditor.EditorUtility.SaveFilePanel("Save process json", Application.streamingAssetsPath,
                "NewInputProcess", "json");
            
            InputProcesses  processes = new InputProcesses();
            processes.buttonProcess = buttonInputProcesses.ToArray();
            processes.floatProcess = floatInputProcesses.ToArray();
            
            Utility.WriteJson(path, processes);
        }
        
        #endif
        
        
        private void Start()
        {
            var sources = InputSourceContainer.Instance.InputSourceBases;

            foreach (var source in sources)
            {
                foreach (var process in buttonInputProcesses)
                {
                    // キーバインド（ボタン）
                    source.OnButtonChanged
                        .Where(x => process.isBinding && x.Value)
                        .Subscribe(x =>
                        {
                            process.DeviceName = x.DeviceName;
                            process.Id = x.Id;
                            process.isBinding = false;
                        });
                        
                    // 入力検出（ボタン）
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
                    // キーバインド（Float）
                    source.OnFloatChanged
                        .Where(x => process.isBinding)
                        .Subscribe(x =>
                        {
                            process.DeviceName = x.DeviceName;
                            process.Id = x.Id;
                            process.isBinding = false;
                        });
                    
                    // 入力検出（Float)
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
        
        [InlineButton("SwitchBind", "Bind")]
        public bool isBinding;

        public InputProcess(string processName, string deviceName, string id)
        {
            ProcessName = processName;
            DeviceName = deviceName;
            Id = id;
            isBinding = false;
        }

        public InputProcess(string processName)
        {
            ProcessName = processName;
            isBinding = false;
        }

        private void SwitchBind()
        {
            isBinding = !isBinding;
        }
    }

    [System.Serializable]
    public class InputProcesses
    {
        public InputProcess[] buttonProcess;
        public InputProcess[] floatProcess;
    }
}
