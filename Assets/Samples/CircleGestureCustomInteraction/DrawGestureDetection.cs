using System;
using System.Collections;
using System.Collections.Generic;
using Samples.CircleGestureCustomInteraction.GestureInputCore;
using Samples.SimpleDemo.GestureInputCore;
using UnityEditor;
using UnityEngine;

public class DrawGestureDetection : MonoBehaviour
{
    public Material material;

    public Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CircleGestureShowCircle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public IEnumerator CircleGestureShowCircle()
    {
        m_Cg = new CircleGesture(() => {}, () => { }, ()=>{}, ()=>{});

        m_Cg.topLeftSlice.onUpdate = (stack, input) => DrawLine(input[input.Count-1], input[input.Count - 2], Color.red);
        m_Cg.topRightSlice.onUpdate = (stack, input) => DrawLine(input[input.Count-1], input[input.Count - 2], Color.green);
        m_Cg.bottomRightSlice.onUpdate = (stack, input) => DrawLine(input[input.Count-1], input[input.Count - 2], Color.blue);
        m_Cg.bottomLeftSlice.onUpdate = (stack, input) => DrawLine(input[input.Count-1], input[input.Count - 2], Color.yellow);
        
        m_Cg.start.onUpdate = (stack, input) => DrawLine(input[input.Count-1], input[input.Count - 2], Color.black);
        m_Cg.end.onUpdate = (stack, input) => { };
            
            
        foreach (var position in DataSample.dataSample)
        {
            m_Cg.Process(position);
            yield return new WaitForSeconds(0.1f);
        }
        
        yield return null;
    }

    void DrawLine(Vector2 start, Vector2 end, Color color)
    {
        GameObject myLine = new GameObject
        {
            transform =
            {
                parent = this.transform,
                localScale = new Vector3(0.1f, 0.1f, 0.1f),
                localPosition = start
                
            }
        };

        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(material);
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 10f;
        lr.endWidth = 10f;
        lr.useWorldSpace = true;
        lr.SetPosition(0, camera.WorldToScreenPoint(start));
        lr.SetPosition(1, camera.WorldToScreenPoint(end));
    }

    private CircleGesture m_Cg;
}
