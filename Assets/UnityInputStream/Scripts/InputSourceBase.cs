using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace Toguchi.UnityInputStream
{
    public class InputSourceBase : MonoBehaviour
    {
        private Subject<ButtonInput> buttonInputSubject = new Subject<ButtonInput>();
        private Subject<FloatInput> floatInputSubject = new Subject<FloatInput>();

        public IObservable<ButtonInput> OnButtonChanged => buttonInputSubject;
        public IObservable<FloatInput> OnFloatChanged => floatInputSubject;

        public void PublishButtonInput(string deviceName, string id, bool value)
        {
            var buttonInput = new ButtonInput(deviceName, id, value);
            buttonInputSubject.OnNext(buttonInput);
        }

        public void PublishFloatInput(string deviceName, string id, float value)
        {
            var floatInput = new FloatInput(deviceName, id, value);
            floatInputSubject.OnNext(floatInput);
        }

        private void Awake()
        {
            InputSourceContainer.Instance.AddSource(this);
        }
    }

    public class ValueInput
    {
        public string DeviceName;
        public string Id;

        public ValueInput(string deviceName, string id)
        {
            DeviceName = deviceName;
            Id = id;
        }
    }

    public class ButtonInput : ValueInput
    {
        public bool Value;

        public ButtonInput(string deviceName, string id, bool value) : base(deviceName, id)
        {
            Value = value;
        }
    }

    public class FloatInput : ValueInput
    {
        public float Value;

        public FloatInput(string deviceName, string id, float value) : base(deviceName, id)
        {
            Value = value;
        }
    }


}
