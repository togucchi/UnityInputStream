using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Toguchi.UnityInputStream
{
    public class ObjectActivationSwitcher : InputBinderBase
    {
        public List<GameObject> objects;

        [Button]
        private void AutoAssignInputs()
        {
            List<InputProcess> inputProcesses = new List<InputProcess>();

            foreach (var obj in objects)
            {
                inputProcesses.Add(new InputProcess(obj.name));
            }

            buttonInputProcesses = inputProcesses;
        }

        private void SwitchActivation(string objName)
        {
            foreach (var obj in objects)
            {
                if (obj.name == objName)
                {
                    obj.SetActive(!obj.activeSelf);
                }
            }
        }

        protected override void OnButtonPushed(string processName, int processIndex)
        {
            SwitchActivation(processName);
        }

        protected override void OnButtonReleased(string processName, int processIndex)
        {
            
        }

        protected override void OnFloatChanged(string processName, int processIndex, float value)
        {
            
        }
    }
}
