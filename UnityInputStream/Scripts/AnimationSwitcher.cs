using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using Sirenix.OdinInspector;
using Debug = UnityEngine.Debug;

namespace Toguchi.UnityInputStream
{
    public class AnimationSwitcher : InputBinderBase
    {
        
        public Animator animator;
        public List<AnimationClip> clips;
        public float transitionTime = 0.1f;
        
        private PlayableGraph graph;
        private AnimationMixerPlayable mixer;
        private AnimationClipPlayable prePlayable, currentPlayable;
        
        private float weight;

        private Coroutine fadeCoroutine;

        [Button]
        public void AutoAssignInputs()
        {
            buttonInputProcesses = new List<InputProcess>();
            foreach (var clip in clips)
            {
                buttonInputProcesses.Add(new InputProcess(clip.name));
            }
        }

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        private void Awake()
        {
            graph = PlayableGraph.Create();
        }

        protected override void Start()
        {
            base.Start();
            
            if (clips.Count <= 0)
            {
                Debug.LogWarning("AnimationClipが登録されていません");
                return;
            }
            
            currentPlayable = AnimationClipPlayable.Create(graph, clips[0]);
            mixer = AnimationMixerPlayable.Create(graph, 2, true);
            mixer.ConnectInput(0, currentPlayable, 0);
            mixer.SetInputWeight(0, 1);

            var output = AnimationPlayableOutput.Create(graph, "output", animator);
            output.SetSourcePlayable(mixer);
            graph.Play();
        }

        private void SwitchAnimation(AnimationClip clip)
        {
            // コルーチンの破棄
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = null;
            }

            fadeCoroutine = StartCoroutine(FadeAnimation(clip));
        }

        IEnumerator FadeAnimation(AnimationClip clip)
        {
            // AnimationClipPlayableの再登録
            graph.Disconnect(mixer, 0);
            graph.Disconnect(mixer, 1);

            if (prePlayable.IsValid())
            {
                prePlayable.Destroy();
            }

            prePlayable = currentPlayable;
            currentPlayable = AnimationClipPlayable.Create(graph, clip);
            mixer.ConnectInput(1, prePlayable, 0);
            mixer.ConnectInput(0, currentPlayable, 0);
            
            
            // 指定時間でのフェード
            float waitTime = Time.timeSinceLevelLoad + transitionTime;
            yield return new WaitWhile(() =>
            {
                var diff = waitTime - Time.timeSinceLevelLoad;
                if (diff <= 0)
                {
                    mixer.SetInputWeight(1, 0);
                    mixer.SetInputWeight(0, 1);
                    return false;
                }
                else
                {
                    var rate = Mathf.Clamp01(diff / transitionTime);
                    mixer.SetInputWeight(1, rate);
                    mixer.SetInputWeight(0, 1- rate);
                    return true;
                }
            });
        }

        private void OnDestroy()
        {
            graph.Destroy();
        }

        protected override void OnButtonPushed(string processName, int processIndex)
        {
            foreach (var clip in clips)
            {
                if (clip.name == processName)
                {
                    SwitchAnimation(clip);
                    break;
                }
            }
        }

        protected override void OnButtonReleased(string processName, int processIndex)
        {
            
        }

        protected override void OnFloatChanged(string processName, int processIndex, float value)
        {
            
        }
    }
}