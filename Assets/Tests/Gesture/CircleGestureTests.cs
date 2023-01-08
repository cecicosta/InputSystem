using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Samples.SimpleDemo.GestureInputCore;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Gesture
{
    public class CircleGestureTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void StateMachineStartedBeforeEndOfFirstQuadrant()
        {
            var hasStarted = false;
            m_Cg = new CircleGesture(() => { hasStarted = true; }, ()=>{}, ()=>{}, ()=>{});
            
            List<Vector2> input = new List<Vector2>();
            for (var i = 0; i < DataSample.dataSample.Length; i++)
            {
                m_Cg.Process(DataSample.dataSample[i]);
                if (i == 80 && !hasStarted)
                {
                    break;
                }
            }
            Assert.True(hasStarted);
        }
        [Test]
        public void StateMachineDoNotStartedBeforeBeforeSampleIsBigEnough()
        {
            var hasStarted = false;
            m_Cg = new CircleGesture(() => { hasStarted = true; }, ()=>{}, ()=>{}, ()=>{});
            
            List<Vector2> input = new List<Vector2>();
            for (var i = 0; i < m_Cg.sampleSize; i++)
            {
                m_Cg.Process(DataSample.dataSample[i]);
            }
            Assert.False(hasStarted);
        }
        [Test]
        public void StateMachineLeaveStartState()
        {
            var hasStarted = false;
            m_Cg = new CircleGesture(() => {}, ()=>{}, ()=>{}, ()=>{});
            m_Cg.start.onExit = state => hasStarted = true;
            
            List<Vector2> input = new List<Vector2>();
            foreach (var position in DataSample.dataSample)
            {
                m_Cg.Process(position);
            }
            Assert.True(hasStarted);
        }
        [Test]
        public void StateMachineLeaveStartStateToTopLeftSliceState()
        {
            var hasStarted = false;
            m_Cg = new CircleGesture(() => {}, ()=>{}, ()=>{}, ()=>{});
            m_Cg.topLeftSlice.onEnter = state => hasStarted = true;
            
            List<Vector2> input = new List<Vector2>();
            foreach (var position in DataSample.dataSample)
            {
                m_Cg.Process(position);
            }
            Assert.True(hasStarted);
        }
        
        [Test]
        public void StateMachineEnterFirstAndSecondSlicesInSequence()
        {
            var sequence = 0;
            var hasEnterFirstSlice = -1;
            var hasEnterSecondSlice = -1;
            m_Cg = new CircleGesture(() => {}, ()=>{}, ()=>{}, ()=>{});
            m_Cg.sampleSize = 8;
            m_Cg.topLeftSlice.onEnter = state => hasEnterFirstSlice = sequence++;
            m_Cg.topRightSlice.onEnter = state => hasEnterSecondSlice = sequence++;
            
            List<Vector2> input = new List<Vector2>();
            foreach (var position in DataSample.dataSampleTopLeft)
            {
                m_Cg.Process(position);
                if (hasEnterFirstSlice >= 0 && hasEnterSecondSlice >= 0)
                {
                    break;
                }
            }
            Assert.Greater(hasEnterSecondSlice, hasEnterFirstSlice);
        }
        [Test]
        public void StateMachineEnterSecondAndThirdSlicesInSequence()
        {
            var sequence = 0;
            var hasEnterSecondSlice = -1;
            var hasEnterThirdSlice = -1;
            m_Cg = new CircleGesture(() => {}, ()=>{}, ()=>{}, ()=>{});
            m_Cg.topRightSlice.onEnter = state => hasEnterSecondSlice = sequence++;
            m_Cg.bottomRightSlice.onEnter = state => hasEnterThirdSlice = sequence++;
            
            List<Vector2> input = new List<Vector2>();
            foreach (var position in DataSample.dataSample)
            {
                m_Cg.Process(position);
                if (hasEnterThirdSlice >= 0 && hasEnterSecondSlice >= 0)
                {
                    break;
                }
            }
            Assert.Greater(hasEnterThirdSlice, hasEnterSecondSlice);
        }
        [Test]
        public void StateMachineEnterThirdAndForthSlicesInSequence()
        {
            var sequence = 0;
            var hasEnterThirdSlice = -1;
            var hasEnterForthSlice = -1;
            m_Cg = new CircleGesture(() => {}, ()=>{}, ()=>{}, ()=>{});
            m_Cg.bottomRightSlice.onEnter = state => hasEnterThirdSlice = sequence++;
            m_Cg.bottomLeftSlice.onEnter = state => hasEnterForthSlice = sequence++;
            
            List<Vector2> input = new List<Vector2>();
            foreach (var position in DataSample.dataSample)
            {
                m_Cg.Process(position);
                if (hasEnterForthSlice >= 0 && hasEnterThirdSlice >= 0)
                {
                    break;
                }
            }
            Assert.Greater(hasEnterForthSlice, hasEnterThirdSlice);
        }
        [Test]
        public void StateMachineEnterForthAndFirstSlicesInSequence()
        {
            var sequence = 0;
            var hasEnterForthSlice = -1;
            var hasEnterFirstSlice = -1;
            m_Cg = new CircleGesture(() => {}, ()=>{}, ()=>{}, ()=>{});
            m_Cg.bottomLeftSlice.onEnter = state => hasEnterForthSlice = sequence++;
            m_Cg.topLeftSlice.onEnter = state => hasEnterFirstSlice = sequence++;
            
            List<Vector2> input = new List<Vector2>();
            foreach (var position in DataSample.dataSample)
            {
                m_Cg.Process(position);
            }
            Assert.Greater(hasEnterFirstSlice, hasEnterForthSlice);
        }
        [Test]
        public void StateMachineDetectedCircle()
        {
 

            bool detected = false;
            m_Cg = new CircleGesture(() => {}, () => { detected = true; }, ()=>{}, ()=>{});
            
            
            
            List<Vector2> input = new List<Vector2>();
            foreach (var position in DataSample.dataSample)
            {
                m_Cg.Process(position);
                
            }
            Assert.True(detected);
        }


        [UnityTest]
        [Category("Samples")]
        public IEnumerator CircleGestureShowCircle()
        {
            bool detected = false;
            m_Cg = new CircleGesture(() => {}, () => { detected = true; }, ()=>{}, ()=>{});

            m_Cg.topLeftSlice.onUpdate = (stack, input) => DrawLine(input[input.Count-1], input[input.Count - 2], Color.red);
            m_Cg.topRightSlice.onUpdate = (stack, input) => DrawLine(input[input.Count-1], input[input.Count - 2], Color.green);
            m_Cg.bottomRightSlice.onUpdate = (stack, input) => DrawLine(input[input.Count-1], input[input.Count - 2], Color.blue);
            m_Cg.bottomLeftSlice.onUpdate = (stack, input) => DrawLine(input[input.Count-1], input[input.Count - 2], Color.yellow);
            m_Cg.start.onUpdate = (stack, input) => DrawLine(input[input.Count-1], input[input.Count - 2], Color.black);
            m_Cg.end.onUpdate = (stack, input) => detected = true;
            
            
            foreach (var position in DataSample.dataSample)
            {
                m_Cg.Process(position);
            }

            while (true)
            {
                yield return new WaitForSeconds(1);
            }
            yield return null;
        }

        void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            GameObject myLine = new GameObject();
            myLine.transform.position = start;
            myLine.AddComponent<LineRenderer>();
            LineRenderer lr = myLine.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Diffuse"));
            lr.SetColors(color, color);
            lr.SetWidth(0.1f, 0.1f);
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
        }

        private CircleGesture m_Cg;
        
    }
}

