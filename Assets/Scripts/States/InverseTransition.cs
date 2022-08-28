using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseTransition : IFSMTransition
{
    IFSMTransition m_Transition;

    public InverseTransition(IFSMTransition transition)
    {
        m_Transition = transition;
    }

    public override bool ToTransition(GameAgent agent)
    {
        return !m_Transition.ToTransition(agent);
    }
}
