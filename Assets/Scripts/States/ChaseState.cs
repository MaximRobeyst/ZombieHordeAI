using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : IFSMState
{
    private NavMeshAgent m_NavmeshAgent;
    private Animator m_Animator;

    public ChaseState(GameAgent gameAgent) 
        : base(gameAgent)
    {
        m_NavmeshAgent = m_Agent.GetComponent<NavMeshAgent>();
        m_Animator = m_Agent.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        m_NavmeshAgent.isStopped = false;
        m_NavmeshAgent.speed = m_Agent.ChaseSpeed;
        m_Animator.SetBool("Moving", true);
    }

    public override void Update()
    {
        m_NavmeshAgent.SetDestination(m_Agent.CurrentCharacterTarget.transform.position);

        // I want to do it the same way left 4 dead does is by check on which navmesh area it is but that is not possible in unity i think
        // So i will just do a simple distance check


    }

    public override void DrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(m_Agent.transform.position, 0.1f);

    }

    public override void OnExit()
    {
        m_NavmeshAgent.isStopped = true;
        m_Animator.SetBool("Moving", false);
    }
}
