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
            cg = new CircleGesture(() => { hasStarted = true; }, ()=>{}, ()=>{}, ()=>{});
            
            List<Vector2> input = new List<Vector2>();
            for (var i = 0; i < dataSample.Length; i++)
            {
                cg.Process(dataSample[i]);
                if (i == 80 && !hasStarted)
                {
                    break;
                }
            }
            Assert.True(hasStarted);
        }
        
        // A Test behaves as an ordinary method
        [Test]
        public void StateMachineDoNotStartedBefore5DifferentInputsOnX()
        {
            var hasStarted = false;
            cg = new CircleGesture(() => { hasStarted = true; }, ()=>{}, ()=>{}, ()=>{});
            
            List<Vector2> input = new List<Vector2>();
            for (var i = 0; i < 11; i++)
            {
                cg.Process(dataSample[i]);
            }
            Assert.False(hasStarted);
        }
        
        // A Test behaves as an ordinary method
        [Test]
        public void StateMachineLeaveStartState()
        {
            var hasStarted = false;
            cg = new CircleGesture(() => {}, ()=>{}, ()=>{}, ()=>{});
            cg.start.onExit = state => hasStarted = true;
            
            List<Vector2> input = new List<Vector2>();
            foreach (var position in dataSample)
            {
                cg.Process(position);
            }
            Assert.True(hasStarted);
        }

        
        // A Test behaves as an ordinary method
        [Test]
        public void StateMachineLeaveStartStateToTopLeftSliceState()
        {
            var hasStarted = false;
            cg = new CircleGesture(() => {}, ()=>{}, ()=>{}, ()=>{});
            cg.topLeftSlice.onEnter = state => hasStarted = true;
            
            List<Vector2> input = new List<Vector2>();
            foreach (var position in dataSample)
            {
                cg.Process(position);
            }
            Assert.True(hasStarted);
        }
        
        [UnityTest]
        public IEnumerator Custom_Interaction_StateMachineStartedAfter5DifferentInputsOnX()
        {
            yield return null;
        }

        private CircleGesture cg;
        
        private Vector2[] dataSample = new Vector2[]
        {
            new Vector2(212, 190), new Vector2(212, 190), new Vector2(212, 190), new Vector2(212, 190),
            new Vector2(212, 191), new Vector2(213, 191), new Vector2(213, 192), new Vector2(213, 192),
            new Vector2(214, 192), new Vector2(214, 192), new Vector2(215, 191), new Vector2(217, 190),
            new Vector2(219, 188), new Vector2(222, 186), new Vector2(230, 180), new Vector2(237, 175),
            new Vector2(247, 168), new Vector2(257, 161), new Vector2(262, 158), new Vector2(267, 156),
            new Vector2(285, 149), new Vector2(303, 143), new Vector2(308, 143), new Vector2(313, 142),
            new Vector2(318, 142), new Vector2(322, 142), new Vector2(328, 143), new Vector2(333, 143),
            new Vector2(338, 144), new Vector2(343, 145), new Vector2(348, 147), new Vector2(352, 149),
            new Vector2(356, 152), new Vector2(360, 154), new Vector2(364, 157), new Vector2(368, 161),
            new Vector2(371, 164), new Vector2(375, 168), new Vector2(378, 172), new Vector2(381, 177),
            new Vector2(384, 181), new Vector2(387, 186), new Vector2(389, 191), new Vector2(391, 197),
            new Vector2(393, 202), new Vector2(395, 208), new Vector2(397, 215), new Vector2(398, 221),
            new Vector2(399, 227), new Vector2(400, 233), new Vector2(400, 247), new Vector2(401, 261),
            new Vector2(400, 268), new Vector2(399, 275), new Vector2(393, 290), new Vector2(386, 305),
            new Vector2(380, 313), new Vector2(375, 322), new Vector2(368, 328), new Vector2(361, 335),
            new Vector2(354, 341), new Vector2(346, 347), new Vector2(338, 352), new Vector2(329, 358),
            new Vector2(320, 363), new Vector2(311, 368), new Vector2(301, 371), new Vector2(292, 375),
            new Vector2(282, 379), new Vector2(272, 382), new Vector2(262, 385), new Vector2(252, 388),
            new Vector2(236, 390), new Vector2(220, 392), new Vector2(218, 392), new Vector2(216, 392),
            new Vector2(209, 392), new Vector2(203, 392), new Vector2(186, 391), new Vector2(170, 391),
            new Vector2(164, 389), new Vector2(157, 387), new Vector2(152, 384), new Vector2(147, 381),
            new Vector2(142, 379), new Vector2(137, 376), new Vector2(133, 372), new Vector2(129, 368),
            new Vector2(126, 364), new Vector2(122, 360), new Vector2(120, 355), new Vector2(117, 351),
            new Vector2(115, 346), new Vector2(113, 341), new Vector2(111, 335), new Vector2(110, 329),
            new Vector2(110, 323), new Vector2(109, 316), new Vector2(109, 311), new Vector2(109, 305),
            new Vector2(110, 300), new Vector2(110, 295), new Vector2(114, 284), new Vector2(118, 274),
            new Vector2(122, 269), new Vector2(125, 264), new Vector2(129, 259), new Vector2(134, 253),
            new Vector2(138, 249), new Vector2(143, 244), new Vector2(148, 240), new Vector2(153, 235),
            new Vector2(159, 231), new Vector2(164, 226), new Vector2(170, 222), new Vector2(175, 218),
            new Vector2(180, 215), new Vector2(184, 213), new Vector2(188, 211), new Vector2(191, 209),
            new Vector2(195, 207), new Vector2(199, 206), new Vector2(202, 205), new Vector2(204, 203),
            new Vector2(207, 203), new Vector2(209, 203), new Vector2(211, 202), new Vector2(213, 202),
            new Vector2(215, 202), new Vector2(217, 202), new Vector2(217, 203), new Vector2(218, 203),
            new Vector2(218, 207), new Vector2(219, 210)
        };
    }
}