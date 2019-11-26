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
    public class MidiInputStreamTest
    {
        private class InputBinder_MinimumTest : InputBinderBase, IMonoBehaviourTest
        {
            public bool IsTestFinished { get; private set; }
            
            private InputProcess process;
            private InputProcess floatProcess;
            private InputProcess finishProcess;
            

            public bool isFloat = false;
            public bool isButton = false;
            private void Awake()
            {
                process = new InputProcess("testProcess");
                buttonInputProcesses.Add(process);

                floatProcess = new InputProcess("testFloatProcess");
                floatInputProcesses.Add(floatProcess);

                finishProcess = new InputProcess("finishProcess");
                buttonInputProcesses.Add(finishProcess);
            }


            protected override void OnButtonPushed(string processName, int processIndex)
            {
                
            }

            protected override void OnButtonReleased(string processName, int processIndex)
            {
                isButton = true;

                if (processName == "finishProcess")
                {
                    IsTestFinished = true;
                }
            }

            protected override void OnFloatChanged(string processName, int processIndex, float value)
            {
                isFloat = true;
            }
        }
        

        [UnityTest]
        public IEnumerator InputStreamMinimumTestWithEnumeratorPasses()
        {
            GameObject inputManager = new GameObject();
            var source = inputManager.AddComponent<MidiInputSource>();

         //   yield return null;
            
            yield return new MonoBehaviourTest<InputBinder_MinimumTest>();
        }
    }
}