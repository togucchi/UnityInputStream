using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Toguchi.UnityInputStream;

namespace Tests
{
    public class InputStreamMinimumTest
    {
        private class InputBinder_MinimumTest : InputBinderBase, IMonoBehaviourTest
        {
            public bool IsTestFinished { get; private set; }

            private int state = 0;
            private InputProcess process;
            private InputProcess floatProcess;
            private void Awake()
            {
                process = new InputProcess("testProcess");
                process.isBinding = true;
                buttonInputProcesses.Add(process);
                    
                floatProcess = new InputProcess("testFloatProcess");
                floatProcess.isBinding = true;
                floatInputProcesses.Add(floatProcess);
                    
                StartCoroutine(TestScenario());

            }

            IEnumerator TestScenario()
            {
                yield return new WaitForSeconds(1f);

                state = 0;
                
                // ButtonBind
                InputSourceContainer.Instance.InputSourceBases.First()
                    .PublishButtonInput("TestDevice", "TestId", true);

                yield return null;
                
                Debug.Log("Device : " + process.DeviceName + ", Id : " + process.Id);
                
                InputSourceContainer.Instance.InputSourceBases.First()
                    .PublishButtonInput("TestDevice", "TestId", false);

                yield return null;
                
                // FloatBind
                InputSourceContainer.Instance.InputSourceBases.First()
                    .PublishFloatInput("TestFloatDevice", "TestFloatId", 1f);

                yield return null;
                
                Debug.Log("Device : " + floatProcess.DeviceName + ", Id : " + floatProcess.Id);

                InputSourceContainer.Instance.InputSourceBases.First()
                    .PublishFloatInput("TestFloatDevice", "TestFloatId", 0f);
                
                
                if (state == 4)
                    IsTestFinished = true;
            }

            protected override void OnButtonPushed(string processName, int processIndex)
            {
                state++;
                Debug.Log(state);
            }

            protected override void OnButtonReleased(string processName, int processIndex)
            {
                state++;
                
                Debug.Log(state);
            }

            protected override void OnFloatChanged(string processName, int processIndex, float value)
            {
                state++;
                Debug.Log(state);
            }
        }
        

        [UnityTest]
        public IEnumerator InputStreamMinimumTestWithEnumeratorPasses()
        {
            GameObject inputManager = new GameObject();
            var source = inputManager.AddComponent<InputSourceBase>();

         //   yield return null;
            
            yield return new MonoBehaviourTest<InputBinder_MinimumTest>();
        }
    }
}