using System;
using System.ComponentModel;
using Samples.SimpleDemo.GestureInputCore;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Editor;


namespace Samples.SimpleDemo
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    [DisplayName("CircleGesture")]
    public class CircleGestureInteraction : IInputInteraction
    {
        static CircleGestureInteraction()
        {
            InputSystem.RegisterInteraction<CircleGestureInteraction>();
        }

        public CircleGestureInteraction()
        {
            m_CircleGesture = new CircleGesture(() => m_IsStarted = true, () => m_IsPerformed = true,
                () => m_IsCanceled = true, () => {});
        }

        public void Process(ref InputInteractionContext context)
        {
            var position = context.ReadValue<Vector2>();
            m_CircleGesture.Process(position);
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
            m_CircleGesture.Reset();
            m_IsStarted = false;
            m_IsPerformed = false;
            m_IsCanceled = false;
        }

        private CircleGesture m_CircleGesture;
        private bool m_IsStarted;
        private bool m_IsPerformed;
        private bool m_IsCanceled;
    }

#if UNITY_EDITOR
    /// <summary>
    /// UI that is displayed when editing <see cref="CircleGestureInteraction"/> in the editor.
    /// </summary>
    internal class CircleGestureInteractionEditor : InputParameterEditor<CircleGestureInteraction>
    {
        protected override void OnEnable()
        {
        }

        public override void OnGUI()
        {
        }
    }
#endif
}