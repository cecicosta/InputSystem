using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DetectCircleGesture : MonoBehaviour
{
    public float actionDuration = 1;
    public UnityEvent onDetected;
    public UnityEvent onCanceled;
    public UnityEvent onActionFinished;


    // Start is called before the first frame update
    void Awake()
    {
        m_Controls = new GestureInputAction();
        m_Controls.Enable();

        m_Controls.Fire.Trigger.performed += ctx =>
        {
            onDetected.Invoke();
            StartCoroutine(DelayOnActionFinished());
        };
        m_Controls.Fire.Trigger.canceled += ctx =>
        {
            onCanceled.Invoke();
            StartCoroutine(DelayOnActionFinished());
        };
    }

    IEnumerator DelayOnActionFinished()
    {
        yield return new WaitForSecondsRealtime(actionDuration);
        onActionFinished.Invoke();
    }
    
    private GestureInputAction m_Controls;
}