using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

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
                // null要素, 重複要素の除去
                IEnumerable<InputSourceBase> sources = _inputSourceBases.Where(x => x != null).Distinct();

                return sources;
            }
        }

        private InputSourceContainer()
        {
            _inputSourceBases = new List<InputSourceBase>();
        }
        
        public void AddSource(InputSourceBase inputSourceBase)
        {
            _inputSourceBases.Add(inputSourceBase);
        }

    }
}
