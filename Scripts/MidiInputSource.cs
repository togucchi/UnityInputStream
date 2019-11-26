using System;
using System.Collections;
using System.Collections.Generic;
using Toguchi.UnityInputStream;
using UnityEngine;
using MidiJack;

namespace  Toguchi.UnityInputStream
{
    public class MidiInputSource : InputSourceBase
    {
        private void OnEnable()
        {
            MidiMaster.noteOnDelegate += DetectNoteOn;
            MidiMaster.noteOffDelegate += DetectNoteOff;
            MidiMaster.knobDelegate += DetectKnob;
        }

        private void OnDisable()
        {
            MidiMaster.noteOnDelegate -= DetectNoteOn;
            MidiMaster.noteOffDelegate -= DetectNoteOff;
            MidiMaster.knobDelegate -= DetectKnob;
        }

        private void DetectNoteOn(MidiChannel channel, int note, float velocity)
        {
            // Debug.Log("Device : " + channel + " ; Note : " + note + " ; Velocity" + velocity);
        }

        private void DetectNoteOff(MidiChannel channel, int note)
        {
            // Debug.Log("Device : " + channel + " ; Note : " + note);

        }

        private void DetectKnob(MidiChannel channel, int knobNumber, float knobValue)
        {
            // Debug.Log("Device : " + channel + " ; KnobNumber : " + knobNumber + " ; KnobValue : " + knobValue);
            
            PublishFloatInput(channel.ToString(), knobNumber.ToString(), knobValue);

            if (knobValue == 1f)
            {
                PublishButtonInput(channel.ToString(), knobNumber.ToString(), true);
            }
            else if (knobValue == 0f)
            {
                PublishButtonInput(channel.ToString(), knobNumber.ToString(), false);
            }
        }
    }
}
