using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : IFSMState
{
    private Animator m_Animator;
    private float m_StartTime= 0;
    private float m_Length;

    public DeathState(GameAgent gameAgent) : base(gameAgent)
    {
        m_Animator = gameAgent.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        m_Animator.SetBool("Dead", true);
        m_StartTime = Time.time;
    }

    public override void Update()
    {
        var currentAnimClipInfo = m_Animator.GetCurrentAnimatorClipInfo(0)[0];
        if(currentAnimClipInfo.clip.name == "Death")
        {
            if (m_StartTime == 0)
                m_StartTime = Time.time;
            m_Length = currentAnimClipInfo.clip.length;
            if (Time.time - m_StartTime >= m_Length)
            {
                m_Animator.enabled = false;
                m_Agent.ActivateRigidbody(true);
                m_Agent.GetComponent<Collider>().enabled = false;
                m_StartTime = 0;
            }
        }
    }

    bool IsPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }
}
