using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    public FiniteStateMachine(IFSMState state, GameAgent agent)
    {
        m_Agent = agent;
        ChangeState(state);
    }

    public string GetCurrentStateName()
    {
        return m_CurrentState.ToString();
    }

    public void AddTransition(IFSMState startState, IFSMState toState, IFSMTransition transition)
    {
        if(m_Transitions.ContainsKey(startState))
        {
            m_Transitions[startState].Add(new KeyValuePair<IFSMTransition, IFSMState>(transition, toState));
            return;
        }
        m_Transitions.Add(startState,  new List<KeyValuePair<IFSMTransition, IFSMState>>());
        m_Transitions[startState].Add(new KeyValuePair<IFSMTransition, IFSMState>(transition, toState));
    }

    private void ChangeState(IFSMState state)
    {
        if (m_CurrentState == state) return;

        if(m_CurrentState != null)
            m_CurrentState.OnExit();

        m_CurrentState = state;

        if (m_CurrentState != null)
            m_CurrentState.OnEnter();
    }

    public void UpdateStateMachine()
    {
        if (m_CurrentState == null) return;

        if(m_Transitions[m_CurrentState].Count != 0)
        {
            for(int i = 0; i < m_Transitions[m_CurrentState].Count; ++i)
            {
                if(m_Transitions[m_CurrentState][i].Key.ToTransition(m_Agent))
                {
                    ChangeState(m_Transitions[m_CurrentState][i].Value);
                }
            }
        }

        if (m_CurrentState != null)
            m_CurrentState.Update();
    }

    public void Gizmos()
    {
        m_CurrentState.DrawGizmos();
    }

    private Dictionary<IFSMState, List<KeyValuePair<IFSMTransition, IFSMState>>> m_Transitions = new Dictionary<IFSMState, List<KeyValuePair<IFSMTransition, IFSMState>>>();
    private IFSMState m_CurrentState;

    private GameAgent m_Agent;
}

public abstract class IFSMState
{
    public IFSMState(GameAgent gameAgent)
    {
        m_Agent = gameAgent;
    }

    public virtual void OnEnter() { }
    public virtual void Update() { }
    public virtual void DrawGizmos() { }
    public virtual void OnExit() { }


    protected GameAgent m_Agent;
}

public abstract class IFSMTransition
{
    public abstract bool ToTransition(GameAgent agent);
}
