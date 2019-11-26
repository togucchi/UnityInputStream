using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Toguchi.UnityInputStream
{
    public class ComponentActivationSwitcher : InputBinderBase
    {
        public List<MonoBehaviour> components;

        [Button]
        public void AutoAssignInputs()
        {
            List<InputProcess> inputProcesses = new List<InputProcess>();

            foreach (var obj in components)
            {
                inputProcesses.Add(new InputProcess(obj.name));
            }

            buttonInputProcesses = inputProcesses;
        }

        private void SwitchActivation(string objName)
        {
            foreach (var obj in components)
            {
                if (obj.name == objName)
                {
                    obj.enabled = !obj.enabled;
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
