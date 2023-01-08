using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Samples.SimpleDemo.GestureInputCore
{
    public class CircleGesture
    {
        private Action onStarted { get; }
        private Action onFinished { get; }

        private Action onCanceled { get; }
        private Action onIdle { get; }

        public CircleGesture(Action onStarted, Action onFinished, Action onCanceled, Action onIdle)
        {
            this.onStarted = onStarted;
            this.onFinished = onFinished;
            this.onCanceled = onCanceled;
            this.onIdle = onIdle;

            start.AddTransition(topLeftSlice, IsTopLeftSlice);
            start.AddTransition(topRightSlice, IsTopRightSlice);
            start.AddTransition(bottomRightSlice, IsBottomRightSlice);
            start.AddTransition(bottomLeftSlice, IsBottomLeftSlice);

            topLeftSlice.AddTransition(topRightSlice, IsTopRightSlice);
            topLeftSlice.AddTransition(end, IsEnd);
            topLeftSlice.AddTransition(start,
                new Func<List<State>, List<Vector2>, bool>((List<State> stack, List<Vector2> input) =>
                    !IsTopLeftSlice(stack, input) && !IsTopRightSlice(stack, input)));

            topRightSlice.AddTransition(bottomRightSlice, IsBottomRightSlice);
            topRightSlice.AddTransition(end, IsEnd);
            topRightSlice.AddTransition(start,
                new Func<List<State>, List<Vector2>, bool>((List<State> stack, List<Vector2> input) =>
                    !IsTopRightSlice(stack, input) && !IsBottomRightSlice(stack, input)));

            bottomRightSlice.AddTransition(bottomLeftSlice, IsBottomLeftSlice);
            bottomRightSlice.AddTransition(end, IsEnd);
            bottomRightSlice.AddTransition(start,
                new Func<List<State>, List<Vector2>, bool>((List<State> stack, List<Vector2> input) =>
                    !IsBottomRightSlice(stack, input) && !IsBottomLeftSlice(stack, input)));

            bottomLeftSlice.AddTransition(topLeftSlice, IsTopLeftSlice);
            bottomLeftSlice.AddTransition(end, IsEnd);
            bottomLeftSlice.AddTransition(start,
                new Func<List<State>, List<Vector2>, bool>((List<State> stack, List<Vector2> input) =>
                    !IsBottomLeftSlice(stack, input) && !IsTopLeftSlice(stack, input)));

            stateMachine = new StateMachine(start, end, 4);
        }

        public void Process(Vector2 position)
        {
            if (!CanUpdate(position))
            {
                onIdle();
                return;
            }

            stateMachine.Update(m_Inputs);
            if (stateMachine.IsStarted())
            {
                if (!m_IsStarted)
                {
                    m_IsStarted = true;
                    onStarted();
                }

                if (stateMachine.IsFinished())
                {
                    onFinished();
                }
            }

            if (!m_IsStarted || stateMachine.IsStarted())
            {
                return;
            }
            Reset();
            onCanceled();
        }

        private void Reset()
        {
            m_IsStarted = false;
            m_Inputs.Clear();
            stateMachine.Reset();
        }

        private bool CanUpdate(Vector2 position)
        {
            if (m_Inputs.Count == 0)
            {
                m_Inputs.Add(position);
            }
            else if (position.x - m_Inputs.Last().x > float.Epsilon)
            {
                m_Inputs.Add(position);
            }

            if (m_Inputs.Count > 5)
            {
                m_Inputs.RemoveAt(0);
            }

            return m_Inputs.Count == 5;
        }

        private static bool GetDerivativesIfDefined(List<Vector2> input, int i, out float dPiResult,
            out float ddPiResult)
        {
            dPiResult = dPi(input, i);
            ddPiResult = ddPi(input, i);
            return !float.IsNaN(dPiResult) && !float.IsNaN(ddPiResult);
        }

        private static bool IsTopLeftSlice(List<State> stack, List<Vector2> input)
        {
            if (GetDerivativesIfDefined(input, 4, out var dPiResult, out var ddPiResult))
            {
                return dPiResult > 0 && ddPiResult < 0;
            }

            return false; 
        }

        private static bool IsTopRightSlice(List<State> stack, List<Vector2> input)
        {
            if (GetDerivativesIfDefined(input, 3, out var dPiResult, out var ddPiResult))
            {
                return dPiResult <= 0 && ddPiResult < 0;
            }

            return false; 
        }

        private static bool IsBottomRightSlice(List<State> stack, List<Vector2> input)
        {
            if (GetDerivativesIfDefined(input, 3, out var dPiResult, out var ddPiResult))
            {
                return dPiResult > 0 && ddPiResult > 0;
            }

            return false; //Derivative is not defined so we allow the verification to continue
        }

        private static bool IsBottomLeftSlice(List<State> stack, List<Vector2> input)
        {
            if (GetDerivativesIfDefined(input, 3, out var dPiResult, out var ddPiResult))
            {
                return dPiResult <= 0 && ddPiResult > 0;
            }

            return false; //Derivative is not defined so we allow the verification to continue
        }

        private bool IsEnd(List<State> stack, List<Vector2> input)
        {
            return stack.Contains(topLeftSlice) && stack.Contains(topRightSlice) && stack.Contains(bottomRightSlice) &&
                   stack.Contains(bottomLeftSlice);
        }

        //Input must have at least 4 points
        // First derivative on the point of index i
        private static readonly Func<List<Vector2>, int, float> dPi = (List<Vector2> input, int i) =>
        {
            if (input[i + 1].x - input[i - 1].x == 0)
            {
                throw new AssertionException("Derivative not defined for dxi = (f(x+i) - f(xi-1)) / (xi+1 - xi-1), with " +
                                             "f(xi-1) = " + input[i-1].y + ", " +
                                             "f(xi+1) = " + input[i+1].y + " and " +
                                             "xi-1 = " + input[i-1].x + ", " +
                                             "xi+1 = " + input[i+1].x + ". Result is unexpected.");
                return float.NaN;
            }

            return (input[i + 1].y - input[i - 1].y) / (input[i + 1].x - input[i - 1].x);
        };

        // Second derivative on the point of index i
        private static readonly Func<List<Vector2>, int, float> ddPi = (List<Vector2> input, int i) =>
        {
            if (input[i + 1].x - input[i - 1].x == 0)
            {
                throw new AssertionException("Derivative not defined for dxi = (f(x+i) - f(xi-1)) / (xi+1 - xi-1), with " +
                                             "f(xi-1) = " + input[i-1].y + ", " +
                                             "f(xi+1) = " + input[i+1].y + " and " +
                                             "xi-1 = " + input[i-1].x + ", " +
                                             "xi+1 = " + input[i+1].x + ". Result is unexpected.");
                return float.NaN;
            }

            var dPiRight = dPi(input, i + 1);
            if (dPiRight == 0)
            {
                return float.NaN;
            }

            var dPiLeft = dPi(input, i - 1);
            if (dPiLeft == 0)
            {
                return float.NaN;
            }

            return (dPiRight - dPiLeft) / (input[i + 1].x - input[i - 1].x);
        };

        private bool m_IsStarted = false;

        public State start { get; } = new State("Start");
        public State end { get; } = new State("End");
        public State topLeftSlice { get; } = new State("1");
        public State topRightSlice { get; } = new State("2");
        public State bottomRightSlice { get; } = new State("3");
        public State bottomLeftSlice { get; } = new State("4");

        private List<Vector2> m_Inputs = new List<Vector2>(6);
        public StateMachine stateMachine { get; }
    }
}