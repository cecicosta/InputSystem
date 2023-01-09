using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Assertions;


namespace Samples.SimpleDemo.GestureInputCore
{
    public class CircleGesture
    {

        public int sampleSize
        {
            get => m_SampleSize;
            set => m_SampleSize = value < 5 ? m_SampleSize : value;
        }
        public float epsilon
        {
            get => m_Epsilon;
            set => m_Epsilon = value;
        }
        public int cancelThreshold
        {
            get => m_CancelThreshold;
            set => m_CancelThreshold = value;
        }
        public int transitionThreshold
        {
            get => m_TransitionThreshold;
            set => m_TransitionThreshold = value;
        }
        public CircleGesture(Action onStarted, Action onFinished, Action onCanceled, Action onIdle)
        {
            this.onStarted = onStarted;
            this.onFinished = onFinished;
            this.onCanceled = onCanceled;
            this.onIdle = onIdle;

            start.AddTransition(topLeftSlice, CreateConditionalWithThreshold(IsTopLeftSlice, 0));
            start.AddTransition(topRightSlice, CreateConditionalWithThreshold(IsTopRightSlice, 0));
            start.AddTransition(bottomRightSlice, CreateConditionalWithThreshold(IsBottomRightSlice, 0));
            start.AddTransition(bottomLeftSlice, CreateConditionalWithThreshold(IsBottomLeftSlice, 0));

            topLeftSlice.AddTransition(end, IsEnd);
            topLeftSlice.AddTransition(topRightSlice, CreateConditionalWithThreshold(IsTopRightSlice, transitionThreshold));
            topLeftSlice.AddTransition(bottomLeftSlice, CreateConditionalWithThreshold(IsBottomLeftSlice, transitionThreshold));
            topLeftSlice.AddTransition(start,
                CreateConditionalWithThreshold((stack, input) => !IsTopLeftSlice(stack, input), cancelThreshold));

            topRightSlice.AddTransition(end, IsEnd);
            topRightSlice.AddTransition(bottomRightSlice, CreateConditionalWithThreshold(IsBottomRightSlice, transitionThreshold));
            topRightSlice.AddTransition(topLeftSlice, CreateConditionalWithThreshold(IsTopLeftSlice, transitionThreshold));
            topRightSlice.AddTransition(start,
                CreateConditionalWithThreshold((stack, input) => !IsTopRightSlice(stack, input), cancelThreshold));

            bottomRightSlice.AddTransition(end, IsEnd);
            bottomRightSlice.AddTransition(bottomLeftSlice, CreateConditionalWithThreshold(IsBottomLeftSlice, transitionThreshold));
            bottomRightSlice.AddTransition(topRightSlice, CreateConditionalWithThreshold(IsTopRightSlice, transitionThreshold));
            bottomRightSlice.AddTransition(start,
                CreateConditionalWithThreshold(
                    (stack, input) => !IsBottomRightSlice(stack, input), cancelThreshold));

            bottomLeftSlice.AddTransition(end, IsEnd);
            bottomLeftSlice.AddTransition(topLeftSlice, CreateConditionalWithThreshold(IsTopLeftSlice, transitionThreshold));
            bottomLeftSlice.AddTransition(bottomRightSlice, CreateConditionalWithThreshold(IsBottomRightSlice, transitionThreshold));
            bottomLeftSlice.AddTransition(start,
                CreateConditionalWithThreshold((stack, input) => !IsBottomLeftSlice(stack, input), cancelThreshold));

            stateMachine = new StateMachine(start, end, 7);
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

            onCanceled();
        }

        public void Reset()
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
            else if (Mathf.Abs(position.x - m_Inputs.Last().x) > epsilon)
            {
                m_Inputs.Add(position);
            }

            if (m_Inputs.Count > sampleSize)
            {
                m_Inputs.RemoveAt(0);
            }

            return m_Inputs.Count == sampleSize;
        }

        private bool GetDerivativesIfDefined(List<Vector2> input, int i, out float dPiResult,
            out float ddPiResult)
        {
            dPiResult = dPi(input, i);
            ddPiResult = ddPi(input, i);
            return !float.IsNaN(dPiResult) && !float.IsNaN(ddPiResult);
        }

