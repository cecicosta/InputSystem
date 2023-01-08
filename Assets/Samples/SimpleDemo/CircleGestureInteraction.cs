using System.ComponentModel;
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


        public void Process(ref InputInteractionContext context)
        {
         
        }

        public void Reset()
        {
     
        }

    
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