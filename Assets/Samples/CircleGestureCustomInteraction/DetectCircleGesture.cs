using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DetectCircleGesture : MonoBehaviour
{
    public float actionDuration = 0.5f;

    [Serializable]
    public class BoolEvent : UnityEvent<bool> {}
    [SerializeField]
    public BoolEvent onDetected;
    [SerializeField]
    public BoolEvent onCanceled;
    [SerializeField]
    public BoolEvent onStarted;
    [SerializeField]
    public UnityEvent onActionFinished;


    // Start is called before the first frame update
    void Awake()
    {
        m_Controls = new GestureInputAction();
        m_Controls.Enable();

        m_Controls.Fire.Trigger.performed += ctx =>
        {
            onDetected.Invoke(true);
            onStarted.Invoke(false);
            StartCoroutine(DelayOnActionFinished(() => ctx.interaction.Reset()));
        };
        m_Controls.Fire.Trigger.canceled += ctx =>
        {
            onCanceled.Invoke(true);
            onStarted.Invoke(false);
        };
        m_Controls.Fire.Trigger.started += ctx =>
        {
            onStarted.Invoke(true);
            onDetected.Invoke(false);
            onCanceled.Invoke(false);
        
        };
    }

    IEnumerator DelayOnActionFinished(Action action)
    {
        yield return new WaitForSecondsRealtime(actionDuration);
        action.Invoke();
    }
    
    private GestureInputAction m_Controls;
}