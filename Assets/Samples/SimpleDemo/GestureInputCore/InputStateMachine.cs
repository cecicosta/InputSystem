using System;
using System.Collections.Generic;
using UnityEngine;

namespace Samples.SimpleDemo.GestureInputCore
{
    public class State
    {
        public Action<State> onEnter { get; set; }

        public Action<State> onExit { get; set; }

        public State(String name)
        {
            m_Name = name;
            onEnter = (State s) => { };
            onExit = (State s) => { };
        }

        public State(String name, Action<State> onEnter)
        {
            m_Name = name;
            this.onEnter = onEnter;
            onExit = (State s) => { };
        }

        public State(String name, Action<State> onEnter, Action<State> onExit)
        {
            m_Name = name;
            this.onEnter = onEnter;
            this.onExit = onExit;
        }
        public void AddTransition(State next, Func<List<State>, List<Vector2>, bool> funcTransition)
        {
            m_Transitions.Add(new Transition(next, funcTransition));
        }
        public State OnUpdate(List<State> stack, List<Vector2> input) {
            foreach (var transition in m_Transitions)
            {
                if (transition.CanTransition(stack, input))
                {
                    return transition.next;
                }
            }

            return this;
        }
        
        private List<Transition> m_Transitions = new List<Transition>();
        private readonly String m_Name;
    }
    class Transition
    {
        public State next => m_Next;
        public Transition(State next, Func<List<State>, List<Vector2>, bool> funcTransition)
        {
            m_Next = next;
            m_FuncTransition = funcTransition;
        }

        public bool CanTransition(List<State> stack, List<Vector2> input)
        {
            return m_FuncTransition(stack, input);
        }
        private readonly Func<List<State>, List<Vector2>, bool> m_FuncTransition;
        private readonly State m_Next;
    }

    public class StateMachine
    {
        public StateMachine(State startState, State endState, int stackSize)
        {
            m_Stack = new List<State>(stackSize);
            m_Current = startState;
            m_Start = startState;
            m_End = endState;
        }
        public void Update(List<Vector2> input)
        {
            var next = m_Current.OnUpdate(m_Stack, input);
            if (next != m_Current)
            {
                m_Current.onExit(m_Current);
                m_Stack.Add(m_Current);
                m_Current = next;
                m_Current.onEnter(m_Current);
            }

            if (m_Stack.Count == m_Stack.Capacity)
            {
                m_Stack.RemoveAt(0);
            }
        }

        public bool IsStarted()
        {
            return m_Current != m_Start;
        }

        public bool IsFinished()
        {
            return m_Current == m_End;
        }

        public void Reset()
        {
            m_Stack.Clear();
            m_Current = m_Start;
        }
        
        private List<State> m_Stack;
        private State m_Current;
        private readonly State m_Start;
        private readonly State m_End;
    }
}