        private bool DerivativeAverageIfDefined(List<Vector2> input, out float dPiAverage, out float ddPiAverage)
        {
            dPiAverage = 0;
            ddPiAverage = 0;
            int count = 0;
            //TODO: Cache the results so we can avoid recalculating the derivative at a given point
            for (int i = 1; i < sampleSize - 2; i++)
            {
                if (GetDerivativesIfDefined(input, i, out var dPiResult, out var ddPiResult))
                {
                    dPiAverage += dPiResult;
                    ddPiAverage += ddPiResult;
                    count++;
                }
            }

            dPiAverage = count > 0 ? dPiAverage / count : 0;
            ddPiAverage = count > 0 ? ddPiAverage / count : 0;
            return count > 0;
        }

        /// <summary>
        /// This method is used to detect if the current coordinates on input list correspond to the TopLeft slice of a circle, allowing a transition of state.
        /// It bases the decision on the average value of the points derivative first and second to analise the curve behavior.
        /// </summary>
        /// <param name="stack"></param>
        /// Last states added to the stack. Can be used to implement transitions sensitive to context.
        /// <param name="input"></param>
        /// List of points read from the input device as Vector2 coordinates.
        /// <returns></returns>
        private bool IsTopLeftSlice(List<State> stack, List<Vector2> input)
        {
            return DerivativeAverageIfDefined(input, out var dPiAverage, out var ddPiAverage) && dPiAverage > 0 &&
                   ddPiAverage <= 0;
        }
        private bool IsTopRightSlice(List<State> stack, List<Vector2> input)
        {
            return DerivativeAverageIfDefined(input, out var dPiAverage, out var ddPiAverage) && dPiAverage <= 0 &&
                   ddPiAverage < 0;
        }
        private bool IsBottomRightSlice(List<State> stack, List<Vector2> input)
        {
            return DerivativeAverageIfDefined(input, out var dPiAverage, out var ddPiAverage) && dPiAverage < 0 &&
                   ddPiAverage >= 0;
        }

        private bool IsBottomLeftSlice(List<State> stack, List<Vector2> input)
        {
            return DerivativeAverageIfDefined(input, out var dPiAverage, out var ddPiAverage) && dPiAverage >= 0 &&
                   ddPiAverage > 0;
        }
        
        private Func<List<State>, List<Vector2>, bool> CreateConditionalWithThreshold(
            Func<List<State>, List<Vector2>, bool> condition,
            int threshold)
        {
            var detectionThresholdCount = 0;
            return (List<State> stack, List<Vector2> input) =>
            {
                if (condition(stack, input))
                {
                    detectionThresholdCount++;
                    if (detectionThresholdCount > threshold)
                    {
                        detectionThresholdCount = 0;
                        return true;
                    }
                }
                else
                {
                    detectionThresholdCount = 0;
                }

                return false;
            };
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
            if (input[i + 1].x - input[i].x == 0)
            {
                throw new Exception("Derivative not defined for dxi = (f(x+i) - f(xi-1)) / (xi+1 - xi-1), with " +
                                    "f(xi-1) = " + input[i].y + ", " +
                                    "f(xi+1) = " + input[i + 1].y + " and " +
                                    "xi-1 = " + input[i].x + ", " +
                                    "xi+1 = " + input[i + 1].x + ". Result is unexpected.");
            }

            //A circle do not behave as a function, but we can as if x intervals were always increasing
            return (input[i + 1].y - input[i].y) / Mathf.Abs(input[i + 1].x - input[i].x);
        };

        // Second derivative on the point of index i
        private static readonly Func<List<Vector2>, int, float> ddPi = (List<Vector2> input, int i) =>
        {
            if (input[i + 1].x - input[i].x == 0)
            {
                throw new Exception("Derivative not defined for dxi = (f(x+i) - f(xi)) / (xi+1 - xi), with " +
                                    "f(xi-1) = " + input[i].y + ", " +
                                    "f(xi+1) = " + input[i + 1].y + " and " +
                                    "xi-1 = " + input[i].x + ", " +
                                    "xi+1 = " + input[i + 1].x + ". Result is unexpected.");
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

            //A circle do not behave as a function, but we can as if x intervals were always increasing
            return (dPiRight - dPiLeft) / (Mathf.Abs(input[i + 1].x - input[i - 1].x));
        };

        private bool m_IsStarted = false;

        private int m_SampleSize = 8;
        private float m_Epsilon = 0.01f;
        private int m_CancelThreshold = 10;
        private int m_TransitionThreshold = 3;

        private Action onStarted { get; }
        private Action onFinished { get; }

        private Action onCanceled { get; }
        private Action onIdle { get; }
        
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