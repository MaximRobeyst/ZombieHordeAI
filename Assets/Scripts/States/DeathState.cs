using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : IFSMState
{
    private Animator m_Animator;

    public DeathState(GameAgent gameAgent) : base(gameAgent)
    {
        m_Animator = gameAgent.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        m_Animator.SetBool("Death", true);
    }
}
