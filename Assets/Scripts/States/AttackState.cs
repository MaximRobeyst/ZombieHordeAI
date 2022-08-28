using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IFSMState
{
    private Animator m_Animaotr;

    public AttackState(GameAgent gameAgent) : base(gameAgent)
    {
        m_Animaotr = gameAgent.GetComponent<Animator>();
    }

    public override void Update()
    {
        m_Agent.transform.LookAt( m_Agent.CurrentCharacterTarget.transform.position, m_Agent.transform.up);
        var currentAnimClipInfo = m_Animaotr.GetCurrentAnimatorClipInfo(0)[0];
        if (currentAnimClipInfo.clip.name == "Punch")
        {
            return;
        }

        m_Animaotr.SetTrigger("Attacking");
    }
}
