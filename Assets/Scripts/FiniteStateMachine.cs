using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : NetworkBehaviour
{
    private Dictionary<IFSMState, List<KeyValuePair<IFSMTransition, IFSMState>>> m_Transitions = new Dictionary<IFSMState, List<KeyValuePair<IFSMTransition, IFSMState>>>();
    
    public IFSMState CurrentState { get; set; }

    public GameAgent m_Agent { get; set; }

    public string GetCurrentStateName()
    {
        return CurrentState.ToString();
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

    public void ChangeState(IFSMState state)
    {
        Debug.Log(state.ToString());

        if (CurrentState == state) return;

        if(CurrentState != null)
            CurrentState.OnExit();

        CurrentState = state;

        if (CurrentState != null)
            CurrentState.OnEnter();
    }

    public void UpdateStateMachine()
    {
        if (!isServer) return;
        if (CurrentState == null) return;


        if (CurrentState != null)
            CurrentState.Update();

        if (!m_Transitions.ContainsKey(CurrentState) || m_Transitions[CurrentState] == null) return;
        if (m_Transitions[CurrentState].Count != 0)
        {
            for(int i = 0; i < m_Transitions[CurrentState].Count; ++i)
            {
                if(m_Transitions[CurrentState][i].Key.ToTransition(m_Agent))
                {
                    ChangeState(m_Transitions[CurrentState][i].Value);
                    return;
                }
            }
        }

    }

    public void Gizmos()
    {
        if (CurrentState == null) return;

        CurrentState.DrawGizmos();
    }
}

[System.Serializable]
public class IFSMState
{
    public IFSMState(GameAgent gameAgent)
    {
        m_Agent = gameAgent;
    }

    public virtual void OnEnter() { }
    public virtual void Update() { }
    public virtual void DrawGizmos() { }
    public virtual void OnExit() { }


    public GameAgent m_Agent { get; private set; }
}

public abstract class IFSMTransition
{
    public abstract bool ToTransition(GameAgent agent);
}


//public static class CustomReadWriteFunctions
//{
//    public static void WriteIFSMState(this NetworkWriter writer, IFSMState state)
//    {
//        NetworkIdentity networkIdentity = state.m_Agent.GetComponent<NetworkIdentity>();
//        writer.WriteNetworkIdentity(networkIdentity);
//    }
//
//    public static IFSMState ReadIFSMState(this NetworkReader reader)
//    {
//
//        return new IFSMState()
//    }
//}
