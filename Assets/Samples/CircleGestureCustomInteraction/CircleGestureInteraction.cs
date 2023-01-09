using System;
using System.ComponentModel;
using Samples.SimpleDemo.GestureInputCore;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Editor;
using EditorGUIUtility = UnityEditor.Experimental.Networking.PlayerConnection.EditorGUIUtility;


namespace Samples.SimpleDemo
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    [DisplayName("CircleGesture")]
    public class CircleGestureInteraction : IInputInteraction
    {
        public CircleGesture circleGesture { get; }

        static CircleGestureInteraction()
        {
            InputSystem.RegisterInteraction<CircleGestureInteraction>();
        }

        public CircleGestureInteraction()
        {
            circleGesture = new CircleGesture(() => m_IsStarted = true, () => m_IsPerformed = true,
                () => m_IsCanceled = true, () => {});
        }

        public void Process(ref InputInteractionContext context)
        {
            var position = context.ReadValue<Vector2>();
            circleGesture.Process(position);
            switch (context.phase)
            {
                case InputActionPhase.Waiting:
                    if (m_IsStarted)
                    {
                        context.SetTimeout(1);
                        context.Started();
                    }
                    break;
                case InputActionPhase.Started:
                    if (m_IsPerformed)
                    {
                        context.Performed();
                        Debug.Log("Performed Circle Detection!");
                    }
                    if (m_IsCanceled || context.timerHasExpired)
                    {
                        context.Canceled();
                    }
                    break;
                case InputActionPhase.Performed:
                case InputActionPhase.Canceled:
                    context.Waiting();
                    Reset();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        public void Reset()
        {
            circleGesture.Reset();
            m_IsStarted = false;
            m_IsPerformed = false;
            m_IsCanceled = false;
        }

        private bool m_IsStarted;
        private bool m_IsPerformed;
        private bool m_IsCanceled;
    }

#if UNITY_EDITOR
    /// <summary>
    /// UI that is displayed when editing <see cref="CircleGestureInteraction"/> in the editor.
    /// </summary>
    public class CircleGestureInteractionEditor : InputParameterEditor<CircleGestureInteraction>
    {
        public override void OnGUI()
        {
            target.circleGesture.epsilon = EditorGUILayout.FloatField("Sample Resolution",  target.circleGesture.epsilon, GUILayout.ExpandWidth(false));
            target.circleGesture.sampleSize = EditorGUILayout.IntField("Sample Size",  target.circleGesture.sampleSize, GUILayout.ExpandWidth(false));
            target.circleGesture.cancelThreshold = EditorGUILayout.IntField("Cancel Input Threshold",  target.circleGesture.cancelThreshold, GUILayout.ExpandWidth(false));
            target.circleGesture.transitionThreshold = EditorGUILayout.IntField("Detection Input Threshold",  target.circleGesture.transitionThreshold, GUILayout.ExpandWidth(false));
        }
    }
#endif
}