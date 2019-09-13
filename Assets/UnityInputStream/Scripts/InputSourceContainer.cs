using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Toguchi.UnityInputStream
{
    // Singleton InputSourceのリストを保持する
    public class InputSourceContainer
    {
        private static InputSourceContainer _containerInstance = new InputSourceContainer();
        private List<InputSourceBase> _inputSourceBases;

        public static InputSourceContainer Instance => _containerInstance;
        
        public IEnumerable<InputSourceBase> InputSourceBases
        {
            get
            {
                // 重複要素の除去
                IEnumerable<InputSourceBase> sources = _inputSourceBases.Distinct();

                return sources;
            }
        }
        
        public void AddSource(InputSourceBase inputSourceBase)
        {
            _inputSourceBases.Add(inputSourceBase);
        }

    }
}
