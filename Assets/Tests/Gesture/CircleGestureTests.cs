using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Samples.CircleGestureCustomInteraction.GestureInputCore;
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
        public void DetectCircleTopLeftOnly()
        {
            bool enterExpected = false;
            bool enterAnyOther = false;
            m_Cg = new CircleGesture(() => {}, ()=>{}, ()=>{}, ()=>{});
            m_Cg.topLeftSlice.onEnter = state => enterExpected = true;
            m_Cg.topRightSlice.onEnter = state => enterAnyOther = true;
            m_Cg.bottomRightSlice.onEnter = state => enterAnyOther = true;
            m_Cg.bottomLeftSlice.onEnter = state => enterAnyOther = true;
            
            List<Vector2> input = new List<Vector2>();
            foreach (var position in DataSample.dataSampleTopRight) //sample data is counter clockwise, so sides are inverted considering to the sign of the derivative
            {
                m_Cg.Process(position);
            }
            Assert.True(enterExpected);
            Assert.False(enterAnyOther);
        }
        
        [Test]
        public void DetectCircleTopRightOnly()
        {
            bool enterExpected = false;
            bool enterAnyOther = false;
            m_Cg = new CircleGesture(() => {}, ()=>{}, ()=>{}, ()=>{});
            m_Cg.topLeftSlice.onEnter = state => enterAnyOther = true;
            m_Cg.topRightSlice.onEnter = state => enterExpected = true;
            m_Cg.bottomRightSlice.onEnter = state => enterAnyOther = true;
            m_Cg.bottomLeftSlice.onEnter = state => enterAnyOther = true;
            
            List<Vector2> input = new List<Vector2>();
            foreach (var position in DataSample.dataSampleTopLeft) //sample data is counter clockwise, so sides are inverted considering to the sign of the derivative
            {
                m_Cg.Process(position);
            }
            Assert.True(enterExpected);
            Assert.False(enterAnyOther);
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
        
        [Test]
        public void StateMachineDetectedCircleClockwise()
        {
            bool detected = false;
            m_Cg = new CircleGesture(() => {}, () => { detected = true; }, ()=>{}, ()=>{});
            
            List<Vector2> input = new List<Vector2>();
            foreach (var position in DataSample.dataSample.Reverse())
            {
                m_Cg.Process(position);
                
            }
            Assert.True(detected);
        }
        
        private CircleGesture m_Cg;
        
    }
}